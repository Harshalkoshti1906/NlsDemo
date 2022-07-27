using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using NlsDemo.data.dbcontext;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NlsDemo.api
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //var identity = context.HttpContext.User.Identity as ClaimsIdentity;
            //var claimCount = identity.Claims.Count();
            //if (claimCount == 0)
            //{
            //    //// not logged in
            //    context.Result = new JsonResult(new { isSuccess = false, statusCode = 401, message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            //}
            //else
            //{
            //    IEnumerable<Claim> claims = identity.Claims;
            //    // or
            //    Claim claim = claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault();
            //    if (true)
            //    {

            //    }
            //}

            try
            {
                var token = context.HttpContext.Request.Headers["Authorization"];
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                if (string.IsNullOrWhiteSpace(token))
                {
                    context.Result = new JsonResult(new { isSuccess = false, statusCode = 401, message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                }
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                {
                    context.Result = new JsonResult(new { isSuccess = false, statusCode = 401, message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                }
                var isAvail = jwtToken.Payload.ContainsKey("unique_name");
                if (!isAvail)
                {
                    context.Result = new JsonResult(new { isSuccess = false, statusCode = 401, message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                }
            }
            catch (Exception)
            {
                context.Result = new JsonResult(new { isSuccess = false, statusCode = 401, message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAdminAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var identity = context.HttpContext.User.Identity as ClaimsIdentity;
            var claimCount = identity.Claims.Count();
            var rname = identity.FindFirst("Name").Value;
            if (rname == "Merchant")
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
