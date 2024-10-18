using FutbolOntology.CargaPFI;
using Gnoss.ApiWrapper.ApiModel;
using Newtonsoft.Json;
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
using System.Threading.Tasks;
using static Gnoss.ApiWrapper.ApiModel.SparqlObject;


namespace FutbolOntology.SPARQL
{
    public class ServiceWIKIDATA
    {


        public static void Jasonget(string nombre)
        {
            WebClient webClient = new();
            webClient.Encoding = Encoding.UTF8;
            webClient.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
            webClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
            webClient.Headers.Add("accept", "application/sparql-results+json");


           

            // Definir la consulta SPARQL
            String select2 = $@"SELECT ?player ?playerLabel ?wikipediaArticle ?award ?awardLabel";
            String where2 = $@" WHERE {{ 
                    ?player wdt:P106 wd:Q937857;  
                        rdfs:label ?playerLabel.
   
            FILTER(CONTAINS(LCASE(?playerLabel), ""{nombre}"")).
            

 
            OPTIONAL {{
             ?wikipediaArticle schema:about ?player;
                      schema:isPartOf <https://en.wikipedia.org/>.
                 }}

                OPTIONAL {{ 
                ?player wdt:P166 ?award.  # Premios (P166)
                ?award rdfs:label ?awardLabel FILTER( LANG(?awardLabel) = ""en"").
                }}
             # Limitar las etiquetas en inglés
                     SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""en"". }}
                  }} LIMIT 1";

            String sparqlQuery = select2 + where2;

            System.Collections.Specialized.NameValueCollection parametros = new     System.Collections.Specialized.NameValueCollection {
                { "query", sparqlQuery.ToString() }
            };
            byte[] responseArray = null;
            int numIntentos = 0;
            string error = "";
            while (responseArray == null && numIntentos < 5)
            {
                numIntentos++;
                try
                {
                    responseArray = webClient.UploadValues("https://query.wikidata.org/sparql", "POST", parametros);
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
            }
            string jsonRespuesta = System.Text.Encoding.UTF8.GetString(responseArray);
            SparqlObject datos = new SparqlObject();
            if (!string.IsNullOrEmpty(jsonRespuesta))
            {
                datos = JsonConvert.DeserializeObject<SparqlObject>(jsonRespuesta);
            }

            if (datos.results != null && datos.results.bindings != null && datos.results.bindings.Count > 0)
            {
                foreach (var binding in datos.results.bindings)
                {
                    if (binding.ContainsKey("playerLabel"))
                    {
                        Console.WriteLine(binding["playerLabel"].value);
                    }
                }
            }
        }

    }
}
