using Templateprj.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Templateprj.Models.ApiModels;
using System.Web;

namespace Templateprj.Controllers
{
    [RoutePrefix("SMSBusiness")]
    public class AuthJwtController : ApiController
    {
       // ApiDbprc _prc = new ApiDbprc();
        [HttpPost]

       
        public IHttpActionResult Authenticate([FromBody] LoginRequest login)
        {
            var loginResponse = new LoginResponse { };
            LoginRequest loginrequest = new LoginRequest { };
            loginrequest.Username = login.Username.ToLower();
            loginrequest.Password = login.Password;
            string ousername = ConfigurationManager.AppSettings["Username"].ToString();
            string opassword = ConfigurationManager.AppSettings["Password"].ToString();

            if ((loginrequest.Username != ousername) || (loginrequest.Password != opassword))
            {
                string errormessage = "please enter valid data";
                return Ok<string>(errormessage);

            }

            else {
                string token = CreateToken(loginrequest.Username);
               
                return Ok<string>(token);

            }

            // IHttpActionResult response;
            // HttpResponseMessage responseMsg = new HttpResponseMessage();
            // int authenticated = 0;
            // string APIId = "0";
            //if (login != null)
            //{
            //    authenticated = _prc.AuthenticateCustomer(loginrequest.Username, loginrequest.Password, out APIId);
            //}
            //if (authenticated == 1)
            //{
            // if credentials are valid
           
                //string status = _prc.InsertTokentoDB(token,APIId);
               // if (status == "1")
               // {
                    // return the token
                   
               // }
               // else
               // {
                  //  loginResponse.responseMsg.StatusCode = HttpStatusCode.Unauthorized;
                    //response = ResponseMessage(loginResponse.responseMsg);
                   // return response;
                //}
            //}
           // else
            //{
                // if credentials are not valid send unauthorized status code in response
               // loginResponse.responseMsg.StatusCode = HttpStatusCode.Unauthorized;
               // response = ResponseMessage(loginResponse.responseMsg);
               // return response;
            //}
        }
        

            private string CreateToken(string username)
        {
            //Set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            int validityinHr = Convert.ToInt32(ConfigurationManager.AppSettings["TokenValidityInHr"].ToString());
            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddHours(validityinHr);

            //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            });

            const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
            var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);


            //create the jwt
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: "http://localhost:50191", audience: "http://localhost:50191",
                        subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public LoginResponse()
        {

            this.Token = "";
            this.responseMsg = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.Unauthorized };
        }

        public string Token { get; set; }
        public HttpResponseMessage responseMsg { get; set; }

    }
}