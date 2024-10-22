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

namespace PersonapfihsOntology
{
	[ExcludeFromCodeCoverage]
	public class Person : GnossOCBase
	{
		public Person() : base() { } 

		public Person(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			GNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			Schema_networth = new List<PriceSpecification>();
			SemanticPropertyModel propSchema_networth = pSemCmsModel.GetPropertyByPath("https://schema.org/networth");
			if(propSchema_networth != null && propSchema_networth.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_networth.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						PriceSpecification schema_networth = new PriceSpecification(propValue.RelatedEntity,idiomaUsuario);
						Schema_networth.Add(schema_networth);
					}
				}
			}
			Schema_nationality = new List<NationalityPath>();
			SemanticPropertyModel propSchema_nationality = pSemCmsModel.GetPropertyByPath("https://schema.org/nationality");
			if(propSchema_nationality != null && propSchema_nationality.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_nationality.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						NationalityPath schema_nationality = new NationalityPath(propValue.RelatedEntity,idiomaUsuario);
						Schema_nationality.Add(schema_nationality);
					}
				}
			}
			Eschema_i_club = new List<SportsClub>();
			SemanticPropertyModel propEschema_i_club = pSemCmsModel.GetPropertyByPath("https://schema.org/extended/i_club");
			if(propEschema_i_club != null && propEschema_i_club.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propEschema_i_club.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						SportsClub eschema_i_club = new SportsClub(propValue.RelatedEntity,idiomaUsuario);
						Eschema_i_club.Add(eschema_i_club);
					}
				}
			}
			SemanticPropertyModel propSchema_birthPlace = pSemCmsModel.GetPropertyByPath("https://schema.org/birthPlace");
			if (propSchema_birthPlace != null && propSchema_birthPlace.PropertyValues.Count > 0 && propSchema_birthPlace.PropertyValues[0].RelatedEntity != null)
			{
				Schema_birthPlace = new PostalAddress(propSchema_birthPlace.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Schema_identifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/identifier"));
			this.Eschema_foot = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/extended/foot"));
			SemanticPropertyModel propSchema_award = pSemCmsModel.GetPropertyByPath("https://schema.org/award");
			this.Schema_award = new List<string>();
			if (propSchema_award != null && propSchema_award.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_award.PropertyValues)
				{
					this.Schema_award.Add(propValue.Value);
				}
			}
			this.Schema_height = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/height"));
			this.Schema_description = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/description"));
			this.Schema_birthDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/birthDate"));
			SemanticPropertyModel propSchema_image = pSemCmsModel.GetPropertyByPath("https://schema.org/image");
			this.Schema_image = new List<string>();
			if (propSchema_image != null && propSchema_image.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_image.PropertyValues)
				{
					this.Schema_image.Add(propValue.Value);
				}
			}
			this.Schema_name = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/name"));
		}

		public Person(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			mGNOSSID = pSemCmsModel.Entity.Uri;
			mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			Schema_networth = new List<PriceSpecification>();
			SemanticPropertyModel propSchema_networth = pSemCmsModel.GetPropertyByPath("https://schema.org/networth");
			if(propSchema_networth != null && propSchema_networth.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_networth.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						PriceSpecification schema_networth = new PriceSpecification(propValue.RelatedEntity,idiomaUsuario);
						Schema_networth.Add(schema_networth);
					}
				}
			}
			Schema_nationality = new List<NationalityPath>();
			SemanticPropertyModel propSchema_nationality = pSemCmsModel.GetPropertyByPath("https://schema.org/nationality");
			if(propSchema_nationality != null && propSchema_nationality.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_nationality.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						NationalityPath schema_nationality = new NationalityPath(propValue.RelatedEntity,idiomaUsuario);
						Schema_nationality.Add(schema_nationality);
					}
				}
			}
			Eschema_i_club = new List<SportsClub>();
			SemanticPropertyModel propEschema_i_club = pSemCmsModel.GetPropertyByPath("https://schema.org/extended/i_club");
			if(propEschema_i_club != null && propEschema_i_club.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propEschema_i_club.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						SportsClub eschema_i_club = new SportsClub(propValue.RelatedEntity,idiomaUsuario);
						Eschema_i_club.Add(eschema_i_club);
					}
				}
			}
			SemanticPropertyModel propSchema_birthPlace = pSemCmsModel.GetPropertyByPath("https://schema.org/birthPlace");
			if (propSchema_birthPlace != null && propSchema_birthPlace.PropertyValues.Count > 0 && propSchema_birthPlace.PropertyValues[0].RelatedEntity != null)
			{
				Schema_birthPlace = new PostalAddress(propSchema_birthPlace.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Schema_identifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/identifier"));
			this.Eschema_foot = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/extended/foot"));
			SemanticPropertyModel propSchema_award = pSemCmsModel.GetPropertyByPath("https://schema.org/award");
			this.Schema_award = new List<string>();
			if (propSchema_award != null && propSchema_award.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_award.PropertyValues)
				{
					this.Schema_award.Add(propValue.Value);
				}
			}
			this.Schema_height = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/height"));
			this.Schema_description = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/description"));
			this.Schema_birthDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/birthDate"));
			SemanticPropertyModel propSchema_image = pSemCmsModel.GetPropertyByPath("https://schema.org/image");
			this.Schema_image = new List<string>();
			if (propSchema_image != null && propSchema_image.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_image.PropertyValues)
				{
					this.Schema_image.Add(propValue.Value);
				}
			}
			this.Schema_name = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/name"));
		}

		public virtual string RdfType { get { return "https://schema.org/Person"; } }
		public virtual string RdfsLabel { get { return "https://schema.org/Person"; } }
		[LABEL(LanguageEnum.es,"Historial Valores Mercado")]
		[LABEL(LanguageEnum.en,"Historical Market Values")]
		[RDFProperty("https://schema.org/networth")]
		public  List<PriceSpecification> Schema_networth { get; set;}

		[LABEL(LanguageEnum.en,"Nationality")]
		[LABEL(LanguageEnum.es,"Nacionalidad")]
		[RDFProperty("https://schema.org/nationality")]
		public  List<NationalityPath> Schema_nationality { get; set;}

		[LABEL(LanguageEnum.es,"Club")]
		[LABEL(LanguageEnum.en,"Club")]
		[RDFProperty("https://schema.org/extended/i_club")]
		public  List<SportsClub> Eschema_i_club { get; set;}
		public List<string> IdsEschema_i_club { get; set;}

		[LABEL(LanguageEnum.es,"Lugar de Nacimiento")]
		[LABEL(LanguageEnum.en,"Place of birth")]
		[RDFProperty("https://schema.org/birthPlace")]
		public  PostalAddress Schema_birthPlace { get; set;}

		[LABEL(LanguageEnum.es,"Id")]
		[RDFProperty("https://schema.org/identifier")]
		public  string Schema_identifier { get; set;}

		[LABEL(LanguageEnum.es,"Pie habil")]
		[LABEL(LanguageEnum.en,"Preferred foot")]
		[RDFProperty("https://schema.org/extended/foot")]
		public  string Eschema_foot { get; set;}

		[LABEL(LanguageEnum.es,"Premios")]
		[LABEL(LanguageEnum.en,"Awards")]
		[RDFProperty("https://schema.org/award")]
		public  List<string> Schema_award { get; set;}

		[LABEL(LanguageEnum.es,"Altura")]
		[LABEL(LanguageEnum.en,"Height")]
		[RDFProperty("https://schema.org/height")]
		public  int? Schema_height { get; set;}

		[LABEL(LanguageEnum.es,"Descripción")]
		[LABEL(LanguageEnum.en,"Description")]
		[RDFProperty("https://schema.org/description")]
		public  string Schema_description { get; set;}

		[LABEL(LanguageEnum.es,"Fecha Nacimiento")]
		[LABEL(LanguageEnum.en,"Birth Date")]
		[RDFProperty("https://schema.org/birthDate")]
		public  DateTime? Schema_birthDate { get; set;}

		[LABEL(LanguageEnum.es,"Image")]
		[RDFProperty("https://schema.org/image")]
		public  List<string> Schema_image { get; set;}

		[LABEL(LanguageEnum.en,"Name")]
		[LABEL(LanguageEnum.es,"Nombre")]
		[RDFProperty("https://schema.org/name")]
		public  string Schema_name { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("eschema:i_club", this.IdsEschema_i_club));
			propList.Add(new StringOntologyProperty("schema:identifier", this.Schema_identifier));
			propList.Add(new StringOntologyProperty("eschema:foot", this.Eschema_foot));
			propList.Add(new ListStringOntologyProperty("schema:award", this.Schema_award));
			propList.Add(new StringOntologyProperty("schema:height", this.Schema_height.ToString()));
			propList.Add(new StringOntologyProperty("schema:description", this.Schema_description));
			if (this.Schema_birthDate.HasValue){
				propList.Add(new DateOntologyProperty("schema:birthDate", this.Schema_birthDate.Value));
				}
			propList.Add(new ListStringOntologyProperty("schema:image", this.Schema_image));
			propList.Add(new StringOntologyProperty("schema:name", this.Schema_name));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Schema_networth!=null){
				foreach(PriceSpecification prop in Schema_networth){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityPriceSpecification = new OntologyEntity("https://schema.org/PriceSpecification", "https://schema.org/PriceSpecification", "schema:networth", prop.propList, prop.entList);
					entList.Add(entityPriceSpecification);
					prop.Entity = entityPriceSpecification;
				}
			}
			if(Schema_nationality!=null){
				foreach(NationalityPath prop in Schema_nationality){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityNationalityPath = new OntologyEntity("http://gnossg.gnoss.com/NationalityPath", "http://gnossg.gnoss.com/NationalityPath", "schema:nationality", prop.propList, prop.entList);
					entList.Add(entityNationalityPath);
					prop.Entity = entityNationalityPath;
				}
			}
			if(Schema_birthPlace!=null){
				Schema_birthPlace.GetProperties();
				Schema_birthPlace.GetEntities();
				OntologyEntity entitySchema_birthPlace = new OntologyEntity("https://schema.org/PostalAddress", "https://schema.org/PostalAddress", "schema:birthPlace", Schema_birthPlace.propList, Schema_birthPlace.entList);
				Schema_birthPlace.Entity = entitySchema_birthPlace;
				entList.Add(entitySchema_birthPlace);
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/Person>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/Person\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Schema_networth != null)
			{
			foreach(var item0 in this.Schema_networth)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PriceSpecification_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/PriceSpecification>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PriceSpecification_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/PriceSpecification\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/PriceSpecification_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "https://schema.org/networth", $"<{resourceAPI.GraphsUrl}items/PriceSpecification_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Schema_validFrom != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PriceSpecification_{ResourceID}_{item0.ArticleID}", "https://schema.org/validFrom",  $"\"{item0.Schema_validFrom.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(item0.Schema_price != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PriceSpecification_{ResourceID}_{item0.ArticleID}", "https://schema.org/price",  $"{item0.Schema_price.Value.ToString()}", list, " . ");
				}
				if(item0.Eschema_identifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PriceSpecification_{ResourceID}_{item0.ArticleID}",  "https://schema.org/extended/identifier", $"\"{GenerarTextoSinSaltoDeLinea(item0.Eschema_identifier)}\"", list, " . ");
				}
			}
			}
			if(this.Schema_nationality != null)
			{
			foreach(var item0 in this.Schema_nationality)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/NationalityPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://gnossg.gnoss.com/NationalityPath>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/NationalityPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://gnossg.gnoss.com/NationalityPath\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/NationalityPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "https://schema.org/nationality", $"<{resourceAPI.GraphsUrl}items/NationalityPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsGnossg_countryBirthNode != null)
				{
					foreach(var item2 in item0.IdsGnossg_countryBirthNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/NationalityPath_{ResourceID}_{item0.ArticleID}", "http://gnossg.gnoss.com/countryBirthNode", $"<{item2}>", list, " . ");
					}
				}
			}
			}
			if(this.Schema_birthPlace != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{this.Schema_birthPlace.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/PostalAddress>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{this.Schema_birthPlace.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/PostalAddress\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{this.Schema_birthPlace.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "https://schema.org/birthPlace", $"<{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{this.Schema_birthPlace.ArticleID}>", list, " . ");
				if(this.Schema_birthPlace.Schema_addressLocality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{this.Schema_birthPlace.ArticleID}",  "https://schema.org/addressLocality", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_birthPlace.Schema_addressLocality)}\"", list, " . ");
				}
				if(this.Schema_birthPlace.Schema_addressCountry != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{this.Schema_birthPlace.ArticleID}", "https://schema.org/addressCountry",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_birthPlace.Schema_addressCountry)}\"", list, " . ");
				}
			}
				if(this.IdsEschema_i_club != null)
				{
					foreach(var item2 in this.IdsEschema_i_club)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "https://schema.org/extended/i_club", $"<{item2}>", list, " . ");
					}
				}
				if(this.Schema_identifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "https://schema.org/identifier",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_identifier)}\"", list, " . ");
				}
				if(this.Eschema_foot != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "https://schema.org/extended/foot",  $"\"{GenerarTextoSinSaltoDeLinea(this.Eschema_foot)}\"", list, " . ");
				}
				if(this.Schema_award != null)
				{
					foreach(var item2 in this.Schema_award)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "https://schema.org/award", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Schema_height != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "https://schema.org/height", $"{this.Schema_height.Value.ToString()}", list, " . ");
				}
				if(this.Schema_description != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "https://schema.org/description", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_description)}\"", list, " . ");
				}
				if(this.Schema_birthDate != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "https://schema.org/birthDate", $"\"{this.Schema_birthDate.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Schema_image != null)
				{
					foreach(var item2 in this.Schema_image)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "https://schema.org/image", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Schema_name != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "https://schema.org/name",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"personapfihs\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"https://schema.org/Person\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
			string search = string.Empty;
			if(this.Schema_networth != null)
			{
			foreach(var item0 in this.Schema_networth)
			{
				AgregarTripleALista($"http://gnossAuxiliar/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasEntidadAuxiliar", $"<{resourceAPI.GraphsUrl}items/PriceSpecification_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/networth", $"<{resourceAPI.GraphsUrl}items/PriceSpecification_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Schema_validFrom != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PriceSpecification_{ResourceID}_{item0.ArticleID}", "https://schema.org/validFrom",  $"{item0.Schema_validFrom.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(item0.Schema_price != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PriceSpecification_{ResourceID}_{item0.ArticleID}", "https://schema.org/price",  $"{item0.Schema_price.Value.ToString()}", list, " . ");
				}
				if(item0.Eschema_identifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PriceSpecification_{ResourceID}_{item0.ArticleID}",  "https://schema.org/extended/identifier", $"\"{GenerarTextoSinSaltoDeLinea(item0.Eschema_identifier)}\"", list, " . ");
				}
			}
			}
			if(this.Schema_nationality != null)
			{
			foreach(var item0 in this.Schema_nationality)
			{
				AgregarTripleALista($"http://gnossAuxiliar/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasEntidadAuxiliar", $"<{resourceAPI.GraphsUrl}items/NationalityPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/nationality", $"<{resourceAPI.GraphsUrl}items/NationalityPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsGnossg_countryBirthNode != null)
				{
					foreach(var item2 in item0.IdsGnossg_countryBirthNode)
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
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/NationalityPath_{ResourceID}_{item0.ArticleID}", "http://gnossg.gnoss.com/countryBirthNode", $"<{itemRegex}>", list, " . ");
					}
				}
			}
			}
			if(this.Schema_birthPlace != null)
			{
				AgregarTripleALista($"http://gnossAuxiliar/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasEntidadAuxiliar", $"<{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{this.Schema_birthPlace.ArticleID}>", list, " . ");
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/birthPlace", $"<{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{this.Schema_birthPlace.ArticleID}>", list, " . ");
				if(this.Schema_birthPlace.Schema_addressLocality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{this.Schema_birthPlace.ArticleID}",  "https://schema.org/addressLocality", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_birthPlace.Schema_addressLocality)}\"", list, " . ");
				}
				if(this.Schema_birthPlace.Schema_addressCountry != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PostalAddress_{ResourceID}_{this.Schema_birthPlace.ArticleID}", "https://schema.org/addressCountry",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_birthPlace.Schema_addressCountry)}\"", list, " . ");
				}
			}
				if(this.IdsEschema_i_club != null)
				{
					foreach(var item2 in this.IdsEschema_i_club)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/extended/i_club", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.Schema_identifier != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/identifier",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_identifier)}\"", list, " . ");
				}
				if(this.Eschema_foot != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/extended/foot",  $"\"{GenerarTextoSinSaltoDeLinea(this.Eschema_foot)}\"", list, " . ");
				}
				if(this.Schema_award != null)
				{
					foreach(var item2 in this.Schema_award)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/award", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Schema_height != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://schema.org/height", $"{this.Schema_height.Value.ToString()}", list, " . ");
				}
				if(this.Schema_description != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://schema.org/description", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_description)}\"", list, " . ");
				}
				if(this.Schema_birthDate != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://schema.org/birthDate", $"{this.Schema_birthDate.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Schema_image != null)
				{
					foreach(var item2 in this.Schema_image)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://schema.org/image", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
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
			return $"{resourceAPI.GraphsUrl}items/PersonapfihsOntology_{ResourceID}_{ArticleID}";
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
