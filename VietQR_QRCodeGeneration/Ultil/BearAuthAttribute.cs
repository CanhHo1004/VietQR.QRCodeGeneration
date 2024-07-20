using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using VietQR_QRCodeGeneration.Models;
using static VietQR_QRCodeGeneration.Ultil.Function;

namespace VietQR_QRCodeGeneration.Ultil
{
    public class BearAuthAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization != null)
            {
                var authToken = actionContext.Request.Headers
                .Authorization.ToString();

                if (string.IsNullOrEmpty(authToken))
                {
                    var result = new Response() { resultCode = (int)ResultCode.Unauthorized, resultString = "Xác thực tài khoản thất bại!" };
                    var request = actionContext.Request;
                    actionContext.Response = request.CreateResponse<Response>(HttpStatusCode.OK, result);
                }
                else
                {
                    var isValid = AuthorizedUser(authToken);
                    if (!isValid)
                    {
                        var result = new Response() { resultCode = (int)ResultCode.Unauthorized, resultString = "Xác thực tài khoản thất bại!" };
                        var request = actionContext.Request;
                        actionContext.Response = request.CreateResponse<Response>(HttpStatusCode.OK, result);
                    }
                }
            }
            else
            {
                var result = new Response() { resultCode = (int)ResultCode.Unauthorized, resultString = "Xác thực tài khoản thất bại!" };
                var request = actionContext.Request;
                actionContext.Response = request.CreateResponse<Response>(HttpStatusCode.OK, result);
            }
        }

        public static bool AuthorizedUser(string token)
        {
            string authorizationToken = Convert.ToString(ConfigurationManager.AppSettings["tokenAccess"]); // Get AuthorizationToken from config
            if (token != null && token == authorizationToken)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}