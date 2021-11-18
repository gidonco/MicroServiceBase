using MicroServiceBase.Template.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroServiceBase.Template
{
    public class Program
    {
        private static NLog.Logger logger;

        public static void Main(string[] args)
        {
            AppDomain appDomain = AppDomain.CurrentDomain;
            appDomain.UnhandledException += AppDomain_UnhandledException;


            logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            NLog.LogManager.Configuration.Variables["logfolder"] = getConfiguration().GetSection(nameof(ServiceOptions)).GetValue<string>(nameof(ServiceOptions.LogDirectory));
            NLog.LogManager.ReconfigExistingLoggers();
            logger.Info("Logger initialized");

            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                CreateHostBuilder(args).Build().Run();
            }
            catch (System.Exception ex)
            {
                logger.Fatal(ex, $"Fatal Exception '{ex.Message}' in Main()");
                throw;
            }
        }

        private static IConfiguration getConfiguration()
        {
            var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
            var currentDirectory = Environment.CurrentDirectory;

            var configuration = new ConfigurationBuilder()
                  .SetBasePath(currentDirectory)
                  .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                  .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                  .Build();

            return configuration;
        }

        private static void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as System.Exception;

            if (ex != null)
            {
                logger.Fatal(ex, $"Unhandled exception {ex.Message}");
            }
            else
                logger.Fatal($"Unkown unhandled exception {e.ExceptionObject}");
        }

        private static bool isDevelopment;

        public static IHostBuilder CreateHostBuilder(string[] args) =>


            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((hostcontext,services) =>
                {
                    var configuration = hostcontext.Configuration;

                    services.Configure<ServiceOptions>(configuration.GetSection(nameof(ServiceOptions)));

                    // add your DI stuff here
                    services.AddSingleton(typeof(GlobalStatus));


                    services.AddHostedService<Worker>();

                    isDevelopment = hostcontext.HostingEnvironment.IsDevelopment();
                }).ConfigureLogging((logging) =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(isDevelopment ? LogLevel.Debug : LogLevel.Trace);
                })
                .UseNLog();
    }
}
