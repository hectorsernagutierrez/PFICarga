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

namespace PersonapfihsOntology
{
	[ExcludeFromCodeCoverage]
	public class PostalAddress : GnossOCBase
	{
		public PostalAddress() : base() { } 

		public PostalAddress(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			mGNOSSID = pSemCmsModel.Entity.Uri;
			mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Schema_addressLocality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/addressLocality"));
			this.Schema_addressCountry = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/addressCountry"));
		}

		public virtual string RdfType { get { return "https://schema.org/PostalAddress"; } }
		public virtual string RdfsLabel { get { return "https://schema.org/PostalAddress"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.en,"City")]
		[LABEL(LanguageEnum.es,"Ciudad")]
		[RDFProperty("https://schema.org/addressLocality")]
		public  string Schema_addressLocality { get; set;}

		[LABEL(LanguageEnum.es,"Pais")]
		[LABEL(LanguageEnum.en,"Country")]
		[RDFProperty("https://schema.org/addressCountry")]
		public  string Schema_addressCountry { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("schema:addressLocality", this.Schema_addressLocality));
			propList.Add(new StringOntologyProperty("schema:addressCountry", this.Schema_addressCountry));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
