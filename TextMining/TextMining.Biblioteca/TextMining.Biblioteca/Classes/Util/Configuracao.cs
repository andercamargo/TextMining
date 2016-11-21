using System;
using System.Globalization;
using System.Text;
using TextMining.Biblioteca.Classes.Conexao;

namespace TextMining.Biblioteca.Classes.Util
{
	//Esta classe não será sobreescrita pelo Telecode
    public partial class Configuracao
    {
        public const string VALOR_PERCENTUAL_DUPLICIDADE = "PercentualDuplicidade";
        public const string VALOR_PERCENTUAL_SIMILARIDADE = "PercentualSimilaridade";
        public const string VALOR_ATIVIDADES_MONITORADAS = "AtividadesMonitoradas";
        public const string VALOR_PERCENTUAL_SIMILARIDADE_PALAVRA = "PercentualSimilaridadePalavra";
        public const string VALOR_IMPORTANCIA_CORRECAO = "ImportanciaCorrecao";


        public static bool TestarCampos(Banco banco, Configuracao configuracao)
        {            
            return true;
        }
		
		public static bool Inserir(Banco banco, Configuracao configuracao)
        {            				
			return Inserir(banco, configuracao,false);
        }

		public static bool Inserir(Banco banco, Configuracao configuracao,bool atribuirColunaIdentidade)
        {            				
			return InserirSQL(banco, configuracao,atribuirColunaIdentidade);
        }
		
		public static bool Alterar(Banco banco, Configuracao configuracao)
        {            
			return AlterarSQL(banco, configuracao);
        }
		
        public static bool Excluir(Banco banco, Configuracao configuracao)
		{
			return ExcluirSQL(banco, configuracao);
		}								
		
		public static bool Gravar(Banco banco, Configuracao configuracao)
        {
            return GravarSQL(banco, configuracao);
        }
		
		public static int ObterProximoCodigo(Banco banco)
        {
            return (int)banco.ObterProcuraFuro(banco, "Configuracoes", "Codigo");
        }

        public static Configuracao RetornarAtividadesMonitoradas()
        {
            var banco = new Banco();
            try
            {
                banco.AbrirConexao();

                var condicao = string.Format("WHERE Descricao = '{0}'", VALOR_ATIVIDADES_MONITORADAS);
                var configuracao = ConsultarSQL(banco, condicao);

                return configuracao[0];
            }
            finally
            {
                banco.FecharConexao();
            }
        }


        public static bool AtividadeMonitorada(double codigoAtividade)
        {
            return RetornarAtividadesMonitoradas().Valor.Contains(codigoAtividade.ToString());
        }

        public static decimal RetornarImportanciaCorrecao()
        {
            var banco = new Banco();
            try
            {
                banco.AbrirConexao();

                var condicao = string.Format("WHERE Descricao = '{0}'", VALOR_IMPORTANCIA_CORRECAO);
                var configuracao = ConsultarSQL(banco, condicao);

                var importancia = configuracao[0].Valor.Replace('.', ',');

                return decimal.Parse(importancia, new NumberFormatInfo() { NumberDecimalSeparator = "," });
            }
            finally
            {
                banco.FecharConexao();
            }
        }


        public static decimal RetornarPercentualConfigurado(int percentual)
        {
            var banco = new Banco();

            try
            {
                decimal valor = 0;
                var condicao = new StringBuilder();
                string valorPercentual;

                switch (percentual)
                {
                    case 0:
                        valorPercentual = VALOR_PERCENTUAL_DUPLICIDADE;
                        break;
                    case 1:
                        valorPercentual = VALOR_PERCENTUAL_SIMILARIDADE;
                        break;
                    default:
                        valorPercentual = VALOR_PERCENTUAL_SIMILARIDADE_PALAVRA;
                        break;

                }

                condicao.AppendFormat("WHERE Descricao = '{0}'", valorPercentual);

                banco.AbrirConexao();

                var configuracao = ConsultarSQL(banco, condicao.ToString());

                if (configuracao != null)
                {
                    valor = Convert.ToDecimal(configuracao[0].Valor);
                }

                return valor;

            }
            finally
            {
                banco.FecharConexao();
            }
        }
    }
}
