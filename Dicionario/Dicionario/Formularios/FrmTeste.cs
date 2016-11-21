using System;
using System.Windows.Forms;
using TextMining.Biblioteca.Classes.Logica;

namespace Dicionario.Formularios
{
    public partial class FrmTeste : Form
    {
        public FrmTeste()
        {
            InitializeComponent();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTestar_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                var texto = txtTexto.Text;
                var componente = Convert.ToInt32(txtComponente.Text);
                var tarefaFinalizada = chkFinalizadas.Checked;
                var controle = new Controle();
                controle.CodTarefa = 123;
                controle.CodComponente = componente;
                controle.TextoDigitado = texto;

                /*var separadores = new[] { " ", "," };

                var textoDigitado = new Texto();
                var palavras = Texto.RetornarPalavrasTexto(texto, separadores);

                foreach (var item in palavras)
                {
                    var textoCorrigido = new Texto();
                    var itens = textoCorrigido.SugerirCorrecao(item);
                    var mensagem = "";
                    foreach (var palavraCorreta in itens)
                    {
                        var somItem = textoDigitado.Soundex(item);
                        var somPalavraCorreta = textoCorrigido.Soundex(palavraCorreta);

                        var distancia = Texto.LevenshteinDistance(item, palavraCorreta);

                        if (!Equals(somItem, somPalavraCorreta)) continue;
                        if (mensagem.Length == 0) mensagem = string.Format("Você digitou {0}\n", item);
                        mensagem += string.Format("Palavra sugerida {0}\n\n", palavraCorreta);
                        mensagem += string.Format("Distancia Levenshtein {0}\n\n", distancia);
                        mensagem += string.Format("Soundex 1º {0} 2º {1}\n\n", somItem, somPalavraCorreta);

                        var palavraTexto = textoCorrigido.CorrigirPalavra(item);

                        mensagem += string.Format("Texto Sugerida {0}\n\n", palavraTexto);

                    }
                    MessageBox.Show(mensagem);
                }*/

                var textoExibicao = controle.AnalisarTarefa();
                if (!string.IsNullOrEmpty(textoExibicao)) MessageBox.Show(textoExibicao);

                /*var teste2 = Tarefa.RetornarTarefasSimilares(texto, componente, tarefaFinalizada);
                var textoExibicao2 = Tarefa.RetornarTextoTarefasSimilares(texto, teste2, tarefaFinalizada);
                if (textoExibicao2.Length > 0) MessageBox.Show(textoExibicao2);*/

                Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Ocorreu um erro na transação: " + ex.Message + "\nPILHA" + ex.StackTrace);
            }
        }
    }
}
