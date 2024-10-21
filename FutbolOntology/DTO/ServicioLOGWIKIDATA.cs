
using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using System.Globalization;

public class ServicioLOGWIKIDATA
{
    // Método para leer el CSV de Direcciones Postales y devolver el diccionario
    public static Dictionary<string, List<ClubpfihsOntology.PostalAddress>> ObtenerDireccionesPostales(string rutaArchivo)
    {
        var direccionesPostales = new Dictionary<string, List<ClubpfihsOntology.PostalAddress>>();

        using (var reader = new StreamReader(rutaArchivo))
        using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var registros = csv.GetRecords<DireccionPostalDTO>().ToList();
            foreach (var direccion in registros)
            {
                // Construir un objeto PostalAddress a partir de los datos de DireccionPostal
                ClubpfihsOntology.PostalAddress pad = new ClubpfihsOntology.PostalAddress
                {
                    Schema_PostalCode = !string.IsNullOrEmpty(direccion.CodigoPostal) ? direccion.CodigoPostal : "",
                    Schema_addressCountry = !string.IsNullOrEmpty(direccion.Pais) ? direccion.Pais : "",
                    Schema_streetAddress = !string.IsNullOrEmpty(direccion.Calle) ? direccion.Calle : "",
                    Schema_addressLocality = !string.IsNullOrEmpty(direccion.Ciudad) ? direccion.Ciudad : ""
                };

                // Verificar si el club ya está en el diccionario
                if (!direccionesPostales.ContainsKey(direccion.ClubNombre))
                {
                    direccionesPostales[direccion.ClubNombre] = new List<ClubpfihsOntology.PostalAddress>();
                }

                // Agregar la dirección postal construida al club correspondiente
                direccionesPostales[direccion.ClubNombre].Add(pad);
            }
        }

        return direccionesPostales;
    }

    // Método para leer el CSV de Entrenadores y devolver el diccionario
    public static Dictionary<string, Dictionary<string, List<DateTime>>> ObtenerEntrenadores(string rutaArchivo)
    {
        var entrenadoresDiccionario = new Dictionary<string, Dictionary<string, List<DateTime>>>();

        using (var reader = new StreamReader(rutaArchivo))
        using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var registros = csv.GetRecords<EntrenadorDTO>().ToList();
            foreach (var entrenador in registros)
            {
                if (!entrenadoresDiccionario.ContainsKey(entrenador.ClubNombre))
                {
                    entrenadoresDiccionario[entrenador.ClubNombre] = new Dictionary<string, List<DateTime>>();
                }

                if (!entrenadoresDiccionario[entrenador.ClubNombre].ContainsKey(entrenador.EntrenadorNombre))
                {
                    entrenadoresDiccionario[entrenador.ClubNombre][entrenador.EntrenadorNombre] = new List<DateTime>();
                }

                entrenadoresDiccionario[entrenador.ClubNombre][entrenador.EntrenadorNombre].Add(entrenador.FechaComienzo ?? DateTime.MinValue);
                entrenadoresDiccionario[entrenador.ClubNombre][entrenador.EntrenadorNombre].Add(entrenador.FechaFin ?? DateTime.MinValue);
            }
        }

        return entrenadoresDiccionario;
    }

    // Método para leer el CSV de Fundación y devolver el diccionario
    public static Dictionary<string, DateTime> ObtenerFundacion(string rutaArchivo)
    {
        var fundacionDiccionario = new Dictionary<string, DateTime>();

        using (var reader = new StreamReader(rutaArchivo))
        using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var registros = csv.GetRecords<FundacionDTO>().ToList();
            foreach (var fundacion in registros)
            {
                if (!fundacionDiccionario.ContainsKey(fundacion.ClubNombre))
                {
                    fundacionDiccionario[fundacion.ClubNombre] = fundacion.FechaFundacion;
                }
            }
        }

        return fundacionDiccionario;
    }

    // Método para leer el CSV de Logotipos y devolver el diccionario
    public static Dictionary<string, string> ObtenerLogotipos(string rutaArchivo)
    {
        var logotiposDiccionario = new Dictionary<string, string>();

        using (var reader = new StreamReader(rutaArchivo))
        using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var registros = csv.GetRecords<LogotipoDTO>().ToList();
            foreach (var logotipo in registros)
            {
                if (!logotiposDiccionario.ContainsKey(logotipo.ClubNombre))
                {
                    logotiposDiccionario[logotipo.ClubNombre] = logotipo.UrlLogotipo;
                }
            }
        }

        return logotiposDiccionario;
    }

    // Método para leer el CSV de Nombres Alternativos y devolver el diccionario
    public static Dictionary<string, List<string>> ObtenerNombresAlternativos(string rutaArchivo)
    {
        var nombresAlternativosDiccionario = new Dictionary<string, List<string>>();

        using (var reader = new StreamReader(rutaArchivo))
        using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var registros = csv.GetRecords<NombresAlternativosDTO>().ToList();
            foreach (var nombresAlternativos in registros)
            {
                if (!nombresAlternativosDiccionario.ContainsKey(nombresAlternativos.ClubNombre))
                {
                    nombresAlternativosDiccionario[nombresAlternativos.ClubNombre] = nombresAlternativos.NombresAlternativos;
                }
            }
        }

        return nombresAlternativosDiccionario;
    }

    // Método para leer el CSV de Premios y devolver el diccionario
    public static Dictionary<string, List<string>> ObtenerPremios(string rutaArchivo)
    {
        var premiosDiccionario = new Dictionary<string, List<string>>();

        using (var reader = new StreamReader(rutaArchivo))
        using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var registros = csv.GetRecords<PremiosDTO>().ToList();
            foreach (var premios in registros)
            {
                if (!premiosDiccionario.ContainsKey(premios.ClubNombre))
                {
                    premiosDiccionario[premios.ClubNombre] = premios.Premios;
                }
            }
        }

        return premiosDiccionario;
    }

    // Método para leer el CSV de Wikipedia y devolver el diccionario
    public static Dictionary<string, string> ObtenerWikipedia(string rutaArchivo)
    {
        var wikipediaDiccionario = new Dictionary<string, string>();

        using (var reader = new StreamReader(rutaArchivo))
        using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var registros = csv.GetRecords<WikipediaDTO>().ToList();
            foreach (var wikipedia in registros)
            {
                if (!wikipediaDiccionario.ContainsKey(wikipedia.ClubNombre))
                {
                    wikipediaDiccionario[wikipedia.ClubNombre] = wikipedia.UrlWikipedia;
                }
            }
        }

        return wikipediaDiccionario;
    }
}
