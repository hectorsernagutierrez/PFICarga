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

namespace OrganizacionpfihsOntology
{
	[ExcludeFromCodeCoverage]
	public class Organization : GnossOCBase
	{
		public Organization(string pIdentificador) : base()
		{
            Identificador = pIdentificador;
        }

		public Organization(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			mGNOSSID = pSemCmsModel.Entity.Uri;
			mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Schema_name = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/name"));
		}

		public virtual string RdfType { get { return "https://schema.org/Organization"; } }
		public virtual string RdfsLabel { get { return "https://schema.org/Organization"; } }
		public string Identificador { get; set; }
		[LABEL(LanguageEnum.en,"Name")]
		[LABEL(LanguageEnum.es,"Nombre")]
		[RDFProperty("https://schema.org/name")]
		public  string Schema_name { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("schema:name", this.Schema_name));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 
		public virtual SecondaryResource ToGnossApiResource(ResourceApi resourceAPI,string identificador)
		{
			SecondaryResource resource = new SecondaryResource();
			List<SecondaryEntity> listSecondaryEntity = null;
			GetProperties();
			SecondaryOntology ontology = new SecondaryOntology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, "https://schema.org/Organization", "https://schema.org/Organization", prefList, propList,identificador,listSecondaryEntity, null);
			resource.SecondaryOntology = ontology;
			AddImages(resource);
			AddFiles(resource);
			return resource;
		}

		public override List<string> ToOntologyGnossTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/{Identificador}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://schema.org/Organization>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/{Identificador}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://schema.org/Organization\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}entidadsecun_{Identificador.ToLower()}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/{Identificador}>", list, " . ");
				if(this.Schema_name != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/{Identificador}", "https://schema.org/name",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
				if(this.Schema_name != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl.ToLower()}items/{Identificador}", "https://schema.org/name",  $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
				}
			return list;
		}

		public override KeyValuePair<Guid, string> ToAcidData(ResourceApi resourceAPI)
		{
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>();

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/OrganizacionpfihsOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
		}





	}
}
