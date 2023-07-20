using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace Seguridad.API.Middleware
{
        public class CustomDelegatingHandler : DelegatingHandler
        {
            private readonly RequestDelegate _next;
            private readonly ILogger _logger;
            public CustomDelegatingHandler(RequestDelegate next, ILoggerFactory loggerFactory)
            {
                _next = next;
                _logger = loggerFactory.CreateLogger(typeof(CustomDelegatingHandler));
            }
            public async Task Invoke(HttpContext context)
            {
                context.Request.EnableBuffering();
                var bodyAsText = await new StreamReader(context.Request.Body).ReadToEndAsync();
                var text = bodyAsText.ToString();
                context.Request.Body.Position = 0;

                if (verifyScript(text))
                {
                var message = "Error en la solicitud, script peligroso detectado";
                context.Response.StatusCode = 400;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(message);

                return;
                }

                await _next(context);

            }

            public bool verifyScript(String response)
            {
                String scriptRegex = @"<script[\s\S]*?>[\s\S]*?<\/script>";
                Regex re = new Regex(scriptRegex, RegexOptions.IgnoreCase);
                var a = re.IsMatch(response);
                return a;
            }

        }
}
