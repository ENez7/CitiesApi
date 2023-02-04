# CitiesAPI

This API was intended to be only for educational purposes. If this is useful to you, please consider giving this repo a star.

To more reference and in-depth explanations, please refer  to the original course
made by Kevin Dockx in [Pluralsight](https://app.pluralsight.com/library/courses/asp-dot-net-core-6-web-api-fundamentals/table-of-contents).

The objective of this project is to build a .NET Core 6 Web API to gain hands-on experience with:

 - .NET Core 6
 - API concepts and design principles
 - Implementing CRUD operations on custom entities
 - Using API controllers and annotations
 - Utilizing Entity Framework Core as the Object-Relational Mapping (ORM) framework
 - Implementing search, filtering, and pagination functionality
 - Securing the API with JSON Web Tokens (JWT)
 - Managing API versioning and documenting API endpoints and functionality.

## Tools used in developing

- [Rider](https://www.jetbrains.com/rider/) by JetBrains<br>
  Note: Rider currently does not support PM Console, use PowerShell console for EF commands instead
- [Postman](https://www.postman.com/)
- [SQL Server Management Studio](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16) by Microsoft<br>
Note: This can be replaced with any database management system of your choice
- Any browser of your preference (If needed, set `"launchBrowser"` value to `true` in `launchSettings.json`)

## Architecture

![Architecture](/Images/architecture-diagram.png)