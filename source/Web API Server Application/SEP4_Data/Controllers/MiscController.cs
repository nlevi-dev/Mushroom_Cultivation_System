using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SEP4_Data.Data;
using SEP4_Data.Model;
using SEP4_Data.Model.Exception;

namespace SEP4_Data.Controllers
{
    [ApiController]
    public class MiscController : ControllerBase
    {
        private readonly IPersistenceService _persistence;
        private readonly IConfigService _config;
        
        public MiscController(IPersistenceService persistence, IConfigService config)
        {
            _persistence = persistence;
            _config = config;
        }
        
        [HttpGet]
        [Route("defined/mushroom/stages")]
        public IActionResult GetMushroomStages()
        {
            try
            {
                var temp = _persistence.GetMushroomStages();
                return StatusCode(200, temp);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpGet]
        [Route("defined/mushroom/types")]
        public IActionResult GetMushroomTypes()
        {
            try
            {
                var temp = _persistence.GetMushroomTypes();
                return StatusCode(200, temp);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpGet]
        [Route("health")]
        public IActionResult HealthCheck()
        {
            return StatusCode(200);
        }
        
        [HttpGet]
        [Route("token")]
        public IActionResult GetToken()
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                var temp = GenerateJwtToken((int) ((User) HttpContext.Items["User"]).Key);
                return StatusCode(200, temp);
            }
            catch (UnauthorizedException e)
            {
                return StatusCode(403, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        private string GenerateJwtToken(int userKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = _config.JwtKey;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("key", userKey.ToString()) }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            if (_config.TokenExpire > 0)
                tokenDescriptor.Expires = DateTime.UtcNow.Add(new TimeSpan(0, _config.TokenExpire, 0));
            else
                tokenDescriptor.Expires = DateTime.Parse("01/01/2030");
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}