#DildoStore

## Starting SQL Server on a docker container

```zsh
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=$sa_password" -e "MSSQL_PID=Evaluation" -p 1433:1433 -v sqlvolume:/var/opt/mssql --rm --name mssql --hostname sqlpreview -d mcr.microsoft.com/mssql/server:2022-preview-ubuntu-22.04
```

#Setting the conn string in secret Manager
$sa_password = "SA_PASSWORD_GOES_HERE"
dotnet user-secrets set "ConnectionStrings:DildoStoreContext" "Server=localhost; DataBase=DildoStore; User Id=sa; Password=$sa_password; TrustServerCertificate=True;"
