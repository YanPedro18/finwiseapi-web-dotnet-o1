using Arch.EntityFrameworkCore.UnitOfWork;
using DataAccess.Context;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebApi.Common;
using WebApi.Extensions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using WebApi.Configurations;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
                .AddDataAnnotationsLocalization()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
//doc api
builder.Services.AddSwaggerGen();

// Ativa o servi�o de localiza��o e define onde est�o os arquivos de recursos (.resx)
// "" significa que os arquivos est�o na raiz da pasta Resources ou padr�o
builder.Services.AddLocalization(options => options.ResourcesPath = "");
// Configura as op��es de localiza��o da requisi��o
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    // Lista de culturas/idiomas suportados pela aplica��o
    var supportedCultures = new List<CultureInfo>
    {
        new CultureInfo(CultureName.PT_BR), // Portugu�s (Brasil)
        new CultureInfo(CultureName.EN_US), // Ingl�s (Estados Unidos)
        new CultureInfo(CultureName.ES_AR)  // Espanhol (Argentina)
    };

    // Define a cultura padr�o da aplica��o (caso o usu�rio n�o especifique)
    options.DefaultRequestCulture = new RequestCulture(
        culture: CultureName.EN_US,    // Cultura para formata��o (datas, n�meros)
        uiCulture: CultureName.EN_US   // Cultura para interface (mensagens, valida��o)
    );

    // Define as culturas suportadas para formata��o
    options.SupportedCultures = supportedCultures;

    // Define as culturas suportadas para interface de usu�rio
    options.SupportedUICultures = supportedCultures;

    // Configura o provedor de cultura para pegar o idioma diretamente da URL
    // Exemplo de URL: /pt-BR/produtos -> usa a cultura pt-BR
    options.RequestCultureProviders = new[]
    {
        new RouteDataRequestCultureProviders
        {
            IndexOfCulture = 1,       // Posi��o da cultura na URL
            IndexofUICulture = 1      // Posi��o da cultura na URL para UI
        }
    };
});

// Configura as op��es de rota da aplica��o
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true; // For�a que todas as URLs sejam em letras min�sculas

    // Adiciona uma restri��o personalizada chamada "culture"
    // Essa restri��o pode validar se a cultura na URL � v�lida (ex: pt-BR, en-US, es-AR)
    options.ConstraintMap.Add("culture", typeof(LanguageRouteConstraint));
});

//Busca a string de conex�o com o banco no appsettings.json
var connectionStringDefault = builder.Configuration.GetConnectionString("CONNECTION_STRING");
//Descobre se o ambiente atual � "Development", "Production" ou outro
var myEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


// L� a configura��o do appsettings.json
var configuration = builder.Configuration;
var databaseProvider = configuration["Logging:DatabaseProvider"];
var connectionString = databaseProvider switch
{
    "SqlServer" => configuration["Logging:ConnectionStrings:SqlServer"],
    "PostgreSQL" => configuration["Logging:ConnectionStrings:PostgreSQL"],
    _ => throw new Exception("Provedor de banco de dados n�o suportado.")
};

var connectionStringPostgreSQL = builder.Configuration.GetConnectionString("PostgreSQLConnection");


// Configura��o condicional para o DbContext
builder.Services.AddDbContext<MainContext>(options =>
{
    if (databaseProvider == "SqlServer")
    {
        options.UseSqlServer(connectionString);
    }
    else if (databaseProvider == "PostgreSQL")
    {
        options.UseNpgsql(connectionString);
    }
    else
    {
        throw new Exception("Provedor de banco de dados n�o suportado.");
    }
})
.AddUnitOfWork<MainContext>(); // Exemplo de adicionar um UnitOfWork



IConfigurationRoot Configuration = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

var appSettings = new AppSettings();
new ConfigureFromConfigurationOptions<AppSettings>(
    Configuration.GetSection("AppSettings"))
        .Configure(appSettings);

builder.Services.AddSingleton(appSettings);
builder.Services.ResolveDependencies();
// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
//builder.Services.AddIdentityConfig(Configuration);
builder.Services.AddMemoryCache(); // Adiciona o servi�o de cache na mem�ria

builder.Services.AddHttpClient("default", client =>
{
    client.Timeout = TimeSpan.FromSeconds(60); // Defina o tempo limite aqui (30 segundos, por exemplo)
});

// JWT e Swagger (iremos configurar na pr�xima etapa)
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseCors(builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());


// Configure the HTTP request pipeline.


app.UseHsts();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();


//'' Configura o middleware de localiza��o
var localizedOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizedOptions.Value);

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
    //endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
    //{
    //    Predicate = _ => true,
    //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    //});
});

app.UseCookiePolicy();

app.Run();
