
## ASP.NET webapp with simple auth.

A very simple ASP.NET webapp with user registration and login. For educational purposes.

User list with passwords are stored in JSON [file](/myDB/users.json) with plain text passwords on server, 
which is updated on every new user registration.

The basis of the application was made using .NET CLI app generator: 

```shell
dotnet new webapp -n AspNetCoreDemo -o AspNetCoreDemo && cd AspNetCoreDemo 
```

See [project_setup.sh](project_setup.sh)

### Deploy 

This app was deployed on an Ubuntu 22.04 server with Nginx.
Required files are stored in [serverFiles](/serverFiles/) directory.

See: 
[Host ASP.NET Core on Linux with Nginx](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-7.0&tabs=linux-ubuntu)

