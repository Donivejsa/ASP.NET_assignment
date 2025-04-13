using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjektHantering.Data;
using ProjektHantering.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<DebugService>();

// Lägg till DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Lägg till Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Registrera ProjectService
builder.Services.AddScoped<IProjectService, ProjectService>();

// Lägg till Razor Pages och Controllers för API
builder.Services.AddRazorPages();
builder.Services.AddControllers();

var app = builder.Build();

// Konfigurera HTTP-pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();