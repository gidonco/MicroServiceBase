# MicroServiceBase
dotnet new template for a basic microservice project

A simple example of a template project. I use this as a template for some microservice scenarios where I need a hybrid worker service/API that can run as a windows service.

to build the nuget package : 
  
    run pack_nuget.cmd

to install the template :

    dotnet new -i dmdevcode.MicrosServiceBase.0.8.1.nupkg
  
to create a project from the template :

    dotnet new dmd-microservice

    
