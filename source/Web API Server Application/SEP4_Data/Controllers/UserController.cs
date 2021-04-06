using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using SEP4_Data.Data;
using SEP4_Data.Model;
using SEP4_Data.Model.Exception;

namespace SEP4_Data.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IPersistenceService _persistence;
        private readonly IConfigService _config;
        
        public UserController(IPersistenceService persistence, IConfigService config)
        {
            _persistence = persistence;
            _config = config;
        }
        
        [HttpPost]
        [Route("user")]
        public IActionResult PostUser([FromBody] User user)
        {
            try
            {
                //check constraints
                CheckUsername(user.Name);
                CheckPassword(user.Password);
                //hash password
                user.Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(user.Password, _config.Salt, KeyDerivationPrf.HMACSHA1, 1000, 256 / 8));
                _persistence.CreateUser(user);
                return StatusCode(200);
            }
            catch (ConflictException e)
            {
                return StatusCode(409, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpGet]
        [Route("user/name/{username}")]
        public IActionResult GetUser([FromRoute] string username)
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                if (username != ((User)HttpContext.Items["User"]).Name && ((User)HttpContext.Items["User"]).PermissionLevel < 2)
                    throw new UnauthorizedException("You don't have high enough clearance for this operation!");
                var temp = _persistence.GetUserByName(username);
                return StatusCode(200, temp);
            }
            catch (UnauthorizedException e)
            {
                return StatusCode(403, e.Message);
            }
            catch (NotFoundException e)
            {
                return StatusCode(404, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpGet]
        [Route("user/name")]
        public IActionResult GetUser()
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                var temp = (User)HttpContext.Items["User"];
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
        
        [HttpDelete]
        [Route("user/key/{userKey}")]
        public IActionResult DeleteUser([FromRoute] int userKey)
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                if (userKey != (int) ((User)HttpContext.Items["User"]).Key && ((User)HttpContext.Items["User"]).PermissionLevel < 2)
                    throw new UnauthorizedException("You don't have high enough clearance for this operation!");
                _persistence.DeleteUser(userKey);
                return StatusCode(200);
            }
            catch (UnauthorizedException e)
            {
                return StatusCode(403, e.Message);
            }
            catch (NotFoundException e)
            {
                return StatusCode(404, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpPatch]
        [Route("user/key/{userKey}/username")]
        public IActionResult PatchUsername([FromRoute] int userKey, [FromBody] User user)
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                if (userKey != (int) ((User)HttpContext.Items["User"]).Key && ((User)HttpContext.Items["User"]).PermissionLevel < 2)
                    throw new UnauthorizedException("You don't have high enough clearance for this operation!");
                CheckUsername(user.Name);
                _persistence.UpdateUsername(userKey, user.Name);
                return StatusCode(200);
            }
            catch (UnauthorizedException e)
            {
                return StatusCode(403, e.Message);
            }
            catch (NotFoundException e)
            {
                return StatusCode(404, e.Message);
            }
            catch (ConflictException e)
            {
                return StatusCode(409, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpPatch]
        [Route("user/key/{userKey}/password")]
        public IActionResult PatchUserPassword([FromRoute] int userKey, [FromBody] User user)
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                if (userKey != (int) ((User)HttpContext.Items["User"]).Key && ((User)HttpContext.Items["User"]).PermissionLevel < 2)
                    throw new UnauthorizedException("You don't have high enough clearance for this operation!");
                CheckPassword(user.Password);
                _persistence.UpdateUserPassword(userKey, user.Password);
                return StatusCode(200);
            }
            catch (UnauthorizedException e)
            {
                return StatusCode(403, e.Message);
            }
            catch (NotFoundException e)
            {
                return StatusCode(404, e.Message);
            }
            catch (ConflictException e)
            {
                return StatusCode(409, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        private static void CheckPassword(string password)
        {
            //check password constraints here, throw conflict exception if needed
            if (password.Length <= 8)
                throw new ConflictException("password must be longer than 8 characters");
        }
        
        private static void CheckUsername(string username)
        {
            //check username constraints here, throw conflict exception if needed
            if (username.Trim().Length != username.Length)
                throw new ConflictException("username can not begin or end with white spaces");
        }
    }
}