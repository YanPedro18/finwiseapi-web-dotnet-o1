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

// Ativa o serviço de localização e define onde estão os arquivos de recursos (.resx)
// "" significa que os arquivos estão na raiz da pasta Resources ou padrão
builder.Services.AddLocalization(options => options.ResourcesPath = "");
// Configura as opções de localização da requisição
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    // Lista de culturas/idiomas suportados pela aplicação
    var supportedCultures = new List<CultureInfo>
    {
        new CultureInfo(CultureName.PT_BR), // Português (Brasil)
        new CultureInfo(CultureName.EN_US), // Inglês (Estados Unidos)
        new CultureInfo(CultureName.ES_AR)  // Espanhol (Argentina)
    };

    // Define a cultura padrão da aplicação (caso o usuário não especifique)
    options.DefaultRequestCulture = new RequestCulture(
        culture: CultureName.EN_US,    // Cultura para formatação (datas, números)
        uiCulture: CultureName.EN_US   // Cultura para interface (mensagens, validação)
    );

    // Define as culturas suportadas para formatação
    options.SupportedCultures = supportedCultures;

    // Define as culturas suportadas para interface de usuário
    options.SupportedUICultures = supportedCultures;

    // Configura o provedor de cultura para pegar o idioma diretamente da URL
    // Exemplo de URL: /pt-BR/produtos -> usa a cultura pt-BR
    options.RequestCultureProviders = new[]
    {
        new RouteDataRequestCultureProviders
        {
            IndexOfCulture = 1,       // Posição da cultura na URL
            IndexofUICulture = 1      // Posição da cultura na URL para UI
        }
    };
});

// Configura as opções de rota da aplicação
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true; // Força que todas as URLs sejam em letras minúsculas

    // Adiciona uma restrição personalizada chamada "culture"
    // Essa restrição pode validar se a cultura na URL é válida (ex: pt-BR, en-US, es-AR)
    options.ConstraintMap.Add("culture", typeof(LanguageRouteConstraint));
});

//Busca a string de conexão com o banco no appsettings.json
var connectionStringDefault = builder.Configuration.GetConnectionString("CONNECTION_STRING");
//Descobre se o ambiente atual é "Development", "Production" ou outro
var myEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


// Lê a configuração do appsettings.json
var configuration = builder.Configuration;
var databaseProvider = configuration["Logging:DatabaseProvider"];
var connectionString = databaseProvider switch
{
    "SqlServer" => configuration["Logging:ConnectionStrings:SqlServer"],
    "PostgreSQL" => configuration["Logging:ConnectionStrings:PostgreSQL"],
    _ => throw new Exception("Provedor de banco de dados não suportado.")
};

var connectionStringPostgreSQL = builder.Configuration.GetConnectionString("PostgreSQLConnection");


// Configuração condicional para o DbContext
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
        throw new Exception("Provedor de banco de dados não suportado.");
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
builder.Services.AddMemoryCache(); // Adiciona o serviço de cache na memória

builder.Services.AddHttpClient("default", client =>
{
    client.Timeout = TimeSpan.FromSeconds(60); // Defina o tempo limite aqui (30 segundos, por exemplo)
});

// JWT e Swagger (iremos configurar na próxima etapa)
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


//'' Configura o middleware de localização
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
