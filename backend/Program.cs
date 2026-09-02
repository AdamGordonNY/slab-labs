using System.Text;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SlabLabs.Api.Data;
using SlabLabs.Api.Services;
using Stripe;
using System.IdentityModel.Tokens.Jwt;
var builder = WebApplication.CreateBuilder(args);
var stripeSecretKey = builder.Configuration["Stripe:SecretKey"];
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJs", policy =>
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod());
});



builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddHealthChecks();

builder.Services.AddScoped<JWTService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});
builder.Services.AddAuthorization();
var app = builder.Build();
app.MapHealthChecks("/health");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    builder.WebHost.UseContentRoot(AppContext.BaseDirectory);
    // Remove file watchers
    foreach (var source in builder.Configuration.Sources.OfType<FileConfigurationSource>())
    {
        source.ReloadOnChange = false;
    }
}

app.UseHttpsRedirection();

app.UseCors("AllowNextJs");



// Later in the pipeline:
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();




// Add services to the container.



