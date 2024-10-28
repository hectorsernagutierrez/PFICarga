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

namespace PartidopfihsOntology
{
	[ExcludeFromCodeCoverage]
	public class SportsEvent : GnossOCBase
	{
		public SportsEvent() : base() { } 

		public SportsEvent(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			GNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			SemanticPropertyModel propSchema_awayTeam = pSemCmsModel.GetPropertyByPath("https://schema.org/awayTeam");
			if (propSchema_awayTeam != null && propSchema_awayTeam.PropertyValues.Count > 0 && propSchema_awayTeam.PropertyValues[0].RelatedEntity != null)
			{
				Schema_awayTeam = new SportsTeam(propSchema_awayTeam.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propSchema_homeTeam = pSemCmsModel.GetPropertyByPath("https://schema.org/homeTeam");
			if (propSchema_homeTeam != null && propSchema_homeTeam.PropertyValues.Count > 0 && propSchema_homeTeam.PropertyValues[0].RelatedEntity != null)
			{
				Schema_homeTeam = new SportsTeam(propSchema_homeTeam.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			Schema_subEvent = new List<Event>();
			SemanticPropertyModel propSchema_subEvent = pSemCmsModel.GetPropertyByPath("https://schema.org/subEvent");
			if(propSchema_subEvent != null && propSchema_subEvent.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_subEvent.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Event schema_subEvent = new Event(propValue.RelatedEntity,idiomaUsuario);
						Schema_subEvent.Add(schema_subEvent);
					}
				}
			}
			this.Eschema_tounamentId = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/extended/tounamentId"));
			SemanticPropertyModel propEschema_referee = pSemCmsModel.GetPropertyByPath("https://schema.org/extended/referee");
			this.Eschema_referee = new List<string>();
			if (propEschema_referee != null && propEschema_referee.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propEschema_referee.PropertyValues)
				{
					this.Eschema_referee.Add(propValue.Value);
				}
			}
			this.Eschema_identifierpartido = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/extended/identifierpartido"));
			this.Schema_date = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/date"));
			this.Eschema_namePartido = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/extended/namePartido"));
			this.Eschema_result = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/extended/result"));
		}

		public SportsEvent(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			mGNOSSID = pSemCmsModel.Entity.Uri;
			mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propSchema_awayTeam = pSemCmsModel.GetPropertyByPath("https://schema.org/awayTeam");
			if (propSchema_awayTeam != null && propSchema_awayTeam.PropertyValues.Count > 0 && propSchema_awayTeam.PropertyValues[0].RelatedEntity != null)
			{
				Schema_awayTeam = new SportsTeam(propSchema_awayTeam.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propSchema_homeTeam = pSemCmsModel.GetPropertyByPath("https://schema.org/homeTeam");
			if (propSchema_homeTeam != null && propSchema_homeTeam.PropertyValues.Count > 0 && propSchema_homeTeam.PropertyValues[0].RelatedEntity != null)
			{
				Schema_homeTeam = new SportsTeam(propSchema_homeTeam.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			Schema_subEvent = new List<Event>();
			SemanticPropertyModel propSchema_subEvent = pSemCmsModel.GetPropertyByPath("https://schema.org/subEvent");
			if(propSchema_subEvent != null && propSchema_subEvent.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_subEvent.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Event schema_subEvent = new Event(propValue.RelatedEntity,idiomaUsuario);
						Schema_subEvent.Add(schema_subEvent);
					}
				}
			}
			this.Eschema_tounamentId = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/extended/tounamentId"));
			SemanticPropertyModel propEschema_referee = pSemCmsModel.GetPropertyByPath("https://schema.org/extended/referee");
			this.Eschema_referee = new List<string>();
			if (propEschema_referee != null && propEschema_referee.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propEschema_referee.PropertyValues)
				{
					this.Eschema_referee.Add(propValue.Value);
				}
			}
			this.Eschema_identifierpartido = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/extended/identifierpartido"));
			this.Schema_date = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/date"));
			this.Eschema_namePartido = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/extended/namePartido"));
			this.Eschema_result = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/extended/result"));
		}

		public virtual string RdfType { get { return "https://schema.org/SportsEvent"; } }
		public virtual string RdfsLabel { get { return "https://schema.org/SportsEvent"; } }
		[LABEL(LanguageEnum.es,"https://schema.org/awayTeam")]
		[RDFProperty("https://schema.org/awayTeam")]
		public  SportsTeam Schema_awayTeam { get; set;}

		[LABEL(LanguageEnum.es,"Local")]
		[RDFProperty("https://schema.org/homeTeam")]
		public  SportsTeam Schema_homeTeam { get; set;}

		[LABEL(LanguageEnum.en,"Match events")]
		[LABEL(LanguageEnum.es,"Eventos del partido")]
		[RDFProperty("https://schema.org/subEvent")]
		public  List<Event> Schema_subEvent { get; set;}

		[RDFProperty("https://schema.org/extended/tounamentId")]
		public  string Eschema_tounamentId { get; set;}

		[LABEL(LanguageEnum.en,"Refere")]
		[LABEL(LanguageEnum.es,"Arbitro")]
		[RDFProperty("https://schema.org/extended/referee")]
		public  List<string> Eschema_referee { get; set;}

		[LABEL(LanguageEnum.es,"https://schema.org/extended/identifierpartido")]
		[RDFProperty("https://schema.org/extended/identifierpartido")]
		public  string Eschema_identifierpartido { get; set;}

		[LABEL(LanguageEnum.es,"Fecha")]
		[RDFProperty("https://schema.org/date")]
		public  DateTime? Schema_date { get; set;}

		[LABEL(LanguageEnum.es,"Name partido")]
		[LABEL(LanguageEnum.en,"Nombre del Partido")]
		[RDFProperty("https://schema.org/extended/namePartido")]
		public  string Eschema_namePartido { get; set;}

		[LABEL(LanguageEnum.en,"Result")]
		[LABEL(LanguageEnum.es,"Resultado")]
		[RDFProperty("https://schema.org/extended/result")]
		public  string Eschema_result { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("eschema:tounamentId", this.Eschema_tounamentId));
			propList.Add(new ListStringOntologyProperty("eschema:referee", this.Eschema_referee));
			propList.Add(new StringOntologyProperty("eschema:identifierpartido", this.Eschema_identifierpartido));
			if (this.Schema_date.HasValue){
				propList.Add(new DateOntologyProperty("schema:date", this.Schema_date.Value));
				}
			propList.Add(new StringOntologyProperty("eschema:namePartido", this.Eschema_namePartido));
			propList.Add(new StringOntologyProperty("eschema:result", this.Eschema_result));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Schema_awayTeam!=null){
				Schema_awayTeam.GetProperties();
				Schema_awayTeam.GetEntities();
				OntologyEntity entitySchema_awayTeam = new OntologyEntity("https://schema.org/SportsTeam", "https://schema.org/SportsTeam", "schema:awayTeam", Schema_awayTeam.propList, Schema_awayTeam.entList);
				Schema_awayTeam.Entity = entitySchema_awayTeam;
				entList.Add(entitySchema_awayTeam);
			}
			if(Schema_homeTeam!=null){
				Schema_homeTeam.GetProperties();
				Schema_homeTeam.GetEntities();
				OntologyEntity entitySchema_homeTeam = new OntologyEntity("https://schema.org/SportsTeam", "https://schema.org/SportsTeam", "schema:homeTeam", Schema_homeTeam.propList, Schema_homeTeam.entList);
				Schema_homeTeam.Entity = entitySchema_homeTeam;
				entList.Add(entitySchema_homeTeam);
			}
			if(Schema_subEvent!=null){
				foreach(Event prop in Schema_subEvent){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityEvent = new OntologyEntity("https://schema.org/Event", "https://schema.org/Event", "schema:subEvent", prop.propList, prop.entList);
					entList.Add(entityEvent);
					prop.Entity = entityEvent;
				}
			}
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
			GetEntities();
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/SportsEvent>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/SportsEvent\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Schema_awayTeam != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_awayTeam.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/SportsTeam>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_awayTeam.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/SportsTeam\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_awayTeam.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{ArticleID}", "https://schema.org/awayTeam", $"<{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_awayTeam.ArticleID}>", list, " . ");
			if(this.Schema_awayTeam.Schema_athlete != null)
			{
			foreach(var item1 in this.Schema_awayTeam.Schema_athlete)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/extended/PersonLinedUp>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/extended/PersonLinedUp\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_awayTeam.ArticleID}", "https://schema.org/athlete", $"<{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.IdEschema_type != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}", "https://schema.org/extended/type",  $"<{item1.IdEschema_type}>", list, " . ");
				}
				if(item1.IdEschema_position != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}", "https://schema.org/extended/position",  $"<{item1.IdEschema_position}>", list, " . ");
				}
				if(item1.IdEschema_player != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}", "https://schema.org/extended/player",  $"<{item1.IdEschema_player}>", list, " . ");
				}
				if(item1.Eschema_bibNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}", "https://schema.org/extended/bibNumber",  $"\"{GenerarTextoSinSaltoDeLinea(item1.Eschema_bibNumber)}\"", list, " . ");
				}
			}
			}
				if(this.Schema_awayTeam.IdSchema_subOrganization != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_awayTeam.ArticleID}", "https://schema.org/subOrganization",  $"<{this.Schema_awayTeam.IdSchema_subOrganization}>", list, " . ");
				}
				if(this.Schema_awayTeam.IdsSchema_coach != null)
				{
					foreach(var item2 in this.Schema_awayTeam.IdsSchema_coach)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_awayTeam.ArticleID}", "https://schema.org/coach", $"<{item2}>", list, " . ");
					}
				}
				if(this.Schema_awayTeam.Eschema_classification != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_awayTeam.ArticleID}", "https://schema.org/extended/classification",  $"{this.Schema_awayTeam.Eschema_classification.Value.ToString()}", list, " . ");
				}
			}
			if(this.Schema_homeTeam != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_homeTeam.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/SportsTeam>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_homeTeam.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/SportsTeam\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_homeTeam.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{ArticleID}", "https://schema.org/homeTeam", $"<{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_homeTeam.ArticleID}>", list, " . ");
			if(this.Schema_homeTeam.Schema_athlete != null)
			{
			foreach(var item1 in this.Schema_homeTeam.Schema_athlete)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/extended/PersonLinedUp>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/extended/PersonLinedUp\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_homeTeam.ArticleID}", "https://schema.org/athlete", $"<{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.IdEschema_type != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}", "https://schema.org/extended/type",  $"<{item1.IdEschema_type}>", list, " . ");
				}
				if(item1.IdEschema_position != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}", "https://schema.org/extended/position",  $"<{item1.IdEschema_position}>", list, " . ");
				}
				if(item1.IdEschema_player != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}", "https://schema.org/extended/player",  $"<{item1.IdEschema_player}>", list, " . ");
				}
				if(item1.Eschema_bibNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}", "https://schema.org/extended/bibNumber",  $"\"{GenerarTextoSinSaltoDeLinea(item1.Eschema_bibNumber)}\"", list, " . ");
				}
			}
			}
				if(this.Schema_homeTeam.IdSchema_subOrganization != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_homeTeam.ArticleID}", "https://schema.org/subOrganization",  $"<{this.Schema_homeTeam.IdSchema_subOrganization}>", list, " . ");
				}
				if(this.Schema_homeTeam.IdsSchema_coach != null)
				{
					foreach(var item2 in this.Schema_homeTeam.IdsSchema_coach)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_homeTeam.ArticleID}", "https://schema.org/coach", $"<{item2}>", list, " . ");
					}
				}
				if(this.Schema_homeTeam.Eschema_classification != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_homeTeam.ArticleID}", "https://schema.org/extended/classification",  $"{this.Schema_homeTeam.Eschema_classification.Value.ToString()}", list, " . ");
				}
			}
			if(this.Schema_subEvent != null)
			{
			foreach(var item0 in this.Schema_subEvent)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/Event>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/Event\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{ArticleID}", "https://schema.org/subEvent", $"<{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdSchema_actor != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item0.ArticleID}", "https://schema.org/actor",  $"<{item0.IdSchema_actor}>", list, " . ");
				}
				if(item0.IdSchema_about != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item0.ArticleID}", "https://schema.org/about",  $"<{item0.IdSchema_about}>", list, " . ");
				}
				if(item0.Eschema_identifierevento != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item0.ArticleID}", "https://schema.org/extended/identifierevento",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Eschema_identifierevento)}\"", list, " . ");
				}
				if(item0.Eschema_Minute != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item0.ArticleID}", "https://schema.org/extended/Minute",  $"{item0.Eschema_Minute.Value.ToString()}", list, " . ");
				}
			}
			}
				if(this.Eschema_tounamentId != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{ArticleID}", "https://schema.org/extended/tounamentId",  $"\"{GenerarTextoSinSaltoDeLinea(this.Eschema_tounamentId)}\"", list, " . ");
				}
				if(this.Eschema_referee != null)
				{
					foreach(var item2 in this.Eschema_referee)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{ArticleID}", "https://schema.org/extended/referee", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Eschema_identifierpartido != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{ArticleID}", "https://schema.org/extended/identifierpartido",  $"\"{GenerarTextoSinSaltoDeLinea(this.Eschema_identifierpartido)}\"", list, " . ");
				}
				if(this.Schema_date != null && this.Schema_date != DateTime.MinValue)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{ArticleID}", "https://schema.org/date",  $"\"{this.Schema_date.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Eschema_namePartido != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{ArticleID}", "https://schema.org/extended/namePartido",  $"\"{GenerarTextoSinSaltoDeLinea(this.Eschema_namePartido)}\"", list, " . ");
				}
				if(this.Eschema_result != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{ArticleID}", "https://schema.org/extended/result",  $"\"{GenerarTextoSinSaltoDeLinea(this.Eschema_result)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"partidopfihs\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"https://schema.org/SportsEvent\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Eschema_namePartido)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Eschema_namePartido)}\"", list, " . ");
			string search = string.Empty;
			if(this.Schema_awayTeam != null)
			{
				AgregarTripleALista($"http://gnossAuxiliar/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasEntidadAuxiliar", $"<{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_awayTeam.ArticleID}>", list, " . ");
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/awayTeam", $"<{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_awayTeam.ArticleID}>", list, " . ");
			if(this.Schema_awayTeam.Schema_athlete != null)
			{
			foreach(var item1 in this.Schema_awayTeam.Schema_athlete)
			{
				AgregarTripleALista($"http://gnossAuxiliar/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasEntidadAuxiliar", $"<{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_awayTeam.ArticleID}", "https://schema.org/athlete", $"<{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.IdEschema_type != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item1.IdEschema_type;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}", "https://schema.org/extended/type",  $"<{itemRegex}>", list, " . ");
				}
				if(item1.IdEschema_position != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item1.IdEschema_position;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}", "https://schema.org/extended/position",  $"<{itemRegex}>", list, " . ");
				}
				if(item1.IdEschema_player != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item1.IdEschema_player;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}", "https://schema.org/extended/player",  $"<{itemRegex}>", list, " . ");
				}
				if(item1.Eschema_bibNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}", "https://schema.org/extended/bibNumber",  $"\"{GenerarTextoSinSaltoDeLinea(item1.Eschema_bibNumber)}\"", list, " . ");
				}
			}
			}
				if(this.Schema_awayTeam.IdSchema_subOrganization != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.Schema_awayTeam.IdSchema_subOrganization;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_awayTeam.ArticleID}", "https://schema.org/subOrganization",  $"<{itemRegex}>", list, " . ");
				}
				if(this.Schema_awayTeam.IdsSchema_coach != null)
				{
					foreach(var item2 in this.Schema_awayTeam.IdsSchema_coach)
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
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_awayTeam.ArticleID}", "https://schema.org/coach", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.Schema_awayTeam.Eschema_classification != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_awayTeam.ArticleID}", "https://schema.org/extended/classification",  $"{this.Schema_awayTeam.Eschema_classification.Value.ToString()}", list, " . ");
				}
			}
			if(this.Schema_homeTeam != null)
			{
				AgregarTripleALista($"http://gnossAuxiliar/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasEntidadAuxiliar", $"<{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_homeTeam.ArticleID}>", list, " . ");
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/homeTeam", $"<{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_homeTeam.ArticleID}>", list, " . ");
			if(this.Schema_homeTeam.Schema_athlete != null)
			{
			foreach(var item1 in this.Schema_homeTeam.Schema_athlete)
			{
				AgregarTripleALista($"http://gnossAuxiliar/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasEntidadAuxiliar", $"<{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_homeTeam.ArticleID}", "https://schema.org/athlete", $"<{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.IdEschema_type != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item1.IdEschema_type;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}", "https://schema.org/extended/type",  $"<{itemRegex}>", list, " . ");
				}
				if(item1.IdEschema_position != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item1.IdEschema_position;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}", "https://schema.org/extended/position",  $"<{itemRegex}>", list, " . ");
				}
				if(item1.IdEschema_player != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item1.IdEschema_player;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}", "https://schema.org/extended/player",  $"<{itemRegex}>", list, " . ");
				}
				if(item1.Eschema_bibNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item1.ArticleID}", "https://schema.org/extended/bibNumber",  $"\"{GenerarTextoSinSaltoDeLinea(item1.Eschema_bibNumber)}\"", list, " . ");
				}
			}
			}
				if(this.Schema_homeTeam.IdSchema_subOrganization != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.Schema_homeTeam.IdSchema_subOrganization;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_homeTeam.ArticleID}", "https://schema.org/subOrganization",  $"<{itemRegex}>", list, " . ");
				}
				if(this.Schema_homeTeam.IdsSchema_coach != null)
				{
					foreach(var item2 in this.Schema_homeTeam.IdsSchema_coach)
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
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_homeTeam.ArticleID}", "https://schema.org/coach", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.Schema_homeTeam.Eschema_classification != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{this.Schema_homeTeam.ArticleID}", "https://schema.org/extended/classification",  $"{this.Schema_homeTeam.Eschema_classification.Value.ToString()}", list, " . ");
				}
			}
			if(this.Schema_subEvent != null)
			{
			foreach(var item0 in this.Schema_subEvent)
			{
				AgregarTripleALista($"http://gnossAuxiliar/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasEntidadAuxiliar", $"<{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/subEvent", $"<{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdSchema_actor != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdSchema_actor;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item0.ArticleID}", "https://schema.org/actor",  $"<{itemRegex}>", list, " . ");
				}
				if(item0.IdSchema_about != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdSchema_about;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item0.ArticleID}", "https://schema.org/about",  $"<{itemRegex}>", list, " . ");
				}
				if(item0.Eschema_identifierevento != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item0.ArticleID}", "https://schema.org/extended/identifierevento",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Eschema_identifierevento)}\"", list, " . ");
				}
				if(item0.Eschema_Minute != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item0.ArticleID}", "https://schema.org/extended/Minute",  $"{item0.Eschema_Minute.Value.ToString()}", list, " . ");
				}
			}
			}
				if(this.Eschema_tounamentId != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/extended/tounamentId",  $"\"{GenerarTextoSinSaltoDeLinea(this.Eschema_tounamentId)}\"", list, " . ");
				}
				if(this.Eschema_referee != null)
				{
					foreach(var item2 in this.Eschema_referee)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/extended/referee", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Eschema_identifierpartido != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/extended/identifierpartido",  $"\"{GenerarTextoSinSaltoDeLinea(this.Eschema_identifierpartido)}\"", list, " . ");
				}
				if(this.Schema_date != null && this.Schema_date != DateTime.MinValue)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/date",  $"{this.Schema_date.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Eschema_namePartido != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/extended/namePartido",  $"\"{GenerarTextoSinSaltoDeLinea(this.Eschema_namePartido)}\"", list, " . ");
				}
				if(this.Eschema_result != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/extended/result",  $"\"{GenerarTextoSinSaltoDeLinea(this.Eschema_result)}\"", list, " . ");
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
			string titulo = $"{this.Eschema_namePartido.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string descripcion = $"{this.Eschema_namePartido.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '{descripcion}', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/PartidopfihsOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
			resource.Title = this.Eschema_namePartido;
		}

		internal void AddResourceDescription(ComplexOntologyResource resource)
		{
			resource.Description = this.Eschema_namePartido;
		}




	}
}
