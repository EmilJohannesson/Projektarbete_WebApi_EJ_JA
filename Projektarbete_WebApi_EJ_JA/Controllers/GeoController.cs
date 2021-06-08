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
    [ApiVersion("1")]
    [ApiVersion("2")]

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
        /// 


        [AllowAnonymous]
        [HttpGet("GetAllGeoMessages")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Object>>> GetAllGeoMessages()
        {


            return await _context.GeoMessages.Select(p => new 
            {
                Message = p.Message,
                Latitude = p.Latitude,
                Longitude = p.Longitude
            }).ToListAsync();

        }


        //[Authorize]
        [HttpPost]
        [SwaggerOperation(
            Summary = "Skapa ett GeoMessage",
            Description = "Skapa ett GeoMessage med Longitud och Latitud, med tillhörande textmedelande"
            )]
        [Route("CreateNewGeoMessage")]
        [MapToApiVersion("1")]
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
            return CreatedAtAction(nameof(GetMessagev2), new { id = geoMessagePost.Id }, geoMessagePost);
        }

        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(
            Summary = "Hämta GeoMessage {Id}",
            Description = "Hämta ett GeoMessage med en specifik {Id}"
            )]
        [Route("{id}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GeoMessage>> GetMessagev2(int id)
        {
            GeoMessage geoMessage = await _context.GeoMessages.FindAsync(id);
            if (geoMessage == null)
            {
                return NotFound();
            }
            return geoMessage;
        }

        // Version 2 starts from here!

        /// <summary>
        /// Ger en lista med alla GeoMessages som skapats
        /// </summary>  
        /// <returns>Lista med GeoMessages</returns>
        /// 

        [AllowAnonymous]
        [HttpGet("GetAllGeoMessages")]
        [SwaggerOperation(
            Summary = "Get GeoMessage",
            Description = ""
            )]
        [MapToApiVersion("2")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Object>>> GetAllGeoMessagesV2(int? minLon, int? minLat, int? maxLon, int? maxLat)
        {
            var TheMessage = await _context.GeoMessages.Where(p => p.Latitude >= minLat && p.Latitude <= maxLat && p.Longitude >= minLon && p.Longitude <= maxLon).ToListAsync();



            if (minLat == null && minLon == null && maxLat == null && maxLon == null)
            {
                return await _context.GeoMessages.ToListAsync();
            }
            else
            {
                return TheMessage;
            }



     
        }
        public class Message
        {
            public string Title { get; set; }
            public string Body { get; set; }
            public string Author { get; set; }

        }

        public class GeoMessageDTO
        {
            public int Id { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public Message Message { get; set; }
        }

        public class GeoMessageDTOv2
        {
            public int Id { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
            public string Author { get; set; }
        }




        //[Authorize]
        [HttpPost]
        [SwaggerOperation(
            Summary = "Skapa ett GeoMessage",
            Description = "Skapa ett GeoMessage med Longitud och Latitud, med tillhörande textmedelande"
            )]
        [Route("CreateNewGeoMessage")]
        [MapToApiVersion("2")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GeoMessageDTOv2>> CreateNewPostAsyncV2(GeoMessageDTOv2 DTO)
        {

            var GeoMessage = new GeoMessage()
            {
                Latitude = DTO.Latitude,
                Longitude = DTO.Longitude,
                Author = "",
                Body = DTO.Body,
                Title = DTO.Title
                
                 
                
            };

            _context.GeoMessages.Add(GeoMessage);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMessagev2), new { id = GeoMessage.Id }, GeoMessage);



        }





        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(
         Summary = "Hämta GeoMessage {Id}",
         Description = "Hämta ett GeoMessage med en specifik {Id}"
         )]
        [Route("{id}")]
        [MapToApiVersion("2")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<object>> GetMessageV2(int id)
        {
            GeoMessage geoMessage = await _context.GeoMessages.FindAsync(id);


            if (geoMessage == null)
            {
                return NotFound();
            }

            GeoMessageDTO GeoMessageDTO = new()
            {
                Id = geoMessage.Id,
                Latitude = geoMessage.Latitude,
                Longitude = geoMessage.Longitude,
                Message = new Message
                {
                    Body = geoMessage.Body,
                    Author = geoMessage.Author,
                    Title = geoMessage.Title,
                }
            };
            

             
            return new JsonResult(GeoMessageDTO);
        }

     



    }
}
