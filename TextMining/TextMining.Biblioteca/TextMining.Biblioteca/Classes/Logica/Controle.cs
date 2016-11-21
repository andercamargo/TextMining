using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using agilis_web.biblioteca.Classes;
using TextMining.Biblioteca.Classes.Conexao;
using TextMining.Biblioteca.Classes.Persistencia;
using Configuracao = TextMining.Biblioteca.Classes.Util.Configuracao;
using Membro = TextMining.Biblioteca.Classes.Pessoa.Membro;
using Tarefa = TextMining.Biblioteca.Classes.Util.Tarefa;

namespace TextMining.Biblioteca.Classes.Logica
{
    public class Controle
    {
        #region Variáveis

        public string TextoDigitado { get; set; }
        public int CodComponente { get; set; }
        public double CodTarefa { get; set; }
        public bool TarefaFinalizada { get; set; }
        public bool TarefaSimilar { get; set; }

        private List<Tarefa> _tarefas;

        public string Tarefas;

        public string Texto { get; set; }

        public string Resposta { get; set; }

        public int CodRelator { get; set; }

        public DateTime DataIdentificacao { get; set; }

        private bool _acerto = true;

        private Persistencia.TextMining _textMining;

        public double CodTextMining { get; set; }

        private const int _tentativa = 0;
        private const int MaximoTentativas = 3;

        #endregion

        #region Analises

        public string AnalisarTarefa()
        {
            var i = 0;
            TarefaFinalizada = false;
            TarefaSimilar = false;
            DataIdentificacao = DateTime.Now;
            IniciarAnaliseTextMining();

            while ((_tarefas.Count == 0) && (i < 2))
            {
                TarefaFinalizada = !TarefaFinalizada;
                IniciarAnaliseTextMining();

                if (_tarefas.Count > 0) break;
                if (!TarefaSimilar) TarefaSimilar = true;
                i++;
            }

            if (_tarefas.Count <= 0) return Texto;

            GravarTextMining(0);

            Texto = RetornarTexto();

            return Texto;
        }

        public bool AnalisarResposta()
        {
            ObterTextMining();

            switch (Resposta)
            {
                case "NaoCadastrarTarefa":
                    if (!TarefaSimilar)
                    {
                        ExcluirTarefa(CodTarefa, _tentativa);
                        if (!TarefaFinalizada)
                            AumentarImportancia(_tentativa);
                    }
                    else
                    {
                        MesclarTarefa(_tentativa);
                        ExcluirTarefasSimilares(_tentativa);
                    }
                    break;

                case "AumentarImportancia":
                    AumentarImportancia(_tentativa);
                    ExcluirTarefa(CodTarefa, _tentativa);
                    break;

                case "EnviarEmail":
                    EnviarEmail(_tentativa);
                    if (!TarefaSimilar && TarefaFinalizada) ExcluirTarefa(CodTarefa, _tentativa);
                    break;

                case "CadastrarTarefa":
                    AlterarPassosReproduzir(_tentativa);
                    _acerto = false;
                    break;
            }
            GravarResposta(0);

            return true;
        }

        #endregion

        #region TextMining

        private void IniciarAnaliseTextMining()
        {
            if (!TarefaSimilar) IniciarTextMiningDuplicidade();
            else IniciarTextMiningSimilar();
        }


        private void IniciarTextMiningDuplicidade()
        {
            _tarefas = Tarefa.RetornarTarefasIguais(TextoDigitado, CodComponente, CodTarefa, TarefaFinalizada);
            if (_tarefas.Count > 0) Tarefa.AjustarPercentualDuplicidade(TextoDigitado, ref _tarefas);
        }


        private void IniciarTextMiningSimilar()
        {
            _tarefas = Tarefa.RetornarTarefasSimilares(TextoDigitado, CodComponente, CodTarefa, TarefaFinalizada);
            if (_tarefas.Count > 0) Tarefa.AjustarPercentualSimilaridade(TextoDigitado, ref _tarefas);
        }

        private string RetornarTexto()
        {
            return !TarefaSimilar
                ? Tarefa.RetornarTextoTarefasIguais(TextoDigitado, _tarefas, TarefaFinalizada)
                : Tarefa.RetornarTextoTarefasSimilares(TextoDigitado, _tarefas, TarefaFinalizada);
        }

