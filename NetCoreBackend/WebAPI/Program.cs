using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Business.Abstract;
using Business.DependencyResolvers.Autofac;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using LinqToDB.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Formatting = Formatting.Indented;
        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        options.SerializerSettings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK"; // veya "yyyy-MM-ddTHH:mm:ss.sssZ" gibi ISO 8601 formatı
    });



// Autofac Service Provider Factory'yi kullan
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new AutofacBusinessModule());
});
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers(); // API Controller'lar�n� aktif eder

// CORS Politikas� Tan�mla (Do�ru Kullan�m)
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // Vue URL'si
                  .WithOrigins("https://vue-frontend-x5vs.onrender.com")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Swagger Deste�i Ekleyelim
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

builder.Services.AddCors();


// JWT Authentication yap�land�rmas�
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
        };
    });




builder.Services.AddDependencyResolvers(new ICoreModule[]
{
    new CoreModule()
});

LinqToDBForEFTools.Initialize();

var app = builder.Build();

// Initialize database with migrations
DataAccess.Concrate.EntityFramework.DatabaseInitializer.InitializeDatabase();

// Swagger UI Geli�tirme Ortam�nda Aktif Edildi
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else if (app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{

}

// Hata y�netimi ve g�venlik
if (!app.Environment.IsDevelopment())
{
    app.ConfigureCustomExceptionMiddleware();
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Kimlik do�rulama ve yetkilendirme middleware'leri
app.UseAuthentication();
app.UseAuthorization();

// Health check endpoint
app.MapGet("/health", () => "OK");

app.MapRazorPages();
app.MapControllers(); // API Controller'ları etkinleştirir

app.Run();


