using System.Text;
using Api.Infraestructura;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.OpenApi.Models;


try
{
    DotNetEnv.Env.TraversePath().Load(".env");
}
catch { }

try
{
    DotNetEnv.Env.TraversePath().Load(".env.example");
}
catch { }

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? builder.Configuration["ConnectionStrings__DefaultConnection"]
                       ?? throw new InvalidOperationException("La cadena de conexión 'ConnectionStrings__DefaultConnection' es requerida.");

builder.Services.AddDbContext<MesaSitecDbContext>(options =>
    options.UseSqlite(connectionString));

var frontendUrl = builder.Configuration["FRONTEND_URL"]
                  ?? throw new InvalidOperationException("La variable de entorno 'FRONTEND_URL' es requerida para configurar CORS.");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(frontendUrl)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var jwtSecret = builder.Configuration["JWT_SECRET"]
                ?? throw new InvalidOperationException("La variable de entorno 'JWT_SECRET' es requerida para autenticación JWT.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddSingleton<JwtTokenService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MesaSitec API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type =ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Sembrar
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var dbContext = services.GetRequiredService<MesaSitecDbContext>();
        logger.LogInformation("Verificando existencia del archivo SQLite y esquema de tablas...");
        await dbContext.Database.EnsureCreatedAsync();

        await DbSembrar.SembrarAsync(dbContext, app.Configuration);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Ocurrió un error al inicializar o sembrar la base de datos SQLite.");
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
