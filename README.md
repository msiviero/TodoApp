
## TodoApp

This is a simple ASP.NET Core Web API project with a SQLite database for managing todo items. It includes unit tests written with XUnit.

### Features

* Add, edit, and delete todo items
* Mark items as completed

### Technologies

* ASP.NET Core Web API
* SQLite
* Entity Framework Core
* XUnit
* OpenAPI (Swagger)

### Prerequisites

To run the sample you need the following tools installed on your machine:

* .NET Core SDK
* Entity framework core tools (ef)

To install the Entity framework core tools, open a terminal and run the following commands:

```
dotnet tool install --global dotnet-ef
```

### Development

1. Clone the repository:

```
git clone git@github.com:msiviero/TodoApp.git
```

2. Install dependencies:

```
dotnet restore
```

3. Run the application:

```
dotnet run --project TodoApp
```

4. Access the API endpoints using Swagger UI:

```
localhost:5000/swagger
```

### Building

To build the application run:

```
dotnet publish TodoApp --self-contained --runtime linux-x64
```

You can use different runtimes if your server has different architecture than linux x64.

### Database

The application uses a SQLite database to store todo items. The schema is managed by Entity Framework Core.

To create a new migration run:

```
dotnet ef --project TodoApp migrations add YourMigrationName
```

To update the migrations against the database run:
```
dotnet ef --project TodoApp database update
```

### Unit Tests

The application includes unit tests for all functionalities of the API endpoints and business logic. The test project is called `TodoApp.Tests`.

to run unit tests:
```
dotnet test
```