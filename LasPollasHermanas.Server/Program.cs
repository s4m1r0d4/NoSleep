using LasPollasHermanas.Server.Models;
using Microsoft.EntityFrameworkCore;
using LasPollasHermanas.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using LasPollasHermanas.Shared;

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
var dildoGroup = app.MapGroup("/dildos").WithParameterValidation();
var authGroup = app.MapGroup("/auth").WithParameterValidation();
#region Entry Points Dildos
// EndPoint
// GET Dildos
dildoGroup.MapGet("/", async (DildoStoreContext context) =>
await context.Dildos.AsNoTracking().ToListAsync());

// GET/Dildo/{id}
dildoGroup.MapGet("/{id}", async (int id, DildoStoreContext context) =>
{
    Dildo? dildo = await context.Dildos.FindAsync(id);
    if (dildo is null) {
        return Results.NotFound();
    }
    return Results.Ok(dildo);
});

// GET /Dildo/search/{query}
dildoGroup.MapGet("/search/{query}", async (string query, DildoStoreContext context) =>
{
    var dildos = await context.Dildos.Where(dildo => dildo.Name != null && dildo.Name.ToLower().Contains(query.ToLower())).ToListAsync();

    if (dildos is null) {
        return Results.NotFound();
    }

    return Results.Ok(dildos);
});

// POST/dildos
dildoGroup.MapPost("/", async (Dildo dildo, DildoStoreContext context) =>
{
    // TODO: Where the F this id comes?
    context.Dildos.Add(dildo);
    await context.SaveChangesAsync();
    return Results.CreatedAtRoute("GetDildo",
    new { id = dildo.Id }, dildo);
}).WithName("GetDildo");

// PUT/dildos/{id}
dildoGroup.MapPut("/{id}", async (int id, Dildo updatedDildo, DildoStoreContext context) =>
{
    var rowsAffected = await context.Dildos.Where(
        dildo => dildo.Id == id)
        .ExecuteUpdateAsync(updates =>
        updates.SetProperty(dildo => dildo.Name, updatedDildo.Name)
               .SetProperty(dildo => dildo.Price, updatedDildo.Price)
               .SetProperty(dildo => dildo.Size, updatedDildo.Size)
               .SetProperty(dildo => dildo.ExpireDate, updatedDildo.ExpireDate)
               .SetProperty(dildo => dildo.Material, updatedDildo.Material)
               .SetProperty(dildo => dildo.Color, updatedDildo.Color)
               .SetProperty(dildo => dildo.Stock, updatedDildo.Stock));

    return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
});

dildoGroup.MapDelete("/{id}", async (int id, DildoStoreContext context) =>
{
    var rowsAffected = await context.Dildos.Where(
        dildo => dildo.Id == id)
        .ExecuteDeleteAsync();
    return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
});
#endregion

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

#endregion

app.Run();
