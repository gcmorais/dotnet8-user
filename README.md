# GestProducts - ServerSide

## <a name="apresentation">Introduction</a>

GestProducts is a product inventory management project where users can create and associate products with categories. It includes authentication and authorization features to control access to endpoints.

## <a name="techsUsage">Technologies Used</a>

For this project I used the following technologies and why:

``.NET``:  A robust and reliable technology selected for server-side development.

``Entity Framework Core (EF Core)``: An ORM used to simplify database access, enabling relationships like One-to-Many.

``SQL Server``: A relational database chosen for its support of relationships and efficient queries.

``JWT``: Used for user authentication and authorization, generating JWT tokens for administrative operations.

``Clean Architecture and CQRS``: Implemented for learning purposes, supporting a more modular and scalable architecture.

## <a name="basicUsage">Key Features</a>

 - Full CRUD operations for User, Category, and Product entities.
 - One-to-Many relationship between entities, allowing products to be associated with categories and managed by users.
 - Data validation with FluentValidation to ensure consistency and meet specific requirements.

## <a name="basicUsage">Basic Usage</a>

Follow these steps to set up the project locally on your machine.


**Prerequisites**
<a name="prerequisites"></a>

Make sure you have the following installed on your machine:

- [Git](https://git-scm.com/)
- [.NET 8.0](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)
- [Visual Studio](https://visualstudio.microsoft.com/pt-br/)
- [Visual Studio Code](https://code.visualstudio.com/)
- [SQL Server Express](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)



**Cloning Repository**
<a name="cloning"></a>

```bash
git clone https://github.com/gcmorais/gestproducts-serverside.git
cd gestproducts-serverside
```

**Installation**
<a name="installation"></a>

Open the solution in Visual Studio to install the project dependencies.

```bash
dotnet8-user.sln
```

> [!note]
>
>  The project has evolved, and a name change became necessary. This is the reason for the difference between dotnet8-user and the final name, gestproducts.
>
>  Run the `dotnet restore` command, the .NET CLI uses NuGet to look for these dependencies and download them if necessary. 


**Connect to the database**
<a name="connectdb"></a>

We first need to connect to the database, so in the <strong>appsettings.json</strong> file in `DefaultConnection` in the server tag you will put the name of your server that was created in SQL Server Express;

```env
"DefaultConnection": "server= localhost\\Example; database= ExampleDB; trusted_connection=true; trustservercertificate=true"
```

I used the database connection via Windows authentication, so use the tag `trusted_connection = true;` 

if you need it, use the tags `user id=login;` `password=password;` see if it makes sense in your use case.

**Configure JWT Token**
<a name="connectdb"></a>

Now let's configure our jwt token, so in the <strong>appsettings.json</strong> navigate to the `Jwt` property and change the following information:

```env
"Jwt": {
    "Key": "your_key_here",
    "Issuer": "gestproducts",
    "Audience": "gestproducts"
}
```

**Running the Project**
<a name="running"></a>

First, let's run the migrations to create the tables for our database.
To do this, we'll open the Package Manager Console and issue the command:

```bash
add-migration FirstMigration
```

If the above command does not generate the migrations, open the *src* folder via terminal and follow the steps:

First, make sure you're in the project's root directory. Enter the "src" folder by typing:

```bash
cd src
```
Now, let’s generate the migration:

```bash
dotnet ef migrations add InitialMigration --startup-project dotnet8-user.Api --project dotnet8-user.Infrastructure
```
Next, let’s apply the migration to persist the data in the database:

```bash
dotnet ef database update --project dotnet8-user.Infrastructure --startup-project dotnet8-user.Api
```

Run your project (CRTL + F5 in visual studio) to open [Swagger](https://swagger.io/) in your browser to view the project.

If you have any questions or ideas, I'm available to talk. Thank you and I hope you make good use of it. 