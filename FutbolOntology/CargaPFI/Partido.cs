using ClubpfihsOntology;
using FutbolOntology.DTO;
using Gnoss.ApiWrapper.Model;
using Gnoss.ApiWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TorneopfihsOntology;
using OrganizacionpfihsOntology;
using Gnoss.ApiWrapper.ApiModel;
using static GnossBase.GnossOCBase;
using FutbolOntology.CargaPFI;

namespace FutbolOntology.CargaPFI
{
    internal class Partido
    {

        private ResourceApi apiRecursos;
        private string ontologiaTorneo = "torneopfihs";
        private string ontologiaOrganizacion = "organizacionpfihs";
        



        /// <summary>
        /// </summary>
        /// <param name="api"></param>
        public Partido(ResourceApi api)
        {
            this.apiRecursos = api;
        }

        public void CargarPartido(string rutaDirectorioPartido, string competitionId)
        {
            var service = new DTOService();
            List<GamesDTO> games = service.ReadGames(rutaDirectorioPartido);
            List<SportsEvent> partidos = new List<SportsEvent>();   
            foreach (var game in games)
            {
                if(game == null) continue;
                if (game.CompetitionId == competitionId)
                {
                    SportsEvent sportsEvent = new SportsEvent();
                    sportsEvent.Eschema_identifier_partido = game.GameId;
                    //sportsEvent.Eschema_result=
                    sportsEvent.Schema_subEvent = CargarEventos();

                    partidos.Add(sportsEvent);

                }

                
            }

        }


    }
}
