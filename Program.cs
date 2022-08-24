using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UserDb>(opt=> opt.UseInMemoryDatabase("Users"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseHttpsRedirection();

//list of users 
app.MapGet("/usersList", async (UserDb db) =>
{
    return await db.Users.ToListAsync();
});

//register user 
app.MapPost("/users", async (User user, UserDb db) => {
    if( await db.Users.FirstOrDefaultAsync(x => x.Id == user.Id) != null)
    {
        return Results.BadRequest();
    }

    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created( $"/Users/{user.Id}", user);
});


//login user
app.MapPost("/users/login",[AllowAnonymous] (User user) =>
{
    if (user.Email !=null && user.Password !=null)
    {
        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes
        (builder.Configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        var stringToken = tokenHandler.WriteToken(token);
        return Results.Ok(stringToken);
    }
    return Results.Unauthorized();
});
app.UseAuthentication();
app.UseAuthorization();


app.Run();  //run the app


public class User
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? UserName { get; set; }
    
}

class UserDb : DbContext
{
    
    public UserDb(DbContextOptions<UserDb> options)
        : base(options) { }


    public DbSet<User> Users{ get; set; }

    //public DbSet<User> Users => Set<User>();

}

