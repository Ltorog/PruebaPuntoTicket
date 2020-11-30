using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaPuntoTicket.Models
{
    public class Especie
    {
        public string Categoria { get; set; }
        public string Fisiologia { get; set; }
        public List<Especie> LstSubEspecies { get; set; }
    }
}
