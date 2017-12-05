using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.UrlFix
{
    public class UrlFixOptions
    {
        /// <summary>
        /// The PathBase to use for urls. Incoming urls that do not have PathBase
        /// equal to this value will be redirected so that they do.
        /// </summary>
        public String CorrectPathBase { get; set; }

        /// <summary>
        /// The proctol to accept for urls. Set to null for any, defaults to https://
        /// </summary>
        public string Protocol { get; set; } = "https://";
    }
}
