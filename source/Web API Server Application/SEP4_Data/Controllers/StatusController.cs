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
                if (_persistence.GetSpecimen(specimenKey).UserKey != ((User) HttpContext.Items["User"]).Key && ((User)HttpContext.Items["User"]).PermissionLevel < 2)
                    throw new ForbiddenException("You don't have high enough clearance for this operation!");
                statusEntry.Specimen = specimenKey;
                statusEntry.StageKey = _persistence.GetMushroomStageKey(statusEntry.Stage);
                _persistence.CreateStatusEntry(statusEntry);
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
        [Route("specimen/key/{specimenKey}/status")]
        public IActionResult GetAllStatus([FromRoute] int specimenKey)
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                if (_persistence.GetSpecimen(specimenKey).UserKey != ((User) HttpContext.Items["User"]).Key && ((User)HttpContext.Items["User"]).PermissionLevel < 2)
                    throw new ForbiddenException("You don't have high enough clearance for this operation!");
                var temp = _persistence.GetAllStatusEntries(specimenKey);
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
        
        [HttpGet]
        [Route("status/key/{statusKey}")]
        public IActionResult GetStatus([FromRoute] int statusKey)
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                if (_persistence.GetSpecimen((int) _persistence.GetStatusEntry(statusKey).Specimen).UserKey != ((User) HttpContext.Items["User"]).Key && ((User)HttpContext.Items["User"]).PermissionLevel < 2)
                    throw new ForbiddenException("You don't have high enough clearance for this operation!");
                var temp = _persistence.GetStatusEntry(statusKey);
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
        [Route("status/key/{statusKey}")]
        public IActionResult DeleteStatus([FromRoute] int statusKey)
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                if (_persistence.GetSpecimen(statusKey).UserKey != ((User) HttpContext.Items["User"]).Key && ((User)HttpContext.Items["User"]).PermissionLevel < 2)
                    throw new ForbiddenException("You don't have high enough clearance for this operation!");
                _persistence.DeleteStatusEntry(statusKey);
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
        [Route("status/key/{statusKey}")]
        public IActionResult PutStatus([FromRoute] int statusKey, [FromBody] StatusEntry statusEntry)
        {
            try
            {
                if (HttpContext.Items["User"] == null)
                    throw new UnauthorizedException("Authorization failed!");
                if (_persistence.GetSpecimen((int) _persistence.GetStatusEntry(statusKey).Specimen).UserKey != ((User) HttpContext.Items["User"]).Key && ((User)HttpContext.Items["User"]).PermissionLevel < 2)
                    throw new ForbiddenException("You don't have high enough clearance for this operation!");
                statusEntry.Key = statusKey;
                statusEntry.StageKey = _persistence.GetMushroomStageKey(statusEntry.Stage);
                _persistence.UpdateStatusEntry(statusEntry);
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
    }
}