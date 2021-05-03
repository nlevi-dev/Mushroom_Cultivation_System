using System;
using Microsoft.AspNetCore.Mvc;
using SEP4_Data.Data;
using SEP4_Data.Model;
using SEP4_Data.Model.Exception;

namespace SEP4_Data.Controllers
{
    public class SpecimenController : ControllerBase
    {
        private readonly IPersistenceService _persistence;
        private readonly IConfigService _config;
        
        public SpecimenController(IPersistenceService persistence, IConfigService config)
        {
            _persistence = persistence;
            _config = config;
        }
        
        [HttpPost]
        [Route("specimen")]
        public IActionResult PostSpecimen([FromBody] Specimen specimen)
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                specimen.UserKey = ((User) HttpContext.Items["User"]).Key;
                specimen.TypeKey = _persistence.GetMushroomTypeKey(specimen.MushroomType);
                specimen.PlantedUnix = null;
                if (specimen.Hardware != null)
                    specimen.HardwareKey = _persistence.GetHardwareById(specimen.Hardware).Key;
                _persistence.CreateSpecimen(specimen);
                if (specimen.Hardware != null)
                {
                    //send data to IoT interface
                    //wait for ack
                    _persistence.UpdateHardware(new Hardware{Key = specimen.HardwareKey, DesiredAirTemperature = specimen.DesiredAirTemperature, DesiredAirHumidity = specimen.DesiredAirHumidity, DesiredAirCo2 = specimen.DesiredAirCo2, DesiredLightLevel = specimen.DesiredLightLevel});
                }
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
        [Route("specimen")]
        public IActionResult GetAllSpecimen()
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                var temp = _persistence.GetAllSpecimen((int) ((User) HttpContext.Items["User"]).Key);
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
        [Route("specimen/key/{specimenKey}")]
        public IActionResult GetSpecimen([FromRoute] int specimenKey)
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                var check = _persistence.GetSpecimen(specimenKey);
                if (check.UserKey == null)
                {
                    if (((User) HttpContext.Items["User"]).PermissionLevel < 3)
                        throw new NotFoundException("You don't have high enough clearance for this operation!");
                }
                else
                {
                    if (check.UserKey != ((User) HttpContext.Items["User"]).Key && ((User)HttpContext.Items["User"]).PermissionLevel < 2)
                        throw new ForbiddenException("You don't have high enough clearance for this operation!");
                }
                var temp = _persistence.GetSpecimen(specimenKey);
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
        [Route("specimen/key/{specimenKey}")]
        public IActionResult DiscardSpecimen([FromRoute] int specimenKey)
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                var check = _persistence.GetSpecimen(specimenKey);
                if (check.UserKey == null)
                {
                    if (((User) HttpContext.Items["User"]).PermissionLevel < 3)
                        throw new NotFoundException("You don't have high enough clearance for this operation!");
                }
                else
                {
                    if (check.UserKey != ((User) HttpContext.Items["User"]).Key && ((User)HttpContext.Items["User"]).PermissionLevel < 2)
                        throw new ForbiddenException("You don't have high enough clearance for this operation!");
                }
                _persistence.DiscardSpecimen(specimenKey);
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
        
        [HttpPut]
        [Route("specimen/key/{specimenKey}")]
        public IActionResult PutSpecimen([FromRoute] int specimenKey, [FromBody] Specimen specimen)
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                var check = _persistence.GetSpecimen(specimenKey);
                if (check.UserKey == null)
                {
                    if (((User) HttpContext.Items["User"]).PermissionLevel < 3)
                        throw new NotFoundException("You don't have high enough clearance for this operation!");
                }
                else
                {
                    if (check.UserKey != ((User) HttpContext.Items["User"]).Key && ((User)HttpContext.Items["User"]).PermissionLevel < 2)
                        throw new ForbiddenException("You don't have high enough clearance for this operation!");
                }
                specimen.Key = specimenKey;
                specimen.UserKey = check.UserKey != null ? ((User) HttpContext.Items["User"]).Key : null;
                if (specimen.MushroomType != null)
                    specimen.TypeKey = _persistence.GetMushroomTypeKey(specimen.MushroomType);
                specimen.PlantedUnix = null;
                if (specimen.Hardware != null)
                    specimen.HardwareKey = _persistence.GetHardwareById(specimen.Hardware).Key;
                _persistence.UpdateSpecimen(specimen);
                if (specimen.Hardware != null)
                {
                    //send data to IoT interface
                    //wait for ack
                    _persistence.UpdateHardware(new Hardware{Key = specimen.HardwareKey, DesiredAirTemperature = specimen.DesiredAirTemperature, DesiredAirHumidity = specimen.DesiredAirHumidity, DesiredAirCo2 = specimen.DesiredAirCo2, DesiredLightLevel = specimen.DesiredLightLevel});
                }
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
        [Route("specimen/key/{specimenKey}/sensor")]
        public IActionResult GetSensorHistory([FromRoute] int specimenKey, [FromQuery] long? filterFrom, [FromQuery] long? filterUntil)
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                var check = _persistence.GetSpecimen(specimenKey);
                if (check.UserKey == null)
                {
                    if (((User) HttpContext.Items["User"]).PermissionLevel < 3)
                        throw new NotFoundException("You don't have high enough clearance for this operation!");
                }
                else
                {
                    if (check.UserKey != ((User) HttpContext.Items["User"]).Key && ((User)HttpContext.Items["User"]).PermissionLevel < 2)
                        throw new ForbiddenException("You don't have high enough clearance for this operation!");
                }
                var temp = _persistence.GetSensorHistory(specimenKey, filterFrom, filterUntil);
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
    }
}