///*
// * Autor:Hector Serna Gutierrez
// * Proyecto Futbol
// * version 1.0
// * Ultima mod 16/10/2024
// * Contacto: hectorserna@gnoss.com
// * Documento: Program
// */

//using Gnoss.ApiWrapper;
//using Gnoss.ApiWrapper.ApiModel;
//using Gnoss.ApiWrapper.Model;
//using System.Xml;
//using System.Text;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Reflection.Metadata;
//using static GnossBase.GnossOCBase;
//;
//#region Conexion comunidad


//string pathOAuth = @"Config\oAuth.config";
//ResourceApi mResourceApi = new ResourceApi(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, pathOAuth));
//CommunityApi mCommunityApi = new CommunityApi(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, pathOAuth));
//ThesaurusApi mThesaurusApi = new ThesaurusApi(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, pathOAuth));
//UserApi mUserApi = new UserApi(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, pathOAuth));
//Console.WriteLine($"Id de la Comunidad -> {mCommunityApi.GetCommunityId()}");
//Console.WriteLine($"Nombre de la Comunidad -> {mCommunityApi.GetCommunityInfo().name}");
//Console.WriteLine($"Nombre Corto de la Comunidad -> {mCommunityApi.GetCommunityInfo().short_name}");
//Console.WriteLine($"Descripción de la comunidad inicial -> {mCommunityApi.GetCommunityInfo().description}");
//Console.WriteLine($"Categorías de la Comunidad -> {string.Join(", ", mCommunityApi.CommunityCategories.Select(categoria => categoria.category_name))}");
//Console.WriteLine("USUARIOS");



//foreach (var guidUsuario in mCommunityApi.GetCommunityInfo().users)
//{
//    Console.WriteLine(guidUsuario.ToString());
//    //KeyValuePair<Guid, Userlite> primerUsuario = mUserApi.GetUsersByIds(new List<Guid>() { guidUsuario }).FirstOrDefault();
//    //string nombrePrimerUsuario = primerUsuario.Value.user_short_name;
//}

//#endregion Conexión comunidad

//#region Carga del tesauro principal de una comunidad desde Archivo XML

//mCommunityApi.Log.Debug("Inicio de la Carga del tesauro de la comunidad");
//mCommunityApi.Log.Debug("**************************************");

//// Si hay recursos Categorizados contra alguna categoría del Tesauro, no se puede borrar el TESAURO
//// Eliminamos el vinculo de cualquier recurso con cualquier Categoría
////BorrarCategoriasDeRecursos("peliculacrudapi");
////BorrarCategoriasDeRecursos("personacrudapi");

//// Leemos del XML la estrucutra del Tesauro (Categorías) a cargar en la Comunidad
//XmlDocument xmlCategorias = new XmlDocument();
//xmlCategorias.Load($"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Documents\\ESTRUCTURA_CATEGORIAS_COMPLETO_MOD_SIN.xml");
////mCommunityApi.CreateThesaurus(xmlCategorias.OuterXml);

////Obtenemos el Tesauro de Categorías de la comunidad (XML)
//string xml = mCommunityApi.GetThesaurus();

////Imprimimos por Consola el Tesauro Cargado
//Console.WriteLine(xml);

//mCommunityApi.Log.Debug("**************************************");
//mCommunityApi.Log.Debug("Fin de la Carga del tesauro de comunidad (categorías)'");

//#endregion Carga del tesauro principal de una comunidad desde Archivo XML

//#region Carga de un tesauro semantico + Modificación y eliminación de categorías

//mCommunityApi.Log.Debug("Inicio de la Carga del tesauro semántico");
//mCommunityApi.Log.Debug("**************************************");

//Dictionary<string, List<string>> d_contiente_paises = new();
//d_contiente_paises.Add("Africa", new List<string>() {
//    "Morocco", "Cameroon", "Angola", "Tunisia", "Nigeria", "DR Congo", "Senegal", "Guinea",
//    "South Africa", "Ghana", "Mali", "Algeria", "Eritrea", "Egypt", "Cape Verde",
//    "Equatorial Guinea", "Togo", "Burkina Faso", "Mozambique", "Burundi", "Gabon", "Sierra Leone",
//    "Madagascar", "Zambia", "Kenya", "Ethiopia", "Seychelles", "Zimbabwe", "Uganda", "Comoros",
//    "Liberia", "Chad", "Mauritania", "Niger", "Somalia", "Tanzania", "Southern Sudan"
//});

//d_contiente_paises.Add("Europe", new List<string>() {
//    "Germany", "Bulgaria", "Czech Republic", "Hungary", "Denmark", "Georgia", "North Macedonia",
//    "Austria", "Belarus", "Croatia", "Turkey", "Ukraine", "France", "England", "Belgium",
//    "Switzerland", "Montenegro", "Bosnia-Herzegovina", "Scotland", "Sweden", "Romania", "Ireland",
//    "Italy", "Spain", "Iceland", "Greece", "Norway", "Wales", "Serbia", "Portugal", "Netherlands",
//    "Slovakia", "Northern Ireland", "Finland", "Russia", "Lithuania", "Albania", "Poland",
//    "Slovenia", "Latvia", "Estonia", "Kosovo", "Luxembourg", "Malta", "Liechtenstein", "Monaco",
//    "Andorra", "San Marino"
//});

