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
using SportsClub = ClubpfihsOntology.SportsClub;
using Organization = OrganizacionpfihsOntology.Organization;

namespace TorneopfihsOntology
{
	[ExcludeFromCodeCoverage]
	public class SportsTournament : GnossOCBase
	{
		public SportsTournament() : base() { } 

		public SportsTournament(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			GNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
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
			this.Schema_description = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/description"));
		}

		public SportsTournament(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			mGNOSSID = pSemCmsModel.Entity.Uri;
			mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
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
			this.Schema_description = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/description"));
		}

		public virtual string RdfType { get { return "https://schema.org/extended/SportsTournament"; } }
		public virtual string RdfsLabel { get { return "https://schema.org/extended/SportsTournament"; } }
		[LABEL(LanguageEnum.es,"Partidos")]
		[LABEL(LanguageEnum.en,"Matches")]
		[RDFProperty("https://schema.org/extended/subEvent")]
		public  List<SportsEvent> Eschema_subEvent { get; set;}

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

		[LABEL(LanguageEnum.es,"")]
		[RDFProperty("https://schema.org/description")]
		public  string Schema_description { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("eschema:winner", this.IdEschema_winner));
			propList.Add(new ListStringOntologyProperty("schema:organizer", this.IdsSchema_organizer));
			propList.Add(new StringOntologyProperty("schema:name", this.Schema_name));
			propList.Add(new StringOntologyProperty("schema:identifier", this.Schema_identifier));
			propList.Add(new StringOntologyProperty("schema:description", this.Schema_description));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Eschema_subEvent!=null){
				foreach(SportsEvent prop in Eschema_subEvent){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entitySportsEvent = new OntologyEntity("https://schema.org/SportsEvent", "https://schema.org/SportsEvent", "eschema:subEvent", prop.propList, prop.entList);
					entList.Add(entitySportsEvent);
					prop.Entity = entitySportsEvent;
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTournament_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/extended/SportsTournament>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTournament_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/extended/SportsTournament\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/SportsTournament_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Eschema_subEvent != null)
			{
			foreach(var item0 in this.Eschema_subEvent)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/SportsEvent>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/SportsEvent\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTournament_{ResourceID}_{ArticleID}", "https://schema.org/extended/subEvent", $"<{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{item0.ArticleID}>", list, " . ");
			if(item0.Schema_awayTeam != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_awayTeam.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/SportsTeam>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_awayTeam.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/SportsTeam\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_awayTeam.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{item0.ArticleID}", "https://schema.org/awayTeam", $"<{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_awayTeam.ArticleID}>", list, " . ");
			if(item0.Schema_awayTeam.Schema_athlete != null)
			{
			foreach(var item2 in item0.Schema_awayTeam.Schema_athlete)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item2.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/extended/PersonLinedUp>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item2.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/extended/PersonLinedUp\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item2.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_awayTeam.ArticleID}", "https://schema.org/athlete", $"<{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item2.ArticleID}>", list, " . ");
				if(item2.IdEschema_type != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item2.ArticleID}", "https://schema.org/extended/type",  $"<{item2.IdEschema_type}>", list, " . ");
				}
				if(item2.IdEschema_player != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item2.ArticleID}", "https://schema.org/extended/player",  $"<{item2.IdEschema_player}>", list, " . ");
				}
				if(item2.IdEschema_position != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item2.ArticleID}", "https://schema.org/extended/position",  $"<{item2.IdEschema_position}>", list, " . ");
				}
				if(item2.Eschema_bibNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item2.ArticleID}", "https://schema.org/extended/bibNumber",  $"{item2.Eschema_bibNumber.Value.ToString()}", list, " . ");
				}
			}
			}
				if(item0.Schema_awayTeam.IdsSchema_coach != null)
				{
					foreach(var item2 in item0.Schema_awayTeam.IdsSchema_coach)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_awayTeam.ArticleID}", "https://schema.org/coach", $"<{item2}>", list, " . ");
					}
				}
				if(item0.Schema_awayTeam.IdSchema_subOrganization != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_awayTeam.ArticleID}", "https://schema.org/subOrganization",  $"<{item0.Schema_awayTeam.IdSchema_subOrganization}>", list, " . ");
				}
				if(item0.Schema_awayTeam.Eschema_classification != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_awayTeam.ArticleID}",  "https://schema.org/extended/classification", $"{item0.Schema_awayTeam.Eschema_classification.Value.ToString()}", list, " . ");
				}
			}
			if(item0.Schema_homeTeam != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_homeTeam.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/SportsTeam>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_homeTeam.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/SportsTeam\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_homeTeam.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{item0.ArticleID}", "https://schema.org/homeTeam", $"<{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_homeTeam.ArticleID}>", list, " . ");
			if(item0.Schema_homeTeam.Schema_athlete != null)
			{
			foreach(var item3 in item0.Schema_homeTeam.Schema_athlete)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/extended/PersonLinedUp>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/extended/PersonLinedUp\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_homeTeam.ArticleID}", "https://schema.org/athlete", $"<{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.ArticleID}>", list, " . ");
				if(item3.IdEschema_type != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.ArticleID}", "https://schema.org/extended/type",  $"<{item3.IdEschema_type}>", list, " . ");
				}
				if(item3.IdEschema_player != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.ArticleID}", "https://schema.org/extended/player",  $"<{item3.IdEschema_player}>", list, " . ");
				}
				if(item3.IdEschema_position != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.ArticleID}", "https://schema.org/extended/position",  $"<{item3.IdEschema_position}>", list, " . ");
				}
				if(item3.Eschema_bibNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.ArticleID}", "https://schema.org/extended/bibNumber",  $"{item3.Eschema_bibNumber.Value.ToString()}", list, " . ");
				}
			}
			}
				if(item0.Schema_homeTeam.IdsSchema_coach != null)
				{
					foreach(var item2 in item0.Schema_homeTeam.IdsSchema_coach)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_homeTeam.ArticleID}", "https://schema.org/coach", $"<{item2}>", list, " . ");
					}
				}
				if(item0.Schema_homeTeam.IdSchema_subOrganization != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_homeTeam.ArticleID}", "https://schema.org/subOrganization",  $"<{item0.Schema_homeTeam.IdSchema_subOrganization}>", list, " . ");
				}
				if(item0.Schema_homeTeam.Eschema_classification != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_homeTeam.ArticleID}",  "https://schema.org/extended/classification", $"{item0.Schema_homeTeam.Eschema_classification.Value.ToString()}", list, " . ");
				}
			}
			if(item0.Schema_subEvent != null)
			{
			foreach(var item3 in item0.Schema_subEvent)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item3.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/Event>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item3.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/Event\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item3.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{item0.ArticleID}", "https://schema.org/subEvent", $"<{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item3.ArticleID}>", list, " . ");
			if(item3.Schema_actor != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.Schema_actor.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/extended/PersonLinedUp>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.Schema_actor.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/extended/PersonLinedUp\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.Schema_actor.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item3.ArticleID}", "https://schema.org/actor", $"<{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.Schema_actor.ArticleID}>", list, " . ");
				if(item3.Schema_actor.IdEschema_type != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.Schema_actor.ArticleID}", "https://schema.org/extended/type",  $"<{item3.Schema_actor.IdEschema_type}>", list, " . ");
				}
				if(item3.Schema_actor.IdEschema_player != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.Schema_actor.ArticleID}", "https://schema.org/extended/player",  $"<{item3.Schema_actor.IdEschema_player}>", list, " . ");
				}
				if(item3.Schema_actor.IdEschema_position != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.Schema_actor.ArticleID}", "https://schema.org/extended/position",  $"<{item3.Schema_actor.IdEschema_position}>", list, " . ");
				}
				if(item3.Schema_actor.Eschema_bibNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.Schema_actor.ArticleID}", "https://schema.org/extended/bibNumber",  $"{item3.Schema_actor.Eschema_bibNumber.Value.ToString()}", list, " . ");
				}
			}
				if(item3.IdSchema_about != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item3.ArticleID}", "https://schema.org/about",  $"<{item3.IdSchema_about}>", list, " . ");
				}
				if(item3.Eschema_identifier_evento != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item3.ArticleID}", "https://schema.org/extended/identifier_evento",  $"\"{GenerarTextoSinSaltoDeLinea(item3.Eschema_identifier_evento)}\"", list, " . ");
				}
				if(item3.Eschema_Minute != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item3.ArticleID}", "https://schema.org/extended/Minute",  $"{item3.Eschema_Minute.Value.ToString()}", list, " . ");
				}
			}
			}
				if(item0.Eschema_identifier_partido != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{item0.ArticleID}", "https://schema.org/extended/identifier_partido",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Eschema_identifier_partido)}\"", list, " . ");
				}
				if(item0.Eschema_result != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{item0.ArticleID}", "https://schema.org/extended/result",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Eschema_result)}\"", list, " . ");
				}
			}
			}
				if(this.IdEschema_winner != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTournament_{ResourceID}_{ArticleID}",  "https://schema.org/extended/winner", $"<{this.IdEschema_winner}>", list, " . ");
				}
				if(this.IdsSchema_organizer != null)
				{
					foreach(var item2 in this.IdsSchema_organizer)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTournament_{ResourceID}_{ArticleID}", "https://schema.org/organizer", $"<{item2}>", list, " . ");
					}
				}
				if(this.Schema_name != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTournament_{ResourceID}_{ArticleID}", "https://schema.org/name",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
				}
				if(this.Schema_identifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTournament_{ResourceID}_{ArticleID}", "https://schema.org/identifier",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_identifier)}\"", list, " . ");
				}
				if(this.Schema_description != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTournament_{ResourceID}_{ArticleID}",  "https://schema.org/description", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_description)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"torneopfihs\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"https://schema.org/extended/SportsTournament\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
			string search = string.Empty;
			if(this.Eschema_subEvent != null)
			{
			foreach(var item0 in this.Eschema_subEvent)
			{
				AgregarTripleALista($"http://gnossAuxiliar/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasEntidadAuxiliar", $"<{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/extended/subEvent", $"<{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{item0.ArticleID}>", list, " . ");
			if(item0.Schema_awayTeam != null)
			{
				AgregarTripleALista($"http://gnossAuxiliar/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasEntidadAuxiliar", $"<{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_awayTeam.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{item0.ArticleID}", "https://schema.org/awayTeam", $"<{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_awayTeam.ArticleID}>", list, " . ");
			if(item0.Schema_awayTeam.Schema_athlete != null)
			{
			foreach(var item2 in item0.Schema_awayTeam.Schema_athlete)
			{
				AgregarTripleALista($"http://gnossAuxiliar/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasEntidadAuxiliar", $"<{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item2.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_awayTeam.ArticleID}", "https://schema.org/athlete", $"<{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item2.ArticleID}>", list, " . ");
				if(item2.IdEschema_type != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2.IdEschema_type;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item2.ArticleID}", "https://schema.org/extended/type",  $"<{itemRegex}>", list, " . ");
				}
				if(item2.IdEschema_player != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2.IdEschema_player;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item2.ArticleID}", "https://schema.org/extended/player",  $"<{itemRegex}>", list, " . ");
				}
				if(item2.IdEschema_position != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2.IdEschema_position;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item2.ArticleID}", "https://schema.org/extended/position",  $"<{itemRegex}>", list, " . ");
				}
				if(item2.Eschema_bibNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item2.ArticleID}", "https://schema.org/extended/bibNumber",  $"{item2.Eschema_bibNumber.Value.ToString()}", list, " . ");
				}
			}
			}
				if(item0.Schema_awayTeam.IdsSchema_coach != null)
				{
					foreach(var item2 in item0.Schema_awayTeam.IdsSchema_coach)
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
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_awayTeam.ArticleID}", "https://schema.org/coach", $"<{itemRegex}>", list, " . ");
					}
				}
				if(item0.Schema_awayTeam.IdSchema_subOrganization != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.Schema_awayTeam.IdSchema_subOrganization;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_awayTeam.ArticleID}", "https://schema.org/subOrganization",  $"<{itemRegex}>", list, " . ");
				}
				if(item0.Schema_awayTeam.Eschema_classification != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_awayTeam.ArticleID}",  "https://schema.org/extended/classification", $"{item0.Schema_awayTeam.Eschema_classification.Value.ToString()}", list, " . ");
				}
			}
			if(item0.Schema_homeTeam != null)
			{
				AgregarTripleALista($"http://gnossAuxiliar/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasEntidadAuxiliar", $"<{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_homeTeam.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{item0.ArticleID}", "https://schema.org/homeTeam", $"<{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_homeTeam.ArticleID}>", list, " . ");
			if(item0.Schema_homeTeam.Schema_athlete != null)
			{
			foreach(var item3 in item0.Schema_homeTeam.Schema_athlete)
			{
				AgregarTripleALista($"http://gnossAuxiliar/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasEntidadAuxiliar", $"<{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_homeTeam.ArticleID}", "https://schema.org/athlete", $"<{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.ArticleID}>", list, " . ");
				if(item3.IdEschema_type != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item3.IdEschema_type;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.ArticleID}", "https://schema.org/extended/type",  $"<{itemRegex}>", list, " . ");
				}
				if(item3.IdEschema_player != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item3.IdEschema_player;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.ArticleID}", "https://schema.org/extended/player",  $"<{itemRegex}>", list, " . ");
				}
				if(item3.IdEschema_position != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item3.IdEschema_position;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.ArticleID}", "https://schema.org/extended/position",  $"<{itemRegex}>", list, " . ");
				}
				if(item3.Eschema_bibNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.ArticleID}", "https://schema.org/extended/bibNumber",  $"{item3.Eschema_bibNumber.Value.ToString()}", list, " . ");
				}
			}
			}
				if(item0.Schema_homeTeam.IdsSchema_coach != null)
				{
					foreach(var item2 in item0.Schema_homeTeam.IdsSchema_coach)
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
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_homeTeam.ArticleID}", "https://schema.org/coach", $"<{itemRegex}>", list, " . ");
					}
				}
				if(item0.Schema_homeTeam.IdSchema_subOrganization != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.Schema_homeTeam.IdSchema_subOrganization;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_homeTeam.ArticleID}", "https://schema.org/subOrganization",  $"<{itemRegex}>", list, " . ");
				}
				if(item0.Schema_homeTeam.Eschema_classification != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.Schema_homeTeam.ArticleID}",  "https://schema.org/extended/classification", $"{item0.Schema_homeTeam.Eschema_classification.Value.ToString()}", list, " . ");
				}
			}
			if(item0.Schema_subEvent != null)
			{
			foreach(var item3 in item0.Schema_subEvent)
			{
				AgregarTripleALista($"http://gnossAuxiliar/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasEntidadAuxiliar", $"<{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item3.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{item0.ArticleID}", "https://schema.org/subEvent", $"<{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item3.ArticleID}>", list, " . ");
			if(item3.Schema_actor != null)
			{
				AgregarTripleALista($"http://gnossAuxiliar/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasEntidadAuxiliar", $"<{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.Schema_actor.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item3.ArticleID}", "https://schema.org/actor", $"<{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.Schema_actor.ArticleID}>", list, " . ");
				if(item3.Schema_actor.IdEschema_type != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item3.Schema_actor.IdEschema_type;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.Schema_actor.ArticleID}", "https://schema.org/extended/type",  $"<{itemRegex}>", list, " . ");
				}
				if(item3.Schema_actor.IdEschema_player != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item3.Schema_actor.IdEschema_player;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.Schema_actor.ArticleID}", "https://schema.org/extended/player",  $"<{itemRegex}>", list, " . ");
				}
				if(item3.Schema_actor.IdEschema_position != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item3.Schema_actor.IdEschema_position;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.Schema_actor.ArticleID}", "https://schema.org/extended/position",  $"<{itemRegex}>", list, " . ");
				}
				if(item3.Schema_actor.Eschema_bibNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonLinedUp_{ResourceID}_{item3.Schema_actor.ArticleID}", "https://schema.org/extended/bibNumber",  $"{item3.Schema_actor.Eschema_bibNumber.Value.ToString()}", list, " . ");
				}
			}
				if(item3.IdSchema_about != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item3.IdSchema_about;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item3.ArticleID}", "https://schema.org/about",  $"<{itemRegex}>", list, " . ");
				}
				if(item3.Eschema_identifier_evento != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item3.ArticleID}", "https://schema.org/extended/identifier_evento",  $"\"{GenerarTextoSinSaltoDeLinea(item3.Eschema_identifier_evento)}\"", list, " . ");
				}
				if(item3.Eschema_Minute != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Event_{ResourceID}_{item3.ArticleID}", "https://schema.org/extended/Minute",  $"{item3.Eschema_Minute.Value.ToString()}", list, " . ");
				}
			}
			}
				if(item0.Eschema_identifier_partido != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{item0.ArticleID}", "https://schema.org/extended/identifier_partido",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Eschema_identifier_partido)}\"", list, " . ");
				}
				if(item0.Eschema_result != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsEvent_{ResourceID}_{item0.ArticleID}", "https://schema.org/extended/result",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Eschema_result)}\"", list, " . ");
				}
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
				if(this.Schema_description != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://schema.org/description", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_description)}\"", list, " . ");
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
			return $"{resourceAPI.GraphsUrl}items/TorneopfihsOntology_{ResourceID}_{ArticleID}";
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
