using System;
using System.Collections.Generic;
using System.Data;
using TextMining.Biblioteca.Classes.Conexao;

namespace TextMining.Biblioteca.Classes.Util
{
	//Esta classe será sobreescrita pelo Telecode
	[Serializable]
    public partial class Configuracao
    {
		#region Propriedades
		
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }

        public const string COLUNA_CODIGO = "Codigo";
        public const string COLUNA_DESCRICAO = "Descricao";
        public const string COLUNA_VALOR = "Valor";
				

		#endregion
		
		#region Consultas ao Banco de Dados
		
		private static List<Configuracao> ConsultarSQL(Banco banco, string complementoSelect)
		{
			return ConsultarSQL(banco, complementoSelect, "");
		}
		
		private static List<Configuracao> ConsultarSQL(Banco banco, string complementoSelect, string prefixoAposSelect)
        {            				
				var lista = new List<Configuracao>();

                string sql = " Select " + prefixoAposSelect + " ";
                sql += "\n";
                sql += "Codigo," + "\n";
                sql += "Descricao," + "\n";
                sql += "Valor" + "\n";
                sql += " From Configuracoes" + "\n";
                sql += " " + complementoSelect;

				var dr = banco.Consultar(sql, 0);

                while (dr.Read())
                {
                    var configuracao = ConverterDataReader(banco, dr);
                    lista.Add(configuracao);
                }
                dr.Close();
                dr.Dispose();				
				
                return lista;            
        }
		
		public static Configuracao ConverterDataReader(Banco banco, IDataReader dr)
		{
			var configuracao = new Configuracao();

                    configuracao.Codigo = Convert.ToInt32(dr["Codigo"].ToString());
                    configuracao.Descricao = dr["Descricao"].ToString();
                    configuracao.Valor = dr["Valor"].ToString();

			
			return configuracao;
		}
		
		public static Configuracao ConsultarChave(Banco banco, int codigo)
        {            
			var lista = ConsultarSQL(banco, " Where Codigo = " + banco.ObterInteiro(codigo));
			
			if (lista.Count == 0)
				return null;            
			
			return lista[0];
        }											
		
		#endregion
		
		#region Manipulação dos dados

		private static bool InserirSQL(Banco banco, Configuracao configuracao)
        {            
		return InserirSQL(banco, configuracao,false);

        }
		
		private static bool InserirSQL(Banco banco, Configuracao configuracao,bool atribuirColunaIdentidade)
        {            
				if (!TestarCampos(banco, configuracao))
					return false;
				
                string campos = "", valores = "";                

                campos += "Codigo, ";
                valores += banco.ObterInteiro(configuracao.Codigo) + ",";

                campos += "Descricao, ";
                valores += banco.ObterTexto(configuracao.Descricao, 128) + ",";

                campos += "Valor ";
                valores += banco.ObterTexto(configuracao.Valor, 256);

                var sql = "Insert into Configuracoes(" + campos + ") Values(" + valores + ") ";

				var retorno = Banco.Inserir(banco, sql);
                return retorno;
			

        }
		
		private static bool AlterarSQL(Banco banco, Configuracao configuracao)
        {            
			if (!TestarCampos(banco, configuracao))
				return false;
					
			var sql = "Update Configuracoes Set ";
			sql += "Descricao = " + banco.ObterTexto(configuracao.Descricao, 128);
			sql += ", Valor = " + banco.ObterTexto(configuracao.Valor, 256);
			sql += " Where Codigo = " + banco.ObterInteiro(configuracao.Codigo);
			
			var retorno = Banco.Alterar(banco, sql);            

            return retorno;
        }
		
        private static bool ExcluirSQL(Banco banco, Configuracao configuracao)
		{
			var sql = "Delete From Configuracoes ";
			sql += " Where Codigo = " + banco.ObterInteiro(configuracao.Codigo);

            var retorno = Banco.Excluir(banco, sql);

            return retorno;
		}								
		
		private static bool GravarSQL(Banco banco, Configuracao configuracao)
        {
            return AlterarSQL(banco, configuracao) || InserirSQL(banco, configuracao);
        }
		 
		#endregion
		 
    }
}
