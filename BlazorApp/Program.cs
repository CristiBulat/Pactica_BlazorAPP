using BlazorApp.Client.Pages;
using BlazorApp.Components;
using BlazorApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using BlazorApp.Domain.Entities;
using DotNetEnv;

var solutionRoot = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName;
var envFilePath = Path.Combine(solutionRoot, ".env");

if (File.Exists(envFilePath))
{
    Env.Load(envFilePath);
}
else
{
    Env.Load();
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5271") });

builder.Services.AddControllers();

var password = Environment.GetEnvironmentVariable("MYSQL_ROOT_PASSWORD");

Console.WriteLine($"Found password: '{password}'");
if (string.IsNullOrEmpty(password))
{
    Console.WriteLine("ERROR: MYSQL_ROOT_PASSWORD is not set. Make sure your .env file is in the project's root directory and contains the MYSQL_ROOT_PASSWORD variable.");
    return;
}

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!
    .Replace("${MYSQL_ROOT_PASSWORD}", password);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionString));

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

    // Look for any products.
    if (!context.Products.Any())
    {
        context.Products.AddRange(
            new Product { Name = "Product 1", Price = 10.99m, Description = "This is product 1" },
            new Product { Name = "Product 2", Price = 20.49m, Description = "This is product 2" },
            new Product { Name = "Product 3", Price = 5.75m, Description = "This is product 3" }
        );
        context.SaveChanges();
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorApp.Client.Routes).Assembly);

app.MapControllers();

app.Run();