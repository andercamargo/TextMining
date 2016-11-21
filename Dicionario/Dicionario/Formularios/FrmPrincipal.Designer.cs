namespace Dicionario.Formularios
{
    partial class FrmPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblPalavrasInseridas = new System.Windows.Forms.Label();
            this.lblPalavrasEncontradas = new System.Windows.Forms.Label();
            this.btnCargaInicial = new System.Windows.Forms.Button();
            this.lblExplicacao = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnFechar = new System.Windows.Forms.Button();
            this.btnTestarMetodo = new System.Windows.Forms.Button();
            this.btnLerArquivo = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // lblPalavrasInseridas
            // 
            this.lblPalavrasInseridas.AutoSize = true;
            this.lblPalavrasInseridas.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPalavrasInseridas.ForeColor = System.Drawing.Color.Navy;
            this.lblPalavrasInseridas.Location = new System.Drawing.Point(13, 202);
            this.lblPalavrasInseridas.Name = "lblPalavrasInseridas";
            this.lblPalavrasInseridas.Size = new System.Drawing.Size(156, 17);
            this.lblPalavrasInseridas.TabIndex = 5;
            this.lblPalavrasInseridas.Text = "Palavras Inseridas 0";
            this.lblPalavrasInseridas.Visible = false;
            // 
            // lblPalavrasEncontradas
            // 
            this.lblPalavrasEncontradas.AutoSize = true;
            this.lblPalavrasEncontradas.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPalavrasEncontradas.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblPalavrasEncontradas.Location = new System.Drawing.Point(12, 170);
            this.lblPalavrasEncontradas.Name = "lblPalavrasEncontradas";
            this.lblPalavrasEncontradas.Size = new System.Drawing.Size(311, 20);
            this.lblPalavrasEncontradas.TabIndex = 4;
            this.lblPalavrasEncontradas.Text = "Foram Encontradas X Novas Palavras";
            this.lblPalavrasEncontradas.Visible = false;
            // 
            // btnCargaInicial
            // 
            this.btnCargaInicial.Location = new System.Drawing.Point(12, 235);
            this.btnCargaInicial.Name = "btnCargaInicial";
            this.btnCargaInicial.Size = new System.Drawing.Size(75, 23);
            this.btnCargaInicial.TabIndex = 6;
            this.btnCargaInicial.Text = "&Carga Inicial";
            this.btnCargaInicial.UseVisualStyleBackColor = true;
            this.btnCargaInicial.Click += new System.EventHandler(this.btnCargaInicial_Click);
            // 
            // lblExplicacao
            // 
            this.lblExplicacao.AutoSize = true;
            this.lblExplicacao.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExplicacao.ForeColor = System.Drawing.Color.Black;
            this.lblExplicacao.Location = new System.Drawing.Point(12, 24);
            this.lblExplicacao.Name = "lblExplicacao";
            this.lblExplicacao.Size = new System.Drawing.Size(432, 20);
            this.lblExplicacao.TabIndex = 0;
            this.lblExplicacao.Text = "Bem vindo ao sistema de manutenção de StopWords";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Teal;
            this.label1.Location = new System.Drawing.Point(13, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Selecione uma das opções abaixo: ";
            // 
            // btnFechar
            // 
            this.btnFechar.Location = new System.Drawing.Point(369, 235);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(75, 23);
            this.btnFechar.TabIndex = 7;
            this.btnFechar.Text = "&Fechar";
            this.btnFechar.UseVisualStyleBackColor = true;
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // btnTestarMetodo
            // 
            this.btnTestarMetodo.Location = new System.Drawing.Point(97, 109);
            this.btnTestarMetodo.Name = "btnTestarMetodo";
            this.btnTestarMetodo.Size = new System.Drawing.Size(114, 23);
            this.btnTestarMetodo.TabIndex = 3;
            this.btnTestarMetodo.Text = "&Testar Text Mining";
            this.btnTestarMetodo.UseVisualStyleBackColor = true;
            this.btnTestarMetodo.Click += new System.EventHandler(this.btnTestarMetodo_Click);
            // 
            // btnLerArquivo
            // 
            this.btnLerArquivo.Location = new System.Drawing.Point(16, 109);
            this.btnLerArquivo.Name = "btnLerArquivo";
            this.btnLerArquivo.Size = new System.Drawing.Size(75, 23);
            this.btnLerArquivo.TabIndex = 2;
            this.btnLerArquivo.Text = "&Ler Arquivo";
            this.btnLerArquivo.UseVisualStyleBackColor = true;
            this.btnLerArquivo.Click += new System.EventHandler(this.btnLerArquivo_Click);
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 268);
            this.Controls.Add(this.btnLerArquivo);
            this.Controls.Add(this.btnTestarMetodo);
            this.Controls.Add(this.btnFechar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblExplicacao);
            this.Controls.Add(this.btnCargaInicial);
            this.Controls.Add(this.lblPalavrasEncontradas);
            this.Controls.Add(this.lblPalavrasInseridas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inserir Stop Words";
            this.Load += new System.EventHandler(this.FrmPrinicipal_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPalavrasInseridas;
        private System.Windows.Forms.Label lblPalavrasEncontradas;
        private System.Windows.Forms.Button btnCargaInicial;
        private System.Windows.Forms.Label lblExplicacao;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnFechar;
        private System.Windows.Forms.Button btnTestarMetodo;
        private System.Windows.Forms.Button btnLerArquivo;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}

