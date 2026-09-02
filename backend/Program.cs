using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlabLabs.Api.Data;
using Stripe;
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
app.MapHealthChecks("/health");

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllers();

app.Run();




// Add services to the container.



