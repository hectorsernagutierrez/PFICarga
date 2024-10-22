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
using HtmlAgilityPack;
using FuzzySharp;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
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

        public void CargarTodosClub(string rutaDirectorioClub, string rutaDirectorioAppearance, string rutaDirectorioValoracion)
        {

            int i = 0;
            var service = new DTOService();

            List<ClubsDTO> clubs = service.ReadClubs(rutaDirectorioClub);
            // Dictionary<string, Dictionary<string, object>> diccionarioGrande = ServiceWIKIDATA.LeerClub2(3000, 0);
            // Dictionary<string, object> diccionariopeque = new Dictionary<string, object>();

            var nombresAlternativosDiccionario = ServiceWIKIDATA.ObtenerTodasLosNombresAlternativos();
            var logotiposDiccionario = ServiceWIKIDATA.LeerLogotiposClub();
            var entrenadoresDiccionario = ServiceWIKIDATA.ObtenerEntrenadoresPorClub(10000, 0);
            var premiosDiccionario = ServiceWIKIDATA.ObtenerTodosLosPremios();
            var direccionesPostalesDiccionario = ServiceWIKIDATA.ObtenerTodasLasDireccionesPostales();
            var wikipediaDiccionario = ServiceWIKIDATA.ObtenerTodosLosArticulosWikipedia();
            var fundacionDiccionario = ServiceWIKIDATA.ObtenerFechasFundacionMasAntiguas();
            // Obtener la ruta base de la aplicación


            Random rnd = new Random();




            foreach (var club in clubs.OrderBy(x => rnd.Next()))
            {

                try
                {
                    Console.WriteLine($@"Cargando club: {club.Name}");

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




                        List<string> prem;
                        List<string> nomalt;
                        List<ClubpfihsOntology.PostalAddress> loc;
                        string logo;
                        string descrip;
                        DateTime fundacion;

                        // Si no encontramos el club, devolver null
                        if (clubClave != null)
                        {
                            Console.WriteLine($"Se encontró ningún club exacto con el nombre {nombreBuscado}");
                            clubClave = nombreBuscado;
                            sportsClub.Schema_alternateName = nombresAlternativosDiccionario.ContainsKey(clubClave) ? nombresAlternativosDiccionario[clubClave] : new List<string>();
                            sportsClub.Schema_award = premiosDiccionario.ContainsKey(clubClave) ? premiosDiccionario[clubClave] : new List<string>();
                            sportsClub.Schema_location = direccionesPostalesDiccionario.ContainsKey(clubClave) ? direccionesPostalesDiccionario[clubClave] : new List<ClubpfihsOntology.PostalAddress>();
                            sportsClub.Schema_logo = logotiposDiccionario.ContainsKey(clubClave) ? logotiposDiccionario[clubClave] : null;
                            sportsClub.Schema_description = wikipediaDiccionario.ContainsKey(clubClave) ? wikipediaDiccionario[clubClave] : null;
                            sportsClub.Schema_foundingDate = fundacionDiccionario.ContainsKey(clubClave) ? (DateTime?)fundacionDiccionario[clubClave] : null;


                           // prem = DiccionarioSimilar<List<string>>.ObtenerIgualOSimilar(premiosDiccionario, clubClave);
                           // nomalt = DiccionarioSimilar<List<string>>.ObtenerIgualOSimilar(nombresAlternativosDiccionario, clubClave);
                           //  loc= DiccionarioSimilar<List<ClubpfihsOntology.PostalAddress>>.ObtenerIgualOSimilar(direccionesPostalesDiccionario, clubClave);
                           // logo = DiccionarioSimilar<string>.ObtenerIgualOSimilar(logotiposDiccionario, clubClave);
                           //descrip  = DiccionarioSimilar<string>.ObtenerIgualOSimilar(wikipediaDiccionario, clubClave);
                           // fundacion = DiccionarioSimilar<DateTime>.ObtenerIgualOSimilar(fundacionDiccionario, clubClave);
                        
                        
                        }
                        else
                        {
                            Console.WriteLine($"NO se encontró ningún club exacto con el nombre {nombreBuscado}");
                            //ServiceWIKIDATA.LeerClub(nombreBuscado, out descrip, out logo, out string cp, out string calle, out string ciudad, out string pais, out List<DateTime> fundacion1, out prem, out nomalt);
                            //fundacion = fundacion1.First();
                            //loc = new List<ClubpfihsOntology.PostalAddress>();
                            //loc.Add(new ClubpfihsOntology.PostalAddress() { Schema_PostalCode = cp , Schema_streetAddress=calle,Schema_addressLocality=ciudad,Schema_addressCountry=pais});

                            //sportsClub.Schema_alternateName = nomalt;
                            //sportsClub.Schema_award = prem;
                            //sportsClub.Schema_location = loc;
                            sportsClub.Schema_logo = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"Documents/Logo.png");
                            //sportsClub.Schema_description = descrip;
                            //sportsClub.Schema_foundingDate = fundacion;
                            clubClave = club.Name;

                        }
                           
                            if (entrenadoresDiccionario.ContainsKey(clubClave))
                            {
                                listaEntr = (Dictionary<string, List<DateTime>>)entrenadoresDiccionario[clubClave];
                            }
                            else
                            {
                                listaEntr = null;
                            }

                        
                        List<AppearancesDTO> appearances = service.ReadAppearances(rutaDirectorioAppearance);

                        Dictionary<string, List<AppearancesDTO>> appearancesClub = AgruparPorClub(appearances);

                        Console.WriteLine($"Cargado {i}: {club.Name}");
                        sportsClub.Schema_parentOrganization = Plantilla2(club.ClubId, appearancesClub[club.ClubId], listaEntr, rutaDirectorioValoracion);
                        sportsClub.Eschema_league = club.DomesticCompetitionId;


                        i++;
                        Console.WriteLine($"{i}:");
                        Console.WriteLine("");

                        apiRecursos.ChangeOntology(ontologiaClub);
                        ComplexOntologyResource recursoPersona = sportsClub.ToGnossApiResource(apiRecursos, new List<string> { "Club" }, Guid.NewGuid(), Guid.NewGuid());
                        apiRecursos.LoadComplexSemanticResource(recursoPersona);
                    }
                }
                catch (Exception ex)
                {
                    Console.Write("error:  "); Console.WriteLine(ex.ToString());
                }
            }




        }

        public List<SportsTeam> Plantilla(string rutaDirectorioAppearance, string rutaDirectorioValoracion, string clubId, Dictionary<string, List<DateTime>> listaEntr)
        {

            Dictionary<string, List<string>> resultado = ExtraerEntrenadoresPorTemporada(listaEntr);

            var service = new DTOService();
            // List<PlayersDTO> players = service.ReadPlayers(rutaDirectorioPersona);
            List<AppearancesDTO> appearances = service.ReadAppearances(rutaDirectorioAppearance);

            // Crear el diccionario usando un Tuple como clave
            var grupos = appearances
                .GroupBy(p => new { p.Temporada, p.playerClubId, p.PlayerId })
                .ToDictionary(
                    g => Tuple.Create(g.Key.playerClubId, g.Key.Temporada, g.Key.PlayerId), // La clave es un Tuple de Club y Temporada
                    g => new ClubTemporadaDTO
                    {
                        Temporada = g.Key.Temporada,
                        Club = g.Key.playerClubId,
                        IdPlayer = g.Key.playerClubId,
                        Jugadores = g.ToList()
                    }
                );
            List<SportsTeam> plantillas = new List<SportsTeam>();
            SportsTeam planti = new SportsTeam();
            foreach (var key in grupos.Keys.Where(k => k.Item1 == clubId))  // Filtrar claves 
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
                    jugadoresuri.Add(t.getPlayerUrl(jugador.PlayerId, jugador.playerName, rutaDirectorioValoracion));
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



        public List<SportsTeam> Plantilla2(string clubId, List<AppearancesDTO> aparicionesDelClub, Dictionary<string, List<DateTime>> listaEntr, string rutaDirectorioValoracion)
        {
            Dictionary<string, List<string>> resultado = ExtraerEntrenadoresPorTemporada(listaEntr);

            // Agrupar las apariciones del club por Temporada
            var aparicionesPorTemporada = aparicionesDelClub
                .GroupBy(a => a.Temporada)
                .ToDictionary(
                    g => g.Key, // Clave: Temporada
                    g => g.ToList() // Valor: Lista de apariciones en esa temporada
                );

            List<SportsTeam> plantillas = new List<SportsTeam>();

            foreach (var temporada in aparicionesPorTemporada.Keys)
            {
                var aparicionesTemporada = aparicionesPorTemporada[temporada]; // Obtener las apariciones de esta temporada
                SportsTeam planti = new SportsTeam
                {
                    Eschema_identifier = "",
                    IdEschema_season = getTemporadaUrl(temporada)
                };

                // Obtener entrenadores sin duplicados para la temporada
                List<string> coachsuri = new List<string>();
                if (resultado != null && resultado.ContainsKey(temporada))
                {
                    // Usar Distinct para asegurar que cada entrenador solo aparece una vez
                    var entrenadoresUnicos = resultado[temporada].Distinct();
                    foreach (var entrenador in entrenadoresUnicos)
                    {
                        Torneo t = new Torneo(apiRecursos);
                        coachsuri.Add(t.getManager(entrenador));
                    }
                }
                planti.IdsSchema_coach = coachsuri;

                // Obtener jugadores sin duplicados para la temporada
                List<string> jugadoresuri = new List<string>();
                Torneo torneo = new Torneo(apiRecursos); // Crear la instancia una vez para evitar overhead

                // Agrupar apariciones por jugador para asegurar que cada jugador solo aparece una vez
                var jugadoresUnicos = aparicionesTemporada
                    .GroupBy(a => new { a.PlayerId, a.playerName }) // Agrupar por jugador
                    .Select(g => g.First()); // Seleccionar solo una aparición por jugador

                foreach (var jugador in jugadoresUnicos)
                {
                    jugadoresuri.Add(torneo.getPlayerUrl(jugador.PlayerId, jugador.playerName, rutaDirectorioValoracion));
                }
                planti.IdsSchema_athlete = jugadoresuri;

                plantillas.Add(planti);
            }

            return plantillas;
        }

        public Dictionary<string, List<AppearancesDTO>> AgruparPorClub(List<AppearancesDTO> appearances)
        {
            // Agrupamos las apariciones por clubId
            return appearances
                .GroupBy(a => a.playerClubId)
                .ToDictionary(g => g.Key, g => g.ToList());
        }





        public Dictionary<string, string> ObtenerLogosClubesWebScraper(string urlPagina)
        {
            // Diccionario para almacenar el nombre del club y la URL de su logo
            Dictionary<string, string> logosClubes = new Dictionary<string, string>();
            // Almacenar la validación SSL original
            RemoteCertificateValidationCallback originalValidationCallback = ServicePointManager.ServerCertificateValidationCallback;

            try
            {
                // Deshabilitar la validación del certificado SSL
                ServicePointManager.ServerCertificateValidationCallback =
                    delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };

                // Cargar la página HTML
                HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(urlPagina);

            // Seleccionar los nombres de los clubes (ejemplo basado en estructura de la página)
            var clubNames = document.DocumentNode.SelectNodes("//div[contains(@class, 'post-content')]/p/a");

            // Seleccionar las URLs de los logos (usualmente en tags <img>)
            var clubLogos = document.DocumentNode.SelectNodes("//div[contains(@class, 'post-content')]/p/a/img");

            if (clubNames != null && clubLogos != null && clubNames.Count == clubLogos.Count)
            {
                for (int i = 0; i < clubNames.Count; i++)
                {
                    // Extraer el nombre del club
                    string nombreClub = clubNames[i].InnerText.Trim();

                    // Extraer la URL del logo
                    string urlLogo = clubLogos[i].GetAttributeValue("src", "");

                    // Agregar al diccionario
                    logosClubes[nombreClub] = urlLogo;
                }
            }
            }
            finally
            {
                // Restaurar la validación SSL original
                ServicePointManager.ServerCertificateValidationCallback = originalValidationCallback;
            }

            return logosClubes;
        }






        public Dictionary<string, string> ObtenerLogosClubesWeb(string urlPagina)
        {
            // Diccionario para almacenar el nombre del club y la URL de su logo
            Dictionary<string, string> logosClubes = new Dictionary<string, string>();

            // Configuración del navegador en modo headless y para ignorar los errores SSL
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless"); // Ejecutar en modo sin interfaz gráfica
            options.AddArgument("--ignore-certificate-errors"); // Ignorar errores de certificados SSL
            options.AddArgument("--allow-insecure-localhost");  // Permitir localhost inseguro
            options.AddArgument("--disable-web-security");      // Desactivar la seguridad web

            // Inicializar el driver de Chrome
            using (IWebDriver driver = new ChromeDriver(options))
            {
                // Cargar la página web
                driver.Navigate().GoToUrl(urlPagina);

                // Esperar a que la página cargue completamente
                System.Threading.Thread.Sleep(3000); // Ajustar tiempo de espera según la velocidad de la página

                // Obtener los nombres de los equipos
                var clubNames = driver.FindElements(By.XPath("//div[contains(@class, 'post-content')]/p/a"));

                // Obtener los logos de los equipos
                var clubLogos = driver.FindElements(By.XPath("//div[contains(@class, 'post-content')]/p/a/img"));

                if (clubNames.Count == clubLogos.Count)
                {
                    for (int i = 0; i < clubNames.Count; i++)
                    {
                        // Extraer el nombre del club
                        string nombreClub = clubNames[i].Text.Trim();

                        // Extraer la URL del logo
                        string urlLogo = clubLogos[i].GetAttribute("src");

                        // Agregar al diccionario
                        logosClubes[nombreClub] = urlLogo;
                    }
                }
                else
                {
                    Console.WriteLine("El número de nombres y logos no coincide.");
                }
            }

            return logosClubes;
        }

    }



}


public class ClubTemporadaDTO
{
    public string Temporada { get; set; }
    public string IdPlayer { get; set; }
    public string NamePlayer { get; set; }
    public string Club { get; set; }
     public List<AppearancesDTO> Jugadores { get; set; }
}