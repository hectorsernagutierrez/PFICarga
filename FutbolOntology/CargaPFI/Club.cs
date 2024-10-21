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
using FutbolOntology.SPARQL;
using Serilog;

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
            int i = 0;
            var service = new DTOService();
            List<ClubsDTO> clubs = service.ReadClubs(rutaDirectorioClub);
            // Dictionary<string, Dictionary<string, object>> diccionarioGrande = ServiceWIKIDATA.LeerClub2(3000, 0);
            // Dictionary<string, object> diccionariopeque = new Dictionary<string, object>();



            var entrenadoresDiccionario = ServiceWIKIDATA.ObtenerEntrenadoresPorClub(10000, 0);
            var nombresAlternativosDiccionario = ServiceWIKIDATA.ObtenerTodasLosNombresAlternativos();
            var premiosDiccionario = ServiceWIKIDATA.ObtenerTodosLosPremios();
            var direccionesPostalesDiccionario = ServiceWIKIDATA.ObtenerTodasLasDireccionesPostales();
            var logotiposDiccionario = ServiceWIKIDATA.ObtenerTodosLosLogotipos();
            var wikipediaDiccionario = ServiceWIKIDATA.ObtenerTodosLosArticulosWikipedia();
            var fundacionDiccionario = ServiceWIKIDATA.ObtenerFechasFundacionMasAntiguas();
            // Obtener la ruta base de la aplicación


            Random rnd = new Random();

           


            foreach (var club in clubs.OrderBy(x => rnd.Next()))
            {

                try
                {


                    //Busqueda sparl para no añadir los que ya están.
                    string select = string.Empty, where = string.Empty;
                    select += $@"SELECT *";
                    where += $@" WHERE {{ ";
                    where += $@"?s ?p ?o.";
                    where += $@"FILTER(?o LIKE '{club.Name}')";
                    //where += $@"FILTER(REGEX(?o, '{nombreGenero}', 'i'))";
                    where += $@"}}";
                    SparqlObject resultado = apiRecursos.VirtuosoQuery(select, where, ontologiaClub);



                    if (resultado != null && resultado.results != null && resultado.results.bindings.Count > 0)
                    {

                        Console.WriteLine("Elemento ya en BD");

                    }
                    else
                    {

                        SportsClub sportsClub = new SportsClub();
                        sportsClub.Schema_identifier = club.ClubId;
                        sportsClub.Schema_name = club.Name;
                        Dictionary<string, List<DateTime>> listaEntr = new Dictionary<string, List<DateTime>>();

                        string nombreBuscado = club.Name;
                        string clubClave = null;

                        // Primero buscar en nombres de clubes principales
                        if (nombresAlternativosDiccionario.ContainsKey(nombreBuscado))
                        {
                            clubClave = nombreBuscado;
                        }
                        else
                        {
                            // Si no está como nombre principal, buscar en los nombres alternativos
                            foreach (var entry in nombresAlternativosDiccionario)
                            {
                                if (entry.Value.Contains(nombreBuscado))
                                {
                                    clubClave = entry.Key;
                                    break;
                                }
                            }
                        }

                        // Si no encontramos el club, devolver null
                        if (clubClave == null)
                        {
                            Console.WriteLine($"No se encontró ningún club con el nombre {nombreBuscado}");

                        }
                        else
                        {




                            sportsClub.Schema_alternateName = nombresAlternativosDiccionario.ContainsKey(clubClave) ? nombresAlternativosDiccionario[clubClave] : new List<string>();
                            sportsClub.Schema_award = premiosDiccionario.ContainsKey(clubClave) ? premiosDiccionario[clubClave] : new List<string>();
                            sportsClub.Schema_location = direccionesPostalesDiccionario.ContainsKey(clubClave) ? direccionesPostalesDiccionario[clubClave] : new List<ClubpfihsOntology.PostalAddress>();
                            sportsClub.Schema_logo = logotiposDiccionario.ContainsKey(clubClave) ? logotiposDiccionario[clubClave] : null;
                            sportsClub.Schema_description = wikipediaDiccionario.ContainsKey(clubClave) ? wikipediaDiccionario[clubClave] : null;
                            sportsClub.Schema_foundingDate = fundacionDiccionario.ContainsKey(clubClave) ? (DateTime?)fundacionDiccionario[clubClave] : null;
                            if (entrenadoresDiccionario.ContainsKey(clubClave))
                            {
                                listaEntr = (Dictionary<string, List<DateTime>>)entrenadoresDiccionario[clubClave];
                            }
                            else
                            {
                                listaEntr = null;
                            }

                        }
                        sportsClub.Schema_parentOrganization = Plantilla(rutaDirectorioPersona, rutaDirectorioValoracion, club.Name, listaEntr);
                        sportsClub.Eschema_league = club.DomesticCompetitionId;


                        i++;
                        Console.WriteLine($"{i}:");
                        Console.WriteLine("");
                       
                        apiRecursos.ChangeOntology(ontologiaClub);
                        ComplexOntologyResource recursoPersona = sportsClub.ToGnossApiResource(apiRecursos, new List<string> { "Club" }, Guid.NewGuid(), Guid.NewGuid());
                        apiRecursos.LoadComplexSemanticResource(recursoPersona);
                    }
                }
                catch (Exception ex )
                {
                    Console.Write("error:  "); Console.WriteLine(ex.ToString());
                }
        }




        }
        
        public List<SportsTeam> Plantilla(string rutaDirectorioPersona, string rutaDirectorioValoracion,string clubName, Dictionary<string, List<DateTime>> listaEntr)
        {
            
            Dictionary<string, List<string>> resultado = ExtraerEntrenadoresPorTemporada(listaEntr);

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
            foreach (var key in grupos.Keys.Where(k => k.Item1 == clubName))  // Filtrar claves 
            {
                var clubTemporada = grupos[key]; // Obtener el valor del diccionario

                planti.Eschema_identifier = "";
                planti.IdEschema_season = getTemporadaUrl(clubTemporada.Temporada);
                List<string> coachsuri = new List<string>();
                if (resultado != null)
                {
                    if (resultado.ContainsKey(clubTemporada.Temporada))
                    {
                        foreach (var entrenador in resultado[clubTemporada.Temporada])
                        {
                            Torneo t = new Torneo(apiRecursos);
                            coachsuri.Add(t.getManager(entrenador));
                        }
                    }
                }
                planti.IdsSchema_coach = coachsuri;

                List<string> jugadoresuri = new List<string>();

                foreach (var jugador in clubTemporada.Jugadores)
                {
                    Torneo t = new Torneo(apiRecursos);
                    jugadoresuri.Add(t.getPlayerUrl(jugador.PlayerId, jugador.Name, rutaDirectorioValoracion));
                }
                planti.IdsSchema_athlete = jugadoresuri;
               
                plantillas.Add(planti);
            }


            return plantillas;

        }
        public static Dictionary<string, List<string>> ExtraerEntrenadoresPorTemporada(Dictionary<string, List<DateTime>> listaEntr)
        {
            // Diccionario para almacenar el resultado (temporada como clave, lista de entrenadores como valor)
            Dictionary<string, List<string>> resultado = new Dictionary<string, List<string>>();
            if (listaEntr == null)
            {
                return resultado;
            }
            foreach (var entrenador in listaEntr)
            {
                string nombreEntrenador = entrenador.Key;
                int anioInicio = entrenador.Value[0].Year;
                int anioFin = entrenador.Value[1].Year;

                // Recorre todos los años que entrenó
                for (int anio = anioInicio; anio <= anioFin; anio++)
                {
                    string temporada = anio.ToString();

                    // Si la temporada ya existe en el diccionario, añade el entrenador a la lista
                    if (resultado.ContainsKey(temporada))
                    {
                        resultado[temporada].Add(nombreEntrenador);
                    }
                    else
                    {
                        // Si no existe la temporada, crea una nueva entrada con el entrenador
                        resultado[temporada] = new List<string> { nombreEntrenador };
                    }
                }
            }

            return resultado;
        }
        public static Dictionary<string, List<string>> ExtraerAniosEntrenador(Dictionary<string, List<DateTime>> listaEntr)
        {
            // Diccionario para almacenar el resultado
            Dictionary<string, List<string>> resultado = new Dictionary<string, List<string>>();

            foreach (var entrenador in listaEntr)
            {
                // Obtener el año de inicio y fin
                int anioInicio = entrenador.Value[0].Year;
                int anioFin = entrenador.Value[1].Year;

                // Crear una lista con todos los años entre el inicio y el fin (inclusivo)
                List<string> aniosEnPuesto = new List<string>();
                for (int anio = anioInicio; anio <= anioFin; anio++)
                {
                    aniosEnPuesto.Add(anio.ToString());
                }

                // Añadir al diccionario de resultados
                resultado.Add(entrenador.Key, aniosEnPuesto);
            }

            return resultado;
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