using Etrade.DAL.Abstract;
using Etrade.DAL.Concrete;
using Etrade.DAL.Context;
using Etrade.Entity.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//Dependency injection tan�mlamalar� yap�ld�
builder.Services.AddDbContext<EtradeDbContext>();
builder.Services.AddScoped<ICategoryDAL, CategoryDAL>();
builder.Services.AddScoped<IProductDAL, ProductDAL>();
builder.Services.AddScoped<IOrderDAL, OrderDAL>();
builder.Services.AddScoped<IOrderLineDAL, OrderLineDAL>();

//AddAuthentication - google 
builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "647254113054-s5herhn6e8pc93oeb33vuhs0g840ncok.apps.googleusercontent.com";
    options.ClientSecret = "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";
});

//AddIdentity ayarlar� yap�ld�
builder.Services.AddIdentity<User, Role>(options =>
{
    //lockout yani kilitlenme s�resi ayarlar� de�i�ikli�i yap�ld�.
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    //password 
}).AddEntityFrameworkStores<EtradeDbContext>().AddDefaultTokenProviders();

//Cookie ayarlar� yap�ld�
builder.Services.ConfigureApplicationCookie(options =>
{
    // giri� varsa y�nlendirilecek yolu belirtiyoruz.
    options.LoginPath = "/Account/SignIn";//giri� yap�lmad�ysa
    options.AccessDeniedPath = "/"; //giri� yapm�� ama yetkisi yoksa anasayfada kals�n dedik
    options.SlidingExpiration = true; // ara�t�r oglum okan
    options.ExpireTimeSpan = TimeSpan.FromMinutes(15); // 15 dk sonra otomatik c�k�s yaps�n 
    options.Cookie = new CookieBuilder
    {
        HttpOnly = false, //https ile giri� yap�ls�n
        SameSite = SameSiteMode.Lax, // ara�t�r o�lum okan
        SecurePolicy = CookieSecurePolicy.Always //cookie g�venli�i olsun
    };
});

//Add Session ayarlar� yap�l�yor...
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//Use Session ayarlar� yap�l�yor.
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // kullan�c� giri� yapm�� m�
app.UseAuthorization();  // kullan�c�n�n yetkisi var m�



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();
