using MediatR;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Infrastructure;
using Application;
using Share;
using Db;
using Db.Libraries;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Template.API", Version = "v1" });
    OpenApiSecurityScheme securityScheme = new()
    {
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", // must be lower case
        BearerFormat = "JWT",
    };

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

// 服務與工廠 (DB的注入服務必須使用AddScoped)
builder.Services.AddScoped<DBContextFactory<EFContext>>();
builder.Services.AddScoped<EFContext>();


builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddShareServices(builder.Configuration);



#region API Cors

// Allow Browser Call APIS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
        });
});

#endregion
var app = builder.Build();
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseAuthentication();
//app.UseAuthorization();
app.UseCors();
app.MapControllers();
//app.UseMiddleware<ExceptionMiddleWare>();
//app.UseMiddleware<AccountValidateMiddleWare>();
app.Run();
