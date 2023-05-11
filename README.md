
## ASP.NET webapp with simple auth.

A very simple ASP.NET webapp with user registration and login. For educational purposes.

User list with passwords are stored in JSON [file](/myDB/users.json) with plain text passwords on server, 
which is updated on every new user registration.

The basis of the application was made using .NET CLI app generator: 

```shell
dotnet new webapp -n AspNetCoreDemo -o AspNetCoreDemo && cd AspNetCoreDemo 
```

See [project_setup.sh](project_setup.sh)

### Run in development 

```shell
dotnet run 
```

### Deploy 

Compile: 

```shell
dotnet publish --configuration Release
```

Production build will be stored in [/bin/Release/net7.0/publish/](/bin/Release/net7.0/publish/)

This app was deployed on an Ubuntu 22.04 server with Nginx.
Required files are stored in [serverFiles](/serverFiles/) directory.

See: 
[Host ASP.NET Core on Linux with Nginx](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-7.0&tabs=linux-ubuntu)

To run on server: 

1) Configure Nginx on server, see [serverFiles/default](serverFiles/default)
2) Copy [serverFiles/aspnet-webapp.service](serverFiles/aspnet-webapp.service)  to ```/etc/systemd/system/aspnet-webapp.service``` on server
3) Copy [/bin/Release/net7.0/publish/](/bin/Release/net7.0/publish/) directory to server  
4)  Start the service and verify that it's running: 

```shell
sudo systemctl enable aspnet-webapp.service
sudo systemctl start aspnet-webapp.service
sudo systemctl status aspnet-webapp.service
```

To see app logs on server: 
```shell 
sudo journalctl -fu aspnet-webapp.service
```

or with data range:

```shell
sudo journalctl -fu aspnet-webapp.service --since "2016-10-18" --until "2016-10-18 04:00" 
```

