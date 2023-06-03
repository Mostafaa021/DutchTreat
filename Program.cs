using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Models;
using DutchTreat.Repositories;
using DutchTreat.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<DutchContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DutchTreatCS")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentity<StoreUser, IdentityRole>(cfg =>
    {
        cfg.User.RequireUniqueEmail = true;
    }).AddEntityFrameworkStores<DutchContext>(); // here to define Identity <User , Role> 

builder.Services.AddAuthentication()
    .AddCookie()
    .AddJwtBearer(config=>
    {
        config.SaveToken = true;
        config.RequireHttpsMetadata = false; // true if use https
        config.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true , 
            ValidateAudience = true ,
            ValidIssuer = builder.Configuration["Tokens:Issuer"],
            ValidAudience =builder.Configuration["Tokens:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Tokens:Key"]))
        };
    }); // here should i assign configuration of  Token Validation Parameters 
builder.Services.AddControllersWithViews() //  servcie to map controller with views 
    .AddRazorRuntimeCompilation() // Compile error to error page if environment not Development
    .AddNewtonsoftJson(cfg => cfg.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);  // to handle self Recursion when order call=>order item call=>product call=>order and so on 
//builder.Services.AddRazorPages(); // Adding service of Razorpages here after adding map razorpages middle ware 
builder.Services.AddTransient<IMailService, NullMailService>(); // Support for Real Mail Service
builder.Services.AddTransient<DutchSeeder>();
builder.Services.AddScoped<IDutchRepository, DutchRepository>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly()); // to use Automapper to current running assembly 
builder.Services.AddMvc();

var app = builder.Build();
// Seeding Between Building WebApp and Running

static void RunSeeding(WebApplication app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using var scope = scopedFactory.CreateScope();
    {
        var service = scope.ServiceProvider.GetService<DutchSeeder>();
        service.SeedAsync().Wait();
    }
}
if (args.Length == 1 && args[0].ToLower() == "/seed")
{
    RunSeeding(app);
}

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Error"); // if not development go to error page 
                                 
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        //app.UseHsts();
    }

    //app.UseHttpsRedirection();
    app.UseStaticFiles(); // only serve files in wwwroot folder
    app.UseRouting();


    app.UseAuthentication(); // after Routing 
    app.UseAuthorization();  // after Authentication 

    //app.MapRazorPages();
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=App}/{action=Index}/{id?}"); // mapping route middleware


    app.Run();
