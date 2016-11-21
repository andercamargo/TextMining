using System;
using System.Web.Services;
using System.Web.UI;
using TextMining.Biblioteca.Classes.Logica;

namespace TextMining
{
    public partial class frmTextMining : Page
    {
        private string retorno;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGravar_Click(object sender, EventArgs e)
        {
            var controle = new Controle();
            controle.TextoDigitado = textoDigitado.Text;
            controle.CodComponente = Convert.ToInt32(codComponente.Text);
            controle.CodTarefa = Convert.ToDouble(codTarefa.Text);
            var retorno = controle.AnalisarTarefa();
        }

        [WebMethod]
        public static Controle AnalisarTarefa(string textoDigitado, int codComponente, double codTarefa)
        {
            var controle = new Controle();
            controle.TextoDigitado = textoDigitado;
            controle.CodComponente = codComponente;
            controle.CodTarefa = codTarefa;
            controle.AnalisarTarefa();

            return controle;
        }

        [WebMethod]
        public static bool AnalisarResposta(string resposta, bool duplicada, bool finalizada, double codTarefa, int codRelator, double codTextMining)
        {
            var controle = new Controle();
            controle.Resposta = resposta;
            controle.TarefaFinalizada = finalizada;
            controle.TarefaSimilar = !duplicada;
            controle.CodTarefa = codTarefa;
            controle.CodRelator = codRelator;
            controle.CodTextMining = codTextMining;

            return controle.AnalisarResposta();
        }

        [WebMethod]
        public static bool AtividadeMonitorada(double codTarefa)
        {
            var controle = new Controle();
            return controle.AtividadeMonitorada(codTarefa);
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnGravar_Click1(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "Confirm();", true);
        }

        protected void btnGravar1_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "Confirm();", true);
        }

    }
}