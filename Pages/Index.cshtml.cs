using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static System.IO.Path;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace AspNetCoreDemo.Pages;

// All fields should be exactly as in JSON file for deserialization
public class AppUser
{
    public string Password;
    public string UserAuthenticationToken;

    // Constructor
    public AppUser(string password, string userAuthenticationToken)
    {
        Password = password;
        UserAuthenticationToken = userAuthenticationToken;
    }
}

public class IndexModel : PageModel
{
    // =========== fields (properties) 

    // logger 
    private readonly ILogger<IndexModel> _logger;

    private string _pathToUsersListFile = "./myDB/users.json";
    private string _userNameCookieKey = "userName";
    private string _userTokenCookieKey = "userAuthenticationToken";

    // "?" means that value can be null
    public string? Greeting = "Hello, who are you?";
    public Dictionary<string, AppUser>? UsersList;
    public string? CurrentUser;
    public bool LoggedIn = false;

    // constructor 
    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    // =========== utility functions 

    public Dictionary<string, AppUser>? ReadUsersList()
    {
        var jsonString = System.IO.File.ReadAllText(_pathToUsersListFile);
        // Dictionary<string, AppUser> dic = JsonSerializer.Deserialize<Dictionary<string, AppUser>>(jsonString); // < does not work for dictionary containing objects as values
        Dictionary<string, AppUser> dic = JsonConvert.DeserializeObject<Dictionary<string, AppUser>>(jsonString);
        return dic;
    }

    // see: 
    // https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings 
    public static string GetRandomAlphaNumeric(int length)
    {
        return GetRandomFileName().Replace(".", "").Substring(0, length);
    }

    // =========== request handlers  

    // GET request handler 
    public void OnGet()
    {
        if (HttpContext.Request.Cookies.ContainsKey(_userNameCookieKey) &&
            HttpContext.Request.Cookies.ContainsKey(_userTokenCookieKey))
        {
            CurrentUser = HttpContext.Request.Cookies[_userNameCookieKey];
            string? userAuthenticationToken = HttpContext.Request.Cookies[_userTokenCookieKey];

            UsersList = ReadUsersList();
            // _logger.Log(LogLevel.Information, userListToString()); // debug 

            if (CurrentUser != null)
            {
                if (UsersList != null && UsersList.ContainsKey(CurrentUser))
                {
                    if (userAuthenticationToken != null &&
                        userAuthenticationToken == UsersList[CurrentUser].UserAuthenticationToken)
                    {
                        LoggedIn = true;
                        Greeting = "Hello, " + CurrentUser + "! Nice to see you again.";
                    }
                    else
                    {
                        Greeting = "Hello! Please login again";
                    }
                }
            }
        }
    }

    // POST request handler
    public void OnPost(string? userName, string? userPassword, string formAction)
    {
        // _logger.Log(LogLevel.Information, $"userName: {userName}, userPassword: {userPassword}");

        if (formAction == "login")
        {
            if (userName != null && userPassword != null)
            {
                userName = userName.ToLower();

                UsersList = ReadUsersList();

                // _logger.Log(LogLevel.Information, $"UsersList length: {UsersList.Count}");
                // _logger.Log(LogLevel.Information, $"{UsersList[userName].ToString()}");

                if (UsersList != null && UsersList.ContainsKey(userName))
                {
                    if (UsersList[userName].Password == userPassword)
                    {
                        Greeting = "Login successful";
                        CurrentUser = userName; // to be used in 
                        string userAuthenticationToken = GetRandomAlphaNumeric(8);
                        UsersList[CurrentUser].UserAuthenticationToken = userAuthenticationToken;

                        // does not work for dictionary with objects as values 
                        // string jsonString = JsonSerializer.Serialize(UsersList); 

                        string jsonString =
                            JsonConvert.SerializeObject(
                                UsersList); // see: https://www.newtonsoft.com/json/help/html/serializingjson.htm 
                        System.IO.File.WriteAllText(_pathToUsersListFile, jsonString);

                        LoggedIn = true;

                        // _logger.Log(LogLevel.Information, $"User {userName} logged in");

                        HttpContext.Response.Cookies.Append("userName", CurrentUser);
                        HttpContext.Response.Cookies.Append("userAuthenticationToken", userAuthenticationToken);
                    }
                }
                else
                {
                    Greeting = "User " + userName + " is not registered yet";
                }
            }
            else
            {
                Greeting = "Please, enter user login and password";
            }
        }

        if (formAction == "signup")
        {
            if (userName != null && userPassword != null)
            {
                userName = userName.ToLower();
                UsersList = ReadUsersList();
                if (UsersList.ContainsKey(userName))
                {
                    Greeting = $"User name '{userName}' already taken";
                }
                else
                {
                    LoggedIn = true;
                    Greeting = "Now you are registered";
                    CurrentUser = userName; //
                    string userAuthenticationToken = GetRandomAlphaNumeric(8);

                    UsersList.Add(CurrentUser, new AppUser(userPassword, userAuthenticationToken));


                    string jsonString =
                        JsonConvert.SerializeObject(
                            UsersList); // see: https://www.newtonsoft.com/json/help/html/serializingjson.htm 
                    System.IO.File.WriteAllText(_pathToUsersListFile, jsonString);

                    HttpContext.Response.Cookies.Append("userName", CurrentUser);
                    HttpContext.Response.Cookies.Append("userAuthenticationToken", userAuthenticationToken);
                }
            }
            else
            {
                Greeting = "To register please provide user login and password";
            }
        }
    }
}