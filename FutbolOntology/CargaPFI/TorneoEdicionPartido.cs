using ClubpfihsOntology;
using FutbolOntology.DTO;
using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FutbolOntology.CargaPFI
{
    internal class TorneoEdicionPartido
    {
        private ResourceApi apiRecursos;
        private Torneo thelper;
        private DTOService servicioDTO;
        private string ontologiaTorneo = "torneopfihs";
        private string ontologiaTorneoedicion = "torneoedicionpfihs";
        private string ontologiaPartido = "partidopfihs";
        private string ontologiaOrganizacion = "organizacionpfihs";
        private string ontologiaClub = "clubpfihs";
        private string ontologiaPersona = "personapfihs";
        private string ontologiaTipo = "tipopfihs";
        private string ontologiaEvento = "eventopfihs";
        private string ontologiaPosicion = "posicionpfihs";
        #region rutas
    
       private  string alineacionesPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"Dataset/game_lineups.csv");
      





        #endregion rutas
        /// <summary>
        /// </summary>
        /// <param name="api"></param>
        public TorneoEdicionPartido(ResourceApi api)
        {
            this.apiRecursos = api;
            this.thelper = new Torneo(api);
             this.servicioDTO = new DTOService();
        }

		/// <summary>
		/// Cargo todos los torneos que se encuentran en el archivo de competiciones
		/// </summary>
		/// <param name="compePath"></param>
		/// <param name="rutaPartidos"></param>
		public void CargarTodosTorneos(string compePath, string rutaPartidos)
        {
            
              List<CompetitionsDTO> TodasCompeticionesDTO =servicioDTO.ReadCompetitions(compePath);
                foreach(var competicion in TodasCompeticionesDTO)
            {
                 TorneopfihsOntology.SportsTournament torneo = new TorneopfihsOntology.SportsTournament();
                torneo.Schema_identifier = competicion.CompetitionId;
                torneo.Schema_name = competicion.CompetitionCode.Replace("-", "  ").ToUpper();
                torneo.Schema_description=competicion.Url;
               
                List<string> organizersuris = new List<string>();
                organizersuris.Add(thelper.getOrganizerURl(competicion.Confederation));
                torneo.IdsSchema_organizer = organizersuris;

                Console.WriteLine("Empiezo a cargar temporadas de " + torneo.Schema_name + " " + torneo.Schema_identifier);                
                torneo.IDsEschema_subEvent = CargarTemporadas(torneo.Schema_identifier, 2018, 2024, rutaPartidos,torneo.Schema_name);
                Console.WriteLine("Termino de cargar temporadas de " + torneo.Schema_name + " " + torneo.Schema_identifier);
                Console.WriteLine("Subo  " + torneo.Schema_name + " " + torneo.Schema_identifier);
                apiRecursos.ChangeOntology(ontologiaTorneo);
                ComplexOntologyResource recursotorneo = torneo.ToGnossApiResource(apiRecursos, new List<string> { "Tournament" }, Guid.NewGuid(), Guid.NewGuid());
                apiRecursos.LoadComplexSemanticResource(recursotorneo);
            }
        }

		/// <summary>
		///  Carga todas las ediciones de un torneo.
		/// Solo las comprendidas entre TempInic y TempFinal.
		/// </summary>
		/// <param name="compId"></param>
		/// <param name="TempInic"></param>
		/// <param name="TempFinal"></param>
		/// <param name="gamePath"></param>
		/// <param name="nombreTorneo"></param>
		/// <returns></returns>
		public List<string> CargarTemporadas(string compId, int TempInic, int TempFinal, string gamePath,string nombreTorneo)
        {
            
             List<string> urisEdicionesTorneos = new List<string>();        
             List<GamesDTO> partidos=  servicioDTO.ReadGames(gamePath);
            string uri = "";
            var agrupartidos = partidos.GroupBy(p => new { p.CompetitionId, p.Season }).ToDictionary(
                    g => Tuple.Create(g.Key.CompetitionId, g.Key.Season), // La clave es un Tuple de Club y Temporada
                    g => new EDICIONDTO
                    {
                        Temporada = g.Key.Season,
                        IdCompe = g.Key.CompetitionId,                       
                        Partidos = g.ToList()
                    }
                );

           for ( int Tactual = TempInic ; Tactual < TempFinal; Tactual++)
            {
                 TorneoedicionpfihsOntology.SportsTournamentEdition EdicionT = new TorneoedicionpfihsOntology.SportsTournamentEdition();
                Club c = new Club(this.apiRecursos);
                EdicionT.IdEschema_season = c.getTemporadaUrl(Tactual.ToString());
                EdicionT.Schema_name = nombreTorneo + " " + Tactual.ToString();
                EdicionT.Schema_identifier = compId + Tactual.ToString();
                List<string> uriParticipantes = new List<string>();
                string uriWinner;
              var k =    Tuple.Create(compId, Tactual.ToString());             
                
                EDICIONDTO gamesEdicion = agrupartidos[k];
                List<GamesDTO> games = gamesEdicion.Partidos;
                EdicionT.IdsEschema_subEvent = CargarPartidos(games, out uriParticipantes, out uriWinner);
                EdicionT.IdsEschema_particpants = uriParticipantes;
                EdicionT.IdEschema_winner = uriWinner;
                apiRecursos.ChangeOntology(ontologiaTorneoedicion);
                ComplexOntologyResource recursoTorneoedicion = EdicionT.ToGnossApiResource(apiRecursos, new List<string> { "Federation" }, Guid.NewGuid(), Guid.NewGuid());
                uri= apiRecursos.LoadComplexSemanticResource(recursoTorneoedicion);
                urisEdicionesTorneos.Add(uri);
            }

            return urisEdicionesTorneos;
        }


		/// <summary>
		/// Cargo todos los partidos de una edición de un torneo.
		/// </summary>
		/// <param name="games"></param>
		/// <param name="uriParticipantes"></param>
		/// <param name="uriWinner"></param>
		/// <returns></returns>
		public List<string> CargarPartidos(List<GamesDTO>  games, out List<string> uriParticipantes, out string uriWinner)
        {
            List<GameLineupsDTO> playeralineados = servicioDTO.ReadGameLineups(alineacionesPath);
            List<string> urisPartidos = new List<string>();
            string uri = "";
             uriWinner = "";
            uriParticipantes = new List<string>();
            foreach ( var g in games)
            {
                 PartidopfihsOntology.SportsEvent partido = new PartidopfihsOntology.SportsEvent();
                partido.Eschema_identifierpartido = g.GameId;
                partido.Schema_date = g.Date;
                partido.Eschema_result = g.HomeClubName + " : " + g.HomeClubGoals.ToString() + "----" + g.AwayClubGoals.ToString() + " : " + g.AwayClubName;
                partido.Schema_subEvent = cargarEventos(g.GameId);

                var agruplayer = playeralineados.GroupBy(p => new { p.GameId,p.ClubId,p.PlayerId }).ToDictionary(
                    g => Tuple.Create(g.Key.GameId,g.Key.ClubId, g.Key.PlayerId), 
                    g => new PLAYERALINEADODTO
                    {
                        gameId = g.Key.GameId,
                        clubId = g.Key.ClubId,
                        PlayerId = g.Key.PlayerId,
                        playerAlineados = g.ToList()
                    }
                );
                List<PartidopfihsOntology.PersonLinedUp> listaAlineadosaway = new List<PartidopfihsOntology.PersonLinedUp>();
                foreach (var key in agruplayer.Keys.Where(k => k.Item1 == g.GameId).Where(k => k.Item2 == g.AwayClubId))  // Filtrar claves 
                {
                    
                    var playeralineado = agruplayer[key]; // Obtener el valor del diccionario
                    List<GameLineupsDTO> lal = playeralineado.playerAlineados;
                    foreach(var pal in lal)
                    {
                        PartidopfihsOntology.PersonLinedUp pa = new PartidopfihsOntology.PersonLinedUp();
                        pa.Eschema_bibNumber = pal.NumberString;
                        pa.IdEschema_player = thelper.getPlayerUrl(pal.PlayerId, pal.PlayerName, alineacionesPath);
                        pa.IdEschema_position = thelper.getPosicionUrl(pal.Position);
                        pa.IdEschema_type = thelper.getTipoUrl(pal.Type);
                        listaAlineadosaway.Add(pa);
                    }
                }




                PartidopfihsOntology.SportsTeam awayt = IniciarTeams(g.HomeClubId, g.HomeClubName);
                awayt.IdsSchema_coach.Add(thelper.getManager(g.HomeClubManagerName));
                awayt.Eschema_classification = int.TryParse(g.HomeClubPosition, out var result2) ? result2 : (int?)-1;
                awayt.Schema_athlete = listaAlineadosaway;
                partido.Schema_awayTeam = awayt;

                List<PartidopfihsOntology.PersonLinedUp> listaAlineadoshome = new List<PartidopfihsOntology.PersonLinedUp>();
                foreach (var key in agruplayer.Keys.Where(k => k.Item1 == g.GameId).Where(k => k.Item2 == g.HomeClubId))  // Filtrar claves 
                {

                    var playeralineado = agruplayer[key]; // Obtener el valor del diccionario
                    List<GameLineupsDTO> lal = playeralineado.playerAlineados;
                    foreach (var pal in lal)
                    {
                        PartidopfihsOntology.PersonLinedUp pa = new PartidopfihsOntology.PersonLinedUp();
                        pa.Eschema_bibNumber = pal.NumberString;
                        pa.IdEschema_player = thelper.getPlayerUrl(pal.PlayerId, pal.PlayerName, alineacionesPath);
                        pa.IdEschema_position = thelper.getPosicionUrl(pal.Position);
                        pa.IdEschema_type = thelper.getTipoUrl(pal.Type);
                        listaAlineadoshome.Add(pa);
                    }
                }

                PartidopfihsOntology.SportsTeam home = IniciarTeams( g.HomeClubId, g.HomeClubName);
                home.IdsSchema_coach.Add(thelper.getManager(g.HomeClubManagerName));
                home.Eschema_classification = int.TryParse(g.HomeClubPosition, out var result3) ? result3 : (int?)-1;
                home.Schema_athlete = listaAlineadoshome;
                partido.Schema_homeTeam = home;

                apiRecursos.ChangeOntology(ontologiaPartido);
                ComplexOntologyResource recursoPartido = partido.ToGnossApiResource(apiRecursos, new List<string> { "UEFA(Europe)" }, Guid.NewGuid(), Guid.NewGuid());
                uri = apiRecursos.LoadComplexSemanticResource(recursoPartido);

                urisPartidos.Add(uri);
               
            }

            return urisPartidos;
        }

		/// <summary>
		/// Inicio las alineaciones de los equipos que participan een un partido
		/// </summary>
		/// <param name="id"></param>
		/// <param name="clubName"></param>
		/// <returns></returns>
		public PartidopfihsOntology.SportsTeam IniciarTeams( string id,string clubName)
        {

            SparqlObject resultado = null;
            string uri = "";
            string select = string.Empty, where = string.Empty;
            select += $@"SELECT *";
            where += $@" WHERE {{ ";
            where += $@"?s ?p ?o.";
            where += $@"FILTER(?o LIKE '{id}')";
            //where += $@"FILTER(REGEX(?o, '{nombre}', 'i'))";
            where += $@"}}";
            try
            {
                resultado = apiRecursos.VirtuosoQuery(select, where, ontologiaClub);
            }
            catch (Exception ex)
            {
                //resultado = apiRecursos.VirtuosoQuery(select, where, ontologiaPersona);

            }



            if (resultado != null && resultado.results != null && resultado.results.bindings.Count > 0)//Si existe
            {
                uri = resultado.results.bindings[0]["s"].value;
            }
            else
            {
                ClubpfihsOntology.SportsClub c = new ClubpfihsOntology.SportsClub();
                c.Schema_identifier = id;
                c.Schema_name = clubName;

                apiRecursos.ChangeOntology(ontologiaClub);
                ComplexOntologyResource recursoClub = c.ToGnossApiResource(apiRecursos, new List<string> { "Club" }, Guid.NewGuid(), Guid.NewGuid());
                uri= apiRecursos.LoadComplexSemanticResource(recursoClub);             



            }
            PartidopfihsOntology.SportsTeam Team = new PartidopfihsOntology.SportsTeam();
            Team.IdSchema_subOrganization = uri;



            return Team;

        }

		/// <summary>
		///  Cargo los eventos que acontecen en un partido de futbol
		/// </summary>
		/// <param name="gameIde"></param>
		/// <returns></returns>
		public List<PartidopfihsOntology.Event> cargarEventos( string gameIde)
        {
            List<PartidopfihsOntology.Event> listaEventos = new List<PartidopfihsOntology.Event>();

            string gameEventsFile = @"Dataset/game_events.csv";
            string rutaEventos = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, gameEventsFile);
            List<GameEventsDTO> levdto= servicioDTO.ReadGameEvents(rutaEventos);

            var agruEven = levdto.GroupBy(p => new { p.GameId }).ToDictionary(
                    g => Tuple.Create(g.Key.GameId),
                    g => new EVENTDTO
                    {
                        gameId = g.Key.GameId,                        
                        eventosPartido = g.ToList()
                    }
                );
            var k = Tuple.Create(gameIde);
            EVENTDTO even = agruEven[k];
            List<GameEventsDTO> levdto2 = even.eventosPartido;
            foreach(var ge in levdto2)
            {
                PartidopfihsOntology.Event e = new PartidopfihsOntology.Event();
                e.Eschema_identifierevento=ge.GameEventId;
                e.IdSchema_about = thelper.getTipoUrl(ge.Type);
                e.Eschema_Minute = ge.Minute;
                e.IdSchema_actor=thelper.getPlayerUrl(ge.PlayerId,ge.PlayerId, "");
                listaEventos.Add(e);
            }
            return listaEventos;
        }


    }
}
/// <summary>
/// DTOs para la agrupación de partidospor temporadas  y competiciones
/// </summary>
public class EDICIONDTO
{
    public string Temporada { get; set; }
    public string IdCompe { get; set; }
       public List<GamesDTO> Partidos { get; set; }
}
/// <summary>
/// DTO para ka agrupación de alineaciones de jugadores por club, partido y jugador
/// </summary>
public class PLAYERALINEADODTO
{
    public string gameId { get; set; }
    public string PlayerId { get; set; }
    public string clubId { get; set; }
    
    public List<GameLineupsDTO> playerAlineados { get; set; }
}
/// <summary>
/// DTO para la agrupación de eventos por partido
/// </summary>
public class EVENTDTO
{
    public string gameId { get; set; }    

    public List<GameEventsDTO> eventosPartido { get; set; }
}

