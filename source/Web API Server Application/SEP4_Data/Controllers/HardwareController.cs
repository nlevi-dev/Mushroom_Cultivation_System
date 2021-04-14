using System;
using Microsoft.AspNetCore.Mvc;
using SEP4_Data.Data;
using SEP4_Data.Model;
using SEP4_Data.Model.Exception;

namespace SEP4_Data.Controllers
{
    public class HardwareController : ControllerBase
    {
        private readonly IPersistenceService _persistence;
        private readonly IConfigService _config;
        
        public HardwareController(IPersistenceService persistence, IConfigService config)
        {
            _persistence = persistence;
            _config = config;
        }
        
        [HttpPost]
        [Route("hardware")]
        public IActionResult PostHardware([FromBody] Hardware hardware)
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                hardware.UserKey = ((User) HttpContext.Items["User"]).Key;
                _persistence.CreateHardware(hardware);
                return StatusCode(200);
            }
            catch (UnauthorizedException e)
            {
                return StatusCode(403, e.Message);
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
        [Route("hardware")]
        public IActionResult GetAllHardware()
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                Hardware[] hardwares = _persistence.GetAllHardware((int) ((User) HttpContext.Items["User"]).Key);
                return StatusCode(200, hardwares);
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
        
        [HttpGet]
        [Route("hardware/key/{hardwareKey}")]
        public IActionResult GetHardware([FromRoute] int hardwareKey)
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                if (_persistence.GetHardware(hardwareKey).UserKey != ((User) HttpContext.Items["User"]).Key)
                {
                    throw new UnauthorizedException("You do not own the hardware!");
                }
                var temp = _persistence.GetHardware(hardwareKey);
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
        
        [HttpDelete]
        [Route("hardware/key/{hardwareKey}")]
        public IActionResult DeleteHardware([FromRoute] int hardwareKey)
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                if (_persistence.GetHardware(hardwareKey).UserKey != ((User) HttpContext.Items["User"]).Key)
                {
                    throw new UnauthorizedException("You do not own the hardware!");
                }
                _persistence.DeleteHardware(hardwareKey);
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
        
        [HttpPut]
        [Route("hardware/key/{hardwareKey}")]
        public IActionResult PutHardware([FromRoute] int hardwareKey, [FromBody] Hardware hardware)
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                hardware.Key = hardwareKey;
                if (_persistence.GetHardware(hardwareKey).UserKey != ((User) HttpContext.Items["User"]).Key)
                {
                    throw new UnauthorizedException("You do not own the hardware!");
                }
                hardware.UserKey = ((User) HttpContext.Items["User"]).Key;
               _persistence.UpdateHardware(hardware);
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
        
        [HttpGet]
        [Route("hardware/key/{hardwareKey}/sensor")]
        public IActionResult GetCurrentSensorValues([FromRoute] int hardwareKey)
        {
            try
            {
                throw new NotImplementedException("Because Levente is a weetard");
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
    }
}