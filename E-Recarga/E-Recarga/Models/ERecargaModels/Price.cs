using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace E_Recarga.Models.ERecargaModels
{
    public class Price
    {
        [ForeignKey("Station")]
        public int StationId { get; set; }
        public Station Station { get; set; }

        [Key]
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public double Cost { get; set; }
    }
}