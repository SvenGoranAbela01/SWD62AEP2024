using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext;
using DataAccess.Repositories;
using Domain.Models;
using Presentation.ActionFilters;
using Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AttendanceContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<CustomUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AttendanceContext>();
builder.Services.AddControllersWithViews();

/*
 * AddScoped - 1 instance per user per request
 * AddTransinent - 1 istance per user per request prer call
 * AddSingleton - 1 instance lof StudentsRepository to be shared by all users accessing your website and all requests
 */

builder.Services.AddControllers(options =>
{
    options.Filters.Add<LogsActionFilter>(); // Add the filter to all controllers });
});


builder.Services.AddScoped<StudentsRepository>();
builder.Services.AddScoped<GroupsRepository>();
builder.Services.AddScoped<SubjectsRepository>();
builder.Services.AddScoped<AttendancesRepository>();

//builder.Services.AddScoped<ILogsRepository, LogsDbRepository>();

var logsSetting = 2;

try
{
    logsSetting = builder.Configuration.GetValue<int>("LogsSetting"); // 1 means db; 2 means file, 3 means email, 4 means cloud
} catch
{
    logsSetting = 1;
}

switch (logsSetting)
{
    case 1:
        builder.Services.AddScoped<ILogsRepository, LogsDbRepository>();
        break;
    case 2:
        builder.Services.AddScoped<ILogsRepository, LogsFileRepository>();
        break;
    default:
        builder.Services.AddScoped<ILogsRepository, LogsDbRepository>();
        break;

}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
