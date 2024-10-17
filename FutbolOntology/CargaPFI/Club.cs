using FutbolOntology.DTO;
using Gnoss.ApiWrapper.Model;
using Gnoss.ApiWrapper;
using PersonapfihsOntology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ClubpfihsOntology;

namespace FutbolOntology.CargaPFI
{
    public class Club
    {
        private ResourceApi apiRecursos;
        private string ontologiaClub = "clubpfihs";



        /// <summary>
        /// </summary>
        /// <param name="api"></param>
        public Club(ResourceApi api)
        {
            this.apiRecursos = api;
        }

        public void CargarTodosClub(string rutaDirectorioClub)
        {
            var service = new DTOService();
            List<ClubsDTO> clubs = service.ReadClubs(rutaDirectorioClub);
            foreach (var club in clubs)
            {
                //Busqueda sparl para no añadir los que ya están.


                SportsClub sportsClub = new SportsClub();
                sportsClub.Schema_identifier = club.ClubId;
                sportsClub.Schema_name = club.Name;


                //Consultas                 
                //sportsClub.Schema_logo =  club.
                //sportsClub.Schema_foundingDate=club
                //ClubpfihsOntology.PostalAddress
                //sportsClub.Schema_award=



                apiRecursos.ChangeOntology(ontologiaClub);
                ComplexOntologyResource recursoPersona = sportsClub.ToGnossApiResource(apiRecursos, new List<string> { "Club" }, Guid.NewGuid(), Guid.NewGuid());
                apiRecursos.LoadComplexSemanticResource(recursoPersona);
            }



        }
    }

        
}
