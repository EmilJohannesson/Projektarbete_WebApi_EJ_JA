using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Projektarbete_WebApi_EJ_JA.Data;
using Projektarbete_WebApi_EJ_JA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projektarbete_WebApi_EJ_JA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeoController : ControllerBase
    {
        private readonly UserDbContext _context;
        public GeoController(UserDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetGeoMessages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GeoMessage>>> GetAllGeoMessages()
        {
            return await _context.GeoMessages.ToListAsync();
        }

        [HttpPost]
        [Route("CreateNewGeoMessage")]
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

        [HttpGet]
        [Route("{id}")]
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
