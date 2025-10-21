# An [`OpenAPI`](https://www.openapis.org/) Example with [`.NET 9.0 Minimal API`](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-9.0) using a Backend-first Approach for multiple Angular SignalStores

> [!IMPORTANT]
> This Example was built with personal Preferences and does not represent official OpenAPI nor .NET best Practices and neither suggests this Structure and Approach as the best possible Solution for building a Backend for multiple SignalStores.

## Project Structure

This Backend is built with the Idea of creating the Models, Database and Routes first and then using the generated OpenAPI JSON File as OAS to generate the Frontend Route-Skeleton.   

---

## File/Folder Overview

### - [`Models`](#Models)

### - [`AppDbContext`](#AppDbContextcs)

### - [`Services`](#Services)

### - [`Program`](#Programcs)

### - [`appsettings.Development.json`](#appsettingsDevelopmentjson)

---

> [!NOTE]
> The Example has a `.sql` and `.http` File with Example Queries and Requests that can be used when creating a Database with the exact same Port, Models, DbContext, Services and Routes.

> [!TIP]
> Each File has the "to-be-used" Imports at the beginning in case of import Errors and/or not recognized Methods.

---

## Dependencies

The following Dependencies are being used in this Example;

__OPTIONAL:__ [`Scalar.AspNetCore`](https://scalar.com/), 
is used for API Documentation.    
__REQUIRED:__ [`Microsoft.EntityFrameworkCore`](https://learn.microsoft.com/en-us/ef/core/) 
is used for the Database.     
__REQUIRED:__ [`Microsoft.EntityFrameworkCore.Design`](https://ngrx.io/guide/signals) 
is used for the Database Migration.  
__REQUIRED:__ [`Pomelo.EntityFrameworkCore.MySql`](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql) 
is used for the Database when using MySQL.  

### Install with:

__NuGet__

```text
Scalar.AspNetCore
```
```text
Microsoft.EntityFrameworkCore
```
```text
Microsoft.EntityFrameworkCore.Design
```
```text
Pomelo.EntityFrameworkCore.MySql
```

__.NET CLI__

```shell
  dotnet add package Scalar.AspNetCore
  dotnet add package Microsoft.EntityFrameworkCore
  dotnet add package Microsoft.EntityFrameworkCore.Design
  dotnet add package Pomelo.EntityFrameworkCore.MySql
```

---

## Recommended Setup Steps

1. Install the Dependencies:

```shell
  dotnet add package Scalar.AspNetCore
  dotnet add package Microsoft.EntityFrameworkCore
  dotnet add package Microsoft.EntityFrameworkCore.Design
  dotnet add package Pomelo.EntityFrameworkCore.MySql
```

---

### [`Models`](./Models)

2. Create a Folder named Models.  
Use Annotations such as `[Range()]` and `[Length()]` to limit Number Ranges and String Lengths.
Adding the `required` Modifier will make the Property necessary.

```csharp
    [MaxLength(50)]
    public string
    [Length(4, 4)]
    [Range(1900, 2050)]
    public required int
```

3. (Optional: Create DTO's for each of your Models for better Granularity when sending Patches to the Database. Adding 
the same Properties the Model has but making them Optional with `?` will remove the need to add unchanged Properties to 
the JSON.)

---

### [`AppDbContext.cs`](./Data/AppDbContext.cs)

```csharp
using Microsoft.EntityFrameworkCore;
```

4. Create a new Folder for the Database Context which is used to create Database Sessions and enables the querying and 
saving of instances of the previously defined [`Models`](./Models).
Adding the `Configuration` Interface will allow to get the Database Connection String from a different File such as 
[`appsettings.Development.json`](appsettings.Development.json) creating a centralized State.
Finally add the DbSet for every Model that should have its Table in the Database and be queryable against with LINQ.

```csharp
public class AppDbContext(IConfiguration config) : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connection = config.GetConnectionString("Database");
        optionsBuilder.UseMySql(connection, ServerVersion.AutoDetect(connection));
    }

    public DbSet<Type> TableName { get; set; }
}
```

---

### [`Services`](./Services)

```csharp
using Microsoft.EntityFrameworkCore;
```
 
5. Now create the Services for each of the queryable Entities in a new Folder, add the previously defined 
[`DbContext.cs`](./Data/AppDbContext.cs), and use either a primitive or the Model as return Type to keep the Services 
free of HTTP Types and let [`Program.cs`](Program.cs) handle those. Make the return Type Nullable for cases like the 
record not existing in the Database and therefore not available for Operations.  

---

### [`Program.cs`](Program.cs)

```csharp
using Scalar.AspNetCore;
```

6. Add the [`DbContext.cs`](./Data/AppDbContext.cs), [`Services`](./Services) as well as a CORS Policy for the Frontend 
Port to the builder before the `Build()` Call.  
Optionally add Scalar to the same Section as OpenApi with `app.MapScalarApiReference();`.  
Next up, tell the app to use the previously defined CORS Policy with `app.UseCors();`.  
Finally define the Routes while wrapping the Service methods in `try-catch` Blocks to handle different `HTTP Responses`.  

```csharp
builder.Services.AddDbContext<DbContext>();
builder.Services.AddScoped<Service>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors();
```

---

### [`appsettings.Development.json`](appsettings.Development.json)

7. Define the Database Connection String under `ConnectionStrings`.  

```json
  "ConnectionStrings": {
    "Database": "server=localhost; database=openapi; user=root; password=;"
  }
```

---

### `CLI`

8. Finally run the EF Core Commands for the Database:

```shell
  dotnet ef migrations add InitialModel
```
and 
```shell
  dotnet ef database update
```

9. After starting with 
```shell
  dotnet run
```
the OpenAPI File is available on [`/openapi/v1.json`](http://localhost:5083/openapi/v1.json) and 
Scalar on [`/scalar/v1`](http://localhost:5083/scalar/v1)

[`Back to Top`](#an-openapi-example-with-net-90-minimal-api-using-a-backend-first-approach-for-multiple-angular-signalstores)