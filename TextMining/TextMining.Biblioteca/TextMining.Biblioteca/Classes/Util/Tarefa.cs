using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextMining.Biblioteca.Classes.Conexao;
using TextMining.Biblioteca.Classes.Referencia;

namespace TextMining.Biblioteca.Classes.Util
{

    public partial class Tarefa
    {

        private decimal _percentualDuplicidade;
        private decimal _percentualSimiliraridade;
        private const string TABELA = "Tarefas";
        private const int TIMEOUT = 0;
        private static readonly string[] Separadores = { " ", "," };
        private const string INICIO_TAREFA_DUPLICADA = " que você quer cadastrar está duplicada em aproximadamente ";
        private const string INICIO_TAREFA_SIMILAR = " que você quer cadastrar é similar em aproximadamente ";
        private const string TAREFA_FINALIZADA = " já finalizadas: ";
        private const string TAREFA_EM_ABERTO = " em aberto: ";

        #region Tarefas Duplicadas
        public static List<Tarefa> RetornarTarefasIguais(string texto, int codComponente, double codTarefa, bool apenasFinalizadas)
        {
            var banco = new Banco();

            try
            {
                banco.AbrirConexao();

                var lista = new List<Tarefa>();


                if (texto.Trim().Length <= 0) return lista;
                var textoUsuario = new Texto();
                textoUsuario.RemoverStopWords(ref texto);

                var codComponentes = Componente.RetornarCodigosComponentesPertencente(codComponente);

                var codAtividadesCorrecoes = Configuracao.RetornarAtividadesMonitoradas();

                var consulta = new StringBuilder();

                consulta.AppendLine("SELECT T.* \n");
                consulta.AppendFormat("FROM {0} T\n", TABELA);
                consulta.AppendFormat("Where T.{0} IN ({1})\n", COLUNA_COD_ATIVIDADE, codAtividadesCorrecoes.Valor);
                consulta.Append(RetornarCondicaoDeTextoDuplicado(banco, texto));
                consulta.AppendFormat(!apenasFinalizadas ? " AND T.{0} IS NULL\n" : " AND T.{0} IS NOT NULL\n", COLUNA_DATA_CONCLUSAO);
                consulta.AppendFormat("AND T.{0} IN({1})\n", COLUNA_COD_COMPONENTE, codComponentes);
                consulta.AppendFormat("AND T.{0} <> {1}\n", COLUNA_COD_TAREFA, codTarefa);
                consulta.AppendFormat("order by T.{0} asc", COLUNA_COD_TAREFA);

                var dr = banco.Consultar(consulta.ToString(), TIMEOUT);

                while (dr.Read())
                {
                    var tarefa = ConverterDataReader(banco, dr);
                    lista.Add(tarefa);
                }

                dr.Close();
                dr.Dispose();

                return lista;

            }
            finally
            {
                banco.FecharConexao();
            }
        }


        private static string RetornarCondicaoDeTextoDuplicado(Banco banco, string texto)
        {
            var palavras = Texto.RetornarPalavrasTexto(texto, Separadores);

            var condicao = new StringBuilder();

            foreach (var palavra in palavras)
            {
                if (palavra.Trim().Length > 0)
                    if (!condicao.ToString().Contains(palavra.Trim()))
                        condicao.AppendFormat("And CONTAINS(T.{0}, {1})\n", COLUNA_DESCRICAO, banco.TratarTextoColocandoAspas(palavra.Trim()));
            }

            return condicao.ToString();
        }

        public static void AjustarPercentualDuplicidade(string texto, ref List<Tarefa> tarefas)
        {
            decimal valor;
            var quantidadeTotal = 0;

            var lista = new List<Tarefa>();
            var textoUsuario = new Texto();
            textoUsuario.RemoverStopWords(ref texto);

            var palavras = Texto.RetornarPalavrasTexto(texto, Separadores);
            var percentualDuplicidade = Configuracao.RetornarPercentualConfigurado(0);

            foreach (var tarefa in tarefas)
            {
                var descricao = tarefa.Descricao;
                textoUsuario.RemoverStopWords(ref descricao);
                var palavrasTarefa = Texto.RetornarPalavrasTexto(descricao, Separadores);
                quantidadeTotal = palavrasTarefa.Length;
                var palavraIgual = palavrasTarefa.Count(palavraTarefa => palavras.Any(palavra => palavra.Trim().ToUpper().Equals(palavraTarefa.Trim().ToUpper())));

                valor = palavraIgual * 100 / quantidadeTotal;

                tarefa._percentualDuplicidade = valor;

                if (tarefa._percentualDuplicidade >= percentualDuplicidade)
                    lista.Add(tarefa);
            }

            tarefas = lista.OrderBy(x => x.CodTarefa).ToList();
        }

        public static string RetornarTextoTarefasIguais(string textoUsuario, List<Tarefa> tarefas, bool finalizadas)
        {
            var texto = new StringBuilder();

            if (tarefas.Count > 0)
            {
                if (tarefas.Count > 0)
                {
                    var media = CalcularPercentual(tarefas, true);

                    texto.AppendFormat("{0}{1}% com as tarefas", INICIO_TAREFA_DUPLICADA, media);
                    texto.Append(finalizadas ? TAREFA_FINALIZADA : TAREFA_EM_ABERTO);
                }
            }

            return texto.ToString();
        }
        #endregion

