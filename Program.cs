using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Motosfera.Data.DbContext;
using Motosfera.Data.Seeder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MotosferaContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Connection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<MotosferaContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
await SeedData.SeedAdminUser(services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Post}/{action=Index}/{id?}");

app.Run();
