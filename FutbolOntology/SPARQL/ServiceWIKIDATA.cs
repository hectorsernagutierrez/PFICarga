using FutbolOntology.CargaPFI;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Helpers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Gnoss.ApiWrapper.ApiModel.SparqlObject;
using static System.Net.WebRequestMethods;


namespace FutbolOntology.SPARQL
{
    public class ServiceWIKIDATA
    {


        public static SparqlObject Jsonget(string select2, string where2)
        {



            String sparqlQuery = select2 + " "+where2;

            System.Collections.Specialized.NameValueCollection parametros = new System.Collections.Specialized.NameValueCollection {
                { "query", sparqlQuery.ToString() }
            };
            byte[] responseArray = null;
            int numIntentos = 0;
            string error = "";
            while (responseArray == null && numIntentos < 50)
            {
                numIntentos++;
                try
                {
                    WebClient webClient = new();
                    webClient.Encoding = Encoding.UTF8;
                    webClient.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                    webClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
                    webClient.Headers.Add("accept", "application/sparql-results+json");
                    webClient.Headers.Add("Cache-Control", "no-cache");
                    webClient.Headers.Add("Pragma", "no-cache");
                    string s = parametros.GetValues(0)[0];
                    Console.WriteLine($"{numIntentos}:         {s}");
                    responseArray = webClient.UploadValues("https://query.wikidata.org/sparql", "POST", parametros);
                }
                catch (Exception ex)
                {
                    // Registrar el error en lugar de llamar recursivamente al método
                    error = ex.Message;
                    Console.WriteLine($"Intento {numIntentos} fallido: {error}");

                    // Si hemos alcanzado el límite de intentos, salir del bucle
                    if (numIntentos >= 500)
                    {
                        throw new Exception($"Error tras 500 intentos: {error}");
                    }
                    
                }
            }
            string jsonRespuesta = System.Text.Encoding.UTF8.GetString(responseArray);
            SparqlObject datos = new SparqlObject();
            if (!string.IsNullOrEmpty(jsonRespuesta))
            {
                var settings = new JsonSerializerSettings
                {
                    // Modo de manejo de errores
                    Error = (sender, args) =>
                    {
                        // Evitar excepciones por errores no críticos
                        args.ErrorContext.Handled = true;
                    },
                    // Otras configuraciones si es necesario
                    Formatting = Formatting.Indented,
                    StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
                };






                datos = JsonConvert.DeserializeObject<SparqlObject>(jsonRespuesta,settings);
            }

            return datos;
        }


