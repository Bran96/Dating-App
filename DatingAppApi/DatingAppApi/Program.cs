using DatingAppApi.Data;
using DatingAppApi.Extensions;
using DatingAppApi.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Extension class
builder.Services.AddApplicationServices(builder.Configuration);
// Step 1: Authentication middleware
// Extension class
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 2ND STEP ENABLING CORS FOR ANGULAR 
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("https://localhost:4200"));

// Step 2: Authentication Middleware
// We need these 2 when using JWT 
// They ask 2 different questions:
app.UseAuthentication(); // Do you have a valid Token ?
app.UseAuthorization(); // Okay, you have a valid token, now what are you allowed to do

app.MapControllers();

// Seeding Data into the database
// And if we delete the database and rerun the backend the database will be returned, but this time with the seeding data that we provide in the database
using var scope = app.Services.CreateScope(); // This is gonna give us access to all the services that we have in this program.cs class
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
}
catch(Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during Migration");
}

app.Run();
