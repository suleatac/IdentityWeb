using IdentityWeb.CustomValidations;
using IdentityWeb.Localization;
using IdentityWeb.Models;
using IdentityWeb.OptionsModels;
using IdentityWeb.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});




builder.Services.AddIdentity<AppUser, AppRole>(options => {

    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuvwxyz1234567890_";

    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = true;

    //Locklama sistemi ayarlarý bu kýsým

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
    options.Lockout.MaxFailedAccessAttempts = 3;

   
})
.AddPasswordValidator<PasswordValidator>()
.AddUserValidator<UserValidator>()
.AddErrorDescriber<LocalizationIdentityErrorDescriber>()
.AddDefaultTokenProviders()
.AddEntityFrameworkStores<AppDbContext>();







builder.Services.ConfigureApplicationCookie(opt => {
    var cookieBuilder=new CookieBuilder();
    cookieBuilder.Name = "IdentiyWeb";
    opt.LoginPath= new PathString("/Home/SignIn");
    opt.Cookie= cookieBuilder;
    opt.ExpireTimeSpan= TimeSpan.FromDays(30);
    opt.SlidingExpiration = true;

    //Logout Path
    opt.LogoutPath= new PathString("/Member/LogOut");
});



//Email Ayarlarý bu kýsým
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt => {
    opt.TokenLifespan = TimeSpan.FromHours(2);
});
builder.Services.Configure<EmailOption>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<IEmailService, EmailService>();



//Kullanýcý resim ekleme iþlemleri
//Klasöre eriþim için referans noktasý belirlendi
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));






var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();




app.Run();
