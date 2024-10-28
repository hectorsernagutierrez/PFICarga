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



		/// <summary> Inicializacion de la clase Club. </summary>
		/// <param name="api"></param>
		public Club(ResourceApi api)
		{
			this.apiRecursos = api;
		}
		/// <summary>
		/// Carga todos los clubes que aparezca en el csv de la rutaDirectorioClub. 
		/// </summary>
		/// <param name="rutaDirectorioClub"></param>
		/// <param name="rutaDirectorioAppearance">Direccion del csv donde se leen participación en cada partido de cada jugador.</param>
		/// <param name="rutaDirectorioValoracion"></param>
		public void CargarTodosClub(string rutaDirectorioClub, string rutaDirectorioAppearance, string rutaDirectorioValoracion)
		{

			int i = 0;
			var service = new DTOService();

			List<ClubsDTO> clubs = service.ReadClubs(rutaDirectorioClub);
			// Dictionary<string, Dictionary<string, object>> diccionarioGrande = ServiceWIKIDATA.LeerClub2(3000, 0);
			// Dictionary<string, object> diccionariopeque = new Dictionary<string, object>();

			//var nombresAlternativosDiccionario = ServiceWIKIDATA.ObtenerTodasLosNombresAlternativos();
			//var logotiposDiccionario = ServiceWIKIDATA.LeerLogotiposClub();
			var entrenadoresDiccionario = ServiceWIKIDATA.ObtenerEntrenadoresPorClub(10000, 0);
			//var premiosDiccionario = ServiceWIKIDATA.ObtenerTodosLosPremios();
			//var direccionesPostalesDiccionario = ServiceWIKIDATA.ObtenerTodasLasDireccionesPostales();
			//var wikipediaDiccionario = ServiceWIKIDATA.ObtenerTodosLosArticulosWikipedia();
			//var fundacionDiccionario = ServiceWIKIDATA.ObtenerFechasFundacionMasAntiguas();
			//// Obtener la ruta base de la aplicación


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
						ServiceWIKIDATA.LeerClub(club.ClubId, out string descrip, out string logo, out string cp, out string calle, out string ciudad, out string pais, out List<DateTime> fundacion1, out List<string> prem, out List<string> nomalt);
						sportsClub.Schema_alternateName = nomalt;
						sportsClub.Schema_description = descrip;
						sportsClub.Schema_award = prem;
						ClubpfihsOntology.PostalAddress loc = new ClubpfihsOntology.PostalAddress();
						loc.Schema_PostalCode = cp;
						loc.Schema_streetAddress = calle;
						loc.Schema_addressLocality = ciudad;
						loc.Schema_addressCountry = pais;
						sportsClub.Schema_location = new List<ClubpfihsOntology.PostalAddress>();
						sportsClub.Schema_logo = logo;
						sportsClub.Schema_location.Add(loc);
						sportsClub.Schema_foundingDate = fundacion1.First();



						if (entrenadoresDiccionario.ContainsKey(club.ClubId))
						{
							listaEntr = (Dictionary<string, List<DateTime>>)entrenadoresDiccionario[club.ClubId];
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
						ComplexOntologyResource recursoClub = sportsClub.ToGnossApiResource(apiRecursos, new List<string> { "Club" }, Guid.NewGuid(), Guid.NewGuid());
						apiRecursos.LoadComplexSemanticResource(recursoClub);
					}
				}
				catch (Exception ex)
				{
					Console.Write("error:  "); Console.WriteLine(ex.ToString());
				}
			}




		}


		/// <summary>
		/// Separar una lista de entrenadores een un club por temporada en un diccionario. 
		/// La clave del diccionario es el nombre del entrenador y el valor es una lista de años en la que el 
		/// entrenador ha formado partee de la plantilla de dicho club.
		/// </summary>
		/// <param name="listaEntr"></param>
		/// <returns></returns>
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


		/// <summary>
		/// Consigo la uri del  OC  temporada een nuestro grafo, si no existe lo creo.
		/// </summary>
		/// <param name="temporadaName"></param>
		/// <returns></returns>
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


		/// <summary>
		/// Carga plantilla por temporada de un club.
		/// </summary>
		/// <param name="clubId"></param>
		/// <param name="aparicionesDelClub"></param>
		/// <param name="listaEntr"></param>
		/// <param name="rutaDirectorioValoracion"></param>
		/// <returns></returns>
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
					Console.WriteLine($@"Cargo Jugador {jugador.playerName}");
					jugadoresuri.Add(torneo.getPlayerUrl(jugador.PlayerId, jugador.playerName, rutaDirectorioValoracion));
				}
				planti.IdsSchema_athlete = jugadoresuri;

				plantillas.Add(planti);
			}

			return plantillas;
		}

		/// <summary>
		/// Agrupo un conjunto de apariciones por clubId para obtener un diccionario con el clubId como clave y una lista de apariciones como valor.
		/// 
		/// </summary>
		/// <param name="appearances"></param>
		/// <returns></returns>

		public Dictionary<string, List<AppearancesDTO>> AgruparPorClub(List<AppearancesDTO> appearances)
		{
			// Agrupamos las apariciones por clubId
			return appearances
				.GroupBy(a => a.playerClubId)
				.ToDictionary(g => g.Key, g => g.ToList());
		}

	}



}

/// <summary>
/// DTO para la agrupación dee jugadores por club y temporada.
/// </summary>
public class ClubTemporadaDTO
{
	public string Temporada { get; set; }
	public string IdPlayer { get; set; }
	public string NamePlayer { get; set; }
	public string Club { get; set; }
	public List<AppearancesDTO> Jugadores { get; set; }
}