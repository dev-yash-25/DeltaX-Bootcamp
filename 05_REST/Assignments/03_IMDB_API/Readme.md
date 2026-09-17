1. We are not having `Update Id` feature, and also remember we dont pass Id in request object, but we require a Id for update, so we pass id as a parameter through update() method
2.  Registering a service in Startup.cs means adding its registration to ASP.NET Core's Dependency Injection (DI) container.\
    Example :-
    ```csharp
    services.AddAutoMapper(typeof(MappingProfile));
    ```
    **This tells ASP.NET Core:** "Add AutoMapper to the application's Dependency Injection container, and load the mappings from MappingProfile."
