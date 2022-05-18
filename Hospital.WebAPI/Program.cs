using Hospital.Persistence;
using Hospital.Persistence.Entities;
using Hospital.WebAPI;
using Hospital.WebAPI.Database;
using Hospital.WebAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<DoctorService>();
builder.Services.AddTransient<MedicalRecordService>();
builder.Services.AddTransient<PatientService>();
builder.Services.AddTransient<ReservationService>();
builder.Services.AddDbContext<DoctorAsUserContext>(options =>
{
    var config = builder.Configuration;
    options.UseSqlServer(config.GetConnectionString("SqlServerConnection"), b => b.MigrationsAssembly("Hospital.WebAPI"));
    options.UseLazyLoadingProxies();
});
builder.Services.AddIdentity<Doctor, IdentityRole<int>>(options =>
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
    .AddEntityFrameworkStores<DoctorAsUserContext>()
    .AddDefaultTokenProviders();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

var serviceScope = app.Services.CreateScope();
await DbInitializer.InitializeAsync(serviceScope);


app.Run();