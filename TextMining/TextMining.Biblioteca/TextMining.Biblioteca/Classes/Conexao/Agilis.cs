using Telecon.Genericos.Classes.BancoDeDados;

namespace TextMining.Biblioteca.Classes.Conexao
{
    class Agilis : Banco
    {
        private Sql2000 _banco;

        public Sql2000 ObterBanco()
        {
            ObterConfiguracoes();

            _banco = new Sql2000(Servidor, BANCO, USUARIO, Senha);
            return  _banco;
        }

        public void AbrirConexao()
        {
            _banco.AbrirConexao();
        }

        public void FecharConexao()
        {
            _banco.FecharConexao();
        }
    }
}
