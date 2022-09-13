using System;
using System.Linq;
using System.Web;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace SomeApiController.Controllers
{
    [ApiController]
    public class UsersController : Controller
    {
        //list of users
        public static List<User> UsersList = new List<User>();
       
        //create user  //route : /users  //POST
        [HttpPost]
        [Route("users")]
        public ActionResult Create([FromBody] NewUserRequest body)
        {
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
                Response.StatusCode = 422;
                return NotFound();
            }
            //add new user to UsersList
            UsersList.Add(user);
            //return response
            var userResponse = new UserResponse { user = user };
            return Ok(userResponse);
        }

        //login users   //route :/users/login  //POST
        [HttpPost]
        [Route("users/login")]
        public ActionResult LogIn([FromBody] LoginUserRequest body)
        {
            if(body==null)
            {
                return BadRequest("Invalid client request");
            }
            //find user from UsersList by email and password  
            User? user = UsersList.Find((u) => u.email == body.user.email && u.password == body.user.password);

            if (user == null)
            {
                return NotFound();
            }

            user.token = Guid.NewGuid().ToString();

            //return response
            UserResponse userResponse = new UserResponse { user = user };
            return Ok(userResponse);
        }

        //Get current user //route:/user // GET
        [HttpGet]
        [Route("user")]
        public ActionResult Current([FromHeader(Name = "Authorization")] string Token)
        {
            //get user with current token 
            User? user = UsersList.Find((u) => u.token == Token);

            if (user == null)
            {
                return NotFound();
            }
            //return response
            UserResponse userResponse = new UserResponse { user = user };
            return Ok(userResponse);
        }

        //update current user informations
        [HttpPut]
        [Route("user")]
        public ActionResult Update([FromHeader(Name = "Authorization")] string Token,[FromBody] UpdateUserRequest body)
        {
            //get user with current token 
            User? user = UsersList.Find((u) => u.token == Token);
           
            if (user == null)
            {
                return NotFound();
            }

            //check data             
            if(body.user.email == null) body.user.email = user.email;
            if(body.user.password == null) body.user.password = user.password ;
            if(body.user.username == null) body.user.username = user.username;
            if(body.user.bio == null) body.user.bio = user.bio;
            if(body.user.image == null) body.user.image = user.image;

            //update user info
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
            return Ok(userResponse);

        }
        
    }
}

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
