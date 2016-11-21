using System;
using System.Collections.Generic;
using System.Data;
using TextMining.Biblioteca.Classes.Conexao;

namespace TextMining.Biblioteca.Classes.Pessoa
{
	//Esta classe será sobreescrita pelo Telecode
	[Serializable]
    public partial class Membro
    {
		#region Propriedades
		
        public int CodMembro { get; set; }
        public bool Administrador { get; set; }
        public bool Ativo { get; set; }
        public bool Bloqueado { get; set; }
        public bool Desenvolvedor { get; set; }
        public bool DestacarNoCronograma { get; set; }
        public string Email { get; set; }
        public bool MonitorarComoProgramador { get; set; }
        public string Nome { get; set; }
        public bool ReceberFeedbackCompilacoes { get; set; }
        public bool ReceberFeedbackRelator { get; set; }
        public string Senha { get; set; }
        public bool Tester { get; set; }
        public decimal PercentualDuplicidade { get; set; }
        public decimal PercentualSimilaridade { get; set; }

        public const string COLUNA_COD_MEMBRO = "CodMembro";
        public const string COLUNA_ADMINISTRADOR = "Administrador";
        public const string COLUNA_ATIVO = "Ativo";
        public const string COLUNA_BLOQUEADO = "Bloqueado";
        public const string COLUNA_DESENVOLVEDOR = "Desenvolvedor";
        public const string COLUNA_DESTACAR_NO_CRONOGRAMA = "DestacarNoCronograma";
        public const string COLUNA_EMAIL = "Email";
        public const string COLUNA_MONITORAR_COMO_PROGRAMADOR = "MonitorarComoProgramador";
        public const string COLUNA_NOME = "Nome";
        public const string COLUNA_RECEBER_FEEDBACK_COMPILACOES = "ReceberFeedbackCompilacoes";
        public const string COLUNA_RECEBER_FEEDBACK_RELATOR = "ReceberFeedbackRelator";
        public const string COLUNA_SENHA = "Senha";
        public const string COLUNA_TESTER = "Tester";
        public const string COLUNA_PERCENTUAL_DUPLICIDADE = "PercentualDuplicidade";
        public const string COLUNA_PERCENTUAL_SIMILARIDADE = "PercentualSimilaridade";

        #endregion

        #region Consultas ao Banco de Dados

        private static List<Membro> ConsultarSQL(Banco banco, string complementoSelect)
		{
			return ConsultarSQL(banco, complementoSelect, "");
		}
		
		private static List<Membro> ConsultarSQL(Banco banco, string complementoSelect, string prefixoAposSelect)
        {            				
				var lista = new List<Membro>();

                var sql = " Select " + prefixoAposSelect + " ";				
				sql += "\n";
                sql += "CodMembro," + "\n";
                sql += "Administrador," + "\n";
                sql += "Ativo," + "\n";
                sql += "Bloqueado," + "\n";
                sql += "Desenvolvedor," + "\n";
                sql += "DestacarNoCronograma," + "\n";
                sql += "Email," + "\n";
                sql += "MonitorarComoProgramador," + "\n";
                sql += "Nome," + "\n";
                sql += "ReceberFeedbackCompilacoes," + "\n";
                sql += "ReceberFeedbackRelator," + "\n";
                sql += "Senha," + "\n";
                sql += "Tester" + "\n";
                sql += " From Membros" + "\n";
                sql += " " + complementoSelect;

				var dr = banco.Consultar(sql, 0);

                while (dr.Read())
                {
                    var membro = ConverterDataReader(banco, dr);
                    lista.Add(membro);
                }
                dr.Close();
                dr.Dispose();				
				
                return lista;            
        }
		
		public static Membro ConverterDataReader(Banco banco, IDataReader dr)
		{
			var membro = new Membro();

                    membro.CodMembro = Convert.ToInt32(dr["CodMembro"].ToString());
                    membro.Administrador = banco.RecuperarBooelan(dr["Administrador"].ToString());
                    membro.Ativo = banco.RecuperarBooelan(dr["Ativo"].ToString());
                    membro.Bloqueado = banco.RecuperarBooelan(dr["Bloqueado"].ToString());
                    membro.Desenvolvedor = banco.RecuperarBooelan(dr["Desenvolvedor"].ToString());
                    membro.DestacarNoCronograma = banco.RecuperarBooelan(dr["DestacarNoCronograma"].ToString());
                    membro.Email = dr["Email"].ToString();
                    membro.MonitorarComoProgramador = banco.RecuperarBooelan(dr["MonitorarComoProgramador"].ToString());
                    membro.Nome = dr["Nome"].ToString();
                    membro.ReceberFeedbackCompilacoes = banco.RecuperarBooelan(dr["ReceberFeedbackCompilacoes"].ToString());
                    membro.ReceberFeedbackRelator = banco.RecuperarBooelan(dr["ReceberFeedbackRelator"].ToString());
                    membro.Senha = dr["Senha"].ToString();
                    membro.Tester = banco.RecuperarBooelan(dr["Tester"].ToString());
            return membro;
		}
		
		public static Membro ConsultarChave(Banco banco, int codMembro)
        {
            var lista = ConsultarSQL(banco, " Where CodMembro = " + banco.ConverterIntNull(codMembro));
			
			if (lista.Count == 0)
				return null;            
			
			return lista[0];
        }											
		
		#endregion
				 
    }
}
