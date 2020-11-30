using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PruebaPuntoTicket.Models;

namespace PruebaPuntoTicket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        List<Especie> lstEspecies = new List<Especie>();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            LlenarEspecies();
        }

        //Lista de subespecies a crear
        List<int> LstSubEspecies = new List<int>() { 0, 0, 0, 1, 1, 1, 2, 3 };

        List<string> lstFisiologia = new List<string>() { "Anfibio", "Mamifero", "Molusco" };

        Random random = new Random();

        private void LlenarEspecies()
        {
            //Se forman 3 especies inicialmente
            for (int i = 1; i < 4; i++)
            {
                Especie newEspecie = new Especie();
                int cantidadSubEspecie = LstSubEspecies[random.Next(LstSubEspecies.Count())];

                newEspecie.Categoria = i.ToString();
                newEspecie.Fisiologia = lstFisiologia[random.Next(lstFisiologia.Count())];

                if (cantidadSubEspecie > 0)
                {
                    newEspecie.LstSubEspecies = LlenarSubEspecie(newEspecie, cantidadSubEspecie);

                    LstSubEspecies.Add(0);
                }

                lstEspecies.Add(newEspecie);
            }
        }

        private List<Especie> LlenarSubEspecie(Especie especie, int cantidadSubEspecies)
        {
            List<Especie> lstNewSubEspecies = new List<Especie>();

            for (int i = 1; i <= cantidadSubEspecies; i++)
            {
                LstSubEspecies.Add(0);

                Especie newEspecie = new Especie();

                newEspecie.Categoria = especie.Categoria + "." + i.ToString();
                newEspecie.Fisiologia = especie.Fisiologia;

                int newCantidadSubEspecie = LstSubEspecies[random.Next(LstSubEspecies.Count())];

                if (newCantidadSubEspecie > 0)
                {
                    newEspecie.LstSubEspecies = LlenarSubEspecie(newEspecie, newCantidadSubEspecie);
                }

                lstNewSubEspecies.Add(newEspecie);
            }

            for (int j = 0; j <= cantidadSubEspecies; j++)
            {
                LstSubEspecies.Remove(0);
            }

            return lstNewSubEspecies;
        }

        [HttpGet]
        public IEnumerable<Especie> Get([FromQuery] string busqueda)
        {

            if (busqueda == null)
            {
                return null;
            }
            var separacion = busqueda.Split(".");


            if (separacion.Length > 0)
            {
                try
                {
                    var filterLstEspecies = lstEspecies.FirstOrDefault(x => x.Categoria.Contains(busqueda)) ?? lstEspecies.SelectMany(product => product.LstSubEspecies).Where(y => y.Categoria.Contains(busqueda)).FirstOrDefault();

                    List<Especie> newList = new List<Especie>();

                    if (filterLstEspecies == null)
                    {
                        return newList;
                    }

                    newList.Add(filterLstEspecies);
                    return newList;
                }
                catch
                {
                    List<Especie> lstException = new List<Especie>();
                    return lstException;
                }
            }
            
            return lstEspecies;
        }
    }
}
