using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Web;
using System.Net.Http;
using System.Net;
using System.Security.Principal;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using BankEncryptionAPI.Models;
using BankEncryptionAPI.Services;
using BankEncryptionAPI.Services.Impl;


namespace BankEncryptionAPI.App_Start
{
    public class JWTAuthenticationFilter : AuthorizationFilterAttribute
    {
        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            Contract.Assert(actionContext != null);
            return actionContext.ActionDescriptor.GetCustomAttributes<System.Web.Http.AllowAnonymousAttribute>().Any()
                   || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<System.Web.Http.AllowAnonymousAttribute>().Any();
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (SkipAuthorization(actionContext))
                return;

            if (!IsUserAuthorized(actionContext))
            {
                ShowAuthenticationError(actionContext);
                return;
            }


            base.OnAuthorization(actionContext);
        }

        private static void ShowAuthenticationError(HttpActionContext filterContext)
        {
            var responseDTO = new Error() { errorCode = "401", message = "Invalid or expired bearer token." };
            filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized, responseDTO);
        }

        public bool IsUserAuthorized(HttpActionContext actionContext)
        {
            var authHeader = FetchFromHeader(actionContext); //fetch authorization token from header

            if (!string.IsNullOrEmpty(authHeader))
            {
                //clean accidental quotes
                authHeader = authHeader.Replace("\"", "");
            }


            if (authHeader != null)
            {
                var auth = new LoginService();
                JwtSecurityToken userPayloadToken = auth.GenerateUserClaimFromJWT(authHeader);

                if (userPayloadToken != null)
                {

                    var identity = auth.PopulateUserIdentity(userPayloadToken);
                    var genericPrincipal = new GenericPrincipal(identity, null);
                    Thread.CurrentPrincipal = genericPrincipal;
                    var authenticationIdentity = Thread.CurrentPrincipal.Identity as JWTAuthenticationIdentity;
                    if (authenticationIdentity != null && !String.IsNullOrEmpty(authenticationIdentity.clientId))
                    {
                        authenticationIdentity.clientId = identity.clientId;
                        authenticationIdentity.clientSecret = identity.clientSecret;
                    }
                    return true;
                }

            }
            return false;


        }
        private static string FetchFromHeader(HttpActionContext actionContext)
        {
            string requestToken = null;

            var authRequest = actionContext.Request.Headers.Authorization;
            if (authRequest != null)
            {
                requestToken = authRequest.Parameter;
            }

            return requestToken;
        }
    
    }
}