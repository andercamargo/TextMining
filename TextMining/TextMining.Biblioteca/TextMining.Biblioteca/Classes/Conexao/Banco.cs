using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using Telecon.Genericos.Classes.Arquivos;

namespace TextMining.Biblioteca.Classes.Conexao
{
    public class Banco
    {

        protected string Servidor { get; set; }
        protected string Senha { get; set; }
        private int Porta { get; set; }
        protected const string BANCO = "agilis_web";
        protected const string USUARIO = "sa";
        private SqlConnection _conexao = null;
        private const string INICIO_ASPAS = "'\"";
        private const string FIM_ASPAS = "\"'";
        private const int TIMEOUT = 0;
        public const string ASPAS_SIMPLES = "\'";

        public Banco()
        {
        }

        public SqlConnection AbrirConexao()
        {

            try
            {
                ObterConfiguracoes();

                var stringDeConexao = new StringBuilder();

                stringDeConexao.AppendFormat("Server={0};Database={1};User Id={2};Password = {3};", Servidor, BANCO, USUARIO, Senha);

                _conexao = new SqlConnection();
                _conexao.ConnectionString = stringDeConexao.ToString();


                if (_conexao.State != ConnectionState.Open)
                    _conexao.Open();

                return _conexao;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public bool FecharConexao()
        {
            try
            {
                if (_conexao.State != ConnectionState.Closed)
                    _conexao.Close();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string ObterConexao(SqlConnection conexao)
        {
            return conexao.State.ToString();
        }


        protected void ObterConfiguracoes()
        {
            var codeBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) + "//Config.ini";
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);

            var caminho = Path.GetDirectoryName(path) + "\\Config.ini";

            var ini = new ConfigIni(caminho);

            Servidor = ini.ObterConfiguracao("SERVIDOR", "...");
            Senha = ini.ObterConfiguracao("SENHA", "...");

        }

        public int ExecutarComando(string comando, int timeout)
        {

            try
            {
                var cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = comando;
                cmd.Connection = _conexao;
                cmd.CommandTimeout = timeout;

                var numero = cmd.ExecuteNonQuery();

                return numero;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #region Manipulação dos Dados

        public static bool Alterar(Banco banco, string comando)
        {
            try
            {
                banco.AbrirConexao();
                var valor = false;
                var linhasAfetadas = banco.ExecutarComando(comando, TIMEOUT);
                if (linhasAfetadas > 0)
                    valor = true;

                return valor;
            }
            finally
            {
                if (banco != null) banco.FecharConexao();
            }
        }


        public static bool Excluir(Banco banco, string comando)
        {
            try
            {
                banco.AbrirConexao();
                var valor = false;
                var linhasAfetadas = banco.ExecutarComando(comando, TIMEOUT);
                if (linhasAfetadas > 0)
                    valor = true;

                return valor;
            }
            finally
            {
                if (banco != null) banco.FecharConexao();
            }
        }

        public static bool Inserir(Banco banco, string comando)
        {
            try
            {
                banco.AbrirConexao();
                var valor = false;
                var linhasAfetadas = banco.ExecutarComando(comando, TIMEOUT);
                if (linhasAfetadas > 0)
                    valor = true;

                return valor;
            }
            finally
            {
                if (banco != null) banco.FecharConexao();
            }
        }

        #endregion

        #region Consulta de Dados

        public SqlDataReader Consultar(string consulta, int timeout)
        {
            var comando = new SqlCommand();

            comando.Connection = _conexao;

            comando.CommandText = consulta;
            comando.CommandTimeout = timeout;

            var dr = comando.ExecuteReader();

            return dr;
        }

        public SqlDataReader ObterProximoCodigo(string chave, string tabela)
        {
            var consulta = string.Format("SELECT ISNULL(MAX({0}),0)+1 As Campo FROM {1}", chave, tabela);
            return Consultar(consulta, 0);
        }

        #endregion


        public double ConverterDoubleNull(object value)
        {
            return value == null || value.ToString() == "" ? 0 : ((IConvertible)value).ToDouble(null);
        }

        public int ConverterIntNull(object value)
        {
            return value == null || value.ToString() == "" ? 0 : ((IConvertible)value).ToInt32(null);
        }

        public DateTime ConverterDataNull(object value)
        {
            return value == null || value.ToString() == "" ? DateTime.Now : ((IConvertible)value).ToDateTime(null);
        }

        public string ConverterTextoNull(object value)
        {
            return value == null ? "" : ((IConvertible)value).ToString(null);
        }

        public string TratarTextoColocandoAspas(string texto)
        {
            return INICIO_ASPAS + texto + FIM_ASPAS;
        }

        public string TratarTexto(string texto)
        {
            return ASPAS_SIMPLES + texto.Replace("\'", "`") + ASPAS_SIMPLES;
        }

        public bool RecuperarBooelan(object value)
        {
            return (string)value == "0" ? false : ((IConvertible)value).ToBoolean(null);
        }

        public decimal RecuperarDecimal(object value)
        {
            return (string)value == "0" ? 0 : ((IConvertible)value).ToDecimal(null);
        }

        public string ObterInteiro(int? valor)
        {
            return !valor.HasValue ? "NULL" : valor.ToString();
        }

        public string ObterTexto(string valor, int p1)
        {
            return string.IsNullOrEmpty(valor) ? "NULL" : valor;
        }

        public int ObterProcuraFuro(Banco banco, string tabela, string campo)
        {
            var comando = string.Format("SELECT MAX({0}) as campo FROM {1}", campo, tabela);
            var dr = banco.Consultar(comando, TIMEOUT);
            var valor = 1;

            if (dr.Read())
            {
                valor = Convert.ToInt32(dr["campo"]);
            }

            return valor;
        }

        public string ObterDuplo(double? codComponente)
        {
            return codComponente.ToString();
        }

        public string ObterVerdadeiroFalso(bool? valor)
        {
            return (bool)valor ? "1" : "0";
        }

        internal string ObterDataHora(DateTime? dateTime)
        {
            var aux = "";

            if (dateTime.HasValue == false)
                return "NULL";
            var sb = new StringBuilder();

            sb.AppendFormat("'{0:yyyy-MM-ddTHH:mm:ss.001}'", dateTime);

            aux = sb.ToString();
            aux = "Convert(DateTime, " + aux + ", 126)";

            return aux;
        }

        internal bool? ConverterBoolNull(string p)
        {
            return p == "1";
        }
    }
}