        #region Tarefas Similares
        public static List<Tarefa> RetornarTarefasSimilares(string texto, int codComponente, double codTarefa, bool apenasFinalizadas)
        {
            var banco = new Banco();

            try
            {
                banco.AbrirConexao();
                var lista = new List<Tarefa>();
                if (texto.Trim().Length <= 0) return lista;
                var textoUsuario = new Texto();
                textoUsuario.RemoverStopWords(ref texto);
                var codAtividadesCorrecoes = Configuracao.RetornarAtividadesMonitoradas();
                var codComponentes = Componente.RetornarCodigosComponentesPertencente(codComponente);

                var consulta = new StringBuilder();

                consulta.AppendLine("SELECT T.*");
                consulta.AppendFormat("FROM {0} T\n", TABELA);
                consulta.AppendFormat("INNER JOIN FREETEXTTABLE({0}, {1}, {2}) AS FT\n", TABELA, COLUNA_DESCRICAO,
                   banco.TratarTextoColocandoAspas(texto));
                consulta.AppendFormat("ON T.{0} = FT.[KEY]\n", COLUNA_COD_TAREFA);
                consulta.AppendFormat("WHERE T.{0} IN ({1})\n", COLUNA_COD_ATIVIDADE, codAtividadesCorrecoes.Valor);
                consulta.AppendFormat(!apenasFinalizadas ? " AND T.{0} IS NULL\n" : " AND T.{0} IS NOT NULL\n", COLUNA_DATA_CONCLUSAO);
                consulta.AppendFormat("AND T.{0} IN({1})\n", COLUNA_COD_COMPONENTE, codComponentes);
                consulta.AppendFormat("AND T.{0} <> {1}\n", COLUNA_COD_TAREFA, codTarefa);
                consulta.AppendLine("AND FT.RANK > 0");
                consulta.AppendLine("ORDER BY FT.RANK DESC");

                var dr = banco.Consultar(consulta.ToString(), TIMEOUT);

                while (dr.Read())
                {
                    var tarefa = ConverterDataReader(banco, dr);
                    lista.Add(tarefa);
                }

                dr.Close();
                dr.Dispose();

                return lista;

            }
            finally
            {
                banco.FecharConexao();
            }
        }

        public static void AjustarPercentualSimilaridade(string texto, ref List<Tarefa> tarefas)
        {
            var lista = new List<Tarefa>();
            var textoUsuario = new Texto();
            textoUsuario.RemoverStopWords(ref texto);

            var palavras = Texto.RetornarPalavrasTexto(texto, Separadores);
            var percentualSimilaridade = Configuracao.RetornarPercentualConfigurado(1);
            var limiteSimilaridade = (float)Configuracao.RetornarPercentualConfigurado(2) / 100;

            foreach (var tarefa in tarefas)
            {
                var descricao = tarefa.Descricao;
                textoUsuario.RemoverStopWords(ref descricao);
                var palavrasTarefa = Texto.RetornarPalavrasTexto(descricao, Separadores);
                var quantidadeTotal = palavrasTarefa.Length - palavrasTarefa.Count(x => x.Length == 0);
                var palavraSimilar = palavrasTarefa.Count(palavraTarefa => palavras.Where(palavra => palavraTarefa.Length != 0 || palavra.Length != 0).Any(palavra => !(textoUsuario.RetornarDistanciaEntrePalavra(palavra.Trim(), palavraTarefa.Trim(), 1) < limiteSimilaridade)));

                var valor = palavraSimilar * 100 / quantidadeTotal;

                tarefa._percentualSimiliraridade = valor;

                if (tarefa._percentualSimiliraridade >= percentualSimilaridade)
                    lista.Add(tarefa);
            }

            tarefas = lista.OrderBy(x => x.CodTarefa).ToList();
        }

        public static string RetornarTextoTarefasSimilares(string textoUsuario, List<Tarefa> tarefas, bool finalizadas)
        {
            var texto = new StringBuilder();


            if (tarefas.Count > 0)
            {
                AjustarPercentualSimilaridade(textoUsuario, ref tarefas);

                if (tarefas.Count > 0)
                {
                    var media = CalcularPercentual(tarefas, false);

                    texto.AppendFormat("{0}{1}% com as tarefas", INICIO_TAREFA_SIMILAR, media);
                    texto.Append(finalizadas ? TAREFA_FINALIZADA : TAREFA_EM_ABERTO);
                }
            }

            return texto.ToString();
        }

        #endregion

        #region Métodos Compartilhados
        private static decimal CalcularPercentual(ICollection<Tarefa> tarefas, bool duplicidade)
        {
            var valorTotal = tarefas.Sum(tarefa => duplicidade ? tarefa._percentualDuplicidade : tarefa._percentualSimiliraridade);

            decimal media = Math.Round(valorTotal / tarefas.Count, 2);
            return media;
        }
        #endregion
    }
}
