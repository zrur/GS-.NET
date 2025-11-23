using HelpLink.Application.Configuration;
using HelpLink.Application.Services;
using HelpLink.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ============ LOGGING (Serilog) ============
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/helplink-.log", 
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// ============ JWT CONFIGURATION ============
var jwtSettings = new JwtSettings();
builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// ============ SERVICES ============
builder.Services.AddScoped<IAuthService, AuthService>();

// ============ TRACING & METRICS (OpenTelemetry) ============
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .AddSource("HelpLink.API")
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService("HelpLink.API", serviceVersion: "1.0.0"))
            .AddAspNetCoreInstrumentation(options =>
            {
                options.RecordException = true;
            })
            .AddHttpClientInstrumentation()
            .AddConsoleExporter();
    })
    .WithMetrics(metricsProviderBuilder =>
    {
        metricsProviderBuilder
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter();
    });

// ============ HEALTH CHECKS ============
builder.Services.AddHealthChecks()
    .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("API is running"));

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ============ SWAGGER COM JWT ============
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HelpLink - API de Doações",
        Version = "v1.0.0",
        Description =
            @"## 🌟 Plataforma de Doações e Gestão Social

**Conectando quem precisa com quem pode ajudar**

---

### Como Começar

1. Faça login em `/api/Auth/login` com:
   - `admin@helplink.com` | `Admin@123`
2. Copie o token retornado
3. Clique no botão 🔒 **Authorize** acima
4. Cole o token no formato: `Bearer SEU_TOKEN_AQUI`
5. Explore os endpoints e teste as funcionalidades!

---

💡 **Dica:** Use os endpoints de **Health Check** para monitorar o status da API em tempo real!",

        Contact = new OpenApiContact
        {
            Name = "Equipe HelpLink",
            Email = "contato@helplink.com",
            Url = new Uri("https://github.com/helplink/api")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    
    // Habilita comentários XML para documentação
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    // Configurar JWT no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = @"<div style='font-family: monospace; background: #f1f5f9; padding: 12px; border-radius: 6px; margin: 8px 0;'>
            <p style='margin: 0 0 8px 0; color: #334155;'><strong>🔐 Autenticação JWT</strong></p>
            <p style='margin: 0 0 4px 0; color: #64748b;'>1. Faça login em <code>/api/Auth/login</code></p>
            <p style='margin: 0 0 4px 0; color: #64748b;'>2. Copie o valor do campo 'token' da resposta</p>
            <p style='margin: 0 0 8px 0; color: #64748b;'>3. Cole aqui no formato: <code style='background: #e2e8f0; padding: 2px 4px; border-radius: 3px;'>Bearer SEU_TOKEN</code></p>
            <p style='margin: 0; color: #ef4444;'><strong>⚠️</strong> Não esqueça da palavra 'Bearer' seguida de um espaço!</p>
            </div>"
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
});

builder.Services.AddDbContext<HelpLinkDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection"))
        .AddInterceptors(new HelpLink.Infrastructure.Interceptors.OracleCommandInterceptor()));
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "🤝 HelpLink API v1.0");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "HelpLink - API de Doações";
    
    // Personalizações estéticas
    c.InjectStylesheet("/swagger-ui/custom.css");
    c.DefaultModelsExpandDepth(-1); // Oculta models por padrão
    c.DefaultModelExpandDepth(1);
    c.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    c.EnableFilter();
    c.ShowExtensions();
    c.EnableValidator();
    c.SupportedSubmitMethods(Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Get, 
                           Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Post, 
                           Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Put, 
                           Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Delete);
    
    // CSS customizado inline como fallback
    c.InjectJavascript("/swagger-ui/custom.js");
});

// ============ HEALTH CHECK ENDPOINTS ============
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// ============ SERVIR ARQUIVOS ESTÁTICOS PARA SWAGGER ============
app.UseStaticFiles();

// ============ AUTENTICAÇÃO E AUTORIZAÇÃO ============
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ============ INICIALIZAÇÃO DO BANCO DE DADOS ============
// DbInitializer temporariamente desabilitado devido a problema bool/int
/*
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<HelpLinkDbContext>();
    DbInitializer.Initialize(context);
    Log.Information("✅ Banco de dados inicializado com dados de teste");
}
*/

Log.Information("🚀 HelpLink API iniciada com JWT + Observabilidade!");
Log.Information("🔐 Autenticação JWT ativa");
Log.Information("🔍 Health Check: /health");
Log.Information("📚 Swagger: /swagger");
Log.Information("💡 Login: POST /api/Auth/login");
Log.Information("   Email: admin@helplink.com | Password: Admin@123");
app.MapGet("/", () => Results.Ok(new
{
    status = "online",
    api = "HelpLink API",
    version = "1.0.0",
    swagger = "/swagger",
    health = "/health"
}));

app.Run();

public partial class Program { }
