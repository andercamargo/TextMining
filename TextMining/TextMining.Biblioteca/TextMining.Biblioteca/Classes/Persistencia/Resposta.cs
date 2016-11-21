using System.Text;
using TextMining.Biblioteca.Classes.Conexao;

namespace TextMining.Biblioteca.Classes.Persistencia
{
	//Esta classe não será sobreescrita pelo Telecode
    public class Resposta
    {
        #region Propriedades

        public double Codigo { get; set; }
        public bool? Acerto { get; set; }
        public int? CodRelator { get; set; }
        public double? CodTextMining { get; set; }

        public const string COLUNA_CODIGO = "Codigo";
        public const string COLUNA_ACERTO = "Acerto";
        public const string COLUNA_COD_RELATOR = "CodRelator";
        public const string COLUNA_COD_TEXT_MINING = "CodTextMining";


        #endregion

        #region Manipulação de Dados
        public bool Inserir()
        {
            var banco = new Banco();

            try
            {
                banco.AbrirConexao();

                var comando = new StringBuilder();
                comando.AppendFormat("INSERT INTO Resposta ({0},{1}, {2})\n", COLUNA_ACERTO, COLUNA_COD_RELATOR, COLUNA_COD_TEXT_MINING);
                comando.AppendFormat("VALUES ({0},{1},{2})", banco.ObterVerdadeiroFalso(Acerto), CodRelator, CodTextMining);
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
