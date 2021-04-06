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
                throw new NotImplementedException();
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
        [Route("specimen")]
        public IActionResult GetAllSpecimen()
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
        [Route("specimen/key/{specimenKey}")]
        public IActionResult GetSpecimen([FromRoute] int specimenKey)
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
        
        [HttpDelete]
        [Route("specimen/key/{specimenKey}")]
        public IActionResult DiscardSpecimen([FromRoute] int specimenKey)
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
        
        [HttpPut]
        [Route("specimen/key/{specimenKey}")]
        public IActionResult PutSpecimen([FromRoute] int specimenKey, [FromBody] Specimen specimen)
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