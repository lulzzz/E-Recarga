using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Recarga.Models.ERecargaModels
{
    public class Price
    {
        public Station Station { get; set; }

        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public double Cost { get; set; }
    }
}