using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;
using VietQR_QRCodeGeneration.Models;
using VietQR_QRCodeGeneration.Ultil;

namespace VietQR_QRCodeGeneration.Controllers
{
    public class AuthorizationController : ApiController
    {

        [HttpPost]
        [Route("api/Login")]
        public Response Login(Login data)
        {
            if (data == null)
            {
                return new Response { resultCode = (int)HttpStatusCode.NotFound, resultString = "Chưa nhập Userame hoặc Password!", resultData = null };
            }

            var username = data.username;
            var password = data.password;
            var token = "";
            if (username == null || password == null)
            {
                return new Response { resultCode = (int)HttpStatusCode.Forbidden, resultString = "Chưa nhập Userame hoặc Password!", resultData = token };
            }
            var validUser = ConfigurationManager.AppSettings["userName"];
            var validPass = ConfigurationManager.AppSettings["Password"];

            if (!(username == validUser && Function.MD5(password) == Function.MD5(validPass)))
            {
                return new Response { resultCode = (int)HttpStatusCode.Unauthorized, resultString = "Đăng nhập không thành công. Sai Username hoặc Password!", resultData = token };
            }
            else
            {
                token = ConfigurationManager.AppSettings["tokenAccess"];
            }
            return new Response { resultCode = (int)HttpStatusCode.OK, resultString = "Success", resultData = token };
        }
    }
}