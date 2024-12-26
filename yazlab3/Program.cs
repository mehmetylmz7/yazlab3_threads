using Microsoft.EntityFrameworkCore;
using yazlab3.Controllers;
using yazlab3.Controllers.LogController;
using yazlab3.Models;

var builder = WebApplication.CreateBuilder(args);

// Servisler buraya eklenir

builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Session'� eklemek i�in gerekli servisler
builder.Services.AddDistributedMemoryCache(); // Bellek tabanl� cache
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout s�resi
    options.Cookie.HttpOnly = true; // G�venlik i�in �erezleri sadece HTTP �zerinden eri�ilebilir yap
    options.Cookie.IsEssential = true; // �erezin temel bir �erez oldu�unu belirt
});

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<Context>();



var app = builder.Build(); // Build i�lemi servisten sonra yap�lmal�

// Middleware'ler burada tan�mlan�r
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Session middleware'i ekle
app.UseSession();

app.UseAuthorization();
app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{action=OrderList}/{id?}",
    defaults: new { controller = "Admin" }
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
