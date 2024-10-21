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
using Concept = TaxonomypfihsOntology.Concept;

namespace PersonapfihsOntology
{
	[ExcludeFromCodeCoverage]
	public class NationalityPath : GnossOCBase
	{
		public NationalityPath() : base() { } 

		public NationalityPath(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			mGNOSSID = pSemCmsModel.Entity.Uri;
			mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			Gnossg_countryBirthNode = new List<Concept>();
			SemanticPropertyModel propGnossg_countryBirthNode = pSemCmsModel.GetPropertyByPath("http://gnossg.gnoss.com/countryBirthNode");
			if(propGnossg_countryBirthNode != null && propGnossg_countryBirthNode.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propGnossg_countryBirthNode.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Concept gnossg_countryBirthNode = new Concept(propValue.RelatedEntity,idiomaUsuario);
						Gnossg_countryBirthNode.Add(gnossg_countryBirthNode);
					}
				}
			}
		}

		public virtual string RdfType { get { return "http://gnossg.gnoss.com/NationalityPath"; } }
		public virtual string RdfsLabel { get { return "http://gnossg.gnoss.com/NationalityPath"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.es,"")]
		[RDFProperty("http://gnossg.gnoss.com/countryBirthNode")]
		public  List<Concept> Gnossg_countryBirthNode { get; set;}
		public List<string> IdsGnossg_countryBirthNode { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("gnossg:countryBirthNode", this.IdsGnossg_countryBirthNode));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
