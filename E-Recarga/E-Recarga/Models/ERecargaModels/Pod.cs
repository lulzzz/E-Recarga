using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace E_Recarga.Models.ERecargaModels
{
    public class Pod
    {
        [ForeignKey("Station")]
        public int StationID { get; set; }
        public Station Station { get; set; }

        [Key]
        [Column(Order = 1)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 2)]
        public int Identifier{ get; set; }
        public PodType Type { get; set; }
        public IList<Appointment> Appointments { get; set; }
    }
}