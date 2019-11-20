using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Recarga.Models.ERecargaModels
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Station> Stations { get; set; }
    }
}