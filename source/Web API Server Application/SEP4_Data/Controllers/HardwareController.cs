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
        private readonly ISampleService _sample;
        
        public HardwareController(IPersistenceService persistence, IConfigService config, ISampleService sample)
        {
            _persistence = persistence;
            _config = config;
            _sample = sample;
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
                hardware.DesiredAirTemperature = null;
                hardware.DesiredAirHumidity = null;
                hardware.DesiredAirCo2 = null;
                hardware.DesiredLightLevel = null;
                _persistence.CreateHardware(hardware);
                return StatusCode(200);
            }
            catch (UnauthorizedException e)
            {
                return StatusCode(401, e.Message);
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
                var temp = _persistence.GetAllHardware((int) ((User) HttpContext.Items["User"]).Key);
                return StatusCode(200, temp);
            }
            catch (UnauthorizedException e)
            {
                return StatusCode(401, e.Message);
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
                if (_persistence.GetHardware(hardwareKey).UserKey != ((User) HttpContext.Items["User"]).Key && ((User)HttpContext.Items["User"]).PermissionLevel < 2)
                    throw new ForbiddenException("You don't have high enough clearance for this operation!");
                var temp = _persistence.GetHardware(hardwareKey);
                return StatusCode(200, temp);
            }
            catch (UnauthorizedException e)
            {
                return StatusCode(401, e.Message);
            }
            catch (ForbiddenException e)
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
                if (_persistence.GetHardware(hardwareKey).UserKey != ((User) HttpContext.Items["User"]).Key && ((User)HttpContext.Items["User"]).PermissionLevel < 2)
                    throw new ForbiddenException("You don't have high enough clearance for this operation!");
                _persistence.DeleteHardware(hardwareKey);
                return StatusCode(200);
            }
            catch (UnauthorizedException e)
            {
                return StatusCode(401, e.Message);
            }
            catch (ForbiddenException e)
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
        [Route("hardware/key/{hardwareKey}")]
        public IActionResult PatchHardwareId([FromRoute] int hardwareKey, [FromBody] Hardware hardware)
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                hardware = new Hardware {Key = hardwareKey, Id = hardware.Id};
                if (_persistence.GetHardware(hardwareKey).UserKey != ((User) HttpContext.Items["User"]).Key && ((User)HttpContext.Items["User"]).PermissionLevel < 2)
                    throw new ForbiddenException("You don't have high enough clearance for this operation!");
                _persistence.UpdateHardware(hardware);
               return StatusCode(200);
            }
            catch (UnauthorizedException e)
            {
                return StatusCode(401, e.Message);
            }
            catch (ForbiddenException e)
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
        [Route("hardware/id/{hardwareId}/sensor")]
        public IActionResult GetCurrentSensorValues([FromRoute] string hardwareId)
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                var hardware = _persistence.GetHardwareById(hardwareId);
                if (hardware.UserKey != ((User) HttpContext.Items["User"]).Key && ((User)HttpContext.Items["User"]).PermissionLevel < 2)
                    throw new ForbiddenException("You don't have high enough clearance for this operation!");
                var temp = _sample.GetLatestEntry((int) ((User) HttpContext.Items["User"]).Key, hardwareId);
                if (temp != null)
                    return StatusCode(200, temp);
                try {
                    var specimenKey = _persistence.GetHardwareById(hardwareId).SpecimenKey;
                    if (specimenKey != null)
                    {
                        temp = _persistence.GetLastEntry((int) specimenKey);
                        return StatusCode(200, temp);
                    }
                } catch (NotFoundException) {/*ignored*/}
                temp = _sample.GetCached(hardwareId);
                if (temp != null)
                    return StatusCode(200, temp);
                throw new NotFoundException("hardware with id \"" + hardwareId + "\" not found");
            }
            catch (UnauthorizedException e)
            {
                return StatusCode(401, e.Message);
            }
            catch (ForbiddenException e)
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