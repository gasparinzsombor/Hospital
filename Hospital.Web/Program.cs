using Hospital;
using Hospital.Database;
using Hospital.Persistence;
using Hospital.Persistence.Entities;
using Hospital.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddTransient<DoctorService>();
builder.Services.AddTransient<PatientService>();
builder.Services.AddTransient<ReservationService>();
builder.Services.AddTransient<LoginService>();
builder.Services.AddTransient<SpecializationService>();
builder.Services.AddDbContext<PatientAsUserContext>(options =>
{
    var config = builder.Configuration;
    options.UseSqlServer(config.GetConnectionString("SqlServerConnection"));
    options.UseLazyLoadingProxies();
});
builder.Services
    .AddIdentity<Patient, IdentityRole<int>>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = false;
            options.Password.RequiredUniqueChars = 3;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
            options.Lockout.MaxFailedAccessAttempts = 10;
            options.Lockout.AllowedForNewUsers = true;
        })
    .AddEntityFrameworkStores<PatientAsUserContext>()
    .AddDefaultTokenProviders();


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
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

// using(var serviceScope = app.Services.CreateScope())
// {
//     await DbInitializer.Initialize(serviceScope);
// }

app.Run();