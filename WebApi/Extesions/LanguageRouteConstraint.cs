using Microsoft.AspNetCore.Http;          // Usado para acessar dados da requisição HTTP
using Microsoft.AspNetCore.Routing;       // Necessário para criar restrições de rota
using WebApi.Common;                      // Onde estão definidos os nomes das culturas suportadas (PT_BR, EN_US, ES_AR)

namespace WebApi.Extensions
{
    // Classe que define uma restrição personalizada para rotas
    public class LanguageRouteConstraint : IRouteConstraint
    {
        // Método que é chamado automaticamente para verificar se a rota é válida
        public bool Match(
            HttpContext httpContext,              // Informações da requisição atual
            IRouter route,                        // Roteador usado
            string routeKey,                      // Nome da chave na rota (ex: "culture")
            RouteValueDictionary values,          // Dicionário com os valores das variáveis na URL
            RouteDirection routeDirection         // Direção da rota (entrada ou geração)
        )
        {
            // Se a URL não tiver o parâmetro "culture", retorna false (rota inválida)
            if (!values.ContainsKey("culture"))
                return false;

            // Pega o valor da cultura da URL
            var culture = values["culture"].ToString();

            // Verifica se o valor da cultura está entre os idiomas suportados
            return culture == CultureName.EN_US ||
                   culture == CultureName.PT_BR ||
                   culture == CultureName.ES_AR;
        }
    }
}
