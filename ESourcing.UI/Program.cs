using ESourcing.Core.Entities;
using ESourcing.Core.Repositories;
using ESourcing.Core.Repositories.Base;
using ESourcing.Infrastructure.Data;
using ESourcing.Infrastructure.Repository;
using ESourcing.Infrastructure.Repository.Base;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddMvc();
builder.Services.AddAuthentication(
        CertificateAuthenticationDefaults.AuthenticationScheme)
        .AddCertificate();
IConfigurationRoot configuration = new ConfigurationBuilder()
   .SetBasePath(Directory.GetCurrentDirectory())
   .AddJsonFile("appsettings.json")
   .Build();
builder.Services.AddDbContext<WebAppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 4;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireDigit = false;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<WebAppDbContext>();
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//                    .AddCookie(options =>
//                    {
//                        options.Cookie.Name = "My Cookie";
//                        options.LoginPath = "Home/Login";
//                        options.LogoutPath = "Home/Logout";
//                        options.ExpireTimeSpan = TimeSpan.FromDays(3);
//                        options.SlidingExpiration = false;
//                    });

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Home/Login";
    options.LogoutPath = $"/Home/Logout";
});
var app = builder.Build();

//builder.Services ==public void ConfigureServices(IServiceCollection services){services.--------}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(name: "Default", pattern: "{controller=Home}/{action=index}/{id?}");
    endpoints.MapRazorPages();
});
app.Run();
