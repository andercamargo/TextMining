<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmTeste.aspx.cs" Inherits="TextMining.frmTeste" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #form-resposta {
            display: none;
        }

        #form-resposta-usuario {
            display: none;
        }

        .modal {
            display: none;
            position: fixed;
            z-index: 1000;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            background: #E6E6E6 url('images/gears.gif') 50% 50% no-repeat;
        }

        .link {
            text-decoration: none;
            font-weight: bold;
        }

        body.loading {
            overflow: hidden;
        }

            body.loading .modal {
                display: block;
            }
    </style>

    <link rel="stylesheet" type="text/css" href="scripts/jquery-ui-1.12.1/jquery-ui.min.css">
    <script src="scripts/jquery-ui-1.12.1/external/jquery/jquery.js"> </script>
    <script src="scripts/jquery-ui-1.12.1/jquery-ui.min.js"> </script>


    <script type="text/javascript">
        var duplicada = false;
        var finalizada = false;
        var dataIdentificacao = new Date();
        var codTextMining = "";
        var tarefas = "";

        $(function () {
            $body = $("body");

            $(document).on({
                ajaxStart: function () { $body.addClass("loading"); },
                ajaxStop: function () { $body.removeClass("loading"); }
            });

            $("#btnGravar").click(function () {
                var textoDigitado = $("#textoDigitado").val().replace(/'/g, '\\\'');
                var codComponente = $("#codComponente").val();
                var codTarefa = $('#codTarefa').val();


                $.ajax({
                    type: "POST",
                    async: true,
                    url: "FrmTeste.aspx/AnalisarTarefa",
                    data: "{textoDigitado: '" + textoDigitado + "', codComponente: '" + codComponente + "', codTarefa: '" + codTarefa + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.d.Texto != null) {

                            var texto = data.d.Texto;
                            $("#resposta").text(texto);
                            dataIdentificacao = data.d.DataIdentificacao;
                            tarefas = data.d.Tarefas;
                            var lista = tarefas.split(",");

                            for (var i = 0; i < lista.length; i++) {

                                var site = 'http://agilis.teleconsistemas.com.br:6060/agilis/BacklogCadItem.aspx?CodTarefa=' + lista[i].trim();

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
                            formatarComponentes();
                            $("#form-resposta").attr("title", titulo);
                            $("#form-resposta").dialog({
                                Width: 930,
                                minWidth: 930,
                                minHeight: 300,
                                buttons: {
                                    Gravar: function () {
                                        $(this).dialog("close");
                                        onclick: analisarResposta();
                                    }
                                },
                                modal: true,
                                closeOnEscape: true,
                                open: function (event, ui) { $(".ui-dialog-titlebar-close").hide(); }
                            });
                            $('button:contains(Gravar)').attr("id", "btnResposta");
                        }
                    },
                    error: function (data) {
                        var texto = "Ocorreu um erro na solicitação";
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
                    }
                });
                return false;
            });

            function analisarResposta() {
                var resposta = $('input[name=rdbOpcoes]:checked', '#formulario-resposta').val();
                var codTarefa = $('#codTarefa').val();
                var codRelator = $('#codRelator').val();

                $.ajax({
                    type: "POST",
                    url: "FrmTeste.aspx/AnalisarResposta",
                    data: "{resposta: '" + resposta + "', duplicada: '" + duplicada + "', finalizada: '" + finalizada + "', codTarefa: '" + codTarefa + "', codRelator: '" + codRelator + "', codTextMining: '" + codTextMining + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var texto = "Procedimento executado com sucesso!";
                        var titulo = "Informação";

                        $("#resposta-usuario").text(texto);
                        $("#form-reposta-usuario").attr("title", titulo);
                        $("#form-reposta-usuario").dialog({
                            buttons: {
                                OK: function () {
                                    $(this).dialog("close");
                                }
                            },
                            modal: true,
                            closeOnEscape: true,
                            open: function (event, ui) { $(".ui-dialog-titlebar-close").hide(); }
                        });
                    },
                    error: function (data) {
                        var texto = "Ocorreu um erro na solicitação";
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
                            closeOnEscape: true,
                            open: function (event, ui) { $(".ui-dialog-titlebar-close").hide(); }
                        });
                    }
                });
                return false;
            }

            function formatarComponentes() {
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

        });
    </script>

</head>
<body style="height: 213px">
    <div class="modal">
        <!-- Place at bottom of page -->
    </div>
    <form id="form1" runat="server">
        <div>
            <label>Texto: </label>
            <asp:TextBox ID="textoDigitado" runat="server" ClientIDMode="Static" TextMode="MultiLine"></asp:TextBox>
        </div>
        <div>
            <label>Componente:</label>
            <asp:TextBox ID="codComponente" runat="server" ClientIDMode="Static"></asp:TextBox>
        </div>
        <div>
            <asp:Label ID="Label1" runat="server" Text="Label">CodTarefa: </asp:Label>
            <asp:TextBox ID="codTarefa" runat="server" ClientIDMode="Static"></asp:TextBox>

        </div>
        <div>
            <asp:Label ID="Label2" runat="server" Text="Label">CodRelator: </asp:Label>
            <asp:TextBox ID="codRelator" runat="server" ClientIDMode="Static"></asp:TextBox>

        </div>
        <div>
            <asp:Button ID="btnGravar" runat="server" Text="Testar" />
        </div>
    </form>
    <div id="form-resposta" title="Atenção: Tarefas Duplicadas Encontradas">
        <form id="formulario-resposta">
            <span id="resposta"></span>
            <div>
                <p>
                    O que você deseja fazer?<br />
                </p>
            </div>
            <input type="radio" name="rdbOpcoes" value="NaoCadastrarTarefa" checked="checked" id="naoCadastrarTarefa">
            <span id="textoNaoCadastrarTarefa"></span>
            <br />
            <input type="radio" name="rdbOpcoes" value="EnviarEmail">
            <span id="textoEnviarEmail"></span>
            <br />
            <div id="aumentarImportancia">
                <input type="radio" name="rdbOpcoes" value="AumentarImportancia">
                <span id="textoAumentarImportancia"></span>
                <br />
            </div>
            <input type="radio" name="rdbOpcoes" value="CadastrarTarefa">
            <span id="textoCadastrarTarefa"></span>
            <br />
        </form>
    </div>

    <div id="form-reposta-usuario">
        <span id="resposta-usuario"></span>
    </div>

</body>
</html>
