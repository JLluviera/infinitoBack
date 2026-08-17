using infinitoBack.Data;
using infinitoBack.Interfaces;
using infinitoBack.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de Base de Datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. CORS para tu front en Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 3. Controladores y Filtro de Autorización Global
builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

// 4. OPENAPI NATIVO (.NET 10 / 11)
// Magia pura: detecta tu seguridad y la aplica sin usar OpenApi.Models
builder.Services.AddEndpointsApiExplorer();

// 5. Configuración de Autenticación y JWT
// Al definir esto bien, AddOpenApi() lo lee y genera el esquema Bearer automáticamente
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),

        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// 2. Generador nativo de OpenAPI de ASP.NET Core 10
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, ct) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        // Definimos el esquema "Bearer"
        document.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            In = ParameterLocation.Header,
            BearerFormat = "JWT",
            Description = "Introduce tu token JWT aquí."
        });

        document.Security ??= new List<OpenApiSecurityRequirement>();

        // Forma nativa moderna de referenciar esquemas en .NET 10 
        document.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        });

        return Task.CompletedTask;
    });
});

// 6. Tus Servicios
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// ==========================================
// PIPELINE HTTP
// ==========================================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        // La UI de Scalar te permite cambiar el título acá sin tocar OpenAPI
        options.WithTitle("Mi API Angular");
        options.WithTheme(ScalarTheme.DeepSpace);
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();