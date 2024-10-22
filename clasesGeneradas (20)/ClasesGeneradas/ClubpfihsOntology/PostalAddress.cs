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
	public class PostalAddress : GnossOCBase
	{
		public PostalAddress() : base() { } 

		public PostalAddress(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			mGNOSSID = pSemCmsModel.Entity.Uri;
			mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Schema_streetAddress = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/streetAddress"));
			this.Schema_PostalCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/PostalCode"));
			this.Schema_postOfficeBoxNumber = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/postOfficeBoxNumber"));
			this.Schema_addressLocality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/addressLocality"));
			this.Schema_addressCountry = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/addressCountry"));
		}

		public virtual string RdfType { get { return "https://schema.org/PostalAddress"; } }
		public virtual string RdfsLabel { get { return "https://schema.org/PostalAddress"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.es,"Calle")]
		[LABEL(LanguageEnum.en,"Street")]
		[RDFProperty("https://schema.org/streetAddress")]
		public  string Schema_streetAddress { get; set;}

		[LABEL(LanguageEnum.es,"Código Postal")]
		[LABEL(LanguageEnum.en,"Postal Code")]
		[RDFProperty("https://schema.org/PostalCode")]
		public  string Schema_PostalCode { get; set;}

		[LABEL(LanguageEnum.es,"Número")]
		[LABEL(LanguageEnum.en,"Number")]
		[RDFProperty("https://schema.org/postOfficeBoxNumber")]
		public  string Schema_postOfficeBoxNumber { get; set;}

		[LABEL(LanguageEnum.es,"Ciudad")]
		[LABEL(LanguageEnum.en,"City")]
		[RDFProperty("https://schema.org/addressLocality")]
		public  string Schema_addressLocality { get; set;}

		[LABEL(LanguageEnum.es,"País")]
		[LABEL(LanguageEnum.en,"Country")]
		[RDFProperty("https://schema.org/addressCountry")]
		public  string Schema_addressCountry { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("schema:streetAddress", this.Schema_streetAddress));
			propList.Add(new StringOntologyProperty("schema:PostalCode", this.Schema_PostalCode));
			propList.Add(new StringOntologyProperty("schema:postOfficeBoxNumber", this.Schema_postOfficeBoxNumber));
			propList.Add(new StringOntologyProperty("schema:addressLocality", this.Schema_addressLocality));
			propList.Add(new StringOntologyProperty("schema:addressCountry", this.Schema_addressCountry));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
