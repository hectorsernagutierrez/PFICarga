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
using TipopfihsOntology;
using PersonapfihsOntology;
using SixLabors.ImageSharp;
using System.Security.Policy;
using System.Numerics;

namespace FutbolOntology.CargaPFI
{
    internal class Torneo
    {
        private ResourceApi apiRecursos;
        private string ontologiaTorneo = "torneopfihs";
        private string ontologiaOrganizacion = "organizacionpfihs";
        private string ontologiaClub = "clubpfihs";
        private string ontologiaPersona = "personapfihs";
        private string ontologiaTipo = "tipopfihs";
        private string ontologiaEvento = "eventopfihs";
        private string ontologiaPosicion = "posicionpfihs";


        /// <summary>
        /// </summary>
        /// <param name="api"></param>
        public Torneo(ResourceApi api)
        {
            this.apiRecursos = api;
        }

        public void CargarTorneo(string rutaDirectorioCompetitions, string rutaDirectorioPartido, string rutaDirectorioEvento, string rutaDirectorioClub,string rutaDirectorioPersonaValoracion)
        {
            var service = new DTOService();
            List<CompetitionsDTO> competitions = service.ReadCompetitions(rutaDirectorioCompetitions);
            foreach (var competition in competitions)
            {
                SportsTournament tournament = new SportsTournament();
                tournament.Schema_name = competition.Name;
                tournament.Schema_identifier = competition.CompetitionId;

                //HACER: CAMPEON CONSULTA

                if (!string.IsNullOrEmpty(competition.Confederation))
                {

                    string uri = getOrganizerURl(competition.Confederation);    
                   
                    tournament.IdsSchema_organizer.Add(uri);
                }

                
                tournament.Eschema_subEvent = CargarPartido(rutaDirectorioPartido, rutaDirectorioEvento,rutaDirectorioClub, rutaDirectorioPersonaValoracion, competition);




                apiRecursos.ChangeOntology(ontologiaTorneo);
                ComplexOntologyResource recursoPersona = tournament.ToGnossApiResource(apiRecursos, new List<string> { "Tournament" }, Guid.NewGuid(), Guid.NewGuid());
                apiRecursos.LoadComplexSemanticResource(recursoPersona);
            }

        }







        public  List<SportsEvent> CargarPartido(string rutaDirectorioPartido, string rutaDirectorioEvento, string rutaDirectorioClub,string rutaDirectorioPersonaValoracion, CompetitionsDTO competition)
        {
            var service = new DTOService();
            List<GamesDTO> games = service.ReadGames(rutaDirectorioPartido);
            List<GameLineupsDTO> playeralineados= service.ReadGameLineups(rutaDirectorioEvento);
            List<SportsEvent> partidos = new List<SportsEvent>();
            foreach (var game in games)
            {
                
                if (game.CompetitionId == competition.CompetitionId)
                {
                    SportsEvent sportsEvent = new SportsEvent();
                    sportsEvent.Eschema_identifier_partido = game.GameId;
                    sportsEvent.Eschema_result = $"{game.HomeClubName} : {game.HomeClubGoals}  - {game.AwayClubGoals} : {game.AwayClubName}  ";
                   
                    //AwayTeam
                    TorneopfihsOntology.SportsTeam away = IniciarTeams(rutaDirectorioClub, game.AwayClubId);
                    away.IdsSchema_coach.Add(getManager(game.AwayClubManagerName));
                    away.Eschema_classification = int.TryParse(game.AwayClubPosition, out var result) ? result : (int?)null;
                    PersonLinedUp athlete ;
                    List<PersonLinedUp> listAthletesAway = new List<PersonLinedUp>();
                    List<PersonLinedUp> listAthletesHome = new List<PersonLinedUp>();
                    foreach (var player in playeralineados)
                    {
                        if (player.GameId == game.GameId) {
                            if (player.ClubId == game.AwayClubId)
                        {

                                athlete = getPlayerAlineado(player, rutaDirectorioPersonaValoracion);
                                listAthletesAway.Add(athlete);
                            }
                            else if (player.ClubId == game.HomeClubId) {
                                athlete = getPlayerAlineado(player, rutaDirectorioPersonaValoracion);
                                listAthletesHome.Add(athlete);
                            }
                        }
                    }
                    away.Schema_athlete = listAthletesAway;
                    sportsEvent.Schema_awayTeam = away;
                    
                    //Home
                    TorneopfihsOntology.SportsTeam home = IniciarTeams(rutaDirectorioClub, game.HomeClubId);
                    home.IdsSchema_coach.Add(getManager(game.HomeClubManagerName));
                    home.Eschema_classification = int.TryParse(game.HomeClubPosition, out var result2) ? result2 : (int?)null;
                    home.Schema_athlete = listAthletesHome;
                    sportsEvent.Schema_homeTeam = home;
                    
                    sportsEvent.Schema_subEvent = CargarEventos(rutaDirectorioEvento, game,sportsEvent);





                    partidos.Add(sportsEvent);

                }


            }
            return partidos;
        }


