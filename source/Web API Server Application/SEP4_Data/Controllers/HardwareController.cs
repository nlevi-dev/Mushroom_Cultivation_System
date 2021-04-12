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
                throw new NotImplementedException();
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
                var existingHardware = _persistence.GetHardware(hardwareKey);

                if (existingHardware != null)
                {   
                    
                }
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
                throw new NotImplementedException();
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