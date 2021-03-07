using System;
using System.Net;
using DepsWebApp.Filters;
using DepsWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DepsWebApp.Controllers
{
    /// <summary>
    /// Authentication controller
    /// </summary>
    [ApiController]
    [CustomExceptionFilter]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Registration
        /// </summary>
        /// <param name="registrationModel">Registration model with login and password</param>
        [HttpPost]
        [Route("register")]
        public void Register([FromBody] RegistrationModel registrationModel)
        {
            throw new NotImplementedException();
        }
    }
}