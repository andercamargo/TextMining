using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TextMining.Biblioteca.Classes.Conexao;
using TextMining.Biblioteca.Classes.Referencia;

namespace Dicionario.Formularios
{
    public partial class FrmPrincipal : Form
    {

        const int TIMEOUT = 0;
        const string textoPalavraNova = "Foram Encontradas ";
        const string complementoPalavraNova = " Novas Palavras";
        const string textoAtualizacao = "Palavras Inseridas ";


        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void FrmPrinicipal_Load(object sender, EventArgs e)
        {
            var banco = new Banco();

            try
            {
                banco.AbrirConexao();

                var valor = false;

                StopWord.CriarTabela(banco);

                if (StopWord.TabelaEstaVazia())
                {
                    valor = true;
                }

                btnCargaInicial.Visible = valor;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro " + ex.Message);
            }
            finally {
                banco.FecharConexao();
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnCargaInicial_Click(object sender, EventArgs e)
        {
            var banco = new Banco();

            try
            {
                banco.AbrirConexao();

                var consulta = new StringBuilder();


                consulta.AppendLine("SELECT count(stopword) as qtd");
                consulta.AppendLine("FROM sys.fulltext_system_stopwords");
                consulta.AppendLine("WHERE  language_id = 2070");

                SqlDataReader dr = null;
                dr = banco.Consultar(consulta.ToString(), TIMEOUT);
                var id = 1;

                if (dr.Read())
                {
                    ExibirLabelAtualizacao(true);
                    lblPalavrasEncontradas.Text = textoPalavraNova + dr["qtd"] + complementoPalavraNova;
                    Application.DoEvents();
                    dr.Close();
                    dr.Dispose();
                }

                consulta = new StringBuilder();
                dr = null;

                consulta.AppendLine("SELECT stopword");
                consulta.AppendLine("FROM sys.fulltext_system_stopwords");
                consulta.AppendLine("WHERE  language_id = 2070");
                consulta.AppendLine("order by stopword");

                dr = banco.Consultar(consulta.ToString(), TIMEOUT);

                while (dr.Read())
                {
                    var stopWord = new StopWord(id, dr["stopword"].ToString());

                    if (StopWord.Inserir(stopWord.Palavra))
                    {
                        lblPalavrasInseridas.Text = textoAtualizacao + id.ToString();
                        Application.DoEvents();
                        id++;
                    }
                }

                lblPalavrasEncontradas.Visible = false;
                btnCargaInicial.Visible = false;

                dr.Close();
                dr.Dispose();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                banco.FecharConexao();
            }
        }


        private void ExibirLabelAtualizacao(bool valor)
        {
            lblPalavrasEncontradas.Visible = valor;
            lblPalavrasInseridas.Visible = valor;
        }

        private void btnTestarMetodo_Click(object sender, EventArgs e)
        {
            var frmTeste = new FrmTeste();

            frmTeste.ShowDialog();
        }

        private void btnLerArquivo_Click(object sender, EventArgs e)
        {
            FormatarComponenteOpenFileDialog();

            try
            {

                foreach (var arquivo in openFileDialog.FileNames)
                {
                    if (!File.Exists(arquivo)) return;

                    DefinirSomenteLeituraComponentes(false);

                    var i = 0;

                    var re = new Regex("\r\n");
                    var fileReader = new StreamReader(arquivo);
                    var stringReader = fileReader.ReadToEnd();
                    fileReader.Close();
                    fileReader.Dispose();
                    var totalLinhas = re.Matches(stringReader).Count + 1;

                    ExibirLabelAtualizacao(true);
                    lblPalavrasEncontradas.Text = textoPalavraNova + totalLinhas + complementoPalavraNova;
                    lblPalavrasEncontradas.Refresh();

                    using (var sr = new StreamReader(arquivo, Encoding.UTF8))
                    {
                        var linha = "";
                        
                        while ((linha = sr.ReadLine()) != null)
                        {
                            if (!StopWord.Inserir(linha.Trim())) continue;
                            i++;
                            lblPalavrasInseridas.Text = textoAtualizacao + i;
                            lblPalavrasInseridas.Refresh();

                        }
                        if (i == 0) lblPalavrasInseridas.Visible = false;
                        lblPalavrasEncontradas.Visible = false;
                    }
                }


                
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Erro ao ler aquivo de texto." + ex.Message);
            }

            DefinirSomenteLeituraComponentes(true);
        }

        private void FormatarComponenteOpenFileDialog()
        {

            var caminhoAplicacao = Environment.CurrentDirectory;

            openFileDialog.InitialDirectory = caminhoAplicacao;
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Arquivos Texto (*.txt)| *.txt";
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.DefaultExt = "txt";
            openFileDialog.FilterIndex = 0;
            openFileDialog.ReadOnlyChecked = true;
            openFileDialog.ShowReadOnly = true;

            openFileDialog.ShowDialog();
        }

        private void DefinirSomenteLeituraComponentes(bool valor)
        {
            btnCargaInicial.Enabled = valor;
            btnLerArquivo.Enabled = valor;
            btnFechar.Enabled = valor;
            btnTestarMetodo.Enabled = valor;
        }
    }
}
