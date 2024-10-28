using FuzzySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutbolOntology.CargaPFI
{

    public class DiccionarioSimilar<T>
    {
		/// <summary>
		///   Método genérico para obtener el valor exacto o similar de una clave en un diccionario por mayor aproximación.
		/// </summary>
		/// <param name="diccionario"></param>
		/// <param name="clave"></param>
		/// <returns></returns>
		public static T ObtenerIgualOSimilar(Dictionary<string, T> diccionario, string clave)
        {
            // Si hay coincidencia exacta
            if (diccionario.ContainsKey(clave))
            {
                return diccionario[clave];
            }

            // Usar FuzzySharp para encontrar la clave más similar
            var resultado = Process.ExtractOne(clave, diccionario.Keys);

            // Si se encuentra una clave similar, devolver el valor
            if (resultado != null && resultado.Value != null)
            {
                return diccionario[resultado.Value];
            }

            // Si no se encuentra ninguna clave similar, devolver el valor por defecto de T
            return default;
        }
    }
}