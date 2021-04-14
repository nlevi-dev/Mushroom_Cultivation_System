using System;
using Microsoft.AspNetCore.Mvc;
using SEP4_Data.Data;
using SEP4_Data.Model;
using SEP4_Data.Model.Exception;

namespace SEP4_Data.Controllers
{
    public class StatusController : ControllerBase
    {
        private readonly IPersistenceService _persistence;
        private readonly IConfigService _config;
        
        public StatusController(IPersistenceService persistence, IConfigService config)
        {
            _persistence = persistence;
            _config = config;
        }
        
        [HttpPost]
        [Route("specimen/key/{specimenKey}/status")]
        public IActionResult PostStatus([FromRoute] int specimenKey, [FromBody] StatusEntry statusEntry)
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                if (_persistence.GetHardware((_persistence.GetSpecimen(specimenKey).Hardware) != ((User) HttpContext.Items["User"]).Key)
                {
                    throw new UnauthorizedException("You do not own the hardware!");
                }
                statusEntry.Specimen = specimenKey;
                statusEntry.StageKey = _persistence.GetMushroomStageKey(statusEntry.Stage);
                _persistence.CreateStatusEntry(statusEntry);
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
        [Route("specimen/key/{specimenKey}/status")]
        public IActionResult GetAllStatus([FromRoute] int specimenKey)
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
        
        [HttpGet]
        [Route("status/key/{statusKey}")]
        public IActionResult GetStatus([FromRoute] int statusKey)
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
        [Route("status/key/{statusKey}")]
        public IActionResult DeleteStatus([FromRoute] int specimenKey)
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
        [Route("status/key/{statusKey}")]
        public IActionResult PutStatus([FromRoute] int specimenKey, [FromBody] StatusEntry statusEntry)
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
    }
}