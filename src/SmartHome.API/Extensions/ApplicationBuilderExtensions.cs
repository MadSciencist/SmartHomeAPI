using System;
using Microsoft.AspNetCore.Builder;

namespace SmartHome.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseLoggingExceptionHandler(this IApplicationBuilder app)
        {
            if (app is null)
                throw new ArgumentNullException(nameof(app));

            app.UseMiddleware<ExceptionLoggingMiddleware>();
        }
    }
}
