namespace Dicionario.Formularios
{
    partial class FrmTeste
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
            this.txtTexto = new System.Windows.Forms.TextBox();
            this.btnTestar = new System.Windows.Forms.Button();
            this.btnFechar = new System.Windows.Forms.Button();
            this.txtComponente = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkFinalizadas = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtTexto
            // 
            this.txtTexto.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTexto.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtTexto.Location = new System.Drawing.Point(13, 25);
            this.txtTexto.Multiline = true;
            this.txtTexto.Name = "txtTexto";
            this.txtTexto.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTexto.Size = new System.Drawing.Size(287, 243);
            this.txtTexto.TabIndex = 0;
            // 
            // btnTestar
            // 
            this.btnTestar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTestar.Location = new System.Drawing.Point(13, 305);
            this.btnTestar.Name = "btnTestar";
            this.btnTestar.Size = new System.Drawing.Size(75, 23);
            this.btnTestar.TabIndex = 3;
            this.btnTestar.Text = "&Testar";
            this.btnTestar.UseVisualStyleBackColor = true;
            this.btnTestar.Click += new System.EventHandler(this.btnTestar_Click);
            // 
            // btnFechar
            // 
            this.btnFechar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFechar.Location = new System.Drawing.Point(94, 305);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(75, 23);
            this.btnFechar.TabIndex = 4;
            this.btnFechar.Text = "&Fechar";
            this.btnFechar.UseVisualStyleBackColor = true;
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // txtComponente
            // 
            this.txtComponente.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtComponente.Location = new System.Drawing.Point(108, 274);
            this.txtComponente.Name = "txtComponente";
            this.txtComponente.Size = new System.Drawing.Size(82, 20);
            this.txtComponente.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 277);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Cod.Componente:";
            // 
            // chkFinalizadas
            // 
            this.chkFinalizadas.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkFinalizadas.AutoSize = true;
            this.chkFinalizadas.Location = new System.Drawing.Point(219, 274);
            this.chkFinalizadas.Name = "chkFinalizadas";
            this.chkFinalizadas.Size = new System.Drawing.Size(78, 17);
            this.chkFinalizadas.TabIndex = 2;
            this.chkFinalizadas.Text = "Finalizadas";
            this.chkFinalizadas.UseVisualStyleBackColor = true;
            // 
            // FrmTeste
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(312, 340);
            this.Controls.Add(this.chkFinalizadas);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtComponente);
            this.Controls.Add(this.btnFechar);
            this.Controls.Add(this.btnTestar);
            this.Controls.Add(this.txtTexto);
            this.MinimumSize = new System.Drawing.Size(328, 272);
            this.Name = "FrmTeste";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Teste";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTexto;
        private System.Windows.Forms.Button btnTestar;
        private System.Windows.Forms.Button btnFechar;
        private System.Windows.Forms.TextBox txtComponente;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkFinalizadas;
    }
}