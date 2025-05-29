using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources
{
    public class CommonResource
    {
        //abstration do microsoft para resource
        protected readonly IStringLocalizer<CommonResource> _localizer;

        //injeção dependencia do resource
        public CommonResource(IStringLocalizer<CommonResource> commonResource)
        {
            _localizer = commonResource;
        }

        //método para obter a mensagem de erro
        public string loginSucesso { get { return GetString(nameof(loginSucesso)); } }



        //Novos Resources da API FinWise

        //Sucessos
        public string SuccessUserValid { get { return GetString(nameof(SuccessUserValid)); } }
        public string SuccessUserCreated { get { return GetString(nameof(SuccessUserCreated)); } }

        public string SuccessTransactionCreated { get { return GetString(nameof(SuccessTransactionCreated)); } }



        protected string GetString(string name) =>
           _localizer.GetString(name);
    }
}
