using Microsoft.EntityFrameworkCore;
using CheckLibrary.Data;
using CheckLibrary.Services;

var builder = WebApplication.CreateBuilder(args);
var connection = builder.Configuration.GetConnectionString("CheckLibraryContext");
builder.Services.AddDbContext<CheckLibraryDbContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));
builder.Services.AddScoped<AuthorService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
