
using TeleHealthLite.Web.Interfaces;
using TeleHealthLite.Web.Repositories;
using Microsoft.EntityFrameworkCore;
using TeleHealthLite.Web.Data;
using TeleHealthLite.Web.Filters;
using TeleHealthLite.Web.Services;
using TeleHealthLite.Web.Events;

var builder = WebApplication.CreateBuilder(args);

// Register MVC services and the custom logging action filter
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<LoggingActionFilter>();
});

// Register patient repository and service
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();

// Register consultation repository and service
builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
builder.Services.AddScoped<IConsultationService, ConsultationService>();

// Register prescription repository
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();

// Register the prescription event logger
builder.Services.AddScoped<PrescriptionEventLogger>();

// Register prescription service and subscribe the event logger
builder.Services.AddScoped<IPrescriptionService>(serviceProvider =>
{
    var service = new PrescriptionService(
        serviceProvider.GetRequiredService<IPrescriptionRepository>());

    var logger = serviceProvider.GetRequiredService<PrescriptionEventLogger>();

    service.PrescriptionCreated += logger.OnPrescriptionCreated;

    return service;
});

// Register follow-up repository and service
builder.Services.AddScoped<IFollowUpRepository, FollowUpRepository>();
builder.Services.AddScoped<IFollowUpService, FollowUpService>();

// Register the custom action filter
builder.Services.AddScoped<LoggingActionFilter>();

// Configure session tracking
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure SQL Server connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Build application
var app = builder.Build();

// Configure error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable session
app.UseSession();

app.UseAuthorization();

// Custom route with integer constraint
app.MapControllerRoute(
    name: "consultation-details",
    pattern: "consultations/details/{id:int}",
    defaults: new
    {
        controller = "Consultation",
        action = "Details"
    });

// Default MVC route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
