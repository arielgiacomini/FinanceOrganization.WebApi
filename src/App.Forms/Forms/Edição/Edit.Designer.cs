namespace App.Forms.Forms.Edição
{
    partial class FrmEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEdit));
            grbTemplateContaPagar = new GroupBox();
            cboHabilitarDataCompra = new CheckBox();
            rdbPagamentoNaoPago = new RadioButton();
            rdbPagamentoPago = new RadioButton();
            txtContaPagarDataPagamento = new TextBox();
            lblContaPagarDataPagamento = new Label();
            dtpContaPagarDataVencimento = new DateTimePicker();
            lblContaPagarDataVencimento = new Label();
            btnContaPagarCadastrar = new Button();
            lblContaPagarDataCriacao = new Label();
            rtbContaPagarMensagemAdicional = new RichTextBox();
            lblContaPagarMensagemAdicional = new Label();
            cboContaPagarTipoCadastro = new ComboBox();
            lblContaPagarTipoCadastro = new Label();
            cboContaPagarFrequencia = new ComboBox();
            lblContaPagarFrequencia = new Label();
            dtpContaPagarDataCompra = new DateTimePicker();
            lblContaPagarDataCompra = new Label();
            txtContaPagarValor = new TextBox();
            lblContaPagarValor = new Label();
            lblContaPagarNameDescription = new Label();
            txtContaPagarNameDescription = new TextBox();
            lblContaPagarCategory = new Label();
            cboContaPagarCategory = new ComboBox();
            cboContaPagarAnoMesInicial = new ComboBox();
            lblContaPagarTipoConta = new Label();
            lblContaPagarAnoMes = new Label();
            cboContaPagarTipoConta = new ComboBox();
            grbTemplateContaPagar.SuspendLayout();
            SuspendLayout();
            // 
            // grbTemplateContaPagar
            // 
            grbTemplateContaPagar.Controls.Add(cboHabilitarDataCompra);
            grbTemplateContaPagar.Controls.Add(rdbPagamentoNaoPago);
            grbTemplateContaPagar.Controls.Add(rdbPagamentoPago);
            grbTemplateContaPagar.Controls.Add(txtContaPagarDataPagamento);
            grbTemplateContaPagar.Controls.Add(lblContaPagarDataPagamento);
            grbTemplateContaPagar.Controls.Add(dtpContaPagarDataVencimento);
            grbTemplateContaPagar.Controls.Add(lblContaPagarDataVencimento);
            grbTemplateContaPagar.Controls.Add(btnContaPagarCadastrar);
            grbTemplateContaPagar.Controls.Add(lblContaPagarDataCriacao);
            grbTemplateContaPagar.Controls.Add(rtbContaPagarMensagemAdicional);
            grbTemplateContaPagar.Controls.Add(lblContaPagarMensagemAdicional);
            grbTemplateContaPagar.Controls.Add(cboContaPagarTipoCadastro);
            grbTemplateContaPagar.Controls.Add(lblContaPagarTipoCadastro);
            grbTemplateContaPagar.Controls.Add(cboContaPagarFrequencia);
            grbTemplateContaPagar.Controls.Add(lblContaPagarFrequencia);
            grbTemplateContaPagar.Controls.Add(dtpContaPagarDataCompra);
            grbTemplateContaPagar.Controls.Add(lblContaPagarDataCompra);
            grbTemplateContaPagar.Controls.Add(txtContaPagarValor);
            grbTemplateContaPagar.Controls.Add(lblContaPagarValor);
            grbTemplateContaPagar.Controls.Add(lblContaPagarNameDescription);
            grbTemplateContaPagar.Controls.Add(txtContaPagarNameDescription);
            grbTemplateContaPagar.Controls.Add(lblContaPagarCategory);
            grbTemplateContaPagar.Controls.Add(cboContaPagarCategory);
            grbTemplateContaPagar.Controls.Add(cboContaPagarAnoMesInicial);
            grbTemplateContaPagar.Controls.Add(lblContaPagarTipoConta);
            grbTemplateContaPagar.Controls.Add(lblContaPagarAnoMes);
            grbTemplateContaPagar.Controls.Add(cboContaPagarTipoConta);
            grbTemplateContaPagar.Location = new Point(12, 12);
            grbTemplateContaPagar.Name = "grbTemplateContaPagar";
            grbTemplateContaPagar.Size = new Size(1181, 234);
            grbTemplateContaPagar.TabIndex = 16;
            grbTemplateContaPagar.TabStop = false;
            grbTemplateContaPagar.Text = "Cadastro de Conta a Pagar - Livre";
            // 
            // cboHabilitarDataCompra
            // 
            cboHabilitarDataCompra.AutoSize = true;
            cboHabilitarDataCompra.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            cboHabilitarDataCompra.Location = new Point(725, 52);
            cboHabilitarDataCompra.Name = "cboHabilitarDataCompra";
            cboHabilitarDataCompra.Size = new Size(161, 17);
            cboHabilitarDataCompra.TabIndex = 36;
            cboHabilitarDataCompra.Text = "Habilitar Data de Compra?";
            cboHabilitarDataCompra.UseVisualStyleBackColor = true;
            cboHabilitarDataCompra.CheckedChanged += CboHabilitarDataCompra_CheckedChanged;
            // 
            // rdbPagamentoNaoPago
            // 
            rdbPagamentoNaoPago.AutoSize = true;
            rdbPagamentoNaoPago.Location = new Point(710, 180);
            rdbPagamentoNaoPago.Name = "rdbPagamentoNaoPago";
            rdbPagamentoNaoPago.Size = new Size(77, 19);
            rdbPagamentoNaoPago.TabIndex = 35;
            rdbPagamentoNaoPago.Text = "Não Pago";
            rdbPagamentoNaoPago.UseVisualStyleBackColor = true;
            // 
            // rdbPagamentoPago
            // 
            rdbPagamentoPago.AutoSize = true;
            rdbPagamentoPago.Checked = true;
            rdbPagamentoPago.Location = new Point(652, 180);
            rdbPagamentoPago.Name = "rdbPagamentoPago";
            rdbPagamentoPago.Size = new Size(52, 19);
            rdbPagamentoPago.TabIndex = 34;
            rdbPagamentoPago.TabStop = true;
            rdbPagamentoPago.Text = "Pago";
            rdbPagamentoPago.UseVisualStyleBackColor = true;
            // 
            // txtContaPagarDataPagamento
            // 
            txtContaPagarDataPagamento.Location = new Point(652, 149);
            txtContaPagarDataPagamento.Name = "txtContaPagarDataPagamento";
            txtContaPagarDataPagamento.Size = new Size(237, 23);
            txtContaPagarDataPagamento.TabIndex = 33;
            // 
            // lblContaPagarDataPagamento
            // 
            lblContaPagarDataPagamento.AutoSize = true;
            lblContaPagarDataPagamento.Location = new Point(532, 151);
            lblContaPagarDataPagamento.Name = "lblContaPagarDataPagamento";
            lblContaPagarDataPagamento.Size = new Size(114, 15);
            lblContaPagarDataPagamento.TabIndex = 32;
            lblContaPagarDataPagamento.Text = "Data de Pagamento:";
            // 
            // dtpContaPagarDataVencimento
            // 
            dtpContaPagarDataVencimento.Location = new Point(652, 105);
            dtpContaPagarDataVencimento.Name = "dtpContaPagarDataVencimento";
            dtpContaPagarDataVencimento.Size = new Size(237, 23);
            dtpContaPagarDataVencimento.TabIndex = 31;
            // 
            // lblContaPagarDataVencimento
            // 
            lblContaPagarDataVencimento.AutoSize = true;
            lblContaPagarDataVencimento.Location = new Point(530, 111);
            lblContaPagarDataVencimento.Name = "lblContaPagarDataVencimento";
            lblContaPagarDataVencimento.Size = new Size(116, 15);
            lblContaPagarDataVencimento.TabIndex = 30;
            lblContaPagarDataVencimento.Text = "Data de Vencimento:";
            // 
            // btnContaPagarCadastrar
            // 
            btnContaPagarCadastrar.AutoSize = true;
            btnContaPagarCadastrar.FlatStyle = FlatStyle.System;
            btnContaPagarCadastrar.Location = new Point(960, 187);
            btnContaPagarCadastrar.Name = "btnContaPagarCadastrar";
            btnContaPagarCadastrar.Size = new Size(176, 34);
            btnContaPagarCadastrar.TabIndex = 29;
            btnContaPagarCadastrar.Text = "Alterar (Conta a Pagar)";
            btnContaPagarCadastrar.UseVisualStyleBackColor = true;
            btnContaPagarCadastrar.Click += BtnContaPagarEditar_Click;
            // 
            // lblContaPagarDataCriacao
            // 
            lblContaPagarDataCriacao.AutoSize = true;
            lblContaPagarDataCriacao.Location = new Point(940, 159);
            lblContaPagarDataCriacao.Name = "lblContaPagarDataCriacao";
            lblContaPagarDataCriacao.Size = new Size(213, 15);
            lblContaPagarDataCriacao.TabIndex = 27;
            lblContaPagarDataCriacao.Text = "Data de Criação: 15/03/1995 às 05:35:01";
            // 
            // rtbContaPagarMensagemAdicional
            // 
            rtbContaPagarMensagemAdicional.Location = new Point(925, 48);
            rtbContaPagarMensagemAdicional.Name = "rtbContaPagarMensagemAdicional";
            rtbContaPagarMensagemAdicional.Size = new Size(242, 96);
            rtbContaPagarMensagemAdicional.TabIndex = 26;
            rtbContaPagarMensagemAdicional.Text = "";
            // 
            // lblContaPagarMensagemAdicional
            // 
            lblContaPagarMensagemAdicional.AutoSize = true;
            lblContaPagarMensagemAdicional.Location = new Point(922, 30);
            lblContaPagarMensagemAdicional.Name = "lblContaPagarMensagemAdicional";
            lblContaPagarMensagemAdicional.Size = new Size(122, 15);
            lblContaPagarMensagemAdicional.TabIndex = 24;
            lblContaPagarMensagemAdicional.Text = "Mensagem Adicional:";
            // 
            // cboContaPagarTipoCadastro
            // 
            cboContaPagarTipoCadastro.FormattingEnabled = true;
            cboContaPagarTipoCadastro.Location = new Point(116, 205);
            cboContaPagarTipoCadastro.Name = "cboContaPagarTipoCadastro";
            cboContaPagarTipoCadastro.Size = new Size(204, 23);
            cboContaPagarTipoCadastro.TabIndex = 23;
            // 
            // lblContaPagarTipoCadastro
            // 
            lblContaPagarTipoCadastro.AutoSize = true;
            lblContaPagarTipoCadastro.Location = new Point(11, 208);
            lblContaPagarTipoCadastro.Name = "lblContaPagarTipoCadastro";
            lblContaPagarTipoCadastro.Size = new Size(99, 15);
            lblContaPagarTipoCadastro.TabIndex = 22;
            lblContaPagarTipoCadastro.Text = "Tipo de Cadastro:";
            // 
            // cboContaPagarFrequencia
            // 
            cboContaPagarFrequencia.FormattingEnabled = true;
            cboContaPagarFrequencia.Location = new Point(652, 205);
            cboContaPagarFrequencia.Name = "cboContaPagarFrequencia";
            cboContaPagarFrequencia.Size = new Size(161, 23);
            cboContaPagarFrequencia.TabIndex = 21;
            // 
            // lblContaPagarFrequencia
            // 
            lblContaPagarFrequencia.AutoSize = true;
            lblContaPagarFrequencia.Location = new Point(578, 208);
            lblContaPagarFrequencia.Name = "lblContaPagarFrequencia";
            lblContaPagarFrequencia.Size = new Size(68, 15);
            lblContaPagarFrequencia.TabIndex = 20;
            lblContaPagarFrequencia.Text = "Frequência:";
            // 
            // dtpContaPagarDataCompra
            // 
            dtpContaPagarDataCompra.Location = new Point(652, 71);
            dtpContaPagarDataCompra.Name = "dtpContaPagarDataCompra";
            dtpContaPagarDataCompra.Size = new Size(237, 23);
            dtpContaPagarDataCompra.TabIndex = 17;
            // 
            // lblContaPagarDataCompra
            // 
            lblContaPagarDataCompra.AutoSize = true;
            lblContaPagarDataCompra.Location = new Point(553, 77);
            lblContaPagarDataCompra.Name = "lblContaPagarDataCompra";
            lblContaPagarDataCompra.Size = new Size(96, 15);
            lblContaPagarDataCompra.TabIndex = 16;
            lblContaPagarDataCompra.Text = "Data da Compra:";
            // 
            // txtContaPagarValor
            // 
            txtContaPagarValor.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point);
            txtContaPagarValor.ForeColor = Color.OrangeRed;
            txtContaPagarValor.Location = new Point(654, 17);
            txtContaPagarValor.Name = "txtContaPagarValor";
            txtContaPagarValor.Size = new Size(133, 27);
            txtContaPagarValor.TabIndex = 13;
            txtContaPagarValor.TextAlign = HorizontalAlignment.Right;
            txtContaPagarValor.Enter += TxtContaPagarValor_Enter;
            txtContaPagarValor.Leave += TxtContaPagarValor_Leave;
            // 
            // lblContaPagarValor
            // 
            lblContaPagarValor.AutoSize = true;
            lblContaPagarValor.Location = new Point(596, 20);
            lblContaPagarValor.Name = "lblContaPagarValor";
            lblContaPagarValor.Size = new Size(52, 15);
            lblContaPagarValor.TabIndex = 12;
            lblContaPagarValor.Text = "Valor R$:";
            // 
            // lblContaPagarNameDescription
            // 
            lblContaPagarNameDescription.AutoSize = true;
            lblContaPagarNameDescription.Location = new Point(11, 35);
            lblContaPagarNameDescription.Name = "lblContaPagarNameDescription";
            lblContaPagarNameDescription.Size = new Size(99, 15);
            lblContaPagarNameDescription.TabIndex = 0;
            lblContaPagarNameDescription.Text = "Nome/Descrição:";
            // 
            // txtContaPagarNameDescription
            // 
            txtContaPagarNameDescription.Location = new Point(116, 32);
            txtContaPagarNameDescription.Name = "txtContaPagarNameDescription";
            txtContaPagarNameDescription.Size = new Size(445, 23);
            txtContaPagarNameDescription.TabIndex = 1;
            // 
            // lblContaPagarCategory
            // 
            lblContaPagarCategory.AutoSize = true;
            lblContaPagarCategory.Location = new Point(49, 72);
            lblContaPagarCategory.Name = "lblContaPagarCategory";
            lblContaPagarCategory.Size = new Size(61, 15);
            lblContaPagarCategory.TabIndex = 2;
            lblContaPagarCategory.Text = "Categoria:";
            // 
            // cboContaPagarCategory
            // 
            cboContaPagarCategory.FormattingEnabled = true;
            cboContaPagarCategory.Location = new Point(116, 69);
            cboContaPagarCategory.Name = "cboContaPagarCategory";
            cboContaPagarCategory.Size = new Size(188, 23);
            cboContaPagarCategory.TabIndex = 3;
            // 
            // cboContaPagarAnoMesInicial
            // 
            cboContaPagarAnoMesInicial.Enabled = false;
            cboContaPagarAnoMesInicial.FormattingEnabled = true;
            cboContaPagarAnoMesInicial.Location = new Point(116, 151);
            cboContaPagarAnoMesInicial.Name = "cboContaPagarAnoMesInicial";
            cboContaPagarAnoMesInicial.Size = new Size(161, 23);
            cboContaPagarAnoMesInicial.TabIndex = 8;
            // 
            // lblContaPagarTipoConta
            // 
            lblContaPagarTipoConta.AutoSize = true;
            lblContaPagarTipoConta.Location = new Point(28, 111);
            lblContaPagarTipoConta.Name = "lblContaPagarTipoConta";
            lblContaPagarTipoConta.Size = new Size(84, 15);
            lblContaPagarTipoConta.TabIndex = 4;
            lblContaPagarTipoConta.Text = "Tipo de Conta:";
            // 
            // lblContaPagarAnoMes
            // 
            lblContaPagarAnoMes.AutoSize = true;
            lblContaPagarAnoMes.Location = new Point(53, 157);
            lblContaPagarAnoMes.Name = "lblContaPagarAnoMes";
            lblContaPagarAnoMes.Size = new Size(59, 15);
            lblContaPagarAnoMes.TabIndex = 7;
            lblContaPagarAnoMes.Text = "Ano/Mês:";
            // 
            // cboContaPagarTipoConta
            // 
            cboContaPagarTipoConta.FormattingEnabled = true;
            cboContaPagarTipoConta.Location = new Point(116, 108);
            cboContaPagarTipoConta.Name = "cboContaPagarTipoConta";
            cboContaPagarTipoConta.Size = new Size(167, 23);
            cboContaPagarTipoConta.TabIndex = 5;
            // 
            // FrmEdit
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Khaki;
            ClientSize = new Size(1208, 254);
            Controls.Add(grbTemplateContaPagar);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmEdit";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Edição de Conta a Pagar";
            Load += FrmEdit_Load;
            grbTemplateContaPagar.ResumeLayout(false);
            grbTemplateContaPagar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grbTemplateContaPagar;
        private Button btnContaPagarCadastrar;
        private Label lblContaPagarDataCriacao;
        private RichTextBox rtbContaPagarMensagemAdicional;
        private Label lblContaPagarMensagemAdicional;
        private ComboBox cboContaPagarTipoCadastro;
        private Label lblContaPagarTipoCadastro;
        private ComboBox cboContaPagarFrequencia;
        private Label lblContaPagarFrequencia;
        private ComboBox cboContaPagarMelhorDiaPagamento;
        private Label lblContaPagarMelhorDiaPagamento;
        private DateTimePicker dtpContaPagarDataCompra;
        private Label lblContaPagarDataCompra;
        private TextBox txtContaPagarValor;
        private Label lblContaPagarValor;
        private Label lblContaPagarNameDescription;
        private ComboBox cboContaPagarAnoMesFinal;
        private TextBox txtContaPagarNameDescription;
        private Label lblContaPagarAnoMesFinal;
        private Label lblContaPagarCategory;
        private CheckBox ckbContaPagarConsideraMesmoMes;
        private ComboBox cboContaPagarCategory;
        private ComboBox cboContaPagarAnoMesInicial;
        private Label lblContaPagarTipoConta;
        private Label lblContaPagarAnoMes;
        private ComboBox cboContaPagarTipoConta;
        private DateTimePicker dtpContaPagarDataVencimento;
        private Label lblContaPagarDataVencimento;
        private TextBox txtContaPagarDataPagamento;
        private Label lblContaPagarDataPagamento;
        private RadioButton rdbPagamentoNaoPago;
        private RadioButton rdbPagamentoPago;
        private CheckBox cboHabilitarDataCompra;
    }
}