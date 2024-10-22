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
using Thing = TipopfihsOntology.Thing;
using Position = PosicionpfihsOntology.Position;
using Person = PersonapfihsOntology.Person;

namespace PartidopfihsOntology
{
	[ExcludeFromCodeCoverage]
	public class PersonLinedUp : GnossOCBase
	{
		public PersonLinedUp() : base() { } 

		public PersonLinedUp(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			mGNOSSID = pSemCmsModel.Entity.Uri;
			mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propEschema_type = pSemCmsModel.GetPropertyByPath("https://schema.org/extended/type");
			if (propEschema_type != null && propEschema_type.PropertyValues.Count > 0 && propEschema_type.PropertyValues[0].RelatedEntity != null)
			{
				Eschema_type = new Thing(propEschema_type.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propEschema_position = pSemCmsModel.GetPropertyByPath("https://schema.org/extended/position");
			if (propEschema_position != null && propEschema_position.PropertyValues.Count > 0 && propEschema_position.PropertyValues[0].RelatedEntity != null)
			{
				Eschema_position = new Position(propEschema_position.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propEschema_player = pSemCmsModel.GetPropertyByPath("https://schema.org/extended/player");
			if (propEschema_player != null && propEschema_player.PropertyValues.Count > 0 && propEschema_player.PropertyValues[0].RelatedEntity != null)
			{
				Eschema_player = new Person(propEschema_player.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Eschema_bibNumber = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/extended/bibNumber"));
		}

		public virtual string RdfType { get { return "https://schema.org/extended/PersonLinedUp"; } }
		public virtual string RdfsLabel { get { return "https://schema.org/extended/PersonLinedUp"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.en,"Type of Line Up")]
		[LABEL(LanguageEnum.es,"Tipado de Alineación")]
		[RDFProperty("https://schema.org/extended/type")]
		public  Thing Eschema_type  { get; set;} 
		public string IdEschema_type  { get; set;} 

		[LABEL(LanguageEnum.en,"Field Position")]
		[LABEL(LanguageEnum.es,"Posición sobre el Campo")]
		[RDFProperty("https://schema.org/extended/position")]
		public  Position Eschema_position  { get; set;} 
		public string IdEschema_position  { get; set;} 

		[LABEL(LanguageEnum.en,"Player")]
		[LABEL(LanguageEnum.es,"Jugador")]
		[RDFProperty("https://schema.org/extended/player")]
		public  Person Eschema_player  { get; set;} 
		public string IdEschema_player  { get; set;} 

		[LABEL(LanguageEnum.en,"Bib Number")]
		[LABEL(LanguageEnum.es,"Dorsal")]
		[RDFProperty("https://schema.org/extended/bibNumber")]
		public  string Eschema_bibNumber { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("eschema:type", this.IdEschema_type));
			propList.Add(new StringOntologyProperty("eschema:position", this.IdEschema_position));
			propList.Add(new StringOntologyProperty("eschema:player", this.IdEschema_player));
			propList.Add(new StringOntologyProperty("eschema:bibNumber", this.Eschema_bibNumber));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
