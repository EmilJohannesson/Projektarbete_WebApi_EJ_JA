using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projektarbete_WebApi_EJ_JA.Data;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projektarbete_WebApi_EJ_JA.Controllers
{
    namespace V1
    {
        /// <summary>
        /// Denna Controllern har metoder som låter oss hämta och skapa GeoMessages.
        /// </summary>
        [ApiController]
        [ApiVersion("1.0")]
        [Route("api/v{version:apiVersion}/[controller]")]
        [Authorize]
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
            [HttpGet("[action]")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            public async Task<ActionResult<IEnumerable<Models.V1.GeoMessage>>> GetAllGeoMessages()
            {
                return await _context.GeoMessages.ToListAsync();
            }

            [HttpPost("[action]")]
            [SwaggerOperation(
                Summary = "Skapa ett GeoMessage",
                Description = "Skapa ett GeoMessage med Longitud och Latitud, med tillhörande textmedelande"
                )]
            [ProducesResponseType(StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<ActionResult<Models.V1.GeoMessage>> CreateNewPostAsync(Models.V1.GeoMessage request)
            {
                if (string.IsNullOrWhiteSpace(request.Message))
                {
                    return BadRequest();
                }
                var geoMessagePost = new Models.V1.GeoMessage
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
            [HttpGet("[action]")]
            [SwaggerOperation(
                Summary = "Hämta GeoMessage {Id}",
                Description = "Hämta ett GeoMessage med en specifik {Id}"
                )]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult<Models.V1.GeoMessage>> GetMessage(int id)
            {
                Models.V1.GeoMessage geoMessage = await _context.GeoMessages.FindAsync(id);
                if (geoMessage == null)
                {
                    return NotFound();
                }
                return geoMessage;
            }
        }
    }
    namespace V2
    {
        /// <summary>
        /// Denna Controllern har metoder som låter oss hämta och skapa GeoMessages.
        /// </summary>
        [Authorize]
        [ApiController]
        [ApiVersion("2.0")]
        [Route("api/v{version:apiVersion}/[controller]")]
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
            [HttpGet("[action]")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            public async Task<ActionResult<IEnumerable<Models.V2.GeoMessage>>> GetAllGeoMessages()
            {
                return await _context.GeoMessagesV2.ToListAsync();
            }

            [HttpPost("[action]")]
            [SwaggerOperation(
                Summary = "Skapa ett GeoMessage",
                Description = "Skapa ett GeoMessage med Longitud och Latitud, med tillhörande textmedelande"
                )]
            [ProducesResponseType(StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<ActionResult<Models.V2.GeoMessage>> CreateNewPostAsync(Models.V2.GeoMessage request)
            {
                if (string.IsNullOrWhiteSpace(request.Message))
                {
                    return BadRequest();
                }
                var geoMessagePost = new Models.V2.GeoMessage
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
            [HttpGet("[action]")]
            [SwaggerOperation(
                Summary = "Hämta GeoMessage {Id}",
                Description = "Hämta ett GeoMessage med en specifik {Id}"
                )]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult<Models.V2.GeoMessage>> GetMessage(int id)
            {
                Models.V2.GeoMessage geoMessage = await _context.GeoMessagesV2.FindAsync(id);
                if (geoMessage == null)
                {
                    return NotFound();
                }
                return geoMessage;
            }
        }

    }
}