        public TorneopfihsOntology.SportsTeam IniciarTeams(string rutaDirectorioClub, string id)
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
                Club c = new Club(apiRecursos);
                c.CargarTodosClub( rutaDirectorioClub);
                IniciarTeams(rutaDirectorioClub,id);



            }
            TorneopfihsOntology.SportsTeam Team = new TorneopfihsOntology.SportsTeam();
            Team.IdSchema_subOrganization = uri;



            return Team;

        }


        public string getManager(string name)
        {

            SparqlObject resultado = null;
            string uri = "";
            string select = string.Empty, where = string.Empty;
            select += $@"SELECT *";
            where += $@" WHERE {{ ";
            where += $@"?s ?p ?o.";
            where += $@"FILTER(?o LIKE '{name}')";
            //where += $@"FILTER(REGEX(?o, '{nombre}', 'i'))";
            where += $@"}}";
            try
            {
                resultado = apiRecursos.VirtuosoQuery(select, where, ontologiaPersona);
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
                Person person = new Person();
                person.Schema_name = name;

                //Consulta a DBPEDIA datos



                apiRecursos.ChangeOntology(ontologiaPersona);
                ComplexOntologyResource recursoPersona = person.ToGnossApiResource(apiRecursos, new List<string> { "Coach" }, Guid.NewGuid(), Guid.NewGuid());
                uri=apiRecursos.LoadComplexSemanticResource(recursoPersona);


            }
            



            return uri;

        }


        public string getPlayerUrl(string name,string rutaDirectorioPersonaValoracion)
        {

            SparqlObject resultado = null;
            string uri = "";
            string select = string.Empty, where = string.Empty;
            select += $@"SELECT *";
            where += $@" WHERE {{ ";
            where += $@"?s ?p ?o.";
            where += $@"FILTER(?o LIKE '{name}')";
            //where += $@"FILTER(REGEX(?o, '{nombre}', 'i'))";
            where += $@"}}";
            try
            {
                resultado = apiRecursos.VirtuosoQuery(select, where, ontologiaPersona);
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
                List<PlayersDTO> players = new List<PlayersDTO>();
                
                Dictionary<string, PlayersDTO> pares = players.GroupBy(player => player.Name).ToDictionary(group => group.Key, group => group.First());
                PlayersDTO p = pares[name];
                
                
                DTOService service = new DTOService();
                List < PlayerValuationsDTO > playerValuations = service.ReadPlayerValuations(rutaDirectorioPersonaValoracion);
                Persona person1 = new Persona(apiRecursos);
                 uri = person1.CargarPersonaSola(p, playerValuations);

            }




            return uri;

        }

        public string getOrganizerURl(string Organizationname)
        {

            SparqlObject resultado = null;
            string uri = "";


            string select = string.Empty, where = string.Empty;
            select += $@"SELECT *";
            where += $@" WHERE {{ ";
            where += $@"?s ?p ?o.";
            where += $@"FILTER(?o LIKE '{Organizationname}')";
            //where += $@"FILTER(REGEX(?o, '{nombreGenero}', 'i'))";
            where += $@"}}";
            resultado = apiRecursos.VirtuosoQuery(select, where, ontologiaOrganizacion);



            if (resultado != null && resultado.results != null && resultado.results.bindings.Count > 0)
            {
                uri = resultado.results.bindings[0]["s"].value;
            }
            else
            {
                string identificador = Guid.NewGuid().ToString();
                Organization nuevaOrganizacion = new Organization(identificador);
                nuevaOrganizacion.Schema_name = Organizationname;
                apiRecursos.ChangeOntology(ontologiaOrganizacion);

                SecondaryResource generoSR = nuevaOrganizacion.ToGnossApiResource(apiRecursos, $"Organization_{identificador}");
                apiRecursos.LoadSecondaryResource(generoSR);
                uri = nuevaOrganizacion.GNOSSID;
            }

            return uri;
        }

        public string getTipoUrl(string tipoName)
        {
            SparqlObject resultado = null;
            string uri = "";


            string select = string.Empty, where = string.Empty;
            select += $@"SELECT *";
            where += $@" WHERE {{ ";
            where += $@"?s ?p ?o.";
            where += $@"FILTER(?o LIKE '{tipoName}')";
            //where += $@"FILTER(REGEX(?o, '{nombreGenero}', 'i'))";
            where += $@"}}";
            resultado = apiRecursos.VirtuosoQuery(select, where, ontologiaTipo);



            if (resultado != null && resultado.results != null && resultado.results.bindings.Count > 0)
            {
                uri = resultado.results.bindings[0]["s"].value;
            }
            else
            {
                string identificador = Guid.NewGuid().ToString();
                TipopfihsOntology.Thing tipo = new Thing(identificador);
                tipo.Schema_name = tipoName;
                apiRecursos.ChangeOntology(ontologiaTipo);

                SecondaryResource generoSR = tipo.ToGnossApiResource(apiRecursos, $"Type_{identificador}");
                apiRecursos.LoadSecondaryResource(generoSR);
                uri = tipo.GNOSSID;
            }
            return uri;
        }
        public string getPosicionUrl(string tipoName)
        {
            SparqlObject resultado = null;
            string uri = "";


            string select = string.Empty, where = string.Empty;
            select += $@"SELECT *";
            where += $@" WHERE {{ ";
            where += $@"?s ?p ?o.";
            where += $@"FILTER(?o LIKE '{tipoName}')";
            //where += $@"FILTER(REGEX(?o, '{nombreGenero}', 'i'))";
            where += $@"}}";
            resultado = apiRecursos.VirtuosoQuery(select, where, ontologiaPosicion);



            if (resultado != null && resultado.results != null && resultado.results.bindings.Count > 0)
            {
                uri = resultado.results.bindings[0]["s"].value;
            }
            else
            {
                string identificador = Guid.NewGuid().ToString();
               PosicionpfihsOntology.Position pos = new PosicionpfihsOntology.Position(identificador);
                pos.Schema_name = tipoName;
                apiRecursos.ChangeOntology(ontologiaPosicion);

                SecondaryResource generoSR = pos.ToGnossApiResource(apiRecursos, $"Position_{identificador}");
                apiRecursos.LoadSecondaryResource(generoSR);
                uri = pos.GNOSSID;
            }
            return uri;
        }






        public PersonLinedUp getPlayerAlineado(GameLineupsDTO player, string rutaDirectorioPersonaValoracion)
        {
            PersonLinedUp person = new PersonLinedUp();
            person.IdEschema_player = getPlayerUrl(player.PlayerName, rutaDirectorioPersonaValoracion);            
            person.Eschema_bibNumber = int.TryParse(player.NumberString, out var result) ? result : 0;
            person.IdEschema_type = getTipoUrl(player.Type);
            person.IdEschema_position= getTipoUrl(player.Position); 
            return person;
            

        }



        public List<Event> CargarEventos(string rutaDirectorioEvento, GamesDTO game,SportsEvent match)
        {
            var service = new DTOService();
            List<GameEventsDTO> events = service.ReadGameEvents(rutaDirectorioEvento);
            List<Event> eventos = new List<Event>();
            foreach (var evento in events)
            {
               
                if (evento.GameId==game.GameId)
                {
                    Event eventoEvent = new Event();
                    eventoEvent.Eschema_identifier_evento = evento.GameEventId;
                    eventoEvent.Eschema_Minute=evento.Minute;

                    string uri = getTipoUrl(evento.Type);
                    eventoEvent.IdSchema_about = uri;
                  
                    List<PersonLinedUp> p = match.Schema_homeTeam.Schema_athlete;
                    List<PersonLinedUp> c=(match.Schema_awayTeam.Schema_athlete);
                  foreach(PersonLinedUp player in p.Concat<PersonLinedUp>(c))
                    {
                        if (player.Eschema_player.Schema_identifier == evento.PlayerId)
                        {
                            eventoEvent.Schema_actor = player;
                        }
                    }
                    eventos.Add(eventoEvent);   
                }


            }
            return eventos;
        }










        

    }

        
    }
