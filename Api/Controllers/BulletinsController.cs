using System;
using Models;
using Api.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BulletinsController : ControllerBase
    {
        private readonly IBulletinRepository bulletinRepository;

        public BulletinsController(IBulletinRepository bulletinRepository)
        {
            this.bulletinRepository = bulletinRepository;
        }
        
        [HttpGet]
        public async Task<ActionResult> GetBulletins()
        {
            try
            {
                return Ok(await bulletinRepository.GetBulletins());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
					"Error retrieving data from the database");
            }
        }
    
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Bulletin>> GetBulletin(int id)
        {
            try
            {
                var result = await bulletinRepository.GetBulletin(id);

                if (result == null) return NotFound();

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
    
        [HttpPost]
        public async Task<ActionResult<Bulletin>> CreateBulletin(Bulletin bulletin)
        {
            try
            {
                if (bulletin == null)
                    return BadRequest();

                // add user data
                // bulletin.CreatedBy = Cookie;
                // add datetime
                bulletin.CreatedAt = DateTime.Now;

                var createdBulletin = await bulletinRepository.AddBulletin(bulletin);

                return CreatedAtAction(nameof(GetBulletin),
                    new { id = createdBulletin.Id }, createdBulletin);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new bulletin record");
            }
        }
    
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Bulletin>> UpdateBulletin(int id, Bulletin bulletin)
        {
            try
            {
                if (id != bulletin.Id)
                    return BadRequest("Bulletin ID mismatch");

                var bulletinToUpdate = await bulletinRepository.GetBulletin(id);

                if (bulletinToUpdate == null)
                    return NotFound($"Bulletin with Id = {id} not found");

                return await bulletinRepository.UpdateBulletin(bulletin);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }
    
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Bulletin>> DeleteBulletin(int id)
        {
            try
            {
                var bulletinToDelete = await bulletinRepository.GetBulletin(id);

                if (bulletinToDelete == null)
                {
                    return NotFound($"Bulletin with Id = {id} not found");
                }

                return await bulletinRepository.DeleteBulletin(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }
    }
}
