# ASP.NET-eShop

![.NET](https://github.com/gchurch/ASP.NET-eShop/workflows/.NET/badge.svg)

I have created a CRUD application in the style of an E-commerce website using ASP.NET Core 5.0. I have created a few different front ends using different technologies. The front-end technologies I have used are Angular, Razor and React. The solution uses a clean architecture which results in the front-ends being easily substitutable for one another. I have also created unit tests and functional tests for the application. The application is deployed on Azure for demonstration.

## Angular Front-end

I have created an SPA front end with Angular. The code for this is in the Web.Angular project.

The Angular app is deployed on Azure here: https://webangular20210218164157.azurewebsites.net/products

The Angular app uses a back-end API created with ASP.NET Core MVC. The code for the Api is in the Api project.

The application has the following API:

| API                       | Description                | Request body | Response body     |
| ------------------------- | -------------------------- | ------------ | ----------------- |
| GET /api/Products         | Get all products           | None         | Array of products |
| GET /api/Products/{id}    | Get a product by ID        | None         | Product           |
| POST /api/Products        | Add a new product          | Product      | Product           |
| PUT /api/Products         | Update an existing product | Product      | Product           |
| DELETE /api/Products/{id} | Delete a product           | None         | None              |

The API is documented using Swagger here: https://webangular20210218164157.azurewebsites.net/swagger/index.html

To run the application locally, in a command prompt navigate to src\Web.Angular\ClientApp and run the command "npm start". Then in Visual Studio set the Web.Angular project as the startup project and then start. 

## Razor Front-end

I have also created a separate front end using ASP.NET Core MVC and Razor. The code for this is in the Web.Razor project. For this version of the application I have implemented authentication and authorization using Identity. Users must be registered and logged in to an account in order to create a product. Users can only edit and delete products that they have created themselves. The admin account can edit and delete any product.

The Razor app is deployed on azure here: https://webrazor20210219144828.azurewebsites.net/Products

To run the application locally, set the Web.Razor project as the startup project and then start.

## React Front-end

I have created another SPA front end, this time using React. This is similar to the Angular app and uses the same back-end API. The code for this is in the Web.React project.

The React app is deployed on Azure here: https://webreact20210218165252.azurewebsites.net/products

To run the application locally, in a command prompt navigate to src\Web.React\ClientApp and run the command "npm start". Then in Visual Studio set the Web.React project as the startup project and then start. 

## Clean Architecture

A Clean Architecture based on https://github.com/ardalis/CleanArchitecture is used to organize the code.

The Clean Architecture separates the application into three layers with each layer being a separate project. The layers are the ApplicationCore, Infrastructure and Web (User Interface). ApplicationCore contains the Entities, interfaces and services; Infrastructure contains the database code; and Web contains the controllers and front-end code. Using a clean architecture results in the different front-ends I created being easily substitutable for one another. You just need to set the front-end project that you want to run as the startup project.

<img src="https://miro.medium.com/max/2750/0*lwCWXSNctrUUYeLR.png" alt="alt text" width="60%">

## Database

An SQL Server database is used to store the application data. Entity Framework Core is used for data access. LINQ is used to query data.

## Unit Tests

Unit testing is performed with MSTest using the Shouldly assertion framework and the Moq mocking framework. I have tried to follow best practice when it comes to unit testing.

## Functional Tests

The WebApplicationFactory class is used to perform functional end to end tests. A test server is created and an in-memory test database is used.
