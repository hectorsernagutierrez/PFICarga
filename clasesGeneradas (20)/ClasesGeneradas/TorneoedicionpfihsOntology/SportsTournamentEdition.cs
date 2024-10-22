using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.Model;
using Gnoss.ApiWrapper.Helpers;
using GnossBase;
using Es.Riam.Gnoss.Web.MVC.Models;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using Gnoss.ApiWrapper.Exceptions;
using System.Diagnostics.CodeAnalysis;
using Thing = TemporadapfihsOntology.Thing;
using SportsEvent = PartidopfihsOntology.SportsEvent;
using SportsClub = ClubpfihsOntology.SportsClub;
using Organization = OrganizacionpfihsOntology.Organization;

namespace TorneoedicionpfihsOntology
{
	[ExcludeFromCodeCoverage]
	public class SportsTournamentEdition : GnossOCBase
	{
		public SportsTournamentEdition() : base() { } 

		public SportsTournamentEdition(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			GNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			SemanticPropertyModel propEschema_season = pSemCmsModel.GetPropertyByPath("https://schema.org/extended/season");
			if (propEschema_season != null && propEschema_season.PropertyValues.Count > 0 && propEschema_season.PropertyValues[0].RelatedEntity != null)
			{
				Eschema_season = new Thing(propEschema_season.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			Eschema_subEvent = new List<SportsEvent>();
			SemanticPropertyModel propEschema_subEvent = pSemCmsModel.GetPropertyByPath("https://schema.org/extended/subEvent");
			if(propEschema_subEvent != null && propEschema_subEvent.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propEschema_subEvent.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						SportsEvent eschema_subEvent = new SportsEvent(propValue.RelatedEntity,idiomaUsuario);
						Eschema_subEvent.Add(eschema_subEvent);
					}
				}
			}
			Eschema_particpants = new List<SportsClub>();
			SemanticPropertyModel propEschema_particpants = pSemCmsModel.GetPropertyByPath("https://schema.org/extended/particpants");
			if(propEschema_particpants != null && propEschema_particpants.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propEschema_particpants.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						SportsClub eschema_particpants = new SportsClub(propValue.RelatedEntity,idiomaUsuario);
						Eschema_particpants.Add(eschema_particpants);
					}
				}
			}
			SemanticPropertyModel propEschema_winner = pSemCmsModel.GetPropertyByPath("https://schema.org/extended/winner");
			if (propEschema_winner != null && propEschema_winner.PropertyValues.Count > 0 && propEschema_winner.PropertyValues[0].RelatedEntity != null)
			{
				Eschema_winner = new SportsClub(propEschema_winner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			Schema_organizer = new List<Organization>();
			SemanticPropertyModel propSchema_organizer = pSemCmsModel.GetPropertyByPath("https://schema.org/organizer");
			if(propSchema_organizer != null && propSchema_organizer.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_organizer.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Organization schema_organizer = new Organization(propValue.RelatedEntity,idiomaUsuario);
						Schema_organizer.Add(schema_organizer);
					}
				}
			}
			this.Schema_name = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/name"));
			this.Schema_identifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/identifier"));
			this.Schema_alternativeName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/alternativeName"));
		}

		public SportsTournamentEdition(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			mGNOSSID = pSemCmsModel.Entity.Uri;
			mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propEschema_season = pSemCmsModel.GetPropertyByPath("https://schema.org/extended/season");
			if (propEschema_season != null && propEschema_season.PropertyValues.Count > 0 && propEschema_season.PropertyValues[0].RelatedEntity != null)
			{
				Eschema_season = new Thing(propEschema_season.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			Eschema_subEvent = new List<SportsEvent>();
			SemanticPropertyModel propEschema_subEvent = pSemCmsModel.GetPropertyByPath("https://schema.org/extended/subEvent");
			if(propEschema_subEvent != null && propEschema_subEvent.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propEschema_subEvent.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						SportsEvent eschema_subEvent = new SportsEvent(propValue.RelatedEntity,idiomaUsuario);
						Eschema_subEvent.Add(eschema_subEvent);
					}
				}
			}
			Eschema_particpants = new List<SportsClub>();
			SemanticPropertyModel propEschema_particpants = pSemCmsModel.GetPropertyByPath("https://schema.org/extended/particpants");
			if(propEschema_particpants != null && propEschema_particpants.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propEschema_particpants.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						SportsClub eschema_particpants = new SportsClub(propValue.RelatedEntity,idiomaUsuario);
						Eschema_particpants.Add(eschema_particpants);
					}
				}
			}
			SemanticPropertyModel propEschema_winner = pSemCmsModel.GetPropertyByPath("https://schema.org/extended/winner");
			if (propEschema_winner != null && propEschema_winner.PropertyValues.Count > 0 && propEschema_winner.PropertyValues[0].RelatedEntity != null)
			{
				Eschema_winner = new SportsClub(propEschema_winner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			Schema_organizer = new List<Organization>();
			SemanticPropertyModel propSchema_organizer = pSemCmsModel.GetPropertyByPath("https://schema.org/organizer");
			if(propSchema_organizer != null && propSchema_organizer.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_organizer.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Organization schema_organizer = new Organization(propValue.RelatedEntity,idiomaUsuario);
						Schema_organizer.Add(schema_organizer);
					}
				}
			}
			this.Schema_name = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/name"));
			this.Schema_identifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/identifier"));
			this.Schema_alternativeName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/alternativeName"));
		}

		public virtual string RdfType { get { return "https://schema.org/extended/SportsTournamentEdition"; } }
		public virtual string RdfsLabel { get { return "https://schema.org/extended/SportsTournamentEdition"; } }
		[LABEL(LanguageEnum.en,"Season")]
		[LABEL(LanguageEnum.es,"Temporada")]
		[RDFProperty("https://schema.org/extended/season")]
		public  Thing Eschema_season  { get; set;} 
		public string IdEschema_season  { get; set;} 

		[LABEL(LanguageEnum.es,"Partidos")]
		[LABEL(LanguageEnum.en,"Matches")]
		[RDFProperty("https://schema.org/extended/subEvent")]
		public  List<SportsEvent> Eschema_subEvent { get; set;}
		public List<string> IdsEschema_subEvent { get; set;}

		[LABEL(LanguageEnum.es,"Clubs Participantes")]
		[LABEL(LanguageEnum.en,"Participants Clubs")]
		[RDFProperty("https://schema.org/extended/particpants")]
		public  List<SportsClub> Eschema_particpants { get; set;}
		public List<string> IdsEschema_particpants { get; set;}

		[LABEL(LanguageEnum.es,"Campeón")]
		[LABEL(LanguageEnum.en,"Champion")]
		[RDFProperty("https://schema.org/extended/winner")]
		public  SportsClub Eschema_winner  { get; set;} 
		public string IdEschema_winner  { get; set;} 

		[LABEL(LanguageEnum.en,"Organizer")]
		[LABEL(LanguageEnum.es,"Organizador")]
		[RDFProperty("https://schema.org/organizer")]
		public  List<Organization> Schema_organizer { get; set;}
		public List<string> IdsSchema_organizer { get; set;}

		[LABEL(LanguageEnum.es,"Nombre")]
		[LABEL(LanguageEnum.en,"Name")]
		[RDFProperty("https://schema.org/name")]
		public  string Schema_name { get; set;}

		[LABEL(LanguageEnum.es,"Id")]
		[RDFProperty("https://schema.org/identifier")]
		public  string Schema_identifier { get; set;}

		[LABEL(LanguageEnum.es,"Alias (AKA)")]
		[RDFProperty("https://schema.org/alternativeName")]
		public  string Schema_alternativeName { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("eschema:season", this.IdEschema_season));
			propList.Add(new ListStringOntologyProperty("eschema:subEvent", this.IdsEschema_subEvent));
			propList.Add(new ListStringOntologyProperty("eschema:particpants", this.IdsEschema_particpants));
			propList.Add(new StringOntologyProperty("eschema:winner", this.IdEschema_winner));
			propList.Add(new ListStringOntologyProperty("schema:organizer", this.IdsSchema_organizer));
			propList.Add(new StringOntologyProperty("schema:name", this.Schema_name));
			propList.Add(new StringOntologyProperty("schema:identifier", this.Schema_identifier));
			propList.Add(new StringOntologyProperty("schema:alternativeName", this.Schema_alternativeName));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 
		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI)
		{
			return ToGnossApiResource(resourceAPI, new List<string>());
		}

		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias)
		{
			return ToGnossApiResource(resourceAPI, listaDeCategorias, Guid.Empty, Guid.Empty);
		}

		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<Guid> listaDeCategorias)
		{
			return ToGnossApiResource(resourceAPI, null, Guid.Empty, Guid.Empty, listaDeCategorias);
		}

		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias, Guid idrecurso, Guid idarticulo, List<Guid> listaIdDeCategorias = null)
		{
			ComplexOntologyResource resource = new ComplexOntologyResource();
			Ontology ontology = null;
			GetProperties();
			if(idrecurso.Equals(Guid.Empty) && idarticulo.Equals(Guid.Empty))
			{
				ontology = new Ontology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, RdfType, RdfsLabel, prefList, propList, entList);
			}
			else{
				ontology = new Ontology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, RdfType, RdfsLabel, prefList, propList, entList,idrecurso,idarticulo);
			}
			resource.Id = GNOSSID;
			resource.Ontology = ontology;
			resource.TextCategories = listaDeCategorias;
			resource.CategoriesIds = listaIdDeCategorias;
			AddResourceTitle(resource);
			AddResourceDescription(resource);
			AddImages(resource);
			AddFiles(resource);
			return resource;
		}

		public override List<string> ToOntologyGnossTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTournamentEdition_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/extended/SportsTournamentEdition>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTournamentEdition_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/extended/SportsTournamentEdition\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/SportsTournamentEdition_{ResourceID}_{ArticleID}>", list, " . ");
				if(this.IdEschema_season != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTournamentEdition_{ResourceID}_{ArticleID}", "https://schema.org/extended/season",  $"<{this.IdEschema_season}>", list, " . ");
				}
				if(this.IdsEschema_subEvent != null)
				{
					foreach(var item2 in this.IdsEschema_subEvent)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTournamentEdition_{ResourceID}_{ArticleID}", "https://schema.org/extended/subEvent", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsEschema_particpants != null)
				{
					foreach(var item2 in this.IdsEschema_particpants)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTournamentEdition_{ResourceID}_{ArticleID}", "https://schema.org/extended/particpants", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdEschema_winner != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTournamentEdition_{ResourceID}_{ArticleID}",  "https://schema.org/extended/winner", $"<{this.IdEschema_winner}>", list, " . ");
				}
				if(this.IdsSchema_organizer != null)
				{
					foreach(var item2 in this.IdsSchema_organizer)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTournamentEdition_{ResourceID}_{ArticleID}", "https://schema.org/organizer", $"<{item2}>", list, " . ");
					}
				}
				if(this.Schema_name != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTournamentEdition_{ResourceID}_{ArticleID}", "https://schema.org/name",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
				}
				if(this.Schema_identifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTournamentEdition_{ResourceID}_{ArticleID}", "https://schema.org/identifier",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_identifier)}\"", list, " . ");
				}
				if(this.Schema_alternativeName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTournamentEdition_{ResourceID}_{ArticleID}",  "https://schema.org/alternativeName", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_alternativeName)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"torneoedicionpfihs\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"https://schema.org/extended/SportsTournamentEdition\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
			string search = string.Empty;
				if(this.IdEschema_season != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdEschema_season;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/extended/season",  $"<{itemRegex}>", list, " . ");
				}
				if(this.IdsEschema_subEvent != null)
				{
					foreach(var item2 in this.IdsEschema_subEvent)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/extended/subEvent", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsEschema_particpants != null)
				{
					foreach(var item2 in this.IdsEschema_particpants)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/extended/particpants", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdEschema_winner != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdEschema_winner;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://schema.org/extended/winner", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdsSchema_organizer != null)
				{
					foreach(var item2 in this.IdsSchema_organizer)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/organizer", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.Schema_name != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/name",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
				}
				if(this.Schema_identifier != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/identifier",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_identifier)}\"", list, " . ");
				}
				if(this.Schema_alternativeName != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://schema.org/alternativeName", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_alternativeName)}\"", list, " . ");
				}
			if (listaSearch != null && listaSearch.Count > 0)
			{
				foreach(string valorSearch in listaSearch)
				{
					search += $"{valorSearch} ";
				}
			}
			if(!string.IsNullOrEmpty(search))
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/search", $"\"{GenerarTextoSinSaltoDeLinea(search.ToLower())}\"", list, " . ");
			}
			return list;
		}

		public override KeyValuePair<Guid, string> ToAcidData(ResourceApi resourceAPI)
		{

			//Insert en la tabla Documento
			string tags = "";
			foreach(string tag in tagList)
			{
				tags += $"{tag}, ";
			}
			if (!string.IsNullOrEmpty(tags))
			{
				tags = tags.Substring(0, tags.LastIndexOf(','));
			}
			string titulo = $"{this.Schema_name.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string descripcion = $"{this.Schema_name.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '{descripcion}', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/TorneoedicionpfihsOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
			resource.Title = this.Schema_name;
		}

		internal void AddResourceDescription(ComplexOntologyResource resource)
		{
			resource.Description = this.Schema_name;
		}




	}
}
