using DatingAppApi.Data;
using DatingAppApi.Extensions;
using DatingAppApi.Interfaces;
using DatingAppApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 2ND STEP ENABLING CORS FOR ANGULAR 
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

// Step 2: Authentication Middleware
// We need these 2 when using JWT 
// They ask 2 different questions:
app.UseAuthentication(); // Do you have a valid Token ?
app.UseAuthorization(); // Okay, you have a valid token, now what are you allowed to do

app.MapControllers();

app.Run();
