var duplicada = false;
var finalizada = false;
var dataIdentificacao = new Date();
var codTextMining = "";
var tarefas = "";
var urlTextMining = "WebServices/Tarefas.asmx/";

function AnalisarTextMining(textoDigitado, codTarefa, codComponente, codRelator) {
    textoDigitado = textoDigitado.replace(/'/g, '\\\'');

    exibirFeedback();

    $.ajax({
        type: "POST",
        async: true,
        url: urlTextMining + "AnalisarTarefa",
        data: "{textoDigitado: '" + textoDigitado + "', codComponente: '" + codComponente + "', codTarefa: '" + codTarefa + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.d.Texto != null) {
                var inicioTexto = "A tarefa ";
                $("#resposta").text(inicioTexto);
                var siteTarefa = 'BacklogCadItem.aspx?CodTarefa=' + codTarefa;
                var linkTarefa = '<a href=' + siteTarefa + ' target="_blank" class="link">' + codTarefa + '</a>';
                $(linkTarefa).appendTo($("#resposta"));
                var texto = data.d.Texto;
                $("#resposta").append(texto);
                dataIdentificacao = data.d.DataIdentificacao;
                tarefas = data.d.Tarefas;
                var lista = tarefas.split(",");

                for (var i = 0; i < lista.length; i++) {

                    var site = 'BacklogCadItem.aspx?CodTarefa=' + lista[i].trim();

                    var sinal = i == lista.length - 1 ? "." : ", ";

                    var link = '<a href=' + site + ' target="_blank" class="link">' + lista[i].trim() + sinal + '</a>';

                    $(link).appendTo($("#resposta"));
                }
                codTextMining = data.d.CodTextMining;
                var inicio = "";
                if (data.d.TarefaFinalizada) {
                    inicio = "Finalizadas";
                    finalizada = true;
                }
                else inicio = "Em Aberto";

                var final = "";

                if (data.d.TarefaSimilar) {
                    inicio = "Similares";
                    final = "Encontradas";
                }
                else {
                    final = "Duplicadas";
                    duplicada = true;
                }

                var titulo = "Atenção: Tarefas " + inicio + " " + final;
                FormatarComponentes();
                $("#form-resposta").attr("title", titulo);
                $("#form-resposta").dialog({
                    Width: 930,
                    minWidth: 930,
                    minHeight: 300,
                    buttons: {
                        Gravar: function () {
                            $(this).dialog("close");
                            onclick: AnalisarResposta(codTarefa, codRelator);
                        },
                        Cancelar: function () {
                            $(this).dialog("close");
                        }
                    },
                    modal: true,
                    closeOnEscape: true,
                });
                $('button:contains(Gravar)').attr("id", "btnResposta");
                $("#dialog").dialog("option", "position", "center");
            }
            ocultarFeedback();
        },
        error: function (data) {
            var texto = "Ocorreu um erro na solicitação" + data;
            var titulo = "Atenção";
            $("#resposta").text(texto);
            $("#form-reposta").attr("title", titulo);
            $("#form-reposta").dialog({
                buttons: {
                    OK: function () {
                        $(this).dialog("close");
                    }
                },
                modal: true,
                closeOnEscape: true,
                open: function (event, ui) { $(".ui-dialog-titlebar-close").hide(); }
            });
            ocultarFeedback();
        }
    });
    return false;
};

function AnalisarResposta(codTarefa, codRelator) {
    exibirFeedback();
    
    var resposta = getRadioValor('rdbOpcoes');

    $.ajax({
        type: "POST",
        url: urlTextMining + "AnalisarResposta",
        data: "{resposta: '" + resposta + "', duplicada: '" + duplicada + "', finalizada: '" + finalizada + "', codTarefa: '" + codTarefa + "', codRelator: '" + codRelator + "', codTextMining: '" + codTextMining + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var texto = "Procedimento executado com sucesso!";
            var titulo = "Informação";

            $("#resposta-usuario").text(texto);
            $("#form-reposta-usuario").attr("title", titulo);
            $("#form-reposta-usuario").dialog({
                buttons: {
                    OK: function() {
                        $(this).dialog("close");
                        onclick: ExibirTarefaAlterada(codTarefa, resposta, duplicada);
                    }
                },
                modal: true,
                closeOnEscape: true
            });
            ocultarFeedback();
        },
        error: function (data) {
            var texto = "Ocorreu um erro na solicitação" + data;
            $("#resposta-usuario").text(texto);
            var titulo = "Atenção";

            $("#resposta-usuario").text(texto);

            $("#form-reposta-usuario").attr("title", titulo);
            $("#form-reposta-usuario").dialog({
                buttons: {
                    OK: function () {
                        $(this).dialog("close");
                    }
                },
                modal: true,
                closeOnEscape: true
            });
            ocultarFeedback();
        }
    });
    return false;
}

function FormatarComponentes() {
        var enviarEmail = "Enviar um e-mail ao analista de sistemas sinalizando o atual problema";
        var complemento = "duplicada";
        $("#aumentarImportancia").hide();

        if (duplicada) {
            var naoCadastrar = "Não cadastrar a tarefa atual";

            if (finalizada) {
                $("#textoNaoCadastrarTarefa").text(naoCadastrar);
                $("#textoEnviarEmail").text(enviarEmail + " e não cadastrar a tarefa atual");
            } else {
                $("#textoNaoCadastrarTarefa")
                    .text(naoCadastrar + " e aumentar a importância para as tarefas abertas " + tarefas);
                $("#textoEnviarEmail").text(enviarEmail);
            }

        } else {
            complemento = "similar";
            var mesclarTarefa = "Mesclar as tarefas " + tarefas + " com a tarefa atual";
            var importancia = "Aumentar a importância para as tarefas relacionadas, apagando a tarefa atual";
            $("#textoNaoCadastrarTarefa").text(mesclarTarefa);
            $("#textoEnviarEmail").text(enviarEmail);
            $("#textoAumentarImportancia").text(importancia);
            $("#aumentarImportancia").show();
        }

        $("#textoCadastrarTarefa").text("Entendo que a tarefa é " + complemento + " mas desejo cadastrar mesmo assim");
    }

    function getRadioValor(name) {
        var rads = document.getElementsByName(name);

        for (var i = 0; i < rads.length; i++) {
            if (rads[i].checked) {
                return rads[i].value;
            }

        }
        return null;
    }


    function ExibirTarefaAlterada(codtarefa, resposta, duplicada) {
        if (resposta == "NaoCadastrarTarefa" && !duplicada) {
            var texto = "Deseja ir para a tarefa " + codtarefa;
            $("#resposta-usuario").text(texto);
            var titulo = "Informação";

            $("#form-reposta-usuario").attr("title", titulo);
            $("#form-reposta-usuario").dialog({
                buttons: {
                    "Sim": function() {
                        $(this).dialog("close");
                        onclick: window.location = 'BacklogCadItem.aspx?CodTarefa=' + codtarefa;
                    },
                    "Não": function() {
                        $(this).dialog("close");
                    },
                },
                modal: true,
                closeOnEscape: true
            });
        }
    }