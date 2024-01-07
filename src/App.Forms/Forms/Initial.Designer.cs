namespace App.Forms.Forms
{
    partial class Initial
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
            lblContaPagarNameDescription = new Label();
            txtContaPagarNameDescription = new TextBox();
            lblContaPagarCategory = new Label();
            cboContaPagarCategory = new ComboBox();
            lblContaPagarTipoConta = new Label();
            cboContaPagarTipoConta = new ComboBox();
            lblContaPagarAnoMesInicial = new Label();
            cboContaPagarAnoMesInicial = new ComboBox();
            ckbContaPagarConsideraMesmoMes = new CheckBox();
            cboContaPagarAnoMesFinal = new ComboBox();
            lblContaPagarAnoMesFinal = new Label();
            SuspendLayout();
            // 
            // lblContaPagarNameDescription
            // 
            lblContaPagarNameDescription.AutoSize = true;
            lblContaPagarNameDescription.Location = new Point(19, 17);
            lblContaPagarNameDescription.Name = "lblContaPagarNameDescription";
            lblContaPagarNameDescription.Size = new Size(99, 15);
            lblContaPagarNameDescription.TabIndex = 0;
            lblContaPagarNameDescription.Text = "Nome/Descrição:";
            // 
            // txtContaPagarNameDescription
            // 
            txtContaPagarNameDescription.Location = new Point(124, 14);
            txtContaPagarNameDescription.Name = "txtContaPagarNameDescription";
            txtContaPagarNameDescription.Size = new Size(303, 23);
            txtContaPagarNameDescription.TabIndex = 1;
            // 
            // lblContaPagarCategory
            // 
            lblContaPagarCategory.AutoSize = true;
            lblContaPagarCategory.Location = new Point(19, 53);
            lblContaPagarCategory.Name = "lblContaPagarCategory";
            lblContaPagarCategory.Size = new Size(61, 15);
            lblContaPagarCategory.TabIndex = 2;
            lblContaPagarCategory.Text = "Categoria:";
            // 
            // cboContaPagarCategory
            // 
            cboContaPagarCategory.FormattingEnabled = true;
            cboContaPagarCategory.Location = new Point(86, 50);
            cboContaPagarCategory.Name = "cboContaPagarCategory";
            cboContaPagarCategory.Size = new Size(188, 23);
            cboContaPagarCategory.TabIndex = 3;
            // 
            // lblContaPagarTipoConta
            // 
            lblContaPagarTipoConta.AutoSize = true;
            lblContaPagarTipoConta.Location = new Point(19, 85);
            lblContaPagarTipoConta.Name = "lblContaPagarTipoConta";
            lblContaPagarTipoConta.Size = new Size(81, 15);
            lblContaPagarTipoConta.TabIndex = 4;
            lblContaPagarTipoConta.Text = "Tipo de Conta";
            // 
            // cboContaPagarTipoConta
            // 
            cboContaPagarTipoConta.FormattingEnabled = true;
            cboContaPagarTipoConta.Location = new Point(117, 82);
            cboContaPagarTipoConta.Name = "cboContaPagarTipoConta";
            cboContaPagarTipoConta.Size = new Size(167, 23);
            cboContaPagarTipoConta.TabIndex = 5;
            // 
            // lblContaPagarAnoMesInicial
            // 
            lblContaPagarAnoMesInicial.AutoSize = true;
            lblContaPagarAnoMesInicial.Location = new Point(21, 124);
            lblContaPagarAnoMesInicial.Name = "lblContaPagarAnoMesInicial";
            lblContaPagarAnoMesInicial.Size = new Size(93, 15);
            lblContaPagarAnoMesInicial.TabIndex = 7;
            lblContaPagarAnoMesInicial.Text = "Ano/Mês Inicial:";
            // 
            // cboContaPagarAnoMesInicial
            // 
            cboContaPagarAnoMesInicial.FormattingEnabled = true;
            cboContaPagarAnoMesInicial.Location = new Point(118, 121);
            cboContaPagarAnoMesInicial.Name = "cboContaPagarAnoMesInicial";
            cboContaPagarAnoMesInicial.Size = new Size(161, 23);
            cboContaPagarAnoMesInicial.TabIndex = 8;
            // 
            // ckbContaPagarConsideraMesmoMes
            // 
            ckbContaPagarConsideraMesmoMes.AutoSize = true;
            ckbContaPagarConsideraMesmoMes.Checked = true;
            ckbContaPagarConsideraMesmoMes.CheckState = CheckState.Checked;
            ckbContaPagarConsideraMesmoMes.Location = new Point(285, 123);
            ckbContaPagarConsideraMesmoMes.Name = "ckbContaPagarConsideraMesmoMes";
            ckbContaPagarConsideraMesmoMes.Size = new Size(214, 19);
            ckbContaPagarConsideraMesmoMes.TabIndex = 9;
            ckbContaPagarConsideraMesmoMes.Text = "Considera como Mês Inicial e Final?";
            ckbContaPagarConsideraMesmoMes.UseVisualStyleBackColor = true;
            ckbContaPagarConsideraMesmoMes.CheckedChanged += ckbContaPagarConsideraMesmoMes_CheckedChanged;
            // 
            // cboContaPagarAnoMesFinal
            // 
            cboContaPagarAnoMesFinal.FormattingEnabled = true;
            cboContaPagarAnoMesFinal.Location = new Point(118, 150);
            cboContaPagarAnoMesFinal.Name = "cboContaPagarAnoMesFinal";
            cboContaPagarAnoMesFinal.Size = new Size(161, 23);
            cboContaPagarAnoMesFinal.TabIndex = 11;
            // 
            // lblContaPagarAnoMesFinal
            // 
            lblContaPagarAnoMesFinal.AutoSize = true;
            lblContaPagarAnoMesFinal.Location = new Point(21, 153);
            lblContaPagarAnoMesFinal.Name = "lblContaPagarAnoMesFinal";
            lblContaPagarAnoMesFinal.Size = new Size(87, 15);
            lblContaPagarAnoMesFinal.TabIndex = 10;
            lblContaPagarAnoMesFinal.Text = "Ano/Mês Final:";
            // 
            // Initial
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(507, 275);
            Controls.Add(cboContaPagarAnoMesFinal);
            Controls.Add(lblContaPagarAnoMesFinal);
            Controls.Add(ckbContaPagarConsideraMesmoMes);
            Controls.Add(cboContaPagarAnoMesInicial);
            Controls.Add(lblContaPagarAnoMesInicial);
            Controls.Add(cboContaPagarTipoConta);
            Controls.Add(lblContaPagarTipoConta);
            Controls.Add(cboContaPagarCategory);
            Controls.Add(lblContaPagarCategory);
            Controls.Add(txtContaPagarNameDescription);
            Controls.Add(lblContaPagarNameDescription);
            Name = "Initial";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Tela inicial - Organização Financeira";
            Load += Initial_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblContaPagarNameDescription;
        private TextBox txtContaPagarNameDescription;
        private Label lblContaPagarCategory;
        private ComboBox cboContaPagarCategory;
        private Label lblContaPagarTipoConta;
        private ComboBox cboContaPagarTipoConta;
        private Label lblContaPagarAnoMesInicial;
        private ComboBox cboContaPagarAnoMesInicial;
        private CheckBox ckbContaPagarConsideraMesmoMes;
        private ComboBox cboContaPagarAnoMesFinal;
        private Label lblContaPagarAnoMesFinal;
    }
}