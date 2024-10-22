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

namespace ClubpfihsOntology
{
	[ExcludeFromCodeCoverage]
	public class SportsClub : GnossOCBase
	{
		public SportsClub() : base() { } 

		public SportsClub(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			GNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			Schema_parentOrganization = new List<SportsTeam>();
			SemanticPropertyModel propSchema_parentOrganization = pSemCmsModel.GetPropertyByPath("https://schema.org/parentOrganization");
			if(propSchema_parentOrganization != null && propSchema_parentOrganization.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_parentOrganization.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						SportsTeam schema_parentOrganization = new SportsTeam(propValue.RelatedEntity,idiomaUsuario);
						Schema_parentOrganization.Add(schema_parentOrganization);
					}
				}
			}
			Schema_location = new List<PostalAddress>();
			SemanticPropertyModel propSchema_location = pSemCmsModel.GetPropertyByPath("https://schema.org/location");
			if(propSchema_location != null && propSchema_location.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_location.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						PostalAddress schema_location = new PostalAddress(propValue.RelatedEntity,idiomaUsuario);
						Schema_location.Add(schema_location);
					}
				}
			}
			this.Eschema_league = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/extended/league"));
			this.Schema_identifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/identifier"));
			this.Schema_logo = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/logo"));
			SemanticPropertyModel propSchema_award = pSemCmsModel.GetPropertyByPath("https://schema.org/award");
			this.Schema_award = new List<string>();
			if (propSchema_award != null && propSchema_award.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_award.PropertyValues)
				{
					this.Schema_award.Add(propValue.Value);
				}
			}
			SemanticPropertyModel propSchema_alternateName = pSemCmsModel.GetPropertyByPath("https://schema.org/alternateName");
			this.Schema_alternateName = new List<string>();
			if (propSchema_alternateName != null && propSchema_alternateName.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_alternateName.PropertyValues)
				{
					this.Schema_alternateName.Add(propValue.Value);
				}
			}
			this.Schema_description = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/description"));
			this.Schema_foundingDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/foundingDate"));
			this.Schema_name = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/name"));
		}

		public SportsClub(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			mGNOSSID = pSemCmsModel.Entity.Uri;
			mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			Schema_parentOrganization = new List<SportsTeam>();
			SemanticPropertyModel propSchema_parentOrganization = pSemCmsModel.GetPropertyByPath("https://schema.org/parentOrganization");
			if(propSchema_parentOrganization != null && propSchema_parentOrganization.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_parentOrganization.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						SportsTeam schema_parentOrganization = new SportsTeam(propValue.RelatedEntity,idiomaUsuario);
						Schema_parentOrganization.Add(schema_parentOrganization);
					}
				}
			}
			Schema_location = new List<PostalAddress>();
			SemanticPropertyModel propSchema_location = pSemCmsModel.GetPropertyByPath("https://schema.org/location");
			if(propSchema_location != null && propSchema_location.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_location.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						PostalAddress schema_location = new PostalAddress(propValue.RelatedEntity,idiomaUsuario);
						Schema_location.Add(schema_location);
					}
				}
			}
			this.Eschema_league = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/extended/league"));
			this.Schema_identifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/identifier"));
			this.Schema_logo = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/logo"));
			SemanticPropertyModel propSchema_award = pSemCmsModel.GetPropertyByPath("https://schema.org/award");
			this.Schema_award = new List<string>();
			if (propSchema_award != null && propSchema_award.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_award.PropertyValues)
				{
					this.Schema_award.Add(propValue.Value);
				}
			}
			SemanticPropertyModel propSchema_alternateName = pSemCmsModel.GetPropertyByPath("https://schema.org/alternateName");
			this.Schema_alternateName = new List<string>();
			if (propSchema_alternateName != null && propSchema_alternateName.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_alternateName.PropertyValues)
				{
					this.Schema_alternateName.Add(propValue.Value);
				}
			}
			this.Schema_description = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/description"));
			this.Schema_foundingDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/foundingDate"));
			this.Schema_name = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/name"));
		}

		public virtual string RdfType { get { return "https://schema.org/SportsClub"; } }
		public virtual string RdfsLabel { get { return "https://schema.org/SportsClub"; } }
		[LABEL(LanguageEnum.es,"Historial de Equipos")]
		[LABEL(LanguageEnum.en,"Historic Teams")]
		[RDFProperty("https://schema.org/parentOrganization")]
		[MinLength(1)]
		public  List<SportsTeam> Schema_parentOrganization { get; set;}

		[LABEL(LanguageEnum.es,"Ubicación")]
		[LABEL(LanguageEnum.en,"Location")]
		[RDFProperty("https://schema.org/location")]
		public  List<PostalAddress> Schema_location { get; set;}

		[LABEL(LanguageEnum.es,"League")]
		[LABEL(LanguageEnum.en,"League")]
		[RDFProperty("https://schema.org/extended/league")]
		public  string Eschema_league { get; set;}

		[LABEL(LanguageEnum.es,"Id")]
		[RDFProperty("https://schema.org/identifier")]
		public  string Schema_identifier { get; set;}

		[LABEL(LanguageEnum.es,"Logo")]
		[RDFProperty("https://schema.org/logo")]
		public  string Schema_logo { get; set;}

		[LABEL(LanguageEnum.es,"Palmarés")]
		[LABEL(LanguageEnum.en,"Awards")]
		[RDFProperty("https://schema.org/award")]
		public  List<string> Schema_award { get; set;}

		[LABEL(LanguageEnum.es,"Alias")]
		[LABEL(LanguageEnum.en,"Also Known As (AKA)")]
		[RDFProperty("https://schema.org/alternateName")]
		public  List<string> Schema_alternateName { get; set;}

		[LABEL(LanguageEnum.es,"Historia del Club")]
		[LABEL(LanguageEnum.en,"Club's History")]
		[RDFProperty("https://schema.org/description")]
		public  string Schema_description { get; set;}

		[LABEL(LanguageEnum.es,"Fecha Fundación del Club")]
		[LABEL(LanguageEnum.en,"Club's Founding Date")]
		[RDFProperty("https://schema.org/foundingDate")]
		public  DateTime? Schema_foundingDate { get; set;}

		[LABEL(LanguageEnum.en,"Name")]
		[LABEL(LanguageEnum.es,"Nombre")]
		[RDFProperty("https://schema.org/name")]
		public  string Schema_name { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("eschema:league", this.Eschema_league));
			propList.Add(new StringOntologyProperty("schema:identifier", this.Schema_identifier));
			propList.Add(new StringOntologyProperty("schema:logo", this.Schema_logo));
			propList.Add(new ListStringOntologyProperty("schema:award", this.Schema_award));
			propList.Add(new ListStringOntologyProperty("schema:alternateName", this.Schema_alternateName));
			propList.Add(new StringOntologyProperty("schema:description", this.Schema_description));
			if (this.Schema_foundingDate.HasValue){
				propList.Add(new DateOntologyProperty("schema:foundingDate", this.Schema_foundingDate.Value));
				}
			propList.Add(new StringOntologyProperty("schema:name", this.Schema_name));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Schema_parentOrganization!=null){
				foreach(SportsTeam prop in Schema_parentOrganization){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entitySportsTeam = new OntologyEntity("https://schema.org/SportsTeam", "https://schema.org/SportsTeam", "schema:parentOrganization", prop.propList, prop.entList);
					entList.Add(entitySportsTeam);
					prop.Entity = entitySportsTeam;
				}
			}
			if(Schema_location!=null){
				foreach(PostalAddress prop in Schema_location){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityPostalAddress = new OntologyEntity("https://schema.org/PostalAddress", "https://schema.org/PostalAddress", "schema:location", prop.propList, prop.entList);
					entList.Add(entityPostalAddress);
					prop.Entity = entityPostalAddress;
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsClub_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/SportsClub>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsClub_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/SportsClub\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/SportsClub_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Schema_parentOrganization != null)
			{
			foreach(var item0 in this.Schema_parentOrganization)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/SportsTeam>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/SportsTeam\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsClub_{ResourceID}_{ArticleID}", "https://schema.org/parentOrganization", $"<{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdEschema_season != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.ArticleID}", "https://schema.org/extended/season",  $"<{item0.IdEschema_season}>", list, " . ");
				}
				if(item0.IdsSchema_coach != null)
				{
					foreach(var item2 in item0.IdsSchema_coach)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.ArticleID}", "https://schema.org/coach", $"<{item2}>", list, " . ");
					}
				}
				if(item0.IdsSchema_athlete != null)
				{
					foreach(var item2 in item0.IdsSchema_athlete)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.ArticleID}", "https://schema.org/athlete", $"<{item2}>", list, " . ");
					}
				}
				if(item0.Eschema_identifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.ArticleID}", "https://schema.org/extended/identifier",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Eschema_identifier)}\"", list, " . ");
				}
			}
			}
			if(this.Schema_location != null)
			{
			foreach(var item0 in this.Schema_location)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/PostalAddress>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/PostalAddress\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsClub_{ResourceID}_{ArticleID}", "https://schema.org/location", $"<{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Schema_streetAddress != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{item0.ArticleID}", "https://schema.org/streetAddress",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Schema_streetAddress)}\"", list, " . ");
				}
				if(item0.Schema_PostalCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{item0.ArticleID}",  "https://schema.org/PostalCode", $"\"{GenerarTextoSinSaltoDeLinea(item0.Schema_PostalCode)}\"", list, " . ");
				}
				if(item0.Schema_postOfficeBoxNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{item0.ArticleID}",  "https://schema.org/postOfficeBoxNumber", $"\"{GenerarTextoSinSaltoDeLinea(item0.Schema_postOfficeBoxNumber)}\"", list, " . ");
				}
				if(item0.Schema_addressLocality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{item0.ArticleID}", "https://schema.org/addressLocality",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Schema_addressLocality)}\"", list, " . ");
				}
				if(item0.Schema_addressCountry != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{item0.ArticleID}", "https://schema.org/addressCountry",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Schema_addressCountry)}\"", list, " . ");
				}
			}
			}
				if(this.Eschema_league != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsClub_{ResourceID}_{ArticleID}",  "https://schema.org/extended/league", $"\"{GenerarTextoSinSaltoDeLinea(this.Eschema_league)}\"", list, " . ");
				}
				if(this.Schema_identifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsClub_{ResourceID}_{ArticleID}", "https://schema.org/identifier",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_identifier)}\"", list, " . ");
				}
				if(this.Schema_logo != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsClub_{ResourceID}_{ArticleID}", "https://schema.org/logo",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_logo)}\"", list, " . ");
				}
				if(this.Schema_award != null)
				{
					foreach(var item2 in this.Schema_award)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsClub_{ResourceID}_{ArticleID}", "https://schema.org/award", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Schema_alternateName != null)
				{
					foreach(var item2 in this.Schema_alternateName)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsClub_{ResourceID}_{ArticleID}", "https://schema.org/alternateName", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Schema_description != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsClub_{ResourceID}_{ArticleID}",  "https://schema.org/description", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_description)}\"", list, " . ");
				}
				if(this.Schema_foundingDate != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsClub_{ResourceID}_{ArticleID}",  "https://schema.org/foundingDate", $"\"{this.Schema_foundingDate.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Schema_name != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsClub_{ResourceID}_{ArticleID}", "https://schema.org/name",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"clubpfihs\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"https://schema.org/SportsClub\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
			string search = string.Empty;
			if(this.Schema_parentOrganization != null)
			{
			foreach(var item0 in this.Schema_parentOrganization)
			{
				AgregarTripleALista($"http://gnossAuxiliar/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasEntidadAuxiliar", $"<{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/parentOrganization", $"<{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdEschema_season != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdEschema_season;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.ArticleID}", "https://schema.org/extended/season",  $"<{itemRegex}>", list, " . ");
				}
				if(item0.IdsSchema_coach != null)
				{
					foreach(var item2 in item0.IdsSchema_coach)
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
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.ArticleID}", "https://schema.org/coach", $"<{itemRegex}>", list, " . ");
					}
				}
				if(item0.IdsSchema_athlete != null)
				{
					foreach(var item2 in item0.IdsSchema_athlete)
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
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.ArticleID}", "https://schema.org/athlete", $"<{itemRegex}>", list, " . ");
					}
				}
				if(item0.Eschema_identifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/SportsTeam_{ResourceID}_{item0.ArticleID}", "https://schema.org/extended/identifier",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Eschema_identifier)}\"", list, " . ");
				}
			}
			}
			if(this.Schema_location != null)
			{
			foreach(var item0 in this.Schema_location)
			{
				AgregarTripleALista($"http://gnossAuxiliar/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasEntidadAuxiliar", $"<{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/location", $"<{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Schema_streetAddress != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{item0.ArticleID}", "https://schema.org/streetAddress",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Schema_streetAddress)}\"", list, " . ");
				}
				if(item0.Schema_PostalCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{item0.ArticleID}",  "https://schema.org/PostalCode", $"\"{GenerarTextoSinSaltoDeLinea(item0.Schema_PostalCode)}\"", list, " . ");
				}
				if(item0.Schema_postOfficeBoxNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{item0.ArticleID}",  "https://schema.org/postOfficeBoxNumber", $"\"{GenerarTextoSinSaltoDeLinea(item0.Schema_postOfficeBoxNumber)}\"", list, " . ");
				}
				if(item0.Schema_addressLocality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{item0.ArticleID}", "https://schema.org/addressLocality",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Schema_addressLocality)}\"", list, " . ");
				}
				if(item0.Schema_addressCountry != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{item0.ArticleID}", "https://schema.org/addressCountry",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Schema_addressCountry)}\"", list, " . ");
				}
			}
			}
				if(this.Eschema_league != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://schema.org/extended/league", $"\"{GenerarTextoSinSaltoDeLinea(this.Eschema_league)}\"", list, " . ");
				}
				if(this.Schema_identifier != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/identifier",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_identifier)}\"", list, " . ");
				}
				if(this.Schema_logo != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/logo",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_logo)}\"", list, " . ");
				}
				if(this.Schema_award != null)
				{
					foreach(var item2 in this.Schema_award)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/award", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Schema_alternateName != null)
				{
					foreach(var item2 in this.Schema_alternateName)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/alternateName", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Schema_description != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://schema.org/description", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_description)}\"", list, " . ");
				}
				if(this.Schema_foundingDate != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://schema.org/foundingDate", $"{this.Schema_foundingDate.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Schema_name != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/name",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
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
			return $"{resourceAPI.GraphsUrl}items/ClubpfihsOntology_{ResourceID}_{ArticleID}";
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
