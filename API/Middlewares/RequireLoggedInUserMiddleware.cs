using System;
using System.Security.Claims;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Middlewares;

public class RequireLoggedInUserMiddleware(IUserService service) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var endpoint = context.GetEndpoint();
        // endpoint voi olla null, jos kirjoitat urlin väärin
        if (endpoint != null)
        {
            // haetaan käyttäjä vain , jos authorize-attribute-on käytössä
            var authorizedAttr = endpoint.Metadata.GetMetadata<AuthorizeAttribute>();
            if (authorizedAttr != null)
            {
                var unAuthorizedResponse = new { title = "Unauthorized" };
                var id = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (id == null)
                {
                    context.Response.StatusCode = 401; // Bad Request
                    await new ObjectResult(unAuthorizedResponse).ExecuteResultAsync(new ActionContext { HttpContext = context });
                    return;
                }

                var success = int.TryParse(id.Value, out int parsedId);
                if (!success)
                {
                    context.Response.StatusCode = 401; // Bad Request
                    await new ObjectResult(unAuthorizedResponse).ExecuteResultAsync(new ActionContext { HttpContext = context });
                    return;
                }

                var user = await service.GetById(parsedId);
                if (user == null)
                {
                    context.Response.StatusCode = 401; // Bad Request
                    await new ObjectResult(unAuthorizedResponse).ExecuteResultAsync(new ActionContext { HttpContext = context });
                    return;
                }

                // laitetaan sisäänkirjautunut käyttäjä osaksi httpcontextia
                context.Items["loggedInUser"] = user;
            }
        }
        // mennään eteenpäin
        await next(context);
    }
}
