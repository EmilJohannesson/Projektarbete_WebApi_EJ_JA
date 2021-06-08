using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Projektarbete_WebApi_EJ_JA.Controllers.GeoController;

namespace Projektarbete_WebApi_EJ_JA.Models
{
    public class GeoMessage
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Body { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }


    }
    public class GeoMessagev2
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }


    }




}
