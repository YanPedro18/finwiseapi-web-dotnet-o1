using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources
{
    public class ErrorResource
    {
        protected readonly IStringLocalizer<ErrorResource> _localizer;

        //injeção dependencia do resource
        public ErrorResource(IStringLocalizer<ErrorResource> commonResource)
        {
            _localizer = commonResource;
        }


        //Erros

        public string Error01UserNotFound { get { return GetString(nameof(Error01UserNotFound)); } }
        public string Error02UserAlreadyExists { get { return GetString(nameof(Error02UserAlreadyExists)); } }
        public string Error03TransactionNotFound { get { return GetString(nameof(Error03TransactionNotFound)); } }
        public string Error04TransactionFailed { get { return GetString(nameof(Error04TransactionFailed)); } }
        protected string GetString(string name) =>
           _localizer.GetString(name);
    }
}
