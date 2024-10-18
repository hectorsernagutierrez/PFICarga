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
using Gnoss.ApiWrapper.ApiModel;

namespace FutbolOntology.CargaPFI
{
    public class Club
    {
        private ResourceApi apiRecursos;
        private string ontologiaClub = "clubpfihs";
        private string ontologiaTemporada = "temporadapfihs";



        /// <summary>
        /// </summary>
        /// <param name="api"></param>
        public Club(ResourceApi api)
        {
            this.apiRecursos = api;
        }

        public void CargarTodosClub(string rutaDirectorioClub, string rutaDirectorioPersona, string rutaDirectorioValoracion)
        {
            var service = new DTOService();
            List<ClubsDTO> clubs = service.ReadClubs(rutaDirectorioClub);
            foreach (var club in clubs)
            {
                //Busqueda sparl para no añadir los que ya están.


                SportsClub sportsClub = new SportsClub();
                sportsClub.Schema_identifier = club.ClubId;
                sportsClub.Schema_name = club.Name;
                sportsClub.Schema_parentOrganization = Plantilla(rutaDirectorioPersona, rutaDirectorioValoracion, club.Name);



                //HACER: SPAQL
                //Consultas                 
                //sportsClub.Schema_logo =  club.
                //sportsClub.Schema_foundingDate=club
                //ClubpfihsOntology.PostalAddress
                //sportsClub.Schema_award=
                //sportsClub.Schema_alternateName


                apiRecursos.ChangeOntology(ontologiaClub);
                ComplexOntologyResource recursoPersona = sportsClub.ToGnossApiResource(apiRecursos, new List<string> { "Club" }, Guid.NewGuid(), Guid.NewGuid());
                apiRecursos.LoadComplexSemanticResource(recursoPersona);
            }





        }

        public List<SportsTeam> Plantilla(string rutaDirectorioPersona , string rutaDirectorioValoracion, string clubName) {

            
            var service = new DTOService();
            List<PlayersDTO> players = service.ReadPlayers(rutaDirectorioPersona);
            

            // Crear el diccionario usando un Tuple como clave
            var grupos = players
                .GroupBy(p => new { p.LastSeason, p.CurrentClubName })
                .ToDictionary(
                    g => Tuple.Create(g.Key.CurrentClubName, g.Key.LastSeason), // La clave es un Tuple de Club y Temporada
                    g => new ClubTemporadaDTO
                    {
                        Temporada = g.Key.LastSeason,
                        Club = g.Key.CurrentClubName,
                        Jugadores = g.ToList()
                    }
                );
            List<SportsTeam> plantillas = new List<SportsTeam>();
            SportsTeam planti = new SportsTeam();   
            foreach (var key in grupos.Keys.Where(k => k.Item1 == clubName))  // Filtrar claves donde el club es "Manchester City"
            {
                var clubTemporada = grupos[key]; // Obtener el valor del diccionario
                
                planti.Eschema_identifier = "";
                planti.IdEschema_season = getTemporadaUrl(clubTemporada.Temporada);
                
                List<string> jugadoresuri = new List<string>();

                foreach (var jugador in clubTemporada.Jugadores)
                {
                    Torneo t = new Torneo(apiRecursos);
                    jugadoresuri.Add(t.getPlayerUrl(jugador.Name, rutaDirectorioValoracion));
                }
                planti.IdsSchema_athlete = jugadoresuri;
                plantillas.Add(planti);
            }


            return plantillas; 
        
        }


        public string getTemporadaUrl(string temporadaName)
        {
            SparqlObject resultado = null;
            string uri = "";


            string select = string.Empty, where = string.Empty;
            select += $@"SELECT *";
            where += $@" WHERE {{ ";
            where += $@"?s ?p ?o.";
            where += $@"FILTER(?o LIKE '{temporadaName}')";
            //where += $@"FILTER(REGEX(?o, '{nombreGenero}', 'i'))";
            where += $@"}}";
            resultado = apiRecursos.VirtuosoQuery(select, where, ontologiaTemporada);



            if (resultado != null && resultado.results != null && resultado.results.bindings.Count > 0)
            {
                uri = resultado.results.bindings[0]["s"].value;
            }
            else
            {
                string identificador = Guid.NewGuid().ToString();
                TemporadapfihsOntology.Thing temporada = new TemporadapfihsOntology.Thing(identificador);
                temporada.Schema_name = temporadaName;
                apiRecursos.ChangeOntology(ontologiaTemporada);

                SecondaryResource generoSR = temporada.ToGnossApiResource(apiRecursos, $"Season_{identificador}");
                apiRecursos.LoadSecondaryResource(generoSR);
                uri = temporada.GNOSSID;
            }
            return uri;
        }




    }



        
}
public class ClubTemporadaDTO
{
    public string Temporada { get; set; }
    public string Club { get; set; }
    public List<PlayersDTO> Jugadores { get; set; }
}