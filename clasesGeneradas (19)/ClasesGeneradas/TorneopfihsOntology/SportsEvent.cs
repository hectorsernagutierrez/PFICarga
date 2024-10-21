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

namespace TorneopfihsOntology
{
	[ExcludeFromCodeCoverage]
	public class SportsEvent : GnossOCBase
	{
		public SportsEvent() : base() { } 

		public SportsEvent(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			mGNOSSID = pSemCmsModel.Entity.Uri;
			mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propSchema_awayTeam = pSemCmsModel.GetPropertyByPath("https://schema.org/awayTeam");
			if (propSchema_awayTeam != null && propSchema_awayTeam.PropertyValues.Count > 0 && propSchema_awayTeam.PropertyValues[0].RelatedEntity != null)
			{
				Schema_awayTeam = new SportsTeam(propSchema_awayTeam.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propSchema_homeTeam = pSemCmsModel.GetPropertyByPath("https://schema.org/homeTeam");
			if (propSchema_homeTeam != null && propSchema_homeTeam.PropertyValues.Count > 0 && propSchema_homeTeam.PropertyValues[0].RelatedEntity != null)
			{
				Schema_homeTeam = new SportsTeam(propSchema_homeTeam.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			Schema_subEvent = new List<Event>();
			SemanticPropertyModel propSchema_subEvent = pSemCmsModel.GetPropertyByPath("https://schema.org/subEvent");
			if(propSchema_subEvent != null && propSchema_subEvent.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_subEvent.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Event schema_subEvent = new Event(propValue.RelatedEntity,idiomaUsuario);
						Schema_subEvent.Add(schema_subEvent);
					}
				}
			}
			this.Eschema_identifier_partido = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/extended/identifier_partido"));
			this.Eschema_result = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://schema.org/extended/result"));
		}

		public virtual string RdfType { get { return "https://schema.org/SportsEvent"; } }
		public virtual string RdfsLabel { get { return "https://schema.org/SportsEvent"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.es,"https://schema.org/awayTeam")]
		[RDFProperty("https://schema.org/awayTeam")]
		public  SportsTeam Schema_awayTeam { get; set;}

		[LABEL(LanguageEnum.es,"Local")]
		[RDFProperty("https://schema.org/homeTeam")]
		public  SportsTeam Schema_homeTeam { get; set;}

		[LABEL(LanguageEnum.en,"Match events")]
		[LABEL(LanguageEnum.es,"Eventos del partido")]
		[RDFProperty("https://schema.org/subEvent")]
		public  List<Event> Schema_subEvent { get; set;}

		[LABEL(LanguageEnum.es,"Id")]
		[RDFProperty("https://schema.org/extended/identifier_partido")]
		public  string Eschema_identifier_partido { get; set;}

		[LABEL(LanguageEnum.en,"Result")]
		[LABEL(LanguageEnum.es,"Resultado")]
		[RDFProperty("https://schema.org/extended/result")]
		public  string Eschema_result { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("eschema:identifier_partido", this.Eschema_identifier_partido));
			propList.Add(new StringOntologyProperty("eschema:result", this.Eschema_result));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Schema_awayTeam!=null){
				Schema_awayTeam.GetProperties();
				Schema_awayTeam.GetEntities();
				OntologyEntity entitySchema_awayTeam = new OntologyEntity("https://schema.org/SportsTeam", "https://schema.org/SportsTeam", "schema:awayTeam", Schema_awayTeam.propList, Schema_awayTeam.entList);
				Schema_awayTeam.Entity = entitySchema_awayTeam;
				entList.Add(entitySchema_awayTeam);
			}
			if(Schema_homeTeam!=null){
				Schema_homeTeam.GetProperties();
				Schema_homeTeam.GetEntities();
				OntologyEntity entitySchema_homeTeam = new OntologyEntity("https://schema.org/SportsTeam", "https://schema.org/SportsTeam", "schema:homeTeam", Schema_homeTeam.propList, Schema_homeTeam.entList);
				Schema_homeTeam.Entity = entitySchema_homeTeam;
				entList.Add(entitySchema_homeTeam);
			}
			if(Schema_subEvent!=null){
				foreach(Event prop in Schema_subEvent){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityEvent = new OntologyEntity("https://schema.org/Event", "https://schema.org/Event", "schema:subEvent", prop.propList, prop.entList);
					entList.Add(entityEvent);
					prop.Entity = entityEvent;
				}
			}
		} 











	}
}
