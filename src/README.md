# MCA-TFM-Sharethings

Comandos para el arranque de la base de datos

https://docs.microsoft.com/es-es/ef/core/cli/dotnet

```
dotnet ef --startup-project ../ShareThings/ migrations add InitialCreate

dotnet ef --startup-project ../ShareThings/ database update

dotnet ef --startup-project ../ShareThings/ migrations add commentDate --context ShareThingsDbContext

dotnet ef --startup-project ../ShareThings/ database update --context ShareThingsDbContext
```
With multiple DBContext
```
dotnet ef migrations add InitialCreate --context ShareThingsIdentityContext
dotnet ef database update --context ShareThingsIdentityContext

dotnet ef database update -c ShareThingsIdentityContext
dotnet ef database update -c ShareThingsDbContext

dotnet ef migrations script --context ShareThingsIdentityContext --idempotent --output "./db-migrations.sql"
dotnet ef migrations script --context ShareThingsdbContext --idempotent --output "./db-migrationsDB.sql"

```


### Servicio de identidad, mailing, reset, recover, bla bla
* https://docs.microsoft.com/es-es/aspnet/core/security/authentication/accconfirm?view=aspnetcore-5.0&tabs=visual-studio
* https://devblogs.microsoft.com/aspnet/aspnetcore-2-1-identity-ui/
* https://docs.microsoft.com/es-es/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-5.0&tabs=visual-studio
* https://docs.microsoft.com/es-es/aspnet/core/security/authentication/identity?view=aspnetcore-5.0&tabs=visual-studio
* https://medium.com/@MisterKevin_js/enabling-email-verification-in-asp-net-core-identity-ui-2-1-b87f028a97e0
* https://codewithmukesh.com/blog/user-management-in-aspnet-core-mvc/


### MVC .Net Core Life cycle
* https://www.c-sharpcorner.com/article/asp-net-core-mvc-request-life-cycle/

* http://www.techbloginterview.com/wp-content/uploads/2018/04/Screen-Shot-2018-04-14-at-10.37.30-PM.png

* https://docs.microsoft.com/es-es/aspnet/core/fundamentals/middleware/?view=aspnetcore-5.0

* https://docs.microsoft.com/es-es/aspnet/core/mvc/controllers/routing?view=aspnetcore-5.0

* https://docs.microsoft.com/es-es/aspnet/core/mvc/overview?view=aspnetcore-5.0

* https://docs.microsoft.com/es-es/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures

### Entity framework
* https://www.tektutorialshub.com/asp-net-core/asp-net-core-identity-tutorial/

### Bootstrap
* https://getbootstrap.com/docs/4.0/layout/grid/

## SQL Server on Docker
https://euedofia.medium.com/dockerize-microsoft-sql-server-on-macos-a8d40f8b1e66
```
docker pull mcr.microsoft.com/mssql/server:2019-latest

docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=TfmInfinit0!" -p 1433:1433 --name dev-mssql-server -d mcr.microsoft.com/mssql/server:2019-latest
```

## Sendgrid
Cuenta: nkoutsourais+tfm@gmail.com | TfmInfinit0!TfmInfinit0!
https://www.ryadel.com/en/asp-net-core-send-email-messages-sendgrid-api/

## Routes MVC
https://docs.microsoft.com/es-es/aspnet/core/mvc/controllers/routing?view=aspnetcore-5.0