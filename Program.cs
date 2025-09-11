using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API.Data;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;
using API.Functions;
using API.Services;
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

builder.Services.AddDbContext<MySQLDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySQLConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySQLConnection")))
); ;
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services
    .AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

builder.Services.AddHttpClient<MyFuncions>(client =>
{
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Configuración de servicios adicionales
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<MyService>();
builder.Services.AddScoped<StorageService>();
builder.Services.AddScoped<MyFuncions>();

builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("private", new OpenApiInfo { Title = "Private API", Version = "v1" });
    c.SwaggerDoc("public", new OpenApiInfo { Title = "Public API", Version = "v1" });


    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor, ingrese 'Bearer' seguido de un espacio y el token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Filtro para incluir solo los controladores específicos en cada documento
    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        if (docName == "public")
        {
            // Incluye solo los endpoints que no requieren autorización
            return !apiDesc.CustomAttributes().Any(attr => attr is AuthorizeAttribute);
        }
        else if (docName == "private")
        {
            // Incluye solo los endpoints que requieren autorización
            return apiDesc.CustomAttributes().Any(attr => attr is AuthorizeAttribute);
        }
        return false;
    });

});
builder.Logging.AddDebug().AddConsole();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/private/swagger.json", "Private API");
        c.SwaggerEndpoint("/swagger/public/swagger.json", "Public API");
        c.RoutePrefix = "swagger";
        c.OAuthClientId("swagger"); // Si usas OAuth
        c.OAuthAppName("Swagger API");
    });
//}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
