using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using TextMining.Biblioteca.Classes.Logica;

namespace TextMining
{
    public partial class frmTeste : System.Web.UI.Page
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


        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}