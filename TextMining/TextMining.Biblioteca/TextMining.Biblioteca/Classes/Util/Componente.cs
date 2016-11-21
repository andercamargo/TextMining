using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextMining.Biblioteca.Classes.Conexao;

namespace TextMining.Biblioteca.Classes.Util
{
    public class Componente
    {
        private static IEnumerable<int> RetornarComponenteFilho(int codigoComponente)
        {
            var banco = new Banco();
            var lista = new List<int>();

            try
            {
                var consulta = new StringBuilder();

                banco.AbrirConexao();

                consulta.AppendLine("WITH filhos AS");
                consulta.AppendLine("(");
                consulta.AppendFormat("SELECT * FROM Componentes WHERE CodComponentePai = {0}\n", codigoComponente);
                consulta.AppendLine("UNION ALL");
                consulta.AppendLine(
                    "SELECT Componentes.* FROM Componentes JOIN filhos ON Componentes.CodComponentePai = filhos.CodComponente)");
                consulta.AppendFormat("SELECT CodComponente, CodComponentePai FROM Componentes WHERE CodComponente = {0}\n",
                    codigoComponente);
                consulta.AppendLine("union");
                consulta.AppendLine("SELECT CodComponente, CodComponentePai  FROM filhos");
                consulta.AppendLine("OPTION(MAXRECURSION 32767)");

                var dr = banco.Consultar(consulta.ToString(), 0);

                while (dr.Read())
                {
                    int numero;

                    if (!int.TryParse(dr["CodComponente"].ToString(), out numero)) continue;
                    if (!lista.Contains(numero)) lista.Add(numero);

                    if (!int.TryParse(dr["CodComponentePai"].ToString(), out numero)) continue;
                    if (!lista.Contains(numero)) lista.Add(numero);
                }

                dr.Close();
                dr.Dispose();

                return lista;

            }
            finally
            {
                banco.FecharConexao();
            }
        }

        public static string RetornarCodigosComponentesPertencente(int codigoComponente)
        {
            var lista = RetornarComponenteFilho(codigoComponente);

            var codigo = lista.Aggregate("", (current, item) => current + (item + ", "));

            codigo = codigo.Substring(0, codigo.Length - 2);

            return codigo;
        }
    }
}
