using System;
using System.Data;
using System.Text;
using TextMining.Biblioteca.Classes.Conexao;

namespace TextMining.Biblioteca.Classes.Persistencia
{
    //Esta classe não será sobreescrita pelo Telecode
    public class TextMining
    {
        #region Propriedades

        public double Codigo { get; set; }
        public double CodTarefa { get; set; }
        public DateTime? DataIndentificacao { get; set; }
        public string Descricao { get; set; }
        public string Tarefas { get; set; }
        public bool? TarefasFinalizadas { get; set; }
        public bool? Tipo { get; set; }

        public const string COLUNA_CODIGO = "Codigo";
        public const string COLUNA_COD_TAREFA = "CodTarefa";
        public const string COLUNA_DATA_INDENTIFICACAO = "DataIndentificacao";
        public const string COLUNA_DESCRICAO = "Descricao";
        public const string COLUNA_TAREFAS = "Tarefas";
        public const string COLUNA_TAREFAS_FINALIZADAS = "TarefasFinalizadas";
        public const string COLUNA_TIPO = "Tipo";

        #endregion

        #region Consulta de Dados
        public static double ObterProximoCodigo()
        {
            var banco = new Banco();
            try
            {
                banco.AbrirConexao();
                double valor = 1;
                var dr = banco.ObterProximoCodigo(COLUNA_CODIGO, "TextMining");
                if (dr.Read())
                {
                    double.TryParse(dr[0].ToString(), out valor);
                }
                dr.Close();
                dr.Dispose();
                return valor;
            }
            finally
            {
                banco.FecharConexao();
            }

        }

        public static TextMining Consultar(double codTextMining)
        {
            var banco = new Banco();

            try
            {
                var textMining = new TextMining();
                var consulta = new StringBuilder();

                consulta.AppendLine("SELECT TOP 1");
                consulta.AppendFormat("{0},{1}, {2}, {3}, {4}, {5}, {6}\n", COLUNA_CODIGO,
                    COLUNA_COD_TAREFA, COLUNA_DATA_INDENTIFICACAO, COLUNA_DESCRICAO, COLUNA_TAREFAS,
                    COLUNA_TAREFAS_FINALIZADAS, COLUNA_TIPO);
                consulta.AppendLine("FROM TextMining");
                consulta.AppendFormat("WHERE {0} = {1}\n", COLUNA_CODIGO, codTextMining);
                consulta.AppendFormat("ORDER BY {0} DESC", COLUNA_CODIGO);

                banco.AbrirConexao();
                var dr = banco.Consultar(consulta.ToString(), 0);


                if (dr.Read())
                {
                    textMining.Codigo = banco.ConverterIntNull(dr[COLUNA_CODIGO]);
                    textMining.CodTarefa = banco.ConverterDoubleNull(dr[COLUNA_COD_TAREFA]);
                    textMining.Descricao = dr[COLUNA_DESCRICAO].ToString();
                    textMining.DataIndentificacao = Convert.ToDateTime(dr[COLUNA_DATA_INDENTIFICACAO].ToString());
                    textMining.Tarefas = dr[COLUNA_TAREFAS].ToString();
                    textMining.Tipo = banco.RecuperarBooelan(dr[COLUNA_TIPO].ToString());
                    textMining.TarefasFinalizadas = banco.RecuperarBooelan(dr[COLUNA_TAREFAS_FINALIZADAS].ToString());
                }

                dr.Dispose();
                dr.Close();

                return textMining;
            }
            finally
            {
                banco.FecharConexao();
            }
        }

        #endregion

        #region Manipulação de Dados
        public bool Inserir()
        {
            var banco = new Banco();

            try
            {
                banco.AbrirConexao();

                var comando = new StringBuilder();
                comando.AppendFormat("INSERT INTO TextMining ({0},{1}, {2}, {3}, {4}, {5}, {6})\n", COLUNA_CODIGO,
                    COLUNA_COD_TAREFA, COLUNA_DATA_INDENTIFICACAO, COLUNA_DESCRICAO, COLUNA_TAREFAS,
                    COLUNA_TAREFAS_FINALIZADAS, COLUNA_TIPO);
                comando.AppendFormat("VALUES ({0},{1},{2},{3}, {4},{5}, {6})", Codigo, CodTarefa,
                   banco.ObterDataHora(DataIndentificacao), banco.TratarTexto(Descricao),
                   banco.TratarTexto(Tarefas), banco.ObterVerdadeiroFalso(TarefasFinalizadas),
                   banco.ObterVerdadeiroFalso(Tipo));
                var retorno = Banco.Inserir(banco, comando.ToString());

                return retorno;
            }
            finally
            {
                banco.FecharConexao();
            }
        }
        #endregion
    }
}
