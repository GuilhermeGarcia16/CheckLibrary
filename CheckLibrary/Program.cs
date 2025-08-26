using Microsoft.EntityFrameworkCore;
using CheckLibrary.Data;
using CheckLibrary.Services;

var builder = WebApplication.CreateBuilder(args);
var connection = builder.Configuration.GetConnectionString("CheckLibraryContext");
builder.Services.AddDbContext<CheckLibraryDbContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));
//Service Scoped
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<AccountService>();
//Service Cache
builder.Services.AddDistributedMemoryCache();
//Service Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);  // Set session timeout
    options.Cookie.HttpOnly = true; // Ensures the session cookie is accessible only by the server
});

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

app.UseSession();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
