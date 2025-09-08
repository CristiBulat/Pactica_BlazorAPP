using BlazorApp.Client.Pages;
using BlazorApp.Components;
using BlazorApp.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!
    .Replace("${MYSQL_ROOT_PASSWORD}", Environment.GetEnvironmentVariable("MYSQL_ROOT_PASSWORD"));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionString));

var app = builder.Build();

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
    .AddAdditionalAssemblies(typeof(BlazorApp.Client.Routes).Assembly); // Use Routes instead of _Imports

app.MapControllers();

app.Run();