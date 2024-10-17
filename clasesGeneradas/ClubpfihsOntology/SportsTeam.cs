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
using Thing = TemporadapfihsOntology.Thing;
using Person = PersonapfihsOntology.Person;
using SportsTournament = TorneopfihsOntology.SportsTournament;

namespace ClubpfihsOntology
{
	[ExcludeFromCodeCoverage]
	public class SportsTeam : GnossOCBase
	{
		public SportsTeam() : base() { } 

		public SportsTeam(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			mGNOSSID = pSemCmsModel.Entity.Uri;
			mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propEschema_season = pSemCmsModel.GetPropertyByPath("https://schema.org/extended/season");
			if (propEschema_season != null && propEschema_season.PropertyValues.Count > 0 && propEschema_season.PropertyValues[0].RelatedEntity != null)
			{
				Eschema_season = new Thing(propEschema_season.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			Schema_athlete = new List<Person>();
			SemanticPropertyModel propSchema_athlete = pSemCmsModel.GetPropertyByPath("https://schema.org/athlete");
			if(propSchema_athlete != null && propSchema_athlete.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_athlete.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Person schema_athlete = new Person(propValue.RelatedEntity,idiomaUsuario);
						Schema_athlete.Add(schema_athlete);
					}
				}
			}
			Eschema_i_winner = new List<SportsTournament>();
			SemanticPropertyModel propEschema_i_winner = pSemCmsModel.GetPropertyByPath("https://schema.org/extended/i_winner");
			if(propEschema_i_winner != null && propEschema_i_winner.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propEschema_i_winner.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						SportsTournament eschema_i_winner = new SportsTournament(propValue.RelatedEntity,idiomaUsuario);
						Eschema_i_winner.Add(eschema_i_winner);
					}
				}
			}
			this.Eschema_identifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/extended/identifier"));
		}

		public virtual string RdfType { get { return "https://schema.org/SportsTeam"; } }
		public virtual string RdfsLabel { get { return "https://schema.org/SportsTeam"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.en,"Season")]
		[LABEL(LanguageEnum.es,"Temporada")]
		[RDFProperty("https://schema.org/extended/season")]
		public  Thing Eschema_season  { get; set;} 
		public string IdEschema_season  { get; set;} 

		[LABEL(LanguageEnum.es,"Entrenador")]
		[LABEL(LanguageEnum.en,"Coach")]
		[RDFProperty("https://schema.org/coach")]
		public  List<Person> Schema_coach { get; set;}
		public List<string> IdsSchema_coach { get; set;}

		[LABEL(LanguageEnum.es,"Jugadores")]
		[LABEL(LanguageEnum.en,"Players")]
		[RDFProperty("https://schema.org/athlete")]
		public  List<Person> Schema_athlete { get; set;}
		public List<string> IdsSchema_athlete { get; set;}

		[LABEL(LanguageEnum.en,"Winned Tournament")]
		[LABEL(LanguageEnum.es,"Torneos Ganados")]
		[RDFProperty("https://schema.org/extended/i_winner")]
		public  List<SportsTournament> Eschema_i_winner { get; set;}
		public List<string> IdsEschema_i_winner { get; set;}

		[RDFProperty("https://schema.org/extended/identifier")]
		public  string Eschema_identifier { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("eschema:season", this.IdEschema_season));
			propList.Add(new ListStringOntologyProperty("schema:coach", this.IdsSchema_coach));
			propList.Add(new ListStringOntologyProperty("schema:athlete", this.IdsSchema_athlete));
			propList.Add(new ListStringOntologyProperty("eschema:i_winner", this.IdsEschema_i_winner));
			propList.Add(new StringOntologyProperty("eschema:identifier", this.Eschema_identifier));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