        public static List<string> LeerJugador(string nombre, out string descripcion)
        {
            // Definir la consulta SPARQL
            String select2 = $@"SELECT ?player ?playerLabel ?wikipediaArticle ?award ?awardLabel";
            String where2 = $@" WHERE {{ 
                    ?player wdt:P106 wd:Q937857;  
                        rdfs:label ?playerLabel.
               FILTER(CONTAINS(?playerLabel,""{nombre}"")).
            
            

 
            OPTIONAL {{
             ?wikipediaArticle schema:about ?player;
                      schema:isPartOf <https://en.wikipedia.org/>.
                 }}

                OPTIONAL {{ 
                ?player wdt:P166 ?award.  
                ?award rdfs:label ?awardLabel FILTER( LANG(?awardLabel) = ""en"").
                }}
            
                     SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""en"". }}
                  }} LIMIT 1";

            SparqlObject datos = new SparqlObject();

            datos = ServiceWIKIDATA.Jsonget(select2, where2);
            List<string> lista = new List<string>();
            descripcion = "N/A";
            if (datos.results != null && datos.results.bindings != null && datos.results.bindings.Count > 0)
            {
                foreach (var binding in datos.results.bindings)
                {
                    if (binding.ContainsKey("wikipediaArticle"))
                    {
                        descripcion = binding["wikipediaArticle"].value;
                    }
                    if (binding.ContainsKey("awardLabel"))
                    {
                        lista.Add(binding["awardLabel"].value);
                    }
                }
            }

            return lista;
        }








        public static void LeerEntrenador(string nombreClub, out Dictionary<string, List<DateTime>> temporadaEntrenador)
        {
            temporadaEntrenador = new Dictionary<string, List<DateTime>>();
            // Definir la consulta SPARQL
            String select2 = $@"SELECT ?club ?clubLabel ?nombreEntrenador ?uriEntrenador ?comienzo ?final";
            String where2 = $@" WHERE {{
  {{
    ?club wdt:P31 wd:Q476028;
          rdfs:label ?clubLabel.
    
    FILTER(LANG(?clubLabel) = ""es"" || LANG(?clubLabel) = ""en"").
    OPTIONAL {{ ?club wdt:P1448 ?alternativeName. }}
    
    FILTER(CONTAINS(LCASE(?clubLabel), ""{nombreClub}"") || CONTAINS(LCASE(?alternativeName), ""{nombreClub}"")).
  }}
  UNION
  {{
    ?club wdt:P31 wd:Q103229495;
          rdfs:label ?clubLabel.
    FILTER(LANG(?clubLabel) = ""es"" || LANG(?clubLabel) = ""en"").
    OPTIONAL {{ ?club wdt:P1448 ?alternativeName. }}
    
    FILTER(CONTAINS(LCASE(?clubLabel), ""{nombreClub}"") || CONTAINS(LCASE(?alternativeName), ""{nombreClub}"")).
  }}
  ?club  p:P286 ?o.
  ?o pq:P580 ?comienzo.
  ?o pq:P582 ?final.
  ?o ps:P286 ?uriEntrenador.
  ?uriEntrenador rdfs:label ?nombreEntrenador
  FILTER (lang(?nombreEntrenador) = 'es')
}}
ORDER BY ?club ?comienzo
LIMIT 100";

            SparqlObject datos = new SparqlObject();

            datos = ServiceWIKIDATA.Jsonget(select2, where2);
            List<string> lista = new List<string>();

            if (datos.results != null && datos.results.bindings != null && datos.results.bindings.Count > 0)
            {
                foreach (var binding in datos.results.bindings)
                {
                    if ((binding.ContainsKey("clubLabel")) && (nombreClub.ToLower().Trim() == binding["clubLabel"].value.ToLower().Trim()))
                    {
                        List<DateTime> list = new List<DateTime>();
                        list.Add(DateTime.Parse(binding["comienzo"].value));
                        list.Add(DateTime.Parse(binding["final"].value));
                        temporadaEntrenador.Add(binding["nombreEntrenador"].value, list);
                    }

                }
            }


        }


        public static Dictionary<string, Dictionary<string, List<DateTime>>> ObtenerEntrenadoresPorClub(int limit = 5000, int offset = 0)
        {
            var clubsDictionary = new Dictionary<string, Dictionary<string, List<DateTime>>>();
            bool hayMasResultados = true;

            while (hayMasResultados)
            {
                // Definir la consulta SPARQL con limit y offset
                string select = @"
        SELECT ?club ?clubLabel ?EntrenadorLabel ?Entrenador ?comienzo ?final";

                string where = $@"WHERE {{
            ?club wdt:P31 wd:Q476028.  
            ?club p:P286 ?o. 
            ?o ps:P286 ?Entrenador.  
            
            ?o pq:P580 ?comienzo.  
            OPTIONAL {{ ?o pq:P582 ?final. }} 
            
            SERVICE wikibase:label {{ bd:serviceParam wikibase:language '[AUTO_LANGUAGE],en'. }}
        }}
        ORDER BY ?club ?comienzo
        LIMIT {limit} OFFSET {offset}";

                // Ejecutar la consulta SPARQL usando el método Jsonget
                SparqlObject datos = ServiceWIKIDATA.Jsonget(select, where);

                // Procesar los resultados
                if (datos.results != null && datos.results.bindings != null && datos.results.bindings.Count > 0)
                {
                    foreach (var binding in datos.results.bindings)
                    {
                        // Obtener el nombre del club
                        string clubName = binding.ContainsKey("clubLabel") ? binding["clubLabel"].value : "N/A";

                        // Obtener el nombre del entrenador
                        string entrenadorName = binding.ContainsKey("EntrenadorLabel") ? binding["EntrenadorLabel"].value : "N/A";

                        // Obtener la fecha de comienzo
                        DateTime comienzo = new DateTime();
                        if (binding.ContainsKey("comienzo") && DateTime.TryParse(binding["comienzo"].value, out DateTime parsedComienzo))
                        {
                            comienzo = parsedComienzo;
                        }

                        // Obtener la fecha de finalización (si existe)
                        DateTime final = new DateTime();
                        

                        // Si el club no está en el diccionario, agregarlo
                        if (!clubsDictionary.ContainsKey(clubName))
                        {
                            clubsDictionary[clubName] = new Dictionary<string, List<DateTime>>();
                        }

                        // Si el entrenador no está en el diccionario del club, agregarlo
                        if (!clubsDictionary[clubName].ContainsKey(entrenadorName))
                        {
                            clubsDictionary[clubName][entrenadorName] = new List<DateTime>();
                        }

                        // Agregar el periodo del entrenador (comienzo, final) a la lista de periodos
                        clubsDictionary[clubName][entrenadorName].Add(comienzo);
                        if (binding.ContainsKey("final") && DateTime.TryParse(binding["final"].value, out DateTime parsedFinal))
                        {
                            final = parsedFinal;
                            clubsDictionary[clubName][entrenadorName].Add(final);
                        }
                        else
                        {
                            clubsDictionary[clubName][entrenadorName].Add(DateTime.Now);
                            
                        }
                        
                    }

                    // Incrementar el offset para la siguiente página
                    offset += limit;
                }
                else
                {
                    // Si no hay más resultados, salir del bucle
                    hayMasResultados = false;
                }
            }

            return clubsDictionary;
        }







        public static void LeerWikiDatasJugadores(int offset,out Dictionary<string, string> d, Dictionary<string, string> daux)
        {
            d = daux;
            // Consulta SPARQL con OFFSET actualizado
            String select2 = $@"SELECT ?player ?playerLabel ?wikipediaArticle ";
            String where2 = $@"WHERE {{
        ?player wdt:P106 wd:Q937857.  
        ?wikipediaArticle schema:about ?player;
                          schema:isPartOf <https://en.wikipedia.org/>.
        SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""en"". }}
    }}
    LIMIT 3000  
    OFFSET {offset}";

            // Obtener datos de Wikidata
            SparqlObject datos = ServiceWIKIDATA.Jsonget(select2, where2);

            // Verificamos que hay resultados y el objeto no es nulo
            if ((datos.results != null) && (datos.results.bindings != null) && (datos.results.bindings.Count > 0))
            {
                // Recorremos los resultados y añadimos al diccionario
                foreach (var binding in datos.results.bindings)
                {
                    // Obtiene el nombre del jugador y el artículo de Wikipedia
                    string playerLabel = binding["playerLabel"].value;
                    string wikipediaArticle = binding["wikipediaArticle"].value;

                    // Añade al diccionario si no existe ya
                    if (!d.ContainsKey(playerLabel))
                    {
                        d.Add(playerLabel, wikipediaArticle);
                    }
                }

                // Si se alcanzo el límite de 3000 registros, llamamos recursivamente para la siguiente página
                if (datos.results.bindings.Count == 3000)
                {
                    // Incrementar el offset para la proxima iteracion
                    LeerWikiDatasJugadores(offset + 3000,out d,d);
                }
            }
        }





        public static void LeerAwards(int offset,out Dictionary<string, List<string>> d, Dictionary<string, List<string>> daux)
        {
            d = daux;
            // Consulta SPARQL con OFFSET actualizado
            String select2 = $@"SELECT ?player ?playerLabel ?awardLabel  ";
            String where2 = $@"WHERE {{
    ?player wdt:P106 wd:Q937857.  
    
  
        ?player wdt:P166 ?award.  
    ?award rdfs:label ?awardLabel FILTER( LANG(?awardLabel) = ""en"").
    
    
    SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""en"". }}
}}
LIMIT 3000  
OFFSET {offset}";

            // Obtener datos de Wikidata
            SparqlObject datos = ServiceWIKIDATA.Jsonget(select2, where2);

            // Verificamos que hay resultados y el objeto no es nulo
            if (datos.results != null && datos.results.bindings != null && datos.results.bindings.Count > 0)
            {
                // Recorremos los resultados obtenidos
                foreach (var binding in datos.results.bindings)
                {
                    string playerLabel = binding["playerLabel"].value;
                    string awardLabel = binding["awardLabel"].value;

                    // Si el jugador ya está en el diccionario, añadimos el premio a su lista
                    if (d.ContainsKey(playerLabel))
                    {
                        d[playerLabel].Add(awardLabel);
                    }
                    else
                    {
                        // Si no está, creamos una nueva lista con su primer premio
                        d.Add(playerLabel, new List<string> { awardLabel });
                    }
                }

                // Si obtenemos exactamente 3000 resultados, hacemos la llamada recursiva con el siguiente offset
                if (datos.results.bindings.Count == 3000)
                {
                    LeerAwards(offset + 3000, out d,d);
                }
            }
        }

        public static void LeerClub(string nombre, out string descripcion, out string logo, out string cp, out string calle, out string ciudad, out string pais, out List<DateTime> fundacion, out List<string> awards, out List<string> alias)
        {
            string clubwikiid2 = ObtenerClubWIKIIDPorNombre(nombre);
            string clubwikiid ="wd:"+ clubwikiid2.Split("/").Last();
            descripcion = "N/A";
            logo = "N/A";
            cp = "";
            calle = "";
            ciudad = "";
            pais = "";
            fundacion = new List<DateTime>();
            awards = new List<string>();
            alias = new List<string>();
            // Definir la consulta SPARQL
            String select2 = $@"SELECT ?club ?clubLabel ?alternativeName ?AlternativeNameLabel ?logo ?foundingDate ?award ?awardLabel ?stadium ?postalCode ?street ?city ?country ?wikipediaArticle ?similarityScore
";
            String where2 = $@"WHERE {{
    {clubwikiid} wdt:P31 wd:Q476028.
         
  
    
        ?club skos:altLabel ?alternativeName.
        

   
    
    OPTIONAL {{ ?club wdt:P154 ?logo. }} 
    OPTIONAL {{ ?club wdt:P571 ?foundingDate. }} 
    OPTIONAL {{ 
        ?club wdt:P166 ?award.
        ?award rdfs:label ?awardLabel FILTER(LANG(?awardLabel) = ""es"").  
    }}
    OPTIONAL {{ 
        ?club wdt:P115 ?stadium.  
        OPTIONAL {{ ?stadium wdt:P281 ?postalCode. }}  
        OPTIONAL {{ ?stadium wdt:P669 ?street. }} 
        OPTIONAL {{ ?stadium wdt:P131 ?city. }}  
        OPTIONAL {{ ?stadium wdt:P17 ?country. }}  
    }}
   
    OPTIONAL {{
        ?wikipediaArticle schema:about ?club;
                          schema:isPartOf <https://en.wikipedia.org/>. 
    }}

    SERVICE wikibase:label {{ 
        bd:serviceParam wikibase:language ""[AUTO_LANGUAGE],en"".  
    }}
}}
ORDER BY ?similarityScore
LIMIT 1
";

            SparqlObject datos = new SparqlObject();

            datos = ServiceWIKIDATA.Jsonget(select2, where2);
            List<string> lista = new List<string>();

            if (datos.results != null && datos.results.bindings != null && datos.results.bindings.Count > 0)
            {
                foreach (var binding in datos.results.bindings)
                {
                    if (binding.ContainsKey("clubLabel"))
                    {
                        if (binding["clubLabel"].value == nombre)
                        {
                            if (logo != "N/A")
                            {
                                descripcion = binding["wikipediaArticle"].value ?? "N/A";
                                logo = binding["logo"].value ?? "N/A";
                                calle = binding["street"].value ?? "N/A";
                                cp = binding["postalCode"].value ?? "N/A";
                                ciudad = binding["city"].value ?? "N/A";
                                pais = binding["country"].value ?? "N/A";
                            }
                            if (binding.ContainsKey("AlternativeNameLabel"))
                            {
                                alias.Add(binding["AlternativeNameLabel"].value);
                            }
                            if (binding.ContainsKey("foundingDate"))
                            {
                                fundacion.Add(DateTime.Parse(binding["foundingDate"].value));
                            }
                            if (binding.ContainsKey("awardLabel"))
                            {
                                awards.Add(binding["awardLabel"].value);
                            }


                        }

                    }
                }
            }


        }

        public static Dictionary<string, Dictionary<string, object>> LeerClub2(int limit = 3000, int offset = 0)
        {
            var clubsDictionary = new Dictionary<string, Dictionary<string, object>>();

            // Definir la consulta SPARQL
            String select2 = $@"SELECT ?club ?clubLabel 
       (GROUP_CONCAT(DISTINCT ?name; separator="", "") AS ?alternativeNames)
       (GROUP_CONCAT(DISTINCT ?foundingDate; separator="", "") AS ?foundingDates)
       (GROUP_CONCAT(DISTINCT ?awardLabel; separator="", "") AS ?awards)
       ?logo ?logoLabel ?stadiumLabel ?postalCodeLabel ?streetLabel ?cityLabel ?countryLabel ?wikipediaArticle ?apodo ";
            String where2 = $@"WHERE {{
  
  ?club wdt:P31 wd:Q476028.
  ?club wdt:P154 ?logo.
  ?club skos:altLabel ?AlternativeNameLabel.
  FILTER(LANG(?AlternativeNameLabel) = ""en"")
  BIND (?AlternativeNameLabel AS ?name)
  
  

   
  
  
  OPTIONAL {{ ?club wdt:P571 ?foundingDate. }}

  
  
    
 OPTIONAL {{   
   ?club wdt:P1449 ?Apodo.
   
   BIND (?ApodoLabel AS ?name)
  }}
   
  
  OPTIONAL {{
    ?club wdt:P166 ?award.
    ?award rdfs:label ?awardLabel.
    FILTER(LANG(?awardLabel) = ""en"")
    
  }}

  
  OPTIONAL {{
    ?club wdt:P115 ?stadium.
    OPTIONAL {{ ?stadium wdt:P281 ?postalCode. }}
    OPTIONAL {{ ?stadium wdt:P669 ?street. }}
    OPTIONAL {{ ?stadium wdt:P131 ?city. }}
    OPTIONAL {{ ?stadium wdt:P17 ?country. }}
  }}

  
  OPTIONAL {{
    ?wikipediaArticle schema:about ?club;
                      schema:isPartOf <https://en.wikipedia.org/>.
  }}

 
  SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""en"". }}

}}
GROUP BY ?club ?clubLabel ?logo ?logoLabel ?stadiumLabel ?postalCodeLabel ?streetLabel ?cityLabel ?countryLabel ?wikipediaArticle ?apodo
LIMIT {limit} OFFSET {offset}
";

            // Ejecutar la consulta SPARQL
            SparqlObject datos = ServiceWIKIDATA.Jsonget(select2, where2);

            // Procesar los resultados
            if (datos.results != null && datos.results.bindings != null && datos.results.bindings.Count > 0)
            {
                foreach (var binding in datos.results.bindings)
                {
                    // Obtener el nombre del club
                    string clubName = binding.ContainsKey("clubLabel") ? binding["clubLabel"].value : "N/A";

                    // Si el club no está en el diccionario, lo añadimos
                    if (!clubsDictionary.ContainsKey(clubName))
                    {
                        // Parseo de foundingDates (listas de DateTime)
                        List<DateTime> foundingDatesList = new List<DateTime>();
                        if (binding.ContainsKey("foundingDates"))
                        {
                            var datesString = binding["foundingDates"].value;
                            var datesArray = datesString.Split(", "); // Separar por comas
                            foreach (var date in datesArray)
                            {
                                if (DateTime.TryParse(date, out DateTime parsedDate))
                                {
                                    foundingDatesList.Add(parsedDate);  // Añadir a la lista si se puede parsear correctamente
                                }
                            }
                        }

                        // Parseo de premios (lista de strings)
                        List<string> awardsList = new List<string>();
                        if (binding.ContainsKey("awards"))
                        {
                            var awardsString = binding["awards"].value;
                            awardsList = awardsString.Split(", ").ToList();  // Convertir a lista de strings
                        }

                        // Parseo de alias (lista de strings)
                        List<string> aliasList = new List<string>();
                        if (binding.ContainsKey("alternativeNames"))
                        {
                            var aliasString = binding["alternativeNames"].value;
                            aliasList = aliasString.Split(", ").ToList();  // Convertir a lista de strings
                        }

                        clubsDictionary[clubName] = new Dictionary<string, object>
                {
                    { "Descripcion", binding.ContainsKey("wikipediaArticle") ? binding["wikipediaArticle"].value : "N/A" },
                    { "Logo", binding.ContainsKey("logo") ? binding["logo"].value : "N/A" },
                    { "Calle", binding.ContainsKey("street") ? binding["street"].value : "N/A" },
                    { "CP", binding.ContainsKey("postalCode") ? binding["postalCode"].value : "N/A" },
                    { "Ciudad", binding.ContainsKey("city") ? binding["city"].value : "N/A" },
                    { "Pais", binding.ContainsKey("country") ? binding["country"].value : "N/A" },
                    { "FoundingDates", foundingDatesList },
                    { "Awards", awardsList },
                    { "AlternativeNames", aliasList }
                };
                    }
                }
            }

            return clubsDictionary;
        }




        public static Dictionary<string, Dictionary<string, object>> LeerClub3(int limit = 3000, int offset = 0)
        {
            var clubsDictionary = new Dictionary<string, Dictionary<string, object>>();

            // Definir la consulta SPARQL
            String select2 = $@"";
            String where2 = $@"";
            SparqlObject datos = ServiceWIKIDATA.Jsonget(select2, where2);

            // Procesar los resultados
            if (datos.results != null && datos.results.bindings != null && datos.results.bindings.Count > 0)
            {
                foreach (var binding in datos.results.bindings)
                {
                }
            }




                    return clubsDictionary;


        }


        public static Dictionary<string, List<DateTime>> LeerFechasFundacionClub(int limit = 3000, int offset = 0)
        {
            var clubsDictionary = new Dictionary<string, List<DateTime>>();

            // Definir la consulta SPARQL
            string select = $@"
    SELECT ?club ?clubLabel 
       (GROUP_CONCAT(DISTINCT ?foundingDate; separator='', '') AS ?foundingDates)";
            string where = $@"WHERE {{
        ?club wdt:P31 wd:Q476028.   
        OPTIONAL {{ ?club wdt:P571 ?foundingDate. }}  
        SERVICE wikibase:label {{ bd:serviceParam wikibase:language 'en,[AUTO_LANGUAGE]'. }}
    }}
    GROUP BY ?club ?clubLabel
    LIMIT {limit} OFFSET {offset}";

            // Ejecutar la consulta SPARQL usando el método Jsonget
            SparqlObject datos = ServiceWIKIDATA.Jsonget(select,where);

            // Procesar los resultados
            if (datos.results != null && datos.results.bindings != null && datos.results.bindings.Count > 0)
            {
                foreach (var binding in datos.results.bindings)
                {
                    // Obtener el nombre del club
                    string clubName = binding.ContainsKey("clubLabel") ? binding["clubLabel"].value : "N/A";

                    // Si el club no está en el diccionario, lo añadimos
                    if (!clubsDictionary.ContainsKey(clubName))
                    {
                        // Parseo de foundingDates (listas de DateTime)
                        List<DateTime> foundingDatesList = new List<DateTime>();

                        if (binding.ContainsKey("foundingDates") && !string.IsNullOrEmpty(binding["foundingDates"].value))
                        {
                            var datesString = binding["foundingDates"].value;
                            var datesArray = datesString.Split(", "); // Separar por comas

                            foreach (var date in datesArray)
                            {
                                if (DateTime.TryParse(date, out DateTime parsedDate))
                                {
                                    foundingDatesList.Add(parsedDate);  // Añadir a la lista si se puede parsear correctamente
                                }
                            }
                        }

                        // Agregar al diccionario
                        clubsDictionary[clubName] = foundingDatesList;
                    }
                }
            }

            return clubsDictionary;
        }

        public static Dictionary<string, List<string>> LeerNombresAlternativosClub(int limit = 15000, int offset = 0)
        {
            var clubsDictionary = new Dictionary<string, List<string>>();

            // Definir la consulta SPARQL
            string select = @"
    SELECT ?club ?clubLabel 
       (GROUP_CONCAT(DISTINCT ?name; separator=', ') AS ?alternativeNames)";

            string where = $@"WHERE {{
    ?club wdt:P31 wd:Q476028.  
    
    # Get alternative names using skos:altLabel
    OPTIONAL {{ 
        ?club skos:altLabel ?AlternativeNameLabel. 
        FILTER(LANG(?AlternativeNameLabel) IN ('es', 'en','it'))  
        BIND(?AlternativeNameLabel AS ?name)
    }}
    
    # Get nicknames (P1449)
    OPTIONAL {{ 
        ?club wdt:P1449 ?Apodo.  
        FILTER(LANG(?Apodo) IN ('es', 'en','it'))  
        BIND(?Apodo AS ?name)
    }}
    
    SERVICE wikibase:label {{ 
        bd:serviceParam wikibase:language 'en,[AUTO_LANGUAGE]'. 
    }}
}}
GROUP BY ?club ?clubLabel
LIMIT {limit} OFFSET {offset}";

            // Ejecutar la consulta SPARQL usando el método Jsonget
            SparqlObject datos = ServiceWIKIDATA.Jsonget(select, where);

            // Procesar los resultados
            if (datos.results != null && datos.results.bindings != null && datos.results.bindings.Count > 0)
            {
                foreach (var binding in datos.results.bindings)
                {
                    // Obtener el nombre del club
                    string clubName = binding.ContainsKey("clubLabel") ? binding["clubLabel"].value : "N/A";

                    // Si el club no está en el diccionario, lo añadimos
                    if (!clubsDictionary.ContainsKey(clubName))
                    {
                        // Parseo de alternativeNames (lista de strings)
                        List<string> alternativeNamesList = new List<string>();

                        if (binding.ContainsKey("alternativeNames") && !string.IsNullOrEmpty(binding["alternativeNames"].value))
                        {
                            // Separar los nombres alternativos por comas
                            var namesString = binding["alternativeNames"].value;
                            alternativeNamesList = namesString.Split(", ").ToList();  // Convertir a lista de strings
                        }

                        // Agregar al diccionario
                        clubsDictionary[clubName] = alternativeNamesList;
                    }
                }
            }

            return clubsDictionary;
        }


        public static Dictionary<string, List<string>> LeerPremiosClub(int limit = 15000, int offset = 0)
        {
            var clubsDictionary = new Dictionary<string, List<string>>();

            // Definir la consulta SPARQL
            string select = @"
    SELECT ?club ?clubLabel 
       (GROUP_CONCAT(DISTINCT ?awardLabel; separator=', ') AS ?awards)";

            string where = $@"WHERE {{
        ?club wdt:P31 wd:Q476028.  
        ?club wdt:P166 ?award.  
        ?award rdfs:label ?awardLabel.  
        FILTER(LANG(?awardLabel) IN ('en', '[AUTO_LANGUAGE]'))  

        
        SERVICE wikibase:label {{ bd:serviceParam wikibase:language 'en,[AUTO_LANGUAGE]'. }}
    }}
    GROUP BY ?club ?clubLabel
    LIMIT {limit} OFFSET {offset}";

            // Ejecutar la consulta SPARQL usando el método Jsonget
            SparqlObject datos = ServiceWIKIDATA.Jsonget(select, where);

            // Procesar los resultados
            if (datos.results != null && datos.results.bindings != null && datos.results.bindings.Count > 0)
            {
                foreach (var binding in datos.results.bindings)
                {
                    // Obtener el nombre del club
                    string clubName = binding.ContainsKey("clubLabel") ? binding["clubLabel"].value : "N/A";

                    // Si el club no está en el diccionario, lo añadimos
                    if (!clubsDictionary.ContainsKey(clubName))
                    {
                        // Parseo de premios (lista de strings)
                        List<string> awardsList = new List<string>();

                        if (binding.ContainsKey("awards") && !string.IsNullOrEmpty(binding["awards"].value))
                        {
                            // Separar los premios por comas
                            var awardsString = binding["awards"].value;
                            awardsList = awardsString.Split(", ").ToList();  // Convertir a lista de strings
                        }

                        // Agregar al diccionario
                        clubsDictionary[clubName] = awardsList;
                    }
                }
            }

            return clubsDictionary;
        }


        public static Dictionary<string, List<ClubpfihsOntology.PostalAddress>> LeerDireccionesPostalesClubes(int limit = 5000, int offset = 0)
        {
            var clubsDictionary = new Dictionary<string, List<ClubpfihsOntology.PostalAddress>>();

            // Definir la consulta SPARQL
            string select = @"
    SELECT ?club ?clubLabel 
       ?stadiumLabel ?postalCodeLabel ?streetLabel ?cityLabel ?countryLabel  ";

            string where = $@"WHERE {{
        ?club wdt:P31 wd:Q476028.  
        
        OPTIONAL {{
            ?club wdt:P115 ?stadium. 
            OPTIONAL {{ ?stadium wdt:P281 ?postalCode. }}  
            OPTIONAL {{ ?stadium wdt:P669 ?street. }}  
            OPTIONAL {{ ?stadium wdt:P131 ?city. }}  
            OPTIONAL {{ ?stadium wdt:P17 ?country. }}  
        }}

        
        SERVICE wikibase:label {{ bd:serviceParam wikibase:language 'en,[AUTO_LANGUAGE]'. }}
    }}
    GROUP BY ?club ?clubLabel  ?stadiumLabel ?postalCodeLabel ?streetLabel ?cityLabel ?countryLabel
    LIMIT {limit} OFFSET {offset}";

            // Ejecutar la consulta SPARQL usando el método Jsonget
            SparqlObject datos = ServiceWIKIDATA.Jsonget(select, where);

            // Procesar los resultados
            if (datos.results != null && datos.results.bindings != null && datos.results.bindings.Count > 0)
            {
                foreach (var binding in datos.results.bindings)
                {
                    // Obtener el nombre del club
                    string clubName = binding.ContainsKey("clubLabel") ? binding["clubLabel"].value : "N/A";

                    // Si el club no está en el diccionario, lo añadimos
                    if (!clubsDictionary.ContainsKey(clubName))
                    {
                        clubsDictionary[clubName] = new List<ClubpfihsOntology.PostalAddress>();
                    }

                    // Crear un nuevo objeto PostalAddress para este estadio
                    ClubpfihsOntology.PostalAddress pad = new ClubpfihsOntology.PostalAddress
                    {
                        Schema_PostalCode = binding.ContainsKey("postalCodeLabel") ? binding["postalCodeLabel"].value : "",
                        Schema_addressCountry = binding.ContainsKey("countryLabel") ? binding["countryLabel"].value : "",
                        Schema_streetAddress = binding.ContainsKey("streetLabel") ? binding["streetLabel"].value : "",
                        Schema_addressLocality = binding.ContainsKey("cityLabel") ? binding["cityLabel"].value : "",
                        //Schema_stadium = binding.ContainsKey("stadiumLabel") ? binding["stadiumLabel"].value : ""
                    };

                    // Añadir la dirección postal al diccionario del club
                    clubsDictionary[clubName].Add(pad);
                }
            }

            return clubsDictionary;
        }

        public static Dictionary<string, string> LeerLogotiposClub(int limit = 5000, int offset = 0)
        {
            var clubsDictionary = new Dictionary<string, string>();

            // Definir la consulta SPARQL
            string select = @"
    SELECT ?club ?clubLabel ?logo ?logoLabel";

            string where = $@"WHERE {{
        ?club wdt:P31 wd:Q476028.  
        ?club wdt:P154 ?logo.  
        
        
        
        SERVICE wikibase:label {{ bd:serviceParam wikibase:language 'en,[AUTO_LANGUAGE]'. }}
    }}
    LIMIT {limit} OFFSET {offset}";

            // Ejecutar la consulta SPARQL usando el método Jsonget
            SparqlObject datos = ServiceWIKIDATA.Jsonget(select, where);

            // Procesar los resultados
            if (datos.results != null && datos.results.bindings != null && datos.results.bindings.Count > 0)
            {
                foreach (var binding in datos.results.bindings)
                {
                    // Obtener el nombre del club
                    string clubName = binding.ContainsKey("clubLabel") ? binding["clubLabel"].value : "N/A";

                    // Obtener el logotipo del club
                    string logoLabel = binding.ContainsKey("logoLabel") ? binding["logoLabel"].value : "";

                    // Agregar al diccionario
                    if (!clubsDictionary.ContainsKey(clubName))
                    {
                        clubsDictionary[clubName] = logoLabel;
                    }
                }
            }

            return clubsDictionary;
        }

        public static Dictionary<string, List<T>> ObtenerTodaLaInfo<T>(Func<int, int, Dictionary<string, List<T>>> obtenerDatos, int limit = 5000)
        {
            var totalDictionary = new Dictionary<string, List<T>>();
            int offset = 0;
            bool hayMasResultados = true;

            while (hayMasResultados)
            {
                // Obtener resultados con limit y offset
                var parcialDictionary = obtenerDatos(limit, offset);

                // Si no hay resultados nuevos, salir del bucle
                if (parcialDictionary.Count == 0)
                {
                    hayMasResultados = false;
                }
                else
                {
                    // Agregar los resultados parciales al diccionario total
                    foreach (var entry in parcialDictionary)
                    {
                        if (!totalDictionary.ContainsKey(entry.Key))
                        {
                            totalDictionary[entry.Key] = new List<T>();
                        }
                        totalDictionary[entry.Key].AddRange(entry.Value);
                    }

                    // Incrementar el offset para la próxima página de resultados
                    offset += limit;
                }
            }

            return totalDictionary;
        }

        public static Dictionary<string, List<string>> ObtenerTodasLosNombresAlternativos()
        {
            return ObtenerTodaLaInfo(LeerNombresAlternativosClub, 5000);
        }

        public static Dictionary<string, List<string>> ObtenerTodosLosPremios()
        {
            return ObtenerTodaLaInfo(LeerPremiosClub, 5000);
        }

        public static Dictionary<string, List<ClubpfihsOntology.PostalAddress>> ObtenerTodasLasDireccionesPostales()
        {
            return ObtenerTodaLaInfo(LeerDireccionesPostalesClubes, 5000);
        }

        public static Dictionary<string, string> ObtenerTodosLosLogotipos(int limit = 5000)
        {
            var clubsDictionary = new Dictionary<string, string>();
            int offset = 0;
            bool hayMasResultados = true;

            while (hayMasResultados)
            {
                // Definir la consulta SPARQL con limit y offset
                string select = @"
        SELECT ?club ?clubLabel ?logo ?logoLabel";

                string where = $@"WHERE {{
            ?club wdt:P31 wd:Q476028.  
            ?club wdt:P154 ?logo.   
            
            FILTER EXISTS {{
                ?club wdt:P31 wd:Q476028.  
            }}
            
            SERVICE wikibase:label {{ bd:serviceParam wikibase:language 'en,[AUTO_LANGUAGE]'. }}
        }}
        LIMIT {limit} OFFSET {offset}";

                // Ejecutar la consulta SPARQL usando el método Jsonget
                SparqlObject datos = ServiceWIKIDATA.Jsonget(select, where);

                // Procesar los resultados
                if (datos.results != null && datos.results.bindings != null && datos.results.bindings.Count > 0)
                {
                    foreach (var binding in datos.results.bindings)
                    {
                        // Obtener el nombre del club
                        string clubName = binding.ContainsKey("clubLabel") ? binding["clubLabel"].value : "N/A";

                        // Obtener el logotipo del club
                        string logoLabel = binding.ContainsKey("logo") ? binding["logo"].value : "";

                        // Agregar al diccionario solo si no está ya presente
                        if (!clubsDictionary.ContainsKey(clubName))
                        {
                            clubsDictionary[clubName] = logoLabel;
                        }
                    }

                    // Incrementar el offset para la siguiente página
                    offset += limit;
                }
                else
                {
                    // Si no hay más resultados, salir del bucle
                    hayMasResultados = false;
                }
            }

            return clubsDictionary;
        }


        public static Dictionary<string, string> ObtenerTodosLosArticulosWikipedia(int limit = 5000)
        {
            var clubsDictionary = new Dictionary<string, string>();
            int offset = 0;
            bool hayMasResultados = true;

            while (hayMasResultados)
            {
                // Definir la consulta SPARQL con limit y offset
                string select = @"
        SELECT ?club ?clubLabel ?wikipediaArticle";

                string where = $@"WHERE {{
            ?club wdt:P31 wd:Q476028.  
            
            
            ?wikipediaArticle schema:about ?club;
                              schema:isPartOf <https://en.wikipedia.org/>.  
            
            SERVICE wikibase:label {{ bd:serviceParam wikibase:language 'en,[AUTO_LANGUAGE]'. }}
        }}
        LIMIT {limit} OFFSET {offset}";

                // Ejecutar la consulta SPARQL usando el método Jsonget
                SparqlObject datos = ServiceWIKIDATA.Jsonget(select, where);

                // Procesar los resultados
                if (datos.results != null && datos.results.bindings != null && datos.results.bindings.Count > 0)
                {
                    foreach (var binding in datos.results.bindings)
                    {
                        // Obtener el nombre del club
                        string clubName = binding.ContainsKey("clubLabel") ? binding["clubLabel"].value : "N/A";

                        // Obtener el URL del artículo de Wikipedia
                        string wikipediaArticle = binding.ContainsKey("wikipediaArticle") ? binding["wikipediaArticle"].value : "";

                        // Agregar al diccionario solo si no está ya presente
                        if (!clubsDictionary.ContainsKey(clubName))
                        {
                            clubsDictionary[clubName] = wikipediaArticle;
                        }
                    }

                    // Incrementar el offset para la siguiente página
                    offset += limit;
                }
                else
                {
                    // Si no hay más resultados, salir del bucle
                    hayMasResultados = false;
                }
            }

            return clubsDictionary;
        }


        public static Dictionary<string, DateTime> ObtenerFechasFundacionMasAntiguas(int limit = 5000, int offset = 0)
        {
            var clubsDictionary = new Dictionary<string, DateTime>();
            bool hayMasResultados = true;

            while (hayMasResultados)
            {
                // Definir la consulta SPARQL con limit y offset
                string select = @"
        SELECT ?club ?clubLabel ?foundingDate";

                string where = $@"WHERE {{
            ?club wdt:P31 wd:Q476028. 
            ?club wdt:P571 ?foundingDate.  
            
            SERVICE wikibase:label {{ bd:serviceParam wikibase:language 'en,[AUTO_LANGUAGE]'. }}
        }}
        GROUP BY ?club ?clubLabel ?foundingDate
        LIMIT {limit} OFFSET {offset}";

                // Ejecutar la consulta SPARQL usando el método Jsonget
                SparqlObject datos = ServiceWIKIDATA.Jsonget(select, where);

                // Procesar los resultados
                if (datos.results != null && datos.results.bindings != null && datos.results.bindings.Count > 0)
                {
                    foreach (var binding in datos.results.bindings)
                    {
                        // Obtener el nombre del club
                        string clubName = binding.ContainsKey("clubLabel") ? binding["clubLabel"].value : "N/A";

                        // Obtener la fecha de fundación
                        if (binding.ContainsKey("foundingDate") && DateTime.TryParse(binding["foundingDate"].value, out DateTime foundingDate))
                        {
                            // Si el club ya tiene una fecha, comparar y quedarse con la más antigua
                            if (clubsDictionary.ContainsKey(clubName))
                            {
                                if (foundingDate < clubsDictionary[clubName])
                                {
                                    clubsDictionary[clubName] = foundingDate;
                                }
                            }
                            else
                            {
                                // Si es la primera vez que encontramos el club, agregarlo
                                clubsDictionary[clubName] = foundingDate;
                            }
                        }
                    }

                    // Incrementar el offset para la siguiente página
                    offset += limit;
                }
                else
                {
                    // Si no hay más resultados, salir del bucle
                    hayMasResultados = false;
                }
            }

            return clubsDictionary;
        }

        public static void ObtenerDatosEntrenador(string nombreEntrenador, out string entrenadorLabel, out DateTime? fechaNacimiento, out string imagenUrl,
                                          out List<string> premios, out string ciudadNacimiento, out string paisNacimiento,
                                          out string nacionalidad, out int altura)
        {
            // Inicializamos los parámetros out
            entrenadorLabel = string.Empty;
            fechaNacimiento = null;
            imagenUrl = string.Empty;
            premios = new List<string>();
            ciudadNacimiento = string.Empty;
            paisNacimiento = string.Empty;
            nacionalidad = string.Empty;
            altura = 0;

            // Definir la consulta SPARQL
            string select = @"
    SELECT ?entrenadorLabel ?fechaNacimiento ?imagen ?premioLabel ?ciudadNacimientoLabel ?paisNacimientoLabel ?nacionalidadLabel ?altura";

            string where = $@"
    WHERE {{
      ?entrenador wdt:P31 wd:Q5;
                  wdt:P106 wd:Q628099.
    
      OPTIONAL {{ ?entrenador wdt:P569 ?fechaNacimiento. }}
      OPTIONAL {{ ?entrenador wdt:P18 ?imagen. }}
      OPTIONAL {{ ?entrenador wdt:P166 ?premio. }}
      OPTIONAL {{ ?entrenador wdt:P19 ?ciudadNacimiento. 
                 ?ciudadNacimiento wdt:P17 ?paisNacimiento. }}
      OPTIONAL {{ ?entrenador wdt:P27 ?nacionalidad. }}
      OPTIONAL {{ ?entrenador wdt:P2048 ?altura. }}

      ?entrenador rdfs:label ?entrenadorNombre.
      
      FILTER(CONTAINS(LCASE(?entrenadorNombre), '{nombreEntrenador.ToLower()}')).
      
      BIND(IF(STRSTARTS(LCASE(?entrenadorNombre), '{nombreEntrenador.ToLower()}'), 1, IF(CONTAINS(LCASE(?entrenadorNombre), '{nombreEntrenador.ToLower()}'), 2, 3)) AS ?similarityScore)
      
      SERVICE wikibase:label {{ 
      bd:serviceParam wikibase:language '[AUTO_LANGUAGE],en'. 
    }}
    }}
    ORDER BY ?similarityScore
    LIMIT 1";  // Limitamos a 1 resultado sin offset

            // Ejecutar la consulta SPARQL usando el método Jsonget
            SparqlObject datos = ServiceWIKIDATA.Jsonget(select, where);

            // Procesar los resultados
            if (datos.results != null && datos.results.bindings != null && datos.results.bindings.Count > 0)
            {
                var binding = datos.results.bindings[0];

                // Nombre del entrenador
                if (binding.ContainsKey("entrenadorLabel"))
                    entrenadorLabel = binding["entrenadorLabel"].value;

                // Fecha de nacimiento
                if (binding.ContainsKey("fechaNacimiento") && DateTime.TryParse(binding["fechaNacimiento"].value, out DateTime parsedFecha))
                    fechaNacimiento = parsedFecha;

                // Imagen del entrenador
                if (binding.ContainsKey("imagen"))
                    imagenUrl = binding["imagen"].value;

                // Premios (puede haber múltiples premios)
                if (binding.ContainsKey("premioLabel"))
                {
                    var premiosString = binding["premioLabel"].value;
                    premios = premiosString.Split(',').ToList();  // Separa los premios por comas y los convierte a lista
                }

                // Ciudad y país de nacimiento
                if (binding.ContainsKey("ciudadNacimientoLabel"))
                    ciudadNacimiento = binding["ciudadNacimientoLabel"].value;

                if (binding.ContainsKey("paisNacimientoLabel"))
                    paisNacimiento = binding["paisNacimientoLabel"].value;

                // Nacionalidad
                if (binding.ContainsKey("nacionalidadLabel"))
                    nacionalidad = binding["nacionalidadLabel"].value;

                // Altura
                if (binding.ContainsKey("altura") && int.TryParse(binding["altura"].value, out int parsedAltura))
                    altura = parsedAltura;
            }
            else
            {
                // Si no hay resultados, dejar los valores por defecto.
                entrenadorLabel = "No se encontraron resultados.";
            }
        }



        public static void ObtenerDatosPlayer(string nombreEntrenador, out string entrenadorLabel, out DateTime? fechaNacimiento, out string imagenUrl,
                                          out List<string> premios, out string ciudadNacimiento, out string paisNacimiento,
                                          out string nacionalidad, out int altura)
        {
            // Inicializamos los parámetros out
            entrenadorLabel = string.Empty;
            fechaNacimiento = null;
            imagenUrl = string.Empty;
            premios = new List<string>();
            ciudadNacimiento = string.Empty;
            paisNacimiento = string.Empty;
            nacionalidad = string.Empty;
            altura = 0;

            // Definir la consulta SPARQL
            string select = $@"
    SELECT ?entrenadorLabel ?fechaNacimiento ?imagen ?premioLabel ?ciudadNacimientoLabel ?paisNacimientoLabel ?nacionalidadLabel ?altura";

            string where = $@"
    WHERE {{
      ?entrenador wdt:P31 wd:Q5;
                  wdt:P106 wd:Q937857.
    
      OPTIONAL {{ ?entrenador wdt:P569 ?fechaNacimiento. }}
      OPTIONAL {{ ?entrenador wdt:P18 ?imagen. }}
      OPTIONAL {{ ?entrenador wdt:P166 ?premio. }}
      OPTIONAL {{ ?entrenador wdt:P19 ?ciudadNacimiento. 
                 ?ciudadNacimiento wdt:P17 ?paisNacimiento. }}
      OPTIONAL {{ ?entrenador wdt:P27 ?nacionalidad. }}
      OPTIONAL {{ ?entrenador wdt:P2048 ?altura. }}

      ?entrenador rdfs:label ?entrenadorNombre.
      
      FILTER(CONTAINS(LCASE(?entrenadorNombre), '{nombreEntrenador.ToLower()}')).
      
      BIND(IF(STRSTARTS(LCASE(?entrenadorNombre), '{nombreEntrenador.ToLower()}'), 1, IF(CONTAINS(LCASE(?entrenadorNombre), '{nombreEntrenador.ToLower()}'), 2, 3)) AS ?similarityScore)
      
      SERVICE wikibase:label {{ 
      bd:serviceParam wikibase:language '[AUTO_LANGUAGE],en'. 
    }}
    }}
    ORDER BY ?similarityScore
    LIMIT 1";  // Limitamos a 1 resultado sin offset

            // Ejecutar la consulta SPARQL usando el método Jsonget
            SparqlObject datos = ServiceWIKIDATA.Jsonget(select, where);

            // Procesar los resultados
            if (datos.results != null && datos.results.bindings != null && datos.results.bindings.Count > 0)
            {
                var binding = datos.results.bindings[0];

                // Nombre del entrenador
                if (binding.ContainsKey("entrenadorLabel"))
                    entrenadorLabel = binding["entrenadorLabel"].value;

                // Fecha de nacimiento
                if (binding.ContainsKey("fechaNacimiento") && DateTime.TryParse(binding["fechaNacimiento"].value, out DateTime parsedFecha))
                    fechaNacimiento = parsedFecha;

                // Imagen del entrenador
                if (binding.ContainsKey("imagen"))
                    imagenUrl = binding["imagen"].value;

                // Premios (puede haber múltiples premios)
                if (binding.ContainsKey("premioLabel"))
                {
                    var premiosString = binding["premioLabel"].value;
                    premios = premiosString.Split(',').ToList();  // Separa los premios por comas y los convierte a lista
                }

                // Ciudad y país de nacimiento
                if (binding.ContainsKey("ciudadNacimientoLabel"))
                    ciudadNacimiento = binding["ciudadNacimientoLabel"].value;

                if (binding.ContainsKey("paisNacimientoLabel"))
                    paisNacimiento = binding["paisNacimientoLabel"].value;

                // Nacionalidad
                if (binding.ContainsKey("nacionalidadLabel"))
                    nacionalidad = binding["nacionalidadLabel"].value;

                // Altura
                if (binding.ContainsKey("altura") && int.TryParse(binding["altura"].value, out int parsedAltura))
                    altura = parsedAltura;
            }
            else
            {
                // Si no hay resultados, dejar los valores por defecto.
                entrenadorLabel = "No se encontraron resultados.";
            }
        }

        public static string ObtenerClubWIKIIDPorNombre(string nombreClub)
        {
            // Definir la consulta SPARQL
            string select = $@"
    SELECT ?club ";

            string where = $@"WHERE {{
    ?club wdt:P31 wd:Q476028;  # Club is an instance of a football club
          rdfs:label ?clubLabel.

    # Normalize clubLabel and input: remove accents, commas, periods, and spaces
    BIND(LCASE(REPLACE(REPLACE(REPLACE(REPLACE(?clubLabel, ""[ÁÉÍÓÚáéíóú]"", ""a""), ""[,\\.]"", """"), ""\\s"", """"), ""-"", """")) AS ?clubNormalizedLabel)
    BIND(LCASE(REPLACE(REPLACE(REPLACE(REPLACE('{nombreClub}', ""[ÁÉÍÓÚáéíóú]"", ""a""), ""[,\\.]"", """"), ""\\s"", """"), ""-"", """")) AS ?inputNormalized)

    # Ensure the input string contains the normalized main club label
    FILTER(CONTAINS(?inputNormalized, ?clubNormalizedLabel))

    # Mandatory: Club must have skos:altLabel
    ?club skos:altLabel ?alternativeName.
    FILTER(LANG(?alternativeName) IN (""en"", ""es""))

    # Normalize skos:altLabel values (alternative names)
    BIND(LCASE(REPLACE(REPLACE(REPLACE(REPLACE(?alternativeName, ""[ÁÉÍÓÚáéíóú]"", ""a""), ""[,\\.]"", """"), ""\\s"", """"), ""-"", """")) AS ?alternativeNormalizedLabel)

    # Ensure the input string contains the normalized alternative name
    FILTER(CONTAINS(?inputNormalized, ?alternativeNormalizedLabel))
}}
LIMIT 1 ";

            // Ejecutar la consulta SPARQL
            SparqlObject datos = ServiceWIKIDATA.Jsonget(select, where);

            // Procesar los resultados
            if (datos.results != null && datos.results.bindings != null && datos.results.bindings.Count > 0)
            {
                // Obtener el URI del club
                var binding = datos.results.bindings[0];
                if (binding.ContainsKey("club"))
                {
                    return binding["club"].value;
                }
            }

            // Retornar null si no se encuentra
            return null;
        }



    }
}
