using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FutbolOntology.DTO;
using Gnoss.ApiWrapper;

namespace FutbolOntology.CargaPFI
{
    internal class ValoracionesMercado
    {
        private ResourceApi apiRecursos;
        private string ontologiaPelicula = "peliculahectors";
        private string ontologiaPersona = "personahectors";
        private string ontologiaGenero = "generohectors";
        //string playerValuationsFile = @"Dataset/player_valuations.csv";
        //string playerValuationsFilePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, playerValuationsFile);


        /// <summary>
        /// </summary>
        /// <param name="api"></param>
        public ValoracionesMercado(ResourceApi api)
        {
            this.apiRecursos = api;
        }

        public void CargarValoracionesMercado(string rutaDirectorio)
        {
            var service = new DTOService();
            List<PlayerValuationsDTO> playerValuations = service.ReadPlayerValuations(rutaDirectorio);







        }

    }
}
