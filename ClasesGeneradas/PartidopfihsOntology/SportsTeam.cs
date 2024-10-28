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
using Person = PersonapfihsOntology.Person;

namespace PartidopfihsOntology
{
	[ExcludeFromCodeCoverage]
	public class SportsTeam : GnossOCBase
	{
		public SportsTeam() : base() { } 

		public SportsTeam(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			mGNOSSID = pSemCmsModel.Entity.Uri;
			mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propSchema_subOrganization = pSemCmsModel.GetPropertyByPath("https://schema.org/subOrganization");
			if (propSchema_subOrganization != null && propSchema_subOrganization.PropertyValues.Count > 0 && propSchema_subOrganization.PropertyValues[0].RelatedEntity != null)
			{
				Schema_subOrganization = new SportsClub(propSchema_subOrganization.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			Schema_coach = new List<Person>();
			SemanticPropertyModel propSchema_coach = pSemCmsModel.GetPropertyByPath("https://schema.org/coach");
			if(propSchema_coach != null && propSchema_coach.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_coach.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Person schema_coach = new Person(propValue.RelatedEntity,idiomaUsuario);
						Schema_coach.Add(schema_coach);
					}
				}
			}
			Schema_athlete = new List<PersonLinedUp>();
			SemanticPropertyModel propSchema_athlete = pSemCmsModel.GetPropertyByPath("https://schema.org/athlete");
			if(propSchema_athlete != null && propSchema_athlete.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_athlete.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						PersonLinedUp schema_athlete = new PersonLinedUp(propValue.RelatedEntity,idiomaUsuario);
						Schema_athlete.Add(schema_athlete);
					}
				}
			}
			this.Eschema_classification = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/extended/classification"));
		}

		public virtual string RdfType { get { return "https://schema.org/SportsTeam"; } }
		public virtual string RdfsLabel { get { return "https://schema.org/SportsTeam"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.es,"Club:")]
		[RDFProperty("https://schema.org/subOrganization")]
		public  SportsClub Schema_subOrganization  { get; set;} 
		public string IdSchema_subOrganization  { get; set;} 

		[LABEL(LanguageEnum.en,"Coach")]
		[LABEL(LanguageEnum.es,"Entrenador")]
		[RDFProperty("https://schema.org/coach")]
		public  List<Person> Schema_coach { get; set;}
		public List<string> IdsSchema_coach { get; set;}

		[LABEL(LanguageEnum.es,"Jugadores")]
		[LABEL(LanguageEnum.en,"Players")]
		[RDFProperty("https://schema.org/athlete")]
		public  List<PersonLinedUp> Schema_athlete { get; set;}

		[LABEL(LanguageEnum.es,"Clas.")]
		[RDFProperty("https://schema.org/extended/classification")]
		public  int? Eschema_classification { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("schema:subOrganization", this.IdSchema_subOrganization));
			propList.Add(new ListStringOntologyProperty("schema:coach", this.IdsSchema_coach));
			propList.Add(new StringOntologyProperty("eschema:classification", this.Eschema_classification.ToString()));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Schema_athlete!=null){
				foreach(PersonLinedUp prop in Schema_athlete){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityPersonLinedUp = new OntologyEntity("https://schema.org/extended/PersonLinedUp", "https://schema.org/extended/PersonLinedUp", "schema:athlete", prop.propList, prop.entList);
					entList.Add(entityPersonLinedUp);
					prop.Entity = entityPersonLinedUp;
				}
			}
		} 











	}
}
