using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projektarbete_WebApi_EJ_JA.Models
{
    namespace V1
    {
        public class GeoMessage
        {
            public int Id { get; set; }
            public string Message { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
        }

    } 
    namespace V2
    {
        public class GeoMessage
        {
            public int Id { get; set; }
            public string Message { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
        }
    }
}
