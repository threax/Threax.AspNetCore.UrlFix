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
        private String correctPathBase;
        private RequestDelegate next;
        private String protocol;

        public UrlFixMiddleware(UrlFixOptions options, RequestDelegate next)
        {
            this.next = next;
            this.correctPathBase = options.CorrectPathBase;
            this.protocol = options.Protocol;
        }

        public async Task Invoke(HttpContext context)
        {
            var pathBase = context.Request.PathBase;
            if (pathBase.HasValue && pathBase.Value != correctPathBase)
            {
                var currentUri = new Uri(context.Request.GetDisplayUrl());
                var uriBuilder = new UriBuilder(currentUri);
                var path = uriBuilder.Path;
                var basePathIndex = path.IndexOf(pathBase.Value);
                if (basePathIndex != -1)
                {
                    path = correctPathBase + path.Substring(pathBase.Value.Length);
                }
                uriBuilder.Path = path;

                context.Response.Redirect(uriBuilder.ToString());
            }
            else
            {
                await next.Invoke(context);
            }
        }
    }
}
