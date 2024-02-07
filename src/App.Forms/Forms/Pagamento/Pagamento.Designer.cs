namespace App.Forms.Forms.Pay
{
    partial class FrmPagamento
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPagamento));
            lblPagamentoIdContaPagar = new Label();
            txtPagamentoIdContaPagar = new TextBox();
            lblPagamentoData = new Label();
            txtPagamentoData = new TextBox();
            rdbPagamentoPago = new RadioButton();
            rdbPagamentoNaoPago = new RadioButton();
            lblPagamentoConta = new Label();
            lblPagamentoMesAno = new Label();
            cboPagamentoConta = new ComboBox();
            cboPagamentoMesAno = new ComboBox();
            btnPagamentoPagar = new Button();
            lblPagamentoNome = new Label();
            lblPagamentoValor = new Label();
            SuspendLayout();
            // 
            // lblPagamentoIdContaPagar
            // 
            lblPagamentoIdContaPagar.AutoSize = true;
            lblPagamentoIdContaPagar.Location = new Point(25, 43);
            lblPagamentoIdContaPagar.Name = "lblPagamentoIdContaPagar";
            lblPagamentoIdContaPagar.Size = new Size(114, 15);
            lblPagamentoIdContaPagar.TabIndex = 0;
            lblPagamentoIdContaPagar.Text = "ID da Conta a Pagar:";
            // 
            // txtPagamentoIdContaPagar
            // 
            txtPagamentoIdContaPagar.Location = new Point(145, 40);
            txtPagamentoIdContaPagar.Name = "txtPagamentoIdContaPagar";
            txtPagamentoIdContaPagar.Size = new Size(287, 23);
            txtPagamentoIdContaPagar.TabIndex = 1;
            // 
            // lblPagamentoData
            // 
            lblPagamentoData.AutoSize = true;
            lblPagamentoData.Location = new Point(25, 130);
            lblPagamentoData.Name = "lblPagamentoData";
            lblPagamentoData.Size = new Size(114, 15);
            lblPagamentoData.TabIndex = 2;
            lblPagamentoData.Text = "Data de Pagamento:";
            // 
            // txtPagamentoData
            // 
            txtPagamentoData.Location = new Point(145, 127);
            txtPagamentoData.Name = "txtPagamentoData";
            txtPagamentoData.Size = new Size(155, 23);
            txtPagamentoData.TabIndex = 3;
            // 
            // rdbPagamentoPago
            // 
            rdbPagamentoPago.AutoSize = true;
            rdbPagamentoPago.Checked = true;
            rdbPagamentoPago.Location = new Point(315, 128);
            rdbPagamentoPago.Name = "rdbPagamentoPago";
            rdbPagamentoPago.Size = new Size(52, 19);
            rdbPagamentoPago.TabIndex = 4;
            rdbPagamentoPago.TabStop = true;
            rdbPagamentoPago.Text = "Pago";
            rdbPagamentoPago.UseVisualStyleBackColor = true;
            // 
            // rdbPagamentoNaoPago
            // 
            rdbPagamentoNaoPago.AutoSize = true;
            rdbPagamentoNaoPago.Location = new Point(373, 128);
            rdbPagamentoNaoPago.Name = "rdbPagamentoNaoPago";
            rdbPagamentoNaoPago.Size = new Size(77, 19);
            rdbPagamentoNaoPago.TabIndex = 5;
            rdbPagamentoNaoPago.Text = "Não Pago";
            rdbPagamentoNaoPago.UseVisualStyleBackColor = true;
            // 
            // lblPagamentoConta
            // 
            lblPagamentoConta.AutoSize = true;
            lblPagamentoConta.Location = new Point(97, 72);
            lblPagamentoConta.Name = "lblPagamentoConta";
            lblPagamentoConta.Size = new Size(42, 15);
            lblPagamentoConta.TabIndex = 6;
            lblPagamentoConta.Text = "Conta:";
            // 
            // lblPagamentoMesAno
            // 
            lblPagamentoMesAno.AutoSize = true;
            lblPagamentoMesAno.Location = new Point(80, 101);
            lblPagamentoMesAno.Name = "lblPagamentoMesAno";
            lblPagamentoMesAno.Size = new Size(59, 15);
            lblPagamentoMesAno.TabIndex = 7;
            lblPagamentoMesAno.Text = "Mês/Ano:";
            // 
            // cboPagamentoConta
            // 
            cboPagamentoConta.FormattingEnabled = true;
            cboPagamentoConta.Location = new Point(145, 69);
            cboPagamentoConta.Name = "cboPagamentoConta";
            cboPagamentoConta.Size = new Size(154, 23);
            cboPagamentoConta.TabIndex = 8;
            // 
            // cboPagamentoMesAno
            // 
            cboPagamentoMesAno.FormattingEnabled = true;
            cboPagamentoMesAno.Location = new Point(145, 98);
            cboPagamentoMesAno.Name = "cboPagamentoMesAno";
            cboPagamentoMesAno.Size = new Size(121, 23);
            cboPagamentoMesAno.TabIndex = 9;
            // 
            // btnPagamentoPagar
            // 
            btnPagamentoPagar.Location = new Point(189, 159);
            btnPagamentoPagar.Name = "btnPagamentoPagar";
            btnPagamentoPagar.Size = new Size(147, 27);
            btnPagamentoPagar.TabIndex = 10;
            btnPagamentoPagar.Text = "Pagar";
            btnPagamentoPagar.UseVisualStyleBackColor = true;
            btnPagamentoPagar.Click += BtnPagamentoPagar_Click;
            // 
            // lblPagamentoNome
            // 
            lblPagamentoNome.AutoSize = true;
            lblPagamentoNome.BackColor = Color.Lime;
            lblPagamentoNome.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblPagamentoNome.ForeColor = SystemColors.HotTrack;
            lblPagamentoNome.Location = new Point(25, 9);
            lblPagamentoNome.Name = "lblPagamentoNome";
            lblPagamentoNome.Size = new Size(148, 19);
            lblPagamentoNome.TabIndex = 11;
            lblPagamentoNome.Text = "Conta que será paga";
            lblPagamentoNome.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPagamentoValor
            // 
            lblPagamentoValor.AutoSize = true;
            lblPagamentoValor.BackColor = Color.Crimson;
            lblPagamentoValor.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblPagamentoValor.ForeColor = SystemColors.ControlLightLight;
            lblPagamentoValor.Location = new Point(388, 86);
            lblPagamentoValor.Name = "lblPagamentoValor";
            lblPagamentoValor.Size = new Size(44, 19);
            lblPagamentoValor.TabIndex = 13;
            lblPagamentoValor.Text = "Valor";
            lblPagamentoValor.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmPagamento
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(512, 198);
            Controls.Add(lblPagamentoValor);
            Controls.Add(lblPagamentoNome);
            Controls.Add(btnPagamentoPagar);
            Controls.Add(cboPagamentoMesAno);
            Controls.Add(cboPagamentoConta);
            Controls.Add(lblPagamentoMesAno);
            Controls.Add(lblPagamentoConta);
            Controls.Add(rdbPagamentoNaoPago);
            Controls.Add(rdbPagamentoPago);
            Controls.Add(txtPagamentoData);
            Controls.Add(lblPagamentoData);
            Controls.Add(txtPagamentoIdContaPagar);
            Controls.Add(lblPagamentoIdContaPagar);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmPagamento";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Realizar Pagamento";
            Load += FrmPagamento_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPagamentoIdContaPagar;
        private TextBox txtPagamentoIdContaPagar;
        private Label lblPagamentoData;
        private TextBox txtPagamentoData;
        private RadioButton rdbPagamentoPago;
        private RadioButton rdbPagamentoNaoPago;
        private Label lblPagamentoConta;
        private Label lblPagamentoMesAno;
        private ComboBox cboPagamentoConta;
        private ComboBox cboPagamentoMesAno;
        private Button btnPagamentoPagar;
        private Label lblPagamentoNome;
        private Label lblPagamentoValor;
    }
}