using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;

namespace Threax.AspNetCore.UrlFix
{
    public class UrlFixMiddleware
    {
        private String correctScheme;
        private String correctPathBase;
        private RequestDelegate next;

        public UrlFixMiddleware(UrlFixOptions options, RequestDelegate next)
        {
            this.next = next;
            this.correctPathBase = options.CorrectPathBase;
            this.correctScheme = options.CorrectScheme?.ToLowerInvariant();
        }

        public async Task Invoke(HttpContext context)
        {
            var pathBase = context.Request.PathBase;
            var scheme = context.Request.Scheme?.ToLowerInvariant();

            bool fixPathBase = correctPathBase != null && pathBase.HasValue && pathBase.Value != correctPathBase;
            bool fixScheme = correctScheme != null && scheme != correctScheme;

            if (fixPathBase || fixScheme)
            {
                var currentUri = new Uri(context.Request.GetDisplayUrl());
                var uriBuilder = new UriBuilder(currentUri);
                if (fixPathBase)
                {
                    var path = uriBuilder.Path;
                    var basePathIndex = path.IndexOf(pathBase.Value);
                    if (basePathIndex != -1)
                    {
                        path = correctPathBase + path.Substring(pathBase.Value.Length);
                    }
                    uriBuilder.Path = path;
                }
                if (fixScheme)
                {
                    uriBuilder.Scheme = correctScheme;
                }

                context.Response.Redirect(uriBuilder.ToString());
            }
            else
            {
                await next.Invoke(context);
            }
        }
    }
}
