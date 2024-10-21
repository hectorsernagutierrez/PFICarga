using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace FutbolOntology.DTO
{



    // Definición del DTO para el archivo CSV
    public class PlayerUriDTO
    {
        public string PlayerName { get; set; }
        public string PlayerUri { get; set; }
    }

    // Mapeo de CSV a DTO para PlayerUriDTO
    public sealed class PlayerUriDTOMap : ClassMap<PlayerUriDTO>
    {
        public PlayerUriDTOMap()
        {
            // Mapeo de columnas del CSV a propiedades del DTO
            Map(m => m.PlayerName).Name("KeyValuePair`2.Key");
            Map(m => m.PlayerUri).Name("KeyValuePair`2.Value");
        }
    }

    
}