//d_contiente_paises.Add("Asia", new List<string>() {
//    "Korea, South", "Iran", "Chinese Taipei", "Syria", "Philippines", "Pakistan", "Japan",
//    "Oman", "Azerbaijan", "Qatar", "Uzbekistan", "Thailand", "Iraq", "India", "Tajikistan",
//    "Kazakhstan", "Malaysia", "Palestine", "Kyrgyzstan", "Saudi Arabia", "China", "Lebanon",
//    "Jordan", "Turkmenistan", "United Arab Emirates", "Vietnam", "Korea, North", "Singapore"
//});

//d_contiente_paises.Add("North America", new List<string>() {
//    "United States", "Canada", "Mexico", "Grenada", "Saint-Martin", "Martinique", "Trinidad and Tobago",
//    "Guadeloupe", "Antigua and Barbuda", "The Gambia", "Tahiti", "Dominican Republic", "El Salvador",
//    "Honduras", "Jamaica", "Costa Rica", "Guatemala", "Haiti", "Panama", "Cuba", "St. Kitts & Nevis",
//    "Neukaledonien", "St. Lucia", "Montserrat", "Bermuda", "Nicaragua", "Bonaire"
//});

//d_contiente_paises.Add("South America", new List<string>() {
//    "Brazil", "Paraguay", "Peru", "Argentina", "Chile", "Colombia", "Uruguay", "Ecuador", "Venezuela", "Bolivia", "Suriname", "Guyana"
//});

//d_contiente_paises.Add("Oceania", new List<string>() {
//    "Australia", "Tahiti", "Papua New Guinea", "New Zealand"
//});

//d_contiente_paises.Add("Others", new List<string>() {
//    "Réunion", "Mauritius", "Neukaledonien", "Sao Tome and Principe", "Macao", "Bahrain", "Brunei Darussalam", "Vietnam", "Somalia", "Southern Sudan"
//});

//Thesaurus tesauro = new Thesaurus();

////Thesaurus tesauro = new Thesaurus();
////mThesaurusApi.DeleteThesaurus("category", "taxonomy");
////mThesaurusApi.DeleteCategory("http://cs.gnoss.com/items/Concept_001", "taxonomy");

////mThesaurusApi.DeleteThesaurus("category", "taxonomy");
//tesauro.Source = "nationality";
//tesauro.Ontology = "taxonomypfihs";
//tesauro.CommunityShortName = "pfi-hector";
//tesauro.Collection = new Collection();
//tesauro.Collection.Member = new List<Concept>();
//tesauro.Collection.ScopeNote = new Dictionary<string, string>() { { "en", "Countries" } };
//tesauro.Collection.Subject = "http://testing.gnoss.com/items/nationality";

//foreach (var continenteTesauroElement in d_contiente_paises.Keys)
//{
//    string nombreContinenteParaURL = continenteTesauroElement.Replace(" ", "-").ToLower();
//    Concept continenteConcept = new Concept();
//    continenteConcept.PrefLabel = new Dictionary<string, string>() { { "en", continenteTesauroElement } };
//    continenteConcept.Symbol = "1";
//    continenteConcept.Identifier = $"nationality_continent-{nombreContinenteParaURL}-id"; //Propiedad identifier
//    continenteConcept.Subject = $"nationality_continent-{nombreContinenteParaURL}-sj"; //Se acopla para formar la URI
//    continenteConcept.Narrower = new List<Concept>();
//    foreach (var paisTesauroElement in d_contiente_paises[continenteTesauroElement])
//    {
//        string nombrePaisParaURL = paisTesauroElement.Replace(" ", "-").ToLower();
//        Concept paisConcept = new Concept();
//        paisConcept.PrefLabel = new Dictionary<string, string>() { { "en", paisTesauroElement } };
//        paisConcept.Symbol = "2";
//        paisConcept.Identifier = $"nationality_country-{nombreContinenteParaURL}-{nombrePaisParaURL}-id";
//        paisConcept.Subject = $"nationality_country-{nombreContinenteParaURL}-{nombrePaisParaURL}-sj";
//        continenteConcept.Narrower.Add(paisConcept);
//    }
//    tesauro.Collection.Member.Add(continenteConcept);
//}
//tesauro.Collection.Member.FirstOrDefault().PrefLabel = new Dictionary<string, string>() { { "en", "Africa" } };

////Modificar categoría
////mThesaurusApi.ModifyCategory(tesauro.Collection.Member.FirstOrDefault(), tesauro.Source, tesauro.Ontology, false);
////Borrar categoría (URI del recurso, Nombre de la ontología)
////mThesaurusApi.DeleteCategory("http://gnoss.com/items/" + tesauro.Collection.Member.FirstOrDefault().Subject, tesauro.Ontology);

////mThesaurusApi.DeleteThesaurus(tesauro.Source,tesauro.Ontology);
////mThesaurusApi.CreateThesaurus(tesauro);
//#endregion Carga de un tesauro semantico