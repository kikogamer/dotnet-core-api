using Elmah.Io.AspNetCore;
using Elmah.Io.AspNetCore.ExceptionFormatters;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DEVIO.Api.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                HandleException(httpContext, ex);
            }
        }

        private static void HandleException(HttpContext context, Exception exception)
        {
            ElmahIoApi.Log(exception, context);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}
