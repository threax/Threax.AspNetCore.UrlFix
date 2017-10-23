using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Threax.AspNetCore.UrlFix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UrlFixExtensions
    {
        /// <summary>
        /// Activate this to rewrite any incoming urls with an incorrect
        /// PathBase to one that is correct. This helps handle cookies that exist in a subpath. 
        /// </summary>
        /// <param name="builder">The app builder.</param>
        /// <param name="options">The options.</param>
        /// <returns>The app builder.</returns>
        public static IApplicationBuilder UseUrlFix(this IApplicationBuilder builder, Action<UrlFixOptions> optionsBuilder)
        {
            var options = new UrlFixOptions();

            optionsBuilder.Invoke(options);

            String pathBase = options.CorrectPathBase;
            //Ensure our input string is in the correct format.
            if (!String.IsNullOrEmpty(pathBase))
            {
                pathBase = pathBase.Replace('\\', '/');
                if (pathBase[0] != '/')
                {
                    pathBase = '/' + pathBase;
                }
                int lastIndex = pathBase.Length - 1;
                if (pathBase[lastIndex] == '/')
                {
                    pathBase = pathBase.Substring(0, lastIndex);
                }
            }
            options.CorrectPathBase = pathBase;

            //only use the middleware if a path is provided.
            if (!String.IsNullOrEmpty(pathBase))
            {
                builder.UseMiddleware<UrlFixMiddleware>(options);
            }

            return builder;
        }
    }
}
