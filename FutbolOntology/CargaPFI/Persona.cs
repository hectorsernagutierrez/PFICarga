using FutbolOntology.DTO;
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
using System.Xml.Serialization;
using Gnoss.ApiWrapper.Model;
using System.Numerics;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using FutbolOntology.SPARQL;


namespace FutbolOntology.CargaPFI
    {
    public class Persona
    {
    
            private ResourceApi apiRecursos;
            private string ontologiaPersona = "personapfihs";

        

        /// <summary>
        /// </summary>
        /// <param name="api"></param>
        public Persona(ResourceApi api)
            {
                this.apiRecursos = api;
            }
		/// <summary>
		/// Cargo las personas en la ontología
		/// </summary>
		/// <param name="rutaDirectorioPersona"></param>
		/// <param name="rutaDirectorioPersonaValoracion"></param>
		/// <param name="rutaDirectorioConsulta"></param>
		public async void CargarPersona(string rutaDirectorioPersona, string rutaDirectorioPersonaValoracion , string rutaDirectorioConsulta)
        {
            var service = new DTOService();
            List<PlayersDTO> players = service.ReadPlayers(rutaDirectorioPersona);
            List<PlayerValuationsDTO> playerValuations = service.ReadPlayerValuations(rutaDirectorioPersonaValoracion);
            Dictionary<string, string> descripciones = DTOService.ReadPlayerUrisFromCsv(rutaDirectorioConsulta);
            Dictionary<string, List<string>> awards = new Dictionary<string, List<string>>();
            ServiceWIKIDATA.LeerAwards(0,out awards, awards);
            //ServiceWIKIDATA.LeerWikiDatasJugadores(0,out descripciones,descripciones);
            foreach (var player in players)
            {
                string descr;
                if (descripciones.ContainsKey(player.Name))
                {
                    descr = descripciones[player.Name];
                }
                else
                {
                    descr = "";
                }



               
                List<string> award;
                if (awards.ContainsKey(player.Name))
                {
                    award = awards[player.Name];
                }
                else
                {
                    award = null;
                }
                        
                string uri = CargarPersonaSola(player, playerValuations, descr, award);
            
            }          
           
         }

		/// <summary>
		/// Carga de un solo jugador en la ontología
		/// </summary>
		/// <param name="player"></param>
		/// <param name="playerValuations"></param>
		/// <param name="descr"></param>
		/// <param name="award"></param>
		/// <returns></returns>
		public string CargarPersonaSola(PlayersDTO player, List<PlayerValuationsDTO> playerValuations,string descr, List<string> award)
        {
            Person persona = new Person();
            persona.Schema_identifier = player.PlayerId;
            persona.Eschema_foot = player.Foot;
            persona.Schema_name = player.Name;
            persona.Schema_image = new List<string>();
            persona.Schema_image.Add(player.ImageUrl);
            persona.Schema_birthDate = player.DateOfBirth;
            PersonapfihsOntology.PostalAddress postalAddress = new PostalAddress();
            postalAddress.Schema_addressLocality = player.CityOfBirth;
            postalAddress.Schema_addressCountry = player.CountryOfBirth;
            persona.Schema_birthPlace = postalAddress;
            persona.Schema_height = player.HeightInCm;
            persona.Schema_networth = new List<PersonapfihsOntology.PriceSpecification>();
            foreach (var valuation in playerValuations)
            {
                if ((valuation != null) && (valuation.PlayerId == player.PlayerId))
                {
                    PriceSpecification priceSpecification = new PriceSpecification();
                    priceSpecification.Schema_price = (int?)valuation.MarketValueInEur;                   
                    priceSpecification.Schema_validFrom =(DateTime?) valuation.Date;
                    persona.Schema_networth.Add(priceSpecification);    
                }
                
            }

            if (!String.IsNullOrEmpty(descr))
            {
                persona.Schema_award = award;
                persona.Schema_description = descr;
            }
            else
            {
                //Llamo a wikidata para rellenar los datos que me quedan;
                //string description;
                //persona.Schema_award = ServiceWIKIDATA.LeerJugador(persona.Schema_name, out description);
                //persona.Schema_description = description;
                persona.Schema_description = $"https://en.wikipedia.org/wiki/{ persona.Schema_name.Replace(" ", "") }";

            }
            Console.WriteLine(persona.ToString());
            Console.WriteLine();    

            apiRecursos.ChangeOntology(ontologiaPersona);
            ComplexOntologyResource recursoPersona = persona.ToGnossApiResource(apiRecursos, new List<string> { "Players" }, Guid.NewGuid(), Guid.NewGuid());
           string uri =  apiRecursos.LoadComplexSemanticResource(recursoPersona);
            return uri;
        }

    }
    }


