using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Security;

var builder = WebApplication.CreateBuilder(args);

// Wy³¹czenie walidacji certyfikatów SSL w œrodowisku deweloperskim
if (builder.Environment.IsDevelopment())
{
	ServicePointManager.ServerCertificateValidationCallback +=
		(sender, certificate, chain, sslPolicyErrors) => true;
}

// Dodaj us³ugi MVC
builder.Services.AddControllersWithViews();

// Dodaj konfiguracjê ApplicationDbContext z po³¹czeniem do bazy danych SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dodaj konfiguracjê uwierzytelniania przy u¿yciu ciastek
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


// Dodaj us³ugi autoryzacji
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

// W³¹czenie uwierzytelniania i autoryzacji
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
