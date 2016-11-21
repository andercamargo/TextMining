using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TextMining.Biblioteca.Classes.Referencia
{
    public class Texto
    {

        public string Digitado { get; set; }

        public void RemoverStopWords(ref string texto)
        {

            var novoTexto = texto;
            novoTexto = novoTexto.ToUpper();
            var stringSeparators = new[] {" ", ".", ",", ";"};
            var palavras = RetornarPalavrasTexto(texto, stringSeparators);
            var lista = StopWord.RetornarStopWords();

            foreach (var item in lista)
            {
                if (palavras[0].Trim().ToUpper().Equals(item.Palavra.Trim().ToUpper()))
                {
                    var posicao = novoTexto.IndexOf(palavras[0].ToUpper(), StringComparison.Ordinal);

                    novoTexto = novoTexto.ToUpper().Remove(posicao, palavras[0].Length);
                    if (novoTexto.Length > 0) novoTexto = novoTexto.Remove(0, 1);
                }
                else if (palavras[palavras.Length - 1].Trim().ToUpper().Equals(item.Palavra.ToString(CultureInfo.InvariantCulture).Trim().ToUpper()))
                {
                    var posicao = novoTexto.ToUpper().IndexOf(palavras[palavras.Length-1].ToUpper(), StringComparison.Ordinal);

                    novoTexto = novoTexto.Remove(posicao, palavras[palavras.Length - 1].Length);
                }

                novoTexto = novoTexto.Replace(" " + item.Palavra.Trim().ToUpper() + " ", " ");
                novoTexto = novoTexto.Replace("-", "");
                novoTexto = novoTexto.Replace("\"","");
                novoTexto = novoTexto.Replace("\'", "");
            }

            novoTexto = RemoverAcento(novoTexto);

            texto = novoTexto;
        }

        public string Soundex(string word)
        {
            word = word.ToUpper();
            word = word[0] +
                   Regex.Replace(
                       Regex.Replace(
                           Regex.Replace(
                               Regex.Replace(
                                   Regex.Replace(
                                       Regex.Replace(
                                           Regex.Replace(word.Substring(1), "[AEIOUYHW]", ""),
                                           "[BFPV]+", "1"),
                                       "[CGJKQSXZ]+", "2"),
                                   "[DT]+", "3"),
                               "[L]+", "4"),
                           "[MN]+", "5"),
                       "[R]+", "6")
                ;
            return word.PadRight(4, '0').Substring(0, 4);
        }

        public static string RemoverAcento(string texto)
        {
            return new string(texto
                .Normalize(NormalizationForm.FormD)
                .Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                .ToArray());
        }

        public static string[] RetornarPalavrasTexto(string texto, string[] separador)
        {
            return texto.ToUpper().Split(separador, StringSplitOptions.None);
        }

        public string QuebraLinha()
        {
            return "\r\n";
        }

        //Calcular Distância entre duas strings usando o algoritmo JaroWinklerDistance
        public float RetornarDistanciaEntrePalavra(string palavra, string outraPalavra, float limite)
        {
            var jaroWinkler = new JaroWinklerDistance {Limite = limite};

            return jaroWinkler.GetDistance(palavra, outraPalavra);
        }
    }
}
