using FutbolOntology.CargaPFI;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        
        public async Task GetCoachAsync()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://query.wikidata.org/sparql?query=SELECT ?club ?clubLabel ?nombreEntrenador ?uriEntrenador ?comienzo ?final" +
            "WHERE {" +
            "{" +
            "    ?club wdt:P31 wd:Q476028;" +
            "    rdfs: label? clubLabel." +
            "    FILTER(LANG(?clubLabel) = \"es\" || LANG(?clubLabel) = \"en\")." + "    OPTIONAL { ?club wdt:P1448? alternativeName. }" +
            "FILTER(CONTAINS(LCASE(?clubLabel), \"fc barcelona\") || CONTAINS(LCASE(?alternativeName), \"barça\"))." +
            "  }" + "    UNION" +
      "{" +
    "    ? club wdt:P31 wd:Q103229495;" +
             " rdfs:label? clubLabel." +

        "FILTER(LANG(? clubLabel) = \"es\" || LANG(?clubLabel) = \"en\")." +
        "OPTIONAL { ?club wdt:P1448? alternativeName. }" +

        "FILTER(CONTAINS(LCASE(? clubLabel), \"fc barcelona\") || CONTAINS(LCASE(?alternativeName), \"barça\"))." +
     " }" +
     " ? club  p:P286? o." +
      "?o pq:P580? comienzo." +
      "?o pq:P582? final." +
      "?o ps:P286? uriEntrenador." +
     " ?uriEntrenador rdfs:label? nombreEntrenador" +
      "FILTER (lang(? nombreEntrenador) = 'es')" +
    "}" +

    "ORDER BY ?club ?comienzo" +
    "LIMIT 100");

            request.Headers.Add("Accept", "application/json");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());

        }












        public async Task GetClubAsync()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://query.wikidata.org/sparql?query=SELECT ?club ?clubLabel ?alternativeName ?logo ?foundingDate ?award ?awardLabel ?stadium ?postalCode ?street ?city ?country ?wikipediaArticle             WHERE {  %23 Football club instance  {    %23 Instancia de club de fútbol    ?club wdt:P31 wd:Q476028;        rdfs: label? clubLabel.    FILTER(LANG(?clubLabel) = \"es\" || LANG(?clubLabel) = \"en\").    OPTIONAL { ?club wdt:P1448? alternativeName. }    % 23 Filtrar por club específico('FC Barcelona' o 'Barça')    FILTER(CONTAINS(LCASE(?clubLabel), \"fc barcelona\") || CONTAINS(LCASE(?alternativeName), \"barça\")).  }        UNION  {    %23 Instancia de club de fútbol masculino    ?club wdt:P31 wd:Q103229495;          rdfs:label? clubLabel.    FILTER(LANG(? clubLabel) = \"es\" || LANG(?clubLabel) = \"en\").    OPTIONAL { ?club wdt:P1448? alternativeName. }    %23 Filtrar por club específico('FC Barcelona' o 'Barça')    FILTER(CONTAINS(LCASE(? clubLabel), \"fc barcelona\") || CONTAINS(LCASE(?alternativeName), \"barça\")).  }  %23 Optional logo URL  OPTIONAL    { ?club wdt:P154 ?logo. }    %23 Optional founding date  OPTIONAL    { ?club wdt:P571 ?foundingDate. }    %23 Optional awards and award labels in Spanish  OPTIONAL    {     ?club wdt:P166 ?award.    ?award rdfs:label ?awardLabel FILTER(LANG(?awardLabel) = \"es\").  }    %23 Optional stadium details  OPTIONAL    {    ?club wdt:P115 ?stadium.    OPTIONAL { ?stadium wdt:P281? postalCode. }  %23 Postal code   OPTIONAL { ?stadium wdt:P669? street. }      %23 Street    OPTIONAL { ?stadium wdt:P131? city. }        %23 City    OPTIONAL { ?stadium wdt:P17? country. }      %23 Country    }   %23 Obtener el artículo de Wikipedia en inglés  OPTIONAL    {   ?wikipediaArticle schema:about ?club;        schema:isPartOf <https://en.wikipedia.org/>.  }  %23 Retrieve club, award, and stadium labels in Spanish and English  SERVICE wikibase:label { bd:serviceParam wikibase:language \"es,en\". }}LIMIT 10");
            request.Headers.Add("Accept", "application/json");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());

        }


        public static async Task GetPlayerAsync( )
        {
            //    // Crear el cliente HTTP
            //    var client = new HttpClient();



            //    // Crear la solicitud HTTP con la consulta SPARQL
            //    var request = new HttpRequestMessage(HttpMethod.Get, "https://query.wikidata.org/sparql?query=SELECT ?player ?playerLabel ?wikipediaArticle ?award ?awardLabel WHERE { ?player wdt:P106 wd:Q937857; rdfs:label ?playerLabel. FILTER(CONTAINS(LCASE(?playerLabel), \"iniesta\")). FILTER(LANG(?playerLabel) = \"en\"). OPTIONAL { ?wikipediaArticle schema:about ?player; schema:isPartOf <https://en.wikipedia.org/>. } OPTIONAL { ?player wdt:P166 ?award. ?award rdfs:label ?awardLabel FILTER( LANG(?awardLabel) = \"en\"). } SERVICE wikibase:label { bd:serviceParam wikibase:language \"en\". } } LIMIT 10");



            //    // Configurar los encabezados para aceptar JSON
            //    request.Headers.Add("Accept", "application/sparql-results+json");



            //    // Enviar la solicitud
            //    var response = await client.SendAsync(request);
            //    response.EnsureSuccessStatusCode();  // Lanza excepción si el código no es exitoso



            //    // Leer el contenido de la respuesta como cadena
            //    var jsonString = await response.Content.ReadAsStringAsync();



            //    // Deserializar el JSON
            //    var jsonDoc = JsonDocument.Parse(jsonString);



            //    // Acceder a los resultados dentro del JSON
            //    var results = jsonDoc.RootElement.GetProperty("results").GetProperty("bindings");



            //    // Recorrer los resultados y extraer los campos necesarios
            //    foreach (var result in results.EnumerateArray())
            //    {
            //        var playerLabel = result.GetProperty("playerLabel").GetProperty("value").GetString();
            //        string wikipediaArticle = result.TryGetProperty("wikipediaArticle", out var wikipediaProp) ? wikipediaProp.GetProperty("value").GetString() : "No Wikipedia article";
            //        string awardLabel = result.TryGetProperty("awardLabel", out var awardProp)? awardProp.GetProperty("value").GetString() : "No Award";
            //        Console.WriteLine($"Player: {playerLabel}");
            //        Console.WriteLine($"Wikipedia Article: {wikipediaArticle}");
            //        Console.WriteLine($"Award: {awardLabel}");
            //        Console.WriteLine("----------------------");
                
            //}
        





    }






    public async Task GetCampeonesAsync()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "https://query.wikidata.org/sparql?query=SELECT ?club ?clubLabel ?competition ?competitionLabel ?TorneoLabel ?season             WHERE {  {    %23 Buscar instancias de club de fútbol    ?club wdt:P31 wd:Q476028;  % 23 Instancia de club de fútbol          rdfs:label? clubLabel.    % 23 Filtro por nombre del club(por ejemplo 'FC Barcelona')    FILTER(CONTAINS(LCASE(?clubLabel), \"fc barcelona\")).    FILTER(LANG(?clubLabel) = \"en\").  }        UNION  {    %23 Buscar instancias de club de fútbol masculino    ? club wdt:P31 wd:Q103229495;  %23 Instancia de club de fútbol masculino          rdfs:label? clubLabel.      %23 Filtro por nombre del club (por ejemplo 'FC Barcelona')   FILTER(CONTAINS(LCASE(? clubLabel), \"fc barcelona\")).    FILTER(LANG(? clubLabel) = \"en\").  }  %23 Obtener competiciones ganadas(P166) y filtrar solo competiciones deportivas(Q3241044)  ? club wdt:P2522? competition.  %23 Premios o competiciones ganadas  ?competition wdt:P3450? Torneo  optional{    ?competition wdt:P585? season    }  %23 Limitar los resultados en inglés  SERVICE wikibase:label { bd:serviceParam wikibase:language \"en\". }}ORDER BY ?competitionLabel");
        request.Headers.Add("Accept", "application/json");
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        Console.WriteLine(await response.Content.ReadAsStringAsync());

    }
}
}
