using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Security;

var builder = WebApplication.CreateBuilder(args);

// Wy��czenie walidacji certyfikat�w SSL w �rodowisku deweloperskim
if (builder.Environment.IsDevelopment())
{
	ServicePointManager.ServerCertificateValidationCallback +=
		(sender, certificate, chain, sslPolicyErrors) => true;
}

// Dodaj us�ugi MVC
builder.Services.AddControllersWithViews();

// Dodaj konfiguracj� ApplicationDbContext z po��czeniem do bazy danych SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dodaj konfiguracj� uwierzytelniania przy u�yciu ciastek
builder.Services.AddAuthentication("UserCookie")
    .AddCookie("UserCookie", options =>
    {
        options.LoginPath = "/Account/Login";  // Login path
        options.LogoutPath = "/Account/Logout";  // Logout path

        // Optionally, you can also set the DefaultReturnUrl if needed
        options.Events.OnRedirectToLogin = context =>
        {
            // Prevent redirect with ReturnUrl
            context.Response.Redirect("/Account/Login");
            return Task.CompletedTask;
        };
    });


// Dodaj us�ugi autoryzacji
builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// W��czenie uwierzytelniania i autoryzacji
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
