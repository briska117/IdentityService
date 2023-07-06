﻿using IdentityService.Configuration;
using IdentityService.Models;
using IdentityService.Services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("CreateToken")]
        public async Task<ActionResult> CreateToken(CreateAccount account)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);  
            }
            var user = await this.authService.Authenticate(account.Email, account.Password);
            if (user == null) {
                return Conflict("User/Password Incorrect.");
            }

            string token = await this.authService.GenerateToken(user);

            return Ok(new { Token = token });
        }
        [HttpGet("AccountInfo")]
        [Authorize(
            Policy = IdentitySettings.ElevatedRightsPolicyName,
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<dynamic> GetAccountInfo()
        {
            var userclaims = User.Claims;
            var email = userclaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var role = userclaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            var idUser = userclaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (null == email)
            {
                return NotFound($" User not Found");
            }

            var response = new
            {
                email,
                role,
                idUser
            };

            return Ok(response);
        }
    }
}