        #endregion

        #region Consulta de Dados

        public bool AtividadeMonitorada(double codAtividade)
        {
            return Configuracao.AtividadeMonitorada(codAtividade);
        }

        private void ObterTextMining()
        {
            _textMining = new Persistencia.TextMining();
            _textMining = Persistencia.TextMining.Consultar(CodTextMining);
        }

        #endregion

        #region Persistência de Dados

        private void GravarTextMining(int tentativas)
        {
            try
            {
                var textMining = new Persistencia.TextMining();
                var tarefas = new StringBuilder();
                foreach (var item in _tarefas)
                    tarefas.AppendLine(item.CodTarefa + ",");

                Tarefas = tarefas.ToString(0, tarefas.Length - 3);
                CodTextMining = Persistencia.TextMining.ObterProximoCodigo();
                textMining.Codigo = CodTextMining;
                textMining.CodTarefa = CodTarefa;
                textMining.DataIndentificacao = DataIdentificacao;
                textMining.Descricao = TextoDigitado;
                textMining.Tarefas = Tarefas;
                textMining.TarefasFinalizadas = TarefaFinalizada;
                textMining.Tipo = TarefaSimilar;

                textMining.Inserir();
            }
            catch (Exception)
            {
                if (tentativas >= MaximoTentativas) return;
                tentativas++;
                Thread.Sleep(100);
                GravarTextMining(tentativas);
            }
        }

        private void GravarResposta(int tentativas)
        {
            try
            {
                var resposta = new Resposta();

                resposta.Acerto = _acerto;
                resposta.CodTextMining = _textMining.Codigo;
                resposta.CodRelator = CodRelator;

                resposta.Inserir();
            }
            catch (Exception)
            {
                if (tentativas >= MaximoTentativas) return;
                tentativas++;
                Thread.Sleep(100);
                GravarResposta(tentativas);
            }
        }

        #endregion

        #region Lógica

        private void AlterarPassosReproduzir(int tentativas)
        {
            var agilis = new Agilis();
            var banco = agilis.ObterBanco();

            try
            {
                agilis.AbrirConexao();

                banco.BeginTrans();

                var tarefa = agilis_web.biblioteca.Classes.Tarefa.ConsultarChave(banco, CodTarefa);
                var relacionadas = new StringBuilder();
                relacionadas.AppendFormat("\n\nVer tarefa(s) {0}", _textMining.Tarefas);
                tarefa.PassosReproduzir += relacionadas.ToString();
                agilis_web.biblioteca.Classes.Tarefa.Alterar(banco, tarefa);

                banco.CommitTrans();
            }
            catch (Exception)
            {
                banco.RollBackTrans();

                if (tentativas < MaximoTentativas)
                {
                    tentativas++;
                    Thread.Sleep(100);
                    AlterarPassosReproduzir(tentativas);
                }
            }
            finally
            {
                agilis.FecharConexao();
            }
        }

        private void AumentarImportancia(int tentativas)
        {
            var agilis = new Agilis();
            var banco = agilis.ObterBanco();

            try
            {
                var valorImportancia = Configuracao.RetornarImportanciaCorrecao();
                var tarefas = _textMining.Tarefas.Split(',');

                agilis.AbrirConexao();

                banco.BeginTrans();

                foreach (var item in tarefas)
                {
                    if (item.Length <= 0) continue;
                    var tarefa = agilis_web.biblioteca.Classes.Tarefa.ConsultarChave(banco, Convert.ToDouble(item));
                    if (tarefa == null) continue;
                    tarefa.Importancia = valorImportancia;
                    agilis_web.biblioteca.Classes.Tarefa.Alterar(banco, tarefa);
                }

                banco.CommitTrans();
            }
            catch (Exception)
            {
                banco.RollBackTrans();

                if (tentativas >= MaximoTentativas) return;
                tentativas++;
                Thread.Sleep(100);
                AumentarImportancia(tentativas);
            }
            finally
            {
                agilis.FecharConexao();
            }
        }

