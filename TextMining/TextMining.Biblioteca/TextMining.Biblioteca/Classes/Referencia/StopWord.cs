using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using TextMining.Biblioteca.Classes.Conexao;

namespace TextMining.Biblioteca.Classes.Referencia
{
    public class StopWord
    {
        public string Palavra { get; set; }
        public int Id { get; set; }
        const int TIMEOUT = 0;
        const string TABELA = "StopWord";
        const string COLUNA_ID = "Id";
        const string COLUNA_PALAVRA = "Palavra";

        public StopWord(int id, string palavra)
        {
            Id = id;
            Palavra = palavra;
        }

        public StopWord() { }

        public static bool CriarTabela(Banco banco)
        {

            var comando = new StringBuilder();
            comando.AppendFormat("SELECT id FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = '{0}'", TABELA);

            var dr = banco.Consultar(comando.ToString(), TIMEOUT);

            if (!dr.Read())
            {
                dr.Close();
                dr.Dispose();

                comando = new StringBuilder();
                comando.AppendFormat("CREATE TABLE {0} (", TABELA);
                comando.AppendLine("ID BigInt IDENTITY(1,1),");
                comando.AppendLine("Palavra nvarchar(100) NOT NULL UNIQUE,");
                comando.AppendLine("CONSTRAINT PK_ID_StopWord PRIMARY KEY(ID))");

                banco.ExecutarComando(comando.ToString(), TIMEOUT);

                dr.Close();
                dr.Dispose();
            }

            return true;

        }

        public static bool Inserir(string palavra)
        {
            Banco banco = new Banco();

            try
            {
                var comando = new StringBuilder();
                banco.AbrirConexao();

                if (PalavraExiste(palavra)) return false;

                palavra = palavra.Replace("\"", "");
                palavra = palavra.Replace("\'", "");

                comando.AppendFormat("INSERT INTO {0} (Palavra) VALUES ('{1}')", TABELA, palavra);

                return banco.ExecutarComando(comando.ToString(), TIMEOUT) > 0;
            }
            finally {
                if (banco != null) banco.FecharConexao();
            }
        }

        public static bool PalavraExiste(string palavra)
        {
            SqlDataReader dr = null;

            var banco = new Banco();

            try
            {
                var comando = new StringBuilder();
                var retorno = false;
                banco.AbrirConexao();

                palavra = palavra.Replace("\"", "");
                palavra = palavra.Replace("\'", "");

                comando.AppendFormat("Select Palavra From {0} Where UPPER(Palavra) = UPPER('{1}')", TABELA, palavra);

                dr = banco.Consultar(comando.ToString(), TIMEOUT);

                if (dr.Read())
                {
                    retorno = true;
                }

                dr.Close();
                dr.Dispose();

                return retorno;
            }
            finally
            {
                if (banco != null) banco.FecharConexao();
            }

        }

        public static bool TabelaEstaVazia()
        {

            SqlDataReader dr = null;
            var banco = new Banco();

            try
            {
                var comando = new StringBuilder();
                var retorno = false;
                banco.AbrirConexao();

                comando.AppendFormat("select count(*) as qtd from {0}", TABELA);

                dr = banco.Consultar(comando.ToString(), TIMEOUT);

                if (dr.Read())
                {
                    if(Convert.ToDouble(dr["qtd"].ToString())  == 0) retorno = true;
                }
                return retorno;
            }
            finally
            {
                if (banco != null) banco.FecharConexao();

                if (dr != null)
                {
                    dr.Close();
                    dr.Dispose();
                }
            }
        }

        public static List<StopWord> RetornarStopWords()
        {
            var banco = new Banco();

            try
            {
                var lista = new List<StopWord>();
                var consulta = new StringBuilder();

                banco.AbrirConexao();

                consulta.AppendFormat("SELECT {0},{1} from {2}", COLUNA_ID, COLUNA_PALAVRA, TABELA);

                var dr = banco.Consultar(consulta.ToString(), TIMEOUT);

                while (dr.Read())
                {
                    var stopWord = new StopWord();
                    stopWord.Id = Convert.ToInt32(dr[COLUNA_ID].ToString());
                    stopWord.Palavra = dr[COLUNA_PALAVRA].ToString();

                    lista.Add(stopWord);
                }

                dr.Close();
                dr.Dispose();

                return lista;
            }
            finally {
                if (banco != null) banco.FecharConexao();
            }
        }
    }
}
