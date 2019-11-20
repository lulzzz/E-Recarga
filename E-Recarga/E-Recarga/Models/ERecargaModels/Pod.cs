using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Recarga.Models.ERecargaModels
{
    public enum PodType {
        Normal,
        Fast
    }

    public class Pod
    {
        public Station Station { get; set; }

        public int Id { get; set; }
        public PodType Type { get; set; }
        //If we add this, it's probably going to create a new table just for this link M to N
        //public List<Appointment> Appointments { get; set; }
    }
}