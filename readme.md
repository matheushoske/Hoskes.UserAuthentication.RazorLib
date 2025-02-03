
# Authentication Razor Class Library

This repository provides a Razor Class Library (RCL) designed to streamline authentication and authorization processes in ASP.NET Core applications. It offers reusable components for user login, logout, account creation, and a basic profile page, all integrated with Entity Framework Core (EF Core) for data management.

## Features

- **User Login:** Allows users to authenticate using their email and password.
- **User Logout:** Enables users to securely sign out of the application.
- **Account Creation:** Provides functionality for new users to register by creating an account.
- **Profile Page:** Offers a basic profile page where users can view and manage their account information.
- **Entity Framework Core Integration:** Utilizes EF Core for efficient data handling and storage.

## Implementation Details

The library is structured to be easily integrated into any ASP.NET Core project. It includes Razor Pages for the aforementioned features and employs EF Core for database operations.

**1. User Login:**

The login functionality is implemented using a Razor Page that accepts the user's email and password, validates the credentials, and establishes an authentication cookie upon successful login.

**2. User Logout:**

A Razor Page handles user logout by clearing the authentication cookie, effectively signing the user out of the application.

**3. Account Creation:**

The account creation feature is provided through a Razor Page that collects user details, validates the input, and creates a new user record in the database using EF Core.

**4. Profile Page:**

A basic profile page is available where authenticated users can view their account information.

**5. Entity Framework Core Integration:**

EF Core is used for data operations, ensuring efficient and scalable database interactions.

## Integration Guide

To integrate this Razor Class Library into your ASP.NET Core project, follow these steps:

**1. Add Project Reference:**

In your main project, add a reference to the Razor Class Library:

- Right-click on the **Dependencies** node in your main project.
- Select **Add Project Reference**.
- Choose the Razor Class Library project and click **OK**.

**2. Configure Services:**

In your `Startup.cs` or `Program.cs`, add the Razor Class Library as an application part:

```csharp
services.AddRazorPages()
        .AddApplicationPart(typeof(Hoskes.UserAuthorization.Pages.LoginAccountModel).Assembly);
```

**3. Configure EF Core:**

Also in your `Startup.cs` or `Program.cs`, add the DBContext injection based on your preferred database (In this example we will be using Mysql):

Important: The library "Microsoft.EntityFrameworkCore" will be necessary for this step
If you're using Mysql, "MySql.EntityFrameworkCore" will be also necessary

```csharp
builder.Services.AddDbContext<HoskesAuthContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("HoskesAuthContext") ?? throw new InvalidOperationException("Connection string 'HoskesAuthContext' not found.")));
```
**4. Configure Connection String:**
In your appsettings.json add your Connection String with the following structure

```json
"ConnectionStrings": {
	"HoskesAuthContext": "Server=127.0.0.1;Database=dbName;Uid=dbUsername;Pwd=dbPassword;"
}
```

**5. Configure Authentication:**

Ensure that authentication services are configured in your main project:

```csharp
services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/LoginAccount";
            options.LogoutPath = "/Logout";
        });
```

**6. Apply Migrations:**

The Razor Class Library includes an EF Core Context, apply it to your database:

- Open the Development console.
- Run the following commands:

  ```
  dotnet package add Microsoft.EntityFrameworkCore.Tools
  dotnet ef migrations add Migration1
  dotnet ef database update
  ```

**7. Access Pages:**

You can now access the following pages provided by the Razor Class Library:

- **Login:** `/LoginAccount`
- **Logout:** `/Logout`
- **Register:** `/CreateAccount`
- **Profile:** `/Profile`

## Additional Resources

For more detailed information on implementing authentication and authorization in ASP.NET Core, refer to the official Microsoft documentation:

- [Introduction to Identity on ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-9.0)
- [Scaffold Identity in ASP.NET Core projects](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-9.0)

By integrating this Razor Class Library, you can efficiently manage user authentication and authorization in your ASP.NET Core applications, leveraging reusable components and EF Core for data management.
