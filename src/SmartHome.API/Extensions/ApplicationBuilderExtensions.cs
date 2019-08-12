using Microsoft.AspNetCore.Builder;
using System;

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
