using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Start.Blog.Extensions
{
    public static class ModelValidExtensions
    {
        public static string GetError(this ModelStateDictionary modelState)
        {
            var key = modelState.Keys.ToList()[0];
            modelState.TryGetValue(key,out ModelStateEntry modelStateEntry);
            var error = modelStateEntry?.Errors[0];
            return error?.ErrorMessage;
        }
    }
}
