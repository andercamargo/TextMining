using System.Collections.Generic;
using System.Text;
using TextMining.Biblioteca.Classes.Conexao;

namespace TextMining.Biblioteca.Classes.Pessoa
{
	//Esta classe não será sobreescrita pelo Telecode
    public partial class Membro
    {
		public static bool TestarCampos(Banco banco, Membro membro)
        {            
            return true;
        }

        public static List<Membro> ConsultarAnalistas()
        {
            var banco = new Banco();
            try
            {
                banco.AbrirConexao();
                var consulta = new StringBuilder();
                consulta.Append("WHERE Analista = '1'");
                return ConsultarSQL(banco, consulta.ToString());
            }
            finally
            {
                banco.FecharConexao();
            }

        }

        public static decimal RetornarPercentualDuplicidadeConfigurado(int codMembro)
        {
            var banco = new Banco();

            try
            {
                decimal valor = 0;

                banco.AbrirConexao();
                var membro = ConsultarChave(banco, codMembro);

                if (membro != null)
                {
                   valor = membro.PercentualDuplicidade;     
                }

                return valor;

            }
            finally {
                banco.FecharConexao();
            }
        }

    }
}
