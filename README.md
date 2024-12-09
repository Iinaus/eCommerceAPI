# eCommerceAPI

## Table of contents
- [About](#about)
- [Getting started](#getting-started)
  - [Dependencies](#dependencies)
  - [Dev setup step-by-step](#dev-setup-step-by-step)
- [API documentation](#api-documentation)


## About

This project is a RESTful API built with C# .NET Core to manage eCommerce data. It serves as a practical exercise in developing secure, scalable, and maintainable web APIs while focusing on core backend development concept. The project was developed as part of the Web Development and Frameworks course at Lapland University of Applied Sciences.

With this exercise we practiced:
- Setting up a .NET Core backend for building secure RESTful APIs.
- Implementing JWT authentication for secure user login and authorization.
- Implementing CRUD operations (Create, Read, Update, Delete) to manage eCommerce data such as orders, cart items, and products.
- Creating custom middleware to handle authentication and logging for requests.
- Utilizing Entity Framework Core for efficient database interactions and migrations.
- Structuring the project with clear separation of concerns across Controllers, Models, Services, and Middleware.

## Getting started

Instructions on how to set up and run the backend.

### Dependencies

This project relies on the following dependencies:

- **.NET Core SDK**: Framework for building and running the application.
- **Entity Framework Core**: An ORM (Object-Relational Mapper) for database interactions. Supports various database providers, including SQLite in this project.
- **JWT Authentication**:
-   **Microsoft.AspNetCore.Authentication.JwtBearer**: For securing API endpoints using JSON Web Tokens (JWT).
-   **System.IdentityModel.Tokens.Jwt**: Handles creation and validation of JWTs.
- **AutoMapper**: Simplifies object-to-object mapping.

Ensure these dependencies are installed to ensure smooth execution of the project.

### Dev Setup Step-by-Step

1. Clone the project
2. Install the .NET Core SDK.
   - Ensure the correct version of the .NET Core SDK is installed. Check the .csproj file for the targeted framework version.
4. Restore Dependencies (NuGet packages):
`dotnet restore`
5. Update the Database
   - If using Entity Framework Core, apply migrations to the database:
`dotnet ef database update`
7. Run the Project
Use the following command to run the application:
`dotnet run`
Alternatively, use dotnet watch to automatically monitor code changes and restart the application when a change is detected:
`dotnet watch`

## API documentation
This project provides an interactive Swagger UI for easy exploration and testing of the API endpoints. When you run the project locally, you can access the Swagger documentation in your browser, which will automatically be generated for all API routes.

With Swagger UI you can:
- View a list of all available API endpoints.
- Test API routes directly from the Swagger UI.
- View detailed request and response formats for each endpoint.
- Generate sample requests and see real-time responses.

### Authentication
All endpoints that require authentication will expect a JWT token in the Authorization header of the request.
Use the POST /Users/login endpoint to obtain a JWT token by providing valid user credentials (username and password).
Once you have the token, include it in the header of subsequent requests as:
`Authorization: Bearer <Your-JWT-Token>`
