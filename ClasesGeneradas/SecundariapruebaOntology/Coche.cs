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

namespace SecundariapruebaOntology
{
	[ExcludeFromCodeCoverage]
	public class Coche : GnossOCBase
	{
		public Coche(string pIdentificador) : base()
		{
            Identificador = pIdentificador;
        }

		public Coche(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			mGNOSSID = pSemCmsModel.Entity.Uri;
			mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Propio_nombre = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.domainpropio.com#nombre"));
		}

		public virtual string RdfType { get { return "http://www.domainpropio.com#Coche"; } }
		public virtual string RdfsLabel { get { return "http://www.domainpropio.com#Coche"; } }
		public string Identificador { get; set; }
		[LABEL(LanguageEnum.es,"http://www.domainpropio.com#nombre")]
		[RDFProperty("http://www.domainpropio.com#nombre")]
		public  string Propio_nombre { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("propio:nombre", this.Propio_nombre));
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
			SecondaryOntology ontology = new SecondaryOntology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, "http://www.domainpropio.com#Coche", "http://www.domainpropio.com#Coche", prefList, propList,identificador,listSecondaryEntity, null);
			resource.SecondaryOntology = ontology;
			AddImages(resource);
			AddFiles(resource);
			return resource;
		}

		public override List<string> ToOntologyGnossTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/{Identificador}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://www.domainpropio.com#Coche>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/{Identificador}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://www.domainpropio.com#Coche\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}entidadsecun_{Identificador.ToLower()}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/{Identificador}>", list, " . ");
				if(this.Propio_nombre != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/{Identificador}", "http://www.domainpropio.com#nombre",  $"\"{GenerarTextoSinSaltoDeLinea(this.Propio_nombre)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
				if(this.Propio_nombre != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl.ToLower()}items/{Identificador}", "http://www.domainpropio.com#nombre",  $"\"{GenerarTextoSinSaltoDeLinea(this.Propio_nombre)}\"", list, " . ");
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
			return $"{resourceAPI.GraphsUrl}items/SecundariapruebaOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
		}





	}
}
