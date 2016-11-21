using System;
using System.Collections.Generic;
using System.Data;
using TextMining.Biblioteca.Classes.Conexao;

namespace TextMining.Biblioteca.Classes.Util
{
    //Esta classe será sobreescrita pelo Telecode
    [Serializable]
    public partial class Tarefa
    {
        #region Propriedades

        public double CodTarefa { get; set; }
        public string Cliente { get; set; }
        public int CodAtividade { get; set; }
        public int CodHistoria { get; set; }
        public int CodRelator { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
        public string Descricao { get; set; }
        public decimal EsforcoRestante { get; set; }
        public decimal EsforcoTotal { get; set; }
        public decimal Importancia { get; set; }
        public string PassosReproduzir { get; set; }
        public string Solucao { get; set; }
        public double? CodComponente { get; set; }
        public int? CodSolucionador { get; set; }
        public int? CodSprint { get; set; }
        public double? CodTeste { get; set; }
        public int? CodVersao { get; set; }
        public DateTime? DataAtribuicao { get; set; }
        public DateTime? DataConclusao { get; set; }
        public string Titulo { get; set; }

        public const string COLUNA_COD_TAREFA = "CodTarefa";
        public const string COLUNA_CLIENTE = "Cliente";
        public const string COLUNA_COD_ATIVIDADE = "CodAtividade";
        public const string COLUNA_COD_HISTORIA = "CodHistoria";
        public const string COLUNA_COD_RELATOR = "CodRelator";
        public const string COLUNA_DATA_CADASTRO = "DataCadastro";
        public const string COLUNA_DATA_ULTIMA_ALTERACAO = "DataUltimaAlteracao";
        public const string COLUNA_DESCRICAO = "Descricao";
        public const string COLUNA_ESFORCO_RESTANTE = "EsforcoRestante";
        public const string COLUNA_ESFORCO_TOTAL = "EsforcoTotal";
        public const string COLUNA_IMPORTANCIA = "Importancia";
        public const string COLUNA_PASSOS_REPRODUZIR = "PassosReproduzir";
        public const string COLUNA_SOLUCAO = "Solucao";
        public const string COLUNA_COD_COMPONENTE = "CodComponente";
        public const string COLUNA_COD_SOLUCIONADOR = "CodSolucionador";
        public const string COLUNA_COD_SPRINT = "CodSprint";
        public const string COLUNA_COD_TESTE = "CodTeste";
        public const string COLUNA_COD_VERSAO = "CodVersao";
        public const string COLUNA_DATA_ATRIBUICAO = "DataAtribuicao";
        public const string COLUNA_DATA_CONCLUSAO = "DataConclusao";
        public const string COLUNA_TITULO = "Titulo";


        #endregion

        #region Consultas ao Banco de Dados

        private static List<Tarefa> ConsultarSQL(Banco banco, string complementoSelect)
        {
            return ConsultarSQL(banco, complementoSelect, "");
        }

        private static List<Tarefa> ConsultarSQL(Banco banco, string complementoSelect, string prefixoAposSelect)
        {
            var lista = new List<Tarefa>();

            var sql = " Select " + prefixoAposSelect + " ";
            sql += "\n";
            sql += "CodTarefa," + "\n";
            sql += "Cliente," + "\n";
            sql += "CodAtividade," + "\n";
            sql += "CodHistoria," + "\n";
            sql += "CodRelator," + "\n";
            sql += "DataCadastro," + "\n";
            sql += "DataUltimaAlteracao," + "\n";
            sql += "Descricao," + "\n";
            sql += "EsforcoRestante," + "\n";
            sql += "EsforcoTotal," + "\n";
            sql += "Importancia," + "\n";
            sql += "PassosReproduzir," + "\n";
            sql += "Solucao," + "\n";
            sql += "CodComponente," + "\n";
            sql += "CodSolucionador," + "\n";
            sql += "CodSprint," + "\n";
            sql += "CodTeste," + "\n";
            sql += "CodVersao," + "\n";
            sql += "DataAtribuicao," + "\n";
            sql += "DataConclusao," + "\n";
            sql += "Titulo" + "\n";
            sql += " From Tarefas" + "\n";
            sql += " " + complementoSelect;

            var dr = banco.Consultar(sql, 0);

            while (dr.Read())
            {
                var tarefa = ConverterDataReader(banco, dr);
                lista.Add(tarefa);
            }
            dr.Close();
            dr.Dispose();

            return lista;
        }

        public static Tarefa ConverterDataReader(Banco banco, IDataReader dr)
        {
            var tarefa = new Tarefa
            {
                CodTarefa = Convert.ToDouble(dr["CodTarefa"].ToString()),
                Cliente = dr["Cliente"].ToString(),
                CodAtividade = Convert.ToInt32(dr["CodAtividade"].ToString()),
                CodHistoria = Convert.ToInt32(dr["CodHistoria"].ToString()),
                CodRelator = Convert.ToInt32(dr["CodRelator"].ToString()),
                DataCadastro = Convert.ToDateTime(dr["DataCadastro"].ToString()),
                DataUltimaAlteracao = Convert.ToDateTime(dr["DataUltimaAlteracao"].ToString()),
                Descricao = dr["Descricao"].ToString(),
                EsforcoRestante = Convert.ToDecimal(dr["EsforcoRestante"].ToString()),
                EsforcoTotal = Convert.ToDecimal(dr["EsforcoTotal"].ToString()),
                Importancia = Convert.ToDecimal(dr["Importancia"].ToString()),
                PassosReproduzir = dr["PassosReproduzir"].ToString(),
                Solucao = banco.ConverterTextoNull(dr["Solucao"].ToString()),
                CodComponente = banco.ConverterDoubleNull(dr["CodComponente"].ToString()),
                CodSolucionador = banco.ConverterIntNull(dr["CodSolucionador"].ToString()),
                CodSprint = banco.ConverterIntNull(dr["CodSprint"].ToString()),
                CodTeste = banco.ConverterDoubleNull(dr["CodTeste"].ToString()),
                CodVersao = banco.ConverterIntNull(dr["CodVersao"].ToString()),
                DataAtribuicao = banco.ConverterDataNull(dr["DataAtribuicao"].ToString()),
                DataConclusao = banco.ConverterDataNull(dr["DataConclusao"].ToString()),
                Titulo = banco.ConverterTextoNull(dr["Titulo"].ToString())
            };


            return tarefa;
        }

        public static Tarefa ConsultarChave(Banco banco, double codTarefa)
        {
            var lista = ConsultarSQL(banco, " Where CodTarefa = " + codTarefa);

            if (lista.Count == 0)
                return null;

            return lista[0];
        }

        #endregion
    }
}
