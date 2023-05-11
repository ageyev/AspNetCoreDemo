
# Create a .NET Core web application using SDK from template 
# (see: https://learn.microsoft.com/en-us/troubleshoot/developer/webapps/aspnetcore/practice-troubleshoot-linux/2-1-create-configure-aspnet-core-applications) 
dotnet new webapp -n AspNetCoreDemo -o AspNetCoreDemo && cd AspNetCoreDemo || exit 

# add Newtonsoft.Json package 
# see: https://www.nuget.org/packages/Newtonsoft.Json/  
dotnet add package Newtonsoft.Json --version 13.0.3