        private void EnviarEmail(int tentativas)
        {
            var agilis = new Agilis();
            try
            {
                var analistas = Membro.ConsultarAnalistas();

                var assunto = "Identificação de Tarefas - " +
                              (_textMining.Tipo == false ? "Em Duplicidade" : "Similares");

                var banco = agilis.ObterBanco();
                agilis.AbrirConexao();

                foreach (var analista in analistas)
                {
                    var mensagem = new StringBuilder();

                    mensagem.AppendFormat("Prezado (a) {0},<br/><br/>", analista.Nome);
                    mensagem.AppendFormat("Com a Descrição: {0},<br/>", _textMining.Descricao);
                    mensagem.AppendFormat("Foram identificadas as tarefas: <b>{0}</b>, como <b>{1}</b>",
                        _textMining.Tarefas, _textMining.Tipo == false ? "Duplicadas" : "Similares");
                    mensagem.AppendFormat(" e com o estado <b>{0}</b>",
                        _textMining.TarefasFinalizadas == true ? "Finalizado" : "Em Aberto");
                    if (TarefaSimilar || !TarefaFinalizada)
                        mensagem.AppendFormat(", à nova tarefa com o código <b>{0}</b>", _textMining.CodTarefa);
                    mensagem.AppendFormat(".<br/><br/>O que sinaliza um possível <b>retorno da não conformidade</b>.");

                    EmailUtil.Enviar(banco, analista.Email, assunto, mensagem.ToString());
                }
            }
            catch (Exception)
            {
                if (tentativas < MaximoTentativas)
                {
                    tentativas++;
                    Thread.Sleep(100);
                    EnviarEmail(tentativas);
                }
            }
            finally
            {
                agilis.FecharConexao();
            }
        }

        private void ExcluirTarefa(double codTarefa, int tentativas)
        {
            var agilis = new Agilis();
            var banco = agilis.ObterBanco();

            try
            {
                agilis.AbrirConexao();

                banco.BeginTrans();

                var tarefa = agilis_web.biblioteca.Classes.Tarefa.ConsultarChave(banco, codTarefa);
                if (tarefa != null) agilis_web.biblioteca.Classes.Tarefa.Excluir(banco, tarefa);

                banco.CommitTrans();
            }
            catch (Exception)
            {
                banco.RollBackTrans();

                if (tentativas < 3)
                {
                    tentativas++;
                    Thread.Sleep(100);
                    ExcluirTarefa(codTarefa, tentativas);
                }
            }
            finally
            {
                agilis.FecharConexao();
            }
        }

        private void ExcluirTarefasSimilares(int tentativas)
        {
            try
            {
                var tarefas = _textMining.Tarefas.Split(',');
                foreach (var item in tarefas.Where(item => item.Trim().Length > 0))
                    ExcluirTarefa(Convert.ToDouble(item), tentativas);
            }
            catch (Exception)
            {
                if (tentativas < MaximoTentativas)
                {
                    tentativas++;
                    Thread.Sleep(100);
                    ExcluirTarefasSimilares(tentativas);
                }
            }
        }

        private void MesclarTarefa(int tentativas)
        {
            var agilis = new Agilis();
            var banco = agilis.ObterBanco();
            try
            {
                banco.AbrirConexao();

                banco.BeginTrans();

                var descricao = new StringBuilder();
                var tarefas = _textMining.Tarefas.Split(',');

                foreach (var item in tarefas)
                {
                    var tarefa = agilis_web.biblioteca.Classes.Tarefa.ConsultarChave(banco, Convert.ToDouble(item));
                    if (tarefa == null) continue;
                    descricao.AppendFormat("{0}\n", tarefa.Descricao);
                }

                var novaTarefa = agilis_web.biblioteca.Classes.Tarefa.ConsultarChave(banco, _textMining.CodTarefa);

                if (novaTarefa == null) return;
                novaTarefa.Descricao += "\n" + descricao;

                agilis_web.biblioteca.Classes.Tarefa.Alterar(banco, novaTarefa);

                banco.CommitTrans();
            }
            catch (Exception)
            {
                banco.RollBackTrans();

                if (tentativas < MaximoTentativas)
                {
                    tentativas++;
                    Thread.Sleep(100);
                    MesclarTarefa(tentativas);
                }
            }
            finally
            {
                agilis.FecharConexao();
            }
        }

        #endregion
    }
}