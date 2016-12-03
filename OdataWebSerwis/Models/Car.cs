using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OdataWebSerwis.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string Colour { get; set; }
        public decimal Price { get; set; }


    }
}