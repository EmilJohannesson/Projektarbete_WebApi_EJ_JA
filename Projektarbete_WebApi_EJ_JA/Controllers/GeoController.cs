using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Projektarbete_WebApi_EJ_JA.Data;
using Projektarbete_WebApi_EJ_JA.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projektarbete_WebApi_EJ_JA.Controllers
{
    /// <summary>
    /// Denna Controllern har metoder som låter oss hämta och skapa GeoMessages.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class GeoController : ControllerBase
    {
        private readonly UserDbContext _context;
        public GeoController(UserDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Ger en lista med alla GeoMessages som skapats
        /// </summary>  
        /// <returns>Lista med GeoMessages</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("GetGeoMessages")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GeoMessage>>> GetAllGeoMessages()
        {
            return await _context.GeoMessages.ToListAsync();
        }

        //[Authorize]
        [HttpPost]
        [SwaggerOperation(
            Summary = "Skapa ett GeoMessage",
            Description = "Skapa ett GeoMessage med Longitud och Latitud, med tillhörande textmedelande"
            )]
        [Route("CreateNewGeoMessage")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GeoMessage>> CreateNewPostAsync(GeoMessage request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest();
            }
            var geoMessagePost = new GeoMessage
            {
                Message = request.Message,
                Longitude = request.Longitude,
                Latitude = request.Latitude
            };

            await _context.AddAsync(geoMessagePost);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMessage), new { id = geoMessagePost.Id }, geoMessagePost);
        }

        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(
            Summary = "Hämta GeoMessage {Id}",
            Description = "Hämta ett GeoMessage med en specifik {Id}"
            )]
        [Route("{id}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GeoMessage>> GetMessage(int id)
        {
            GeoMessage geoMessage = await _context.GeoMessages.FindAsync(id);
            if (geoMessage == null)
            {
                return NotFound();
            }
            return geoMessage;
        }
    }
}
