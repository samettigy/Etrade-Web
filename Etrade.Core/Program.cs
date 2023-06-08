using Etrade.DAL.Abstract;
using Etrade.DAL.Concrete;
using Etrade.DAL.Context;
using Etrade.Entity.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//Dependency injection tanýmlamalarý yapýldý
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

//AddIdentity ayarlarý yapýldý
builder.Services.AddIdentity<User, Role>(options =>
{
    //lockout yani kilitlenme süresi ayarlarý deðiþikliði yapýldý.
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

//Cookie ayarlarý yapýldý
builder.Services.ConfigureApplicationCookie(options =>
{
    // giriþ varsa yönlendirilecek yolu belirtiyoruz.
    options.LoginPath = "/Account/SignIn";//giriþ yapýlmadýysa
    options.AccessDeniedPath = "/"; //giriþ yapmýþ ama yetkisi yoksa anasayfada kalsýn dedik
    options.SlidingExpiration = true; // araþtýr oglum okan
    options.ExpireTimeSpan = TimeSpan.FromMinutes(15); // 15 dk sonra otomatik cýkýs yapsýn 
    options.Cookie = new CookieBuilder
    {
        HttpOnly = false, //https ile giriþ yapýlsýn
        SameSite = SameSiteMode.Lax, // araþtýr oðlum okan
        SecurePolicy = CookieSecurePolicy.Always //cookie güvenliði olsun
    };
});

//Add Session ayarlarý yapýlýyor...
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//Use Session ayarlarý yapýlýyor.
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // kullanýcý giriþ yapmýþ mý
app.UseAuthorization();  // kullanýcýnýn yetkisi var mý



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
