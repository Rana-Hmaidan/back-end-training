using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//list of users
List<User> UsersList = new List<User>();

//register users
app.MapPost("/users", (HttpContext ctx, [FromBody] NewUserRequest body) =>
{
    //create user 
    User user = new User
    {
        email = body.User.email,
        password = body.User.password,
        username = body.User.username,
        bio = "",
        image = "",
        token = Guid.NewGuid().ToString()
    };
    //check if user is exist
    bool exists = UsersList.Exists((u) => u.email == user.email);
    if (exists)
    {
        ctx.Response.StatusCode = 422;
        return null;
    }
    //add new user to UsersList
    UsersList.Add(user);

    //return response
    var userResponse = new UserResponse { user = user };
    ctx.Response.StatusCode = StatusCodes.Status201Created; // `ctx.Response.StatusCode = 201` works as well
    return userResponse;
});

//login users 
app.MapPost("/users/login", (HttpContext ctx, [FromBody] LoginUserRequest body) =>
{

    //find user from UsersList by email and password
    User? user = UsersList.Find((u) => u.email == body.user.email && u.password == body.user.password);
    if (user == null)
    {
        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return null;
    }

    user.token = Guid.NewGuid().ToString();

    //return response
    UserResponse userResponse = new UserResponse { user = user };

    return userResponse;
});

//Get current user
app.MapGet("/user", (HttpContext ctx, [FromHeader (Name = "Authorization")] string Token) =>
{
    
//get user with current token 
    User? user = UsersList.Find((u) => u.token == Token);
   
    if (user == null)
    {
        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return null;
    }

    //return response
    UserResponse userResponse = new UserResponse { user = user };
    return userResponse;

});

//update current user informations
app.MapPut("/user",(HttpContext ctx,[FromHeader (Name = "Authorization")] string Token,[FromBody] UpdateUserRequest body) =>
{
    //get user with current token 
    User? user = UsersList.Find((u) => u.token == Token);

   //check if user with this token is exists 
    if (user == null)
    {
        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return null;
    }

//update user info
if(body.user.email == null) body.user.email = user.email;
if(body.user.password == null) body.user.password = user.password ;
if(body.user.username == null) body.user.username = user.username;
if(body.user.bio == null) body.user.bio = user.bio;
if(body.user.image == null) body.user.image = user.image;
  
   user = new User
     {
        email = body.user.email,
        password = body.user.password,
        username = body.user.username,
        bio = body.user.bio,
        image = body.user.image,
        token = Token
    };

     //return response 
    var userResponse = new UserResponse { user = user };
    ctx.Response.StatusCode = StatusCodes.Status201Created;
    return userResponse;

});

app.Run();   //run the app


public class NewUser
{
    public string username { get; set; }
    public string email { get; set; }
    public string password { get; set; }
}

public class NewUserRequest
{
    public NewUser User { get; set; }
}

public class User
{
    public string email { get; set; }
    [JsonIgnore]
    public string password { get; set; }
    public string username { get; set; }
    public string bio { get; set; }
    public string image { get; set; }
    public string token { get; set; }
}

public class UserResponse
{
    public User user { get; set; }
}

public class LoginUserRequest
{
    public LoginUser user { get; set; }

}

public class LoginUser
{
    public string email { get; set; }
    public string password { get; set; }
}

public class UpdateUserRequest
{
    public UpdateUser user { get; set; }
}

public class UpdateUser
{
    public string email { get; set; }
    [JsonIgnore]
    public string password { get; set; }
    public string username { get; set; }
    public string bio { get; set; }
    public string image { get; set; }
    public string token { get; set; }
}