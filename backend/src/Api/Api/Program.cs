using Api.Infraestructura;
using Microsoft.EntityFrameworkCore;


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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
app.UseAuthorization();

app.MapControllers();

app.Run();
