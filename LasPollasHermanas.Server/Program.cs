using LasPollasHermanas.Server.Models;
using Microsoft.EntityFrameworkCore;
using LasPollasHermanas.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Internal;

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
    if (dildo is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(dildo);
});

// GET /Dildo/search/{query}
dildoGroup.MapGet("/search/{query}", async (string query, DildoStoreContext context) =>
{
    var dildos = await context.Dildos.Where(dildo => dildo.Name != null && dildo.Name.ToLower().Contains(query.ToLower())).ToListAsync();

    if (dildos is null)
    {
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

// POST/dildos/buy/{dildoId}
dildoGroup.MapPost("/buy/{dildoId}", async ([FromRoute] int dildoId, [FromBody] string userEmail, DildoStoreContext context) =>
{
    var user = await context.MortalUsers
    .Include(u => u.BoughtDildos)
    .Include(u => u.Account)
    .FirstOrDefaultAsync(u => userEmail == u.Account.Email);
    if (user == null) return Results.NotFound();

    var dildo = await context.Dildos.FindAsync(dildoId);

    if (dildo == null) return Results.NotFound();

    // if (user.BoughtDildos == null) {
    //     user.BoughtDildos = new List<Dildo>{
    //         dildo
    //     };
    // } else {
    //     user.BoughtDildos.Add(dildo);
    // }
    var boughtDildo = new BoughtDildo
    {
        DildoId = dildo.Id,
        UserId = user.Id
    };

    await context.BoughtDildos.AddAsync(boughtDildo);
    int rows = await context.SaveChangesAsync();

    return rows == 0 ? Results.Problem() : Results.NoContent();
});

// GET/dildos/history/{userEmail}
dildoGroup.MapGet("/history/{userEmail}", async (string userEmail, DildoStoreContext context) =>
{
    var user = await context.MortalUsers
        .Include(u => u.BoughtDildos)
        .FirstOrDefaultAsync(u => userEmail == u.Account.Email);

    if (user == null)
    {
        return Results.NotFound();
    }

    var boughtDildos = await context.Dildos.Join(
        context.BoughtDildos,
        dildo => dildo.Id,
        boughtDildo => boughtDildo.DildoId,
        (dildo, boughtDildo) => dildo).ToListAsync();
    return Results.Ok(boughtDildos);
});

dildoGroup.MapDelete("/{id}", async (int id, DildoStoreContext context) =>
{
    var rowsAffected = await context.Dildos.Where(
        dildo => dildo.Id == id)
        .ExecuteDeleteAsync();
    return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
});

// DELETE/dildos/select/{userEmail}
// dildoGroup.MapDelete("/select/{dildoId}/{userEmail}", async ([FromRoute] int dildoId, [FromRoute] string userEmail, DildoStoreContext context) =>
// {
//     var user = await context.MortalUsers
//         .Include(u => u.CurrentDildos)
//         .FirstOrDefaultAsync(u => userEmail == u.Account.Email);

//     if (user == null)
//     {
//         return Results.NotFound();
//     }

//     if (user.CurrentDildos == null)
//     {
//         return Results.NoContent();
//     }

//     int index = user.CurrentDildos.FindIndex(dildo => dildo.Id == dildoId);
//     if (index == -1)
//     {
//         return Results.NotFound();
//     }
//     user.CurrentDildos.RemoveAt(index);

//     await context.SaveChangesAsync();
//     return Results.NoContent();
// });


// POST/dildos/buy/{dildoId}
// dildoGroup.MapPost("/buy/{dildoId}", async ([FromRoute] int dildoId, [FromBody] string userEmail, DildoStoreContext context) =>
// {
//     var user = await context.MortalUsers
//     .Include(u => u.CurrentDildos)
//     .Include(u => u.Account)
//     .FirstOrDefaultAsync(u => userEmail == u.Account.Email);

//     if (user == null)
//     {
//         return Results.NotFound();
//     }

//     var dildo = await context.Dildos.FindAsync(dildoId);

//     if (dildo == null)
//     {
//         return Results.NotFound();
//     }

//     if (user.BoughtDildos == null) {
//         user.BoughtDildos = new List<Dildo>{ dildo };
//     } else {
//         user.BoughtDildos.Add(dildo);
//     }

//     await context.SaveChangesAsync();
//     return Results.NoContent();
// });

#endregion

app.Run();
