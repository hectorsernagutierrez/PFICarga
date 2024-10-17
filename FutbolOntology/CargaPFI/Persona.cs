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


namespace FutbolOntology.CargaPFI
    {
    internal class Persona
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

        public void CargarPersona(string rutaDirectorioPersona, string rutaDirectorioPersonaValoracion)
        {
            var service = new DTOService();
            List<PlayersDTO> players = service.ReadPlayers(rutaDirectorioPersona);
            List<PlayerValuationsDTO> playerValuations = service.ReadPlayerValuations(rutaDirectorioPersonaValoracion);
            foreach (var player in players)
            {
                string uri = CargarPersonaAClub(player, playerValuations);
            }
           


         }

        public string CargarPersonaSola(PlayersDTO player, List<PlayerValuationsDTO> playerValuations)
        {
            Person persona = new Person();
            persona.Schema_identifier = player.PlayerId;
            persona.Schema_name = player.Name;
            persona.Schema_image.Add(player.ImageUrl);
            persona.Schema_birthDate = player.DateOfBirth;
            PersonapfihsOntology.PostalAddress postalAddress = new PostalAddress();
            postalAddress.Schema_addressLocality = player.CityOfBirth;
            postalAddress.Schema_addressCountry = player.CountryOfBirth;
            persona.Schema_birthPlace = postalAddress;
            persona.Schema_height = player.HeightInCm;

            foreach (var valuation in playerValuations)
            {
                if ((valuation != null) && (valuation.PlayerId == player.PlayerId))
                {
                    PriceSpecification priceSpecification = new PriceSpecification();
                    priceSpecification.Schema_price = (int?)valuation.MarketValueInEur;
                    priceSpecification.Schema_validFrom = (DateTime.TryParseExact(valuation.Date, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var result) ? result : (DateTime?)null);
                }

            }

            //Consultas
            //persona.Schema_description=player.
            //persona.Schema_award=player.


            apiRecursos.ChangeOntology(ontologiaPersona);
            ComplexOntologyResource recursoPersona = persona.ToGnossApiResource(apiRecursos, new List<string> { "Players" }, Guid.NewGuid(), Guid.NewGuid());
           string uri =  apiRecursos.LoadComplexSemanticResource(recursoPersona);
            return uri;
        }

    }
    }


