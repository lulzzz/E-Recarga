using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Recarga.Models.ERecargaModels
{
    public class Station
    {
        public Company Company { get; set; }

        public int Id { get; set; }
        public string ComercialName { get; set; }
        public long Longitude { get; set; }
        public long Latitude { get; set; }
        public string Address { get; set; }
        public List<Pod> Pods { get; set; }
    }
}