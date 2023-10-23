using Microsoft.EntityFrameworkCore;
using LasPollasHermanas.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using LasPollasHermanas.Shared;
using LasPollasHermanas.Auth;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options => options.AddDefaultPolicy(
    builder =>
    {
        builder.WithOrigins("http://localhost:5289")
        .AllowAnyHeader()
        .AllowAnyMethod();
    }));
var connectionString = builder.Configuration.GetConnectionString("DildoStoreContext");
builder.Services.AddSqlServer<DildoStoreContext>(connectionString);
var app = builder.Build();
app.UseCors();
app.MapGet("/", () => "Hello World!");

var authGroup = app.MapGroup("/auth");

#region Entry Point Auth

authGroup.MapPost("/register/mortaluser", async ([FromBody] MortalUserDTO req, DildoStoreContext context) =>
{
    bool prevAccount = await context.Accounts.AnyAsync(a => a.Email == req.Email);

    if (prevAccount) {
        return Results.BadRequest("Account already exists");
    }

    var account = new Account
    {
        Email = req.Email,
        Password = req.Password,
    };

    var mortalUser = new MortalUser
    {
        Name = req.Name,
        Surname = req.Surname,
        BirthDate = req.BirthDate,
    };

    context.Accounts.Add(account);
    await context.SaveChangesAsync();

    mortalUser.AccountId = account.Id;
    context.MortalUsers.Add(mortalUser);

    await context.SaveChangesAsync();

    return Results.CreatedAtRoute("GetMortalUser",
        new { id = mortalUser.Id }, new MortalUserDTO
        {
            Email = account.Email,
            Password = account.Password,
            Name = mortalUser.Name,
            Surname = mortalUser.Surname,
            BirthDate = mortalUser.BirthDate,
            AccountId = mortalUser.AccountId,
        });
}).WithName("GetMortalUser");

authGroup.MapPost("/login", async ([FromBody] LoginDTO data, DildoStoreContext context) =>
{
    var account = await context.Accounts
        .Include(a => a.AdminUser)
        .Include(a => a.MortalUser)
        .FirstOrDefaultAsync(a => a.Email == data.Email && a.Password == data.Password);

    if (account is null) {
        return Results.BadRequest("Invalid credentials");
    }

    if (account.AdminUser is not null) {
        return Results.Ok(new SessionDTO
        {
            Email = account.Email,
            Name = account.Password,
            Role = "Admin",
        });
    }   

    if (account.MortalUser is not null) {
        return Results.Ok(new SessionDTO
        {
            Email = account.Email,
            Name = account.Password,
            Role = "Mortal",
        });
    }

    return Results.BadRequest("Invalid credentials");
});

authGroup.MapGet("/user/{email}", async (string email, DildoStoreContext context) =>
{
    var mortalUser = await context.MortalUsers.FirstOrDefaultAsync(m => m.Account.Email == email);

    if (mortalUser is null) {
        return Results.NotFound();
    }

    return Results.Ok(mortalUser);
});


#endregion

app.Run();
