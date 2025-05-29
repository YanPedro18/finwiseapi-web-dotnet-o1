using Microsoft.AspNetCore.Http; // Permite acessar a requisição HTTP
using Microsoft.AspNetCore.Localization; // Trabalha com localização e cultura
using System;
using System.Threading.Tasks;

namespace WebApi.Extensions
{
    // Classe customizada que determina a cultura baseada na URL
    public class RouteDataRequestCultureProviders : RequestCultureProvider
    {
        // Índice da cultura na URL (ex: /pt-BR/home => pt-BR está na posição 1)
        public int IndexOfCulture;

        // Índice da cultura da interface (normalmente igual ao IndexOfCulture)
        public int IndexofUICulture;

        // Método que determina qual cultura será usada
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            // Se o contexto da requisição estiver nulo, lança exceção
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            string culture = null;
            string uiCulture = null;

            // Pega o caminho da URL e separa pelos slashes '/'
            // Exemplo: "/pt-BR/home" -> ["", "pt-BR", "home"]
            culture = uiCulture = httpContext.Request.Path.Value.Split('/')[IndexOfCulture]?.ToString();

            // Cria um resultado de cultura com os valores extraídos da URL
            var providerResultCulture = new ProviderCultureResult(culture, uiCulture);

            // Retorna a cultura como um Task (por ser async)
            return Task.FromResult(providerResultCulture);
        }
    }
}
