using System.Text;
using System.Text.Json.Serialization;
using Application.Api.Data;
using Application.Api.Installers;
using Application.Api.Models;
using Application.Api.Services.Account;
using Application.Api.Services.Authentication;
using Application.Api.Services.Data;
using Application.Api.Services.Generators;
using Application.Api.Services.MailingService;
using Application.Api.Services.Newsletter;
using Application.Api.Services.Orders;
using Application.Api.Services.Products;
using Application.Api.Services.Redis;
using Application.SMTP;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AuthenticationService = Application.Api.Services.Authentication.AuthenticationService;
using IAuthenticationService = Application.Api.Services.Authentication.IAuthenticationService;

var builder = WebApplication.CreateBuilder(args);

var settings = new Settings();
builder.Configuration.Bind("Settings", settings);
builder.Services.AddSingleton(settings);

// Add services to the container.
builder.Services.AddControllers();

// Add Db access
builder.Services.AddDbContext<ApplicationContext>(o 
    => o.UseNpgsql(builder.Configuration.GetConnectionString("DbString")));

// Add Services
#region API_Services

builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderBuilder, OrderBuilder>();
builder.Services.AddScoped<IAccountActionsService, AccountActionsService>();

builder.Services.AddScoped<ILoginHelper, LoginHelper>();
builder.Services.AddScoped<IFileGenerator, PdfGenerator>();

#endregion

#region Mailing_Services

builder.Services.AddScoped<IMailBuilder, MailBuilder>();
builder.Services.AddTransient<IMailingService, MailingService>();
builder.Services.AddTransient<INewsletterService, NewsletterService>();

#endregion

// Sometimes this should be added, cuz sometimes somehow it cant serialize json
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Redis
IInstaller cacheInstaller = new CacheInstaller();
cacheInstaller.InstallServices(builder.Services, builder.Configuration);

var isDevelopment = string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "development", StringComparison.InvariantCultureIgnoreCase);

Console.WriteLine($"Development: {isDevelopment}");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => 
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.BearerKey)),
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();