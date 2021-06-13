using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        [HttpGet("geo-comments")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Object>>> GetAllGeoMessages()
        {


            return await _context.GeoMessages.Select(p => new
            {
                Message = p.Body,
                Latitude = p.Latitude,
                Longitude = p.Longitude
            }).ToListAsync();

        }

        public class GeoMessagev1DTO
        {
            public string Message { get; set; }
            public double longitude { get; set; }
            public double latitude { get; set; }
        }

        //[Authorize]
        [HttpPost]
        [SwaggerOperation(
            Summary = "Skapa ett GeoMessage",
            Description = "Skapa ett GeoMessage med Longitud och Latitud, med tillhörande textmedelande"
            )]
        [Route("geo-comments")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GeoMessagev1DTO>> CreateNewPostAsync(GeoMessagev1DTO DTO)
        {

            var geoMessagePost = new GeoMessage
            {
                Longitude = DTO.longitude,
                Latitude = DTO.latitude,
                Body = DTO.Message,
            };
            var GeoMessageV1DTOresponse = PostGeoMessageToDTOV1(geoMessagePost);
            await _context.AddAsync(geoMessagePost);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMessagev2), new { id = geoMessagePost.Id }, GeoMessageV1DTOresponse);
        }
        static GeoMessagev1DTO PostGeoMessageToDTOV1(GeoMessage geoMessage)
        {
            return new GeoMessagev1DTO
            {
                Message = geoMessage.Body,
                longitude = geoMessage.Longitude,
                latitude = geoMessage.Latitude
            };
        }

        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(
            Summary = "Hämta GeoMessage {Id}",
            Description = "Hämta ett GeoMessage med en specifik {Id}"
            )]
        [Route("geo-comments/{id}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GeoMessagev1DTO>> GetMessagev2(int id)
        {
            GeoMessage geoMessage = await _context.GeoMessages.FindAsync(id);

            if (geoMessage == null)
            {
                return NotFound();
            }
            else
            {
            var geoMessagePost = new GeoMessagev1DTO
            {
                longitude = geoMessage.Longitude,
                latitude =  geoMessage.Longitude,
                Message =   geoMessage.Body
            };
            return geoMessagePost;
            }
        }

        // Version 2 starts from here!

        /// <summary>
        /// Ger en lista med alla GeoMessages som skapats
        /// </summary>  
        /// <returns>Lista med GeoMessages</returns>
        /// 

        [AllowAnonymous]
        [HttpGet("geo-comments")]
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
            public Message Message { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }


        //För att dölja Author i post body i swagger
        public class GeoMessageDTOPost
        {
            public MessageDTOPost Message { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
        public class MessageDTOPost
        {
            public string Title { get; set; }
            public string Body { get; set; }
        }

        //[Authorize]
        [HttpPost]
        [SwaggerOperation(
            Summary = "Skapa ett GeoMessage",
            Description = "Skapa ett GeoMessage med Longitud och Latitud, med tillhörande textmedelande"
            )]
        [Route("geo-comments")]
        [MapToApiVersion("2")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GeoMessageDTOPost>> CreateNewPostAsyncV2(GeoMessageDTOPost DTO)
        {
            var userId = User.Identity.GetUserId();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var author = user.FirstName + " " + user.LastName;

            var geoMessage = new GeoMessage()
            {
                Author = author,
                Body = DTO.Message.Body,
                Title = DTO.Message.Title,
                Longitude = DTO.Longitude,
                Latitude = DTO.Latitude,
            };
            var GeoMessageDTOresponse = PostGeoMessageToDTO(geoMessage);
            _context.GeoMessages.Add(geoMessage);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMessagev2), new { id = geoMessage.Id, }, GeoMessageDTOresponse);
        }

        static GeoMessageDTO PostGeoMessageToDTO(GeoMessage geoMessage)
        {
            return new GeoMessageDTO
            {
                Message = new Message
                {
                    Title = geoMessage.Title,
                    Author = geoMessage.Author,
                    Body = geoMessage.Body,
                },
                Longitude = geoMessage.Longitude,
                Latitude = geoMessage.Latitude
            };
        }

        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(
         Summary = "Hämta GeoMessage {Id}",
         Description = "Hämta ett GeoMessage med en specifik {Id}"
         )]
        [Route("geo-comments/{id}")]
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
                Message = new Message
                {
                    Body = geoMessage.Body,
                    Author = geoMessage.Author,
                    Title = geoMessage.Title,
                },
                //Id = geoMessage.Id,
                Latitude = geoMessage.Latitude,
                Longitude = geoMessage.Longitude,
            };



            return new JsonResult(GeoMessageDTO);
        }





    }
}
