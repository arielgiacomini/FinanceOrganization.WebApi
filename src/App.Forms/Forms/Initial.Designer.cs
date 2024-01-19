﻿using App.Forms.DataSource;

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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            tbcInitial = new TabControl();
            tbpContaPagarLivre = new TabPage();
            grbTemplateContaPagar = new GroupBox();
            btnContaPagarCadastrar = new Button();
            dgvContaPagar = new DataGridView();
            grbContaPagarHistorico = new GroupBox();
            lblContaPagarDataCriacao = new Label();
            rtbContaPagarMensagemAdicional = new RichTextBox();
            lblContaPagarMensagemAdicional = new Label();
            cboContaPagarTipoCadastro = new ComboBox();
            lblContaPagarTipoCadastro = new Label();
            cboContaPagarFrequencia = new ComboBox();
            lblContaPagarFrequencia = new Label();
            cboContaPagarMelhorDiaPagamento = new ComboBox();
            lblContaPagarMelhorDiaPagamento = new Label();
            dtpContaPagarDataCompra = new DateTimePicker();
            lblContaPagarDataCompra = new Label();
            txtContaPagarValor = new TextBox();
            lblContaPagarValor = new Label();
            lblContaPagarNameDescription = new Label();
            cboContaPagarAnoMesFinal = new ComboBox();
            txtContaPagarNameDescription = new TextBox();
            lblContaPagarAnoMesFinal = new Label();
            lblContaPagarCategory = new Label();
            ckbContaPagarConsideraMesmoMes = new CheckBox();
            cboContaPagarCategory = new ComboBox();
            cboContaPagarAnoMesInicial = new ComboBox();
            lblContaPagarTipoConta = new Label();
            lblContaPagarAnoMesInicial = new Label();
            cboContaPagarTipoConta = new ComboBox();
            tbpContaPagarCartaoCredito = new TabPage();
            tbpEfetuarPagamento = new TabPage();
            lblGridViewValorTotal = new Label();
            lblGridViewQuantidadeTotal = new Label();
            lblEfetuarPagamentoCategoria = new Label();
            cboEfetuarPagamentoCategoria = new ComboBox();
            lblEfetuarPagamentoInformativoDuploCliqueGrid = new Label();
            lblPagamentoInfo = new Label();
            btnPagamentoAvulso = new Button();
            btnEfetuarPagamentoBuscar = new Button();
            lblEfetuarPagamentoAnoMes = new Label();
            cboEfetuarPagamentoAnoMes = new ComboBox();
            dgvEfetuarPagamentoListagem = new DataGridView();
            tbcInitial.SuspendLayout();
            tbpContaPagarLivre.SuspendLayout();
            grbTemplateContaPagar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvContaPagar).BeginInit();
            tbpEfetuarPagamento.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEfetuarPagamentoListagem).BeginInit();
            SuspendLayout();
            // 
            // tbcInitial
            // 
            tbcInitial.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tbcInitial.Controls.Add(tbpContaPagarLivre);
            tbcInitial.Controls.Add(tbpContaPagarCartaoCredito);
            tbcInitial.Controls.Add(tbpEfetuarPagamento);
            tbcInitial.Location = new Point(0, 0);
            tbcInitial.Name = "tbcInitial";
            tbcInitial.SelectedIndex = 0;
            tbcInitial.Size = new Size(1367, 544);
            tbcInitial.TabIndex = 14;
            tbcInitial.SelectedIndexChanged += TbcInitial_SelectedIndexChanged;
            // 
            // tbpContaPagarLivre
            // 
            tbpContaPagarLivre.Controls.Add(grbTemplateContaPagar);
            tbpContaPagarLivre.Location = new Point(4, 24);
            tbpContaPagarLivre.Name = "tbpContaPagarLivre";
            tbpContaPagarLivre.Padding = new Padding(3);
            tbpContaPagarLivre.Size = new Size(1359, 516);
            tbpContaPagarLivre.TabIndex = 0;
            tbpContaPagarLivre.Text = "Conta a Pagar - Livre";
            tbpContaPagarLivre.UseVisualStyleBackColor = true;
            // 
            // grbTemplateContaPagar
            // 
            grbTemplateContaPagar.Controls.Add(btnContaPagarCadastrar);
            grbTemplateContaPagar.Controls.Add(dgvContaPagar);
            grbTemplateContaPagar.Controls.Add(grbContaPagarHistorico);
            grbTemplateContaPagar.Controls.Add(lblContaPagarDataCriacao);
            grbTemplateContaPagar.Controls.Add(rtbContaPagarMensagemAdicional);
            grbTemplateContaPagar.Controls.Add(lblContaPagarMensagemAdicional);
            grbTemplateContaPagar.Controls.Add(cboContaPagarTipoCadastro);
            grbTemplateContaPagar.Controls.Add(lblContaPagarTipoCadastro);
            grbTemplateContaPagar.Controls.Add(cboContaPagarFrequencia);
            grbTemplateContaPagar.Controls.Add(lblContaPagarFrequencia);
            grbTemplateContaPagar.Controls.Add(cboContaPagarMelhorDiaPagamento);
            grbTemplateContaPagar.Controls.Add(lblContaPagarMelhorDiaPagamento);
            grbTemplateContaPagar.Controls.Add(dtpContaPagarDataCompra);
            grbTemplateContaPagar.Controls.Add(lblContaPagarDataCompra);
            grbTemplateContaPagar.Controls.Add(txtContaPagarValor);
            grbTemplateContaPagar.Controls.Add(lblContaPagarValor);
            grbTemplateContaPagar.Controls.Add(lblContaPagarNameDescription);
            grbTemplateContaPagar.Controls.Add(cboContaPagarAnoMesFinal);
            grbTemplateContaPagar.Controls.Add(txtContaPagarNameDescription);
            grbTemplateContaPagar.Controls.Add(lblContaPagarAnoMesFinal);
            grbTemplateContaPagar.Controls.Add(lblContaPagarCategory);
            grbTemplateContaPagar.Controls.Add(ckbContaPagarConsideraMesmoMes);
            grbTemplateContaPagar.Controls.Add(cboContaPagarCategory);
            grbTemplateContaPagar.Controls.Add(cboContaPagarAnoMesInicial);
            grbTemplateContaPagar.Controls.Add(lblContaPagarTipoConta);
            grbTemplateContaPagar.Controls.Add(lblContaPagarAnoMesInicial);
            grbTemplateContaPagar.Controls.Add(cboContaPagarTipoConta);
            grbTemplateContaPagar.Location = new Point(18, 10);
            grbTemplateContaPagar.Name = "grbTemplateContaPagar";
            grbTemplateContaPagar.Size = new Size(1407, 476);
            grbTemplateContaPagar.TabIndex = 15;
            grbTemplateContaPagar.TabStop = false;
            grbTemplateContaPagar.Text = "Cadastro de Conta a Pagar - Livre";
            // 
            // btnContaPagarCadastrar
            // 
            btnContaPagarCadastrar.AutoSize = true;
            btnContaPagarCadastrar.FlatStyle = FlatStyle.System;
            btnContaPagarCadastrar.Location = new Point(960, 187);
            btnContaPagarCadastrar.Name = "btnContaPagarCadastrar";
            btnContaPagarCadastrar.Size = new Size(176, 34);
            btnContaPagarCadastrar.TabIndex = 29;
            btnContaPagarCadastrar.Text = "Cadastrar (Conta a Pagar)";
            btnContaPagarCadastrar.UseVisualStyleBackColor = true;
            btnContaPagarCadastrar.Click += BtnContaPagarCadastrar_Click;
            // 
            // dgvContaPagar
            // 
            dgvContaPagar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgvContaPagar.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgvContaPagar.BackgroundColor = SystemColors.Window;
            dgvContaPagar.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvContaPagar.Location = new Point(25, 249);
            dgvContaPagar.Name = "dgvContaPagar";
            dgvContaPagar.RowTemplate.Height = 25;
            dgvContaPagar.Size = new Size(1304, 203);
            dgvContaPagar.TabIndex = 15;
            // 
            // grbContaPagarHistorico
            // 
            grbContaPagarHistorico.Location = new Point(8, 227);
            grbContaPagarHistorico.Name = "grbContaPagarHistorico";
            grbContaPagarHistorico.Size = new Size(1327, 243);
            grbContaPagarHistorico.TabIndex = 28;
            grbContaPagarHistorico.TabStop = false;
            grbContaPagarHistorico.Text = "Histórico de Cadastros";
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
            cboContaPagarTipoCadastro.Location = new Point(652, 187);
            cboContaPagarTipoCadastro.Name = "cboContaPagarTipoCadastro";
            cboContaPagarTipoCadastro.Size = new Size(204, 23);
            cboContaPagarTipoCadastro.TabIndex = 23;
            // 
            // lblContaPagarTipoCadastro
            // 
            lblContaPagarTipoCadastro.AutoSize = true;
            lblContaPagarTipoCadastro.Location = new Point(547, 190);
            lblContaPagarTipoCadastro.Name = "lblContaPagarTipoCadastro";
            lblContaPagarTipoCadastro.Size = new Size(99, 15);
            lblContaPagarTipoCadastro.TabIndex = 22;
            lblContaPagarTipoCadastro.Text = "Tipo de Cadastro:";
            // 
            // cboContaPagarFrequencia
            // 
            cboContaPagarFrequencia.FormattingEnabled = true;
            cboContaPagarFrequencia.Location = new Point(652, 149);
            cboContaPagarFrequencia.Name = "cboContaPagarFrequencia";
            cboContaPagarFrequencia.Size = new Size(161, 23);
            cboContaPagarFrequencia.TabIndex = 21;
            // 
            // lblContaPagarFrequencia
            // 
            lblContaPagarFrequencia.AutoSize = true;
            lblContaPagarFrequencia.Location = new Point(578, 152);
            lblContaPagarFrequencia.Name = "lblContaPagarFrequencia";
            lblContaPagarFrequencia.Size = new Size(68, 15);
            lblContaPagarFrequencia.TabIndex = 20;
            lblContaPagarFrequencia.Text = "Frequência:";
            // 
            // cboContaPagarMelhorDiaPagamento
            // 
            cboContaPagarMelhorDiaPagamento.BackColor = SystemColors.Window;
            cboContaPagarMelhorDiaPagamento.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point);
            cboContaPagarMelhorDiaPagamento.FormatString = "N0";
            cboContaPagarMelhorDiaPagamento.FormattingEnabled = true;
            cboContaPagarMelhorDiaPagamento.Location = new Point(652, 111);
            cboContaPagarMelhorDiaPagamento.Name = "cboContaPagarMelhorDiaPagamento";
            cboContaPagarMelhorDiaPagamento.Size = new Size(62, 27);
            cboContaPagarMelhorDiaPagamento.TabIndex = 19;
            // 
            // lblContaPagarMelhorDiaPagamento
            // 
            lblContaPagarMelhorDiaPagamento.AutoSize = true;
            lblContaPagarMelhorDiaPagamento.Location = new Point(499, 114);
            lblContaPagarMelhorDiaPagamento.Name = "lblContaPagarMelhorDiaPagamento";
            lblContaPagarMelhorDiaPagamento.Size = new Size(147, 15);
            lblContaPagarMelhorDiaPagamento.TabIndex = 18;
            lblContaPagarMelhorDiaPagamento.Text = "Melhor dia de Pagamento:";
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
            txtContaPagarValor.Location = new Point(652, 27);
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
            lblContaPagarValor.Location = new Point(594, 30);
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
            // cboContaPagarAnoMesFinal
            // 
            cboContaPagarAnoMesFinal.FormattingEnabled = true;
            cboContaPagarAnoMesFinal.Location = new Point(116, 187);
            cboContaPagarAnoMesFinal.Name = "cboContaPagarAnoMesFinal";
            cboContaPagarAnoMesFinal.Size = new Size(161, 23);
            cboContaPagarAnoMesFinal.TabIndex = 11;
            // 
            // txtContaPagarNameDescription
            // 
            txtContaPagarNameDescription.Location = new Point(116, 32);
            txtContaPagarNameDescription.Name = "txtContaPagarNameDescription";
            txtContaPagarNameDescription.Size = new Size(445, 23);
            txtContaPagarNameDescription.TabIndex = 1;
            // 
            // lblContaPagarAnoMesFinal
            // 
            lblContaPagarAnoMesFinal.AutoSize = true;
            lblContaPagarAnoMesFinal.Location = new Point(23, 190);
            lblContaPagarAnoMesFinal.Name = "lblContaPagarAnoMesFinal";
            lblContaPagarAnoMesFinal.Size = new Size(87, 15);
            lblContaPagarAnoMesFinal.TabIndex = 10;
            lblContaPagarAnoMesFinal.Text = "Ano/Mês Final:";
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
            // ckbContaPagarConsideraMesmoMes
            // 
            ckbContaPagarConsideraMesmoMes.AutoSize = true;
            ckbContaPagarConsideraMesmoMes.Checked = true;
            ckbContaPagarConsideraMesmoMes.CheckState = CheckState.Checked;
            ckbContaPagarConsideraMesmoMes.Location = new Point(283, 159);
            ckbContaPagarConsideraMesmoMes.Name = "ckbContaPagarConsideraMesmoMes";
            ckbContaPagarConsideraMesmoMes.Size = new Size(124, 34);
            ckbContaPagarConsideraMesmoMes.TabIndex = 9;
            ckbContaPagarConsideraMesmoMes.Text = "Considera como \r\nMês Inicial e Final?";
            ckbContaPagarConsideraMesmoMes.UseVisualStyleBackColor = true;
            ckbContaPagarConsideraMesmoMes.CheckedChanged += CkbContaPagarConsideraMesmoMes_CheckedChanged;
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
            cboContaPagarAnoMesInicial.FormattingEnabled = true;
            cboContaPagarAnoMesInicial.Location = new Point(116, 151);
            cboContaPagarAnoMesInicial.Name = "cboContaPagarAnoMesInicial";
            cboContaPagarAnoMesInicial.Size = new Size(161, 23);
            cboContaPagarAnoMesInicial.TabIndex = 8;
            cboContaPagarAnoMesInicial.SelectedValueChanged += CboContaPagarAnoMesInicial_SelectedValueChanged;
            cboContaPagarAnoMesInicial.Leave += CboContaPagarAnoMesInicial_Leave;
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
            // lblContaPagarAnoMesInicial
            // 
            lblContaPagarAnoMesInicial.AutoSize = true;
            lblContaPagarAnoMesInicial.Location = new Point(19, 154);
            lblContaPagarAnoMesInicial.Name = "lblContaPagarAnoMesInicial";
            lblContaPagarAnoMesInicial.Size = new Size(93, 15);
            lblContaPagarAnoMesInicial.TabIndex = 7;
            lblContaPagarAnoMesInicial.Text = "Ano/Mês Inicial:";
            // 
            // cboContaPagarTipoConta
            // 
            cboContaPagarTipoConta.FormattingEnabled = true;
            cboContaPagarTipoConta.Location = new Point(116, 108);
            cboContaPagarTipoConta.Name = "cboContaPagarTipoConta";
            cboContaPagarTipoConta.Size = new Size(167, 23);
            cboContaPagarTipoConta.TabIndex = 5;
            // 
            // tbpContaPagarCartaoCredito
            // 
            tbpContaPagarCartaoCredito.Location = new Point(4, 24);
            tbpContaPagarCartaoCredito.Name = "tbpContaPagarCartaoCredito";
            tbpContaPagarCartaoCredito.Padding = new Padding(3);
            tbpContaPagarCartaoCredito.Size = new Size(1359, 516);
            tbpContaPagarCartaoCredito.TabIndex = 1;
            tbpContaPagarCartaoCredito.Text = "Lançar no Cartão de Crédito";
            tbpContaPagarCartaoCredito.UseVisualStyleBackColor = true;
            // 
            // tbpEfetuarPagamento
            // 
            tbpEfetuarPagamento.Controls.Add(lblGridViewValorTotal);
            tbpEfetuarPagamento.Controls.Add(lblGridViewQuantidadeTotal);
            tbpEfetuarPagamento.Controls.Add(lblEfetuarPagamentoCategoria);
            tbpEfetuarPagamento.Controls.Add(cboEfetuarPagamentoCategoria);
            tbpEfetuarPagamento.Controls.Add(lblEfetuarPagamentoInformativoDuploCliqueGrid);
            tbpEfetuarPagamento.Controls.Add(lblPagamentoInfo);
            tbpEfetuarPagamento.Controls.Add(btnPagamentoAvulso);
            tbpEfetuarPagamento.Controls.Add(btnEfetuarPagamentoBuscar);
            tbpEfetuarPagamento.Controls.Add(lblEfetuarPagamentoAnoMes);
            tbpEfetuarPagamento.Controls.Add(cboEfetuarPagamentoAnoMes);
            tbpEfetuarPagamento.Controls.Add(dgvEfetuarPagamentoListagem);
            tbpEfetuarPagamento.Location = new Point(4, 24);
            tbpEfetuarPagamento.Name = "tbpEfetuarPagamento";
            tbpEfetuarPagamento.Size = new Size(1359, 516);
            tbpEfetuarPagamento.TabIndex = 2;
            tbpEfetuarPagamento.Text = "Pagamento";
            tbpEfetuarPagamento.UseVisualStyleBackColor = true;
            // 
            // lblGridViewValorTotal
            // 
            lblGridViewValorTotal.AutoSize = true;
            lblGridViewValorTotal.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            lblGridViewValorTotal.ForeColor = Color.OrangeRed;
            lblGridViewValorTotal.Location = new Point(146, 42);
            lblGridViewValorTotal.Name = "lblGridViewValorTotal";
            lblGridViewValorTotal.Size = new Size(79, 17);
            lblGridViewValorTotal.TabIndex = 10;
            lblGridViewValorTotal.Text = "Valor Total:";
            // 
            // lblGridViewQuantidadeTotal
            // 
            lblGridViewQuantidadeTotal.AutoSize = true;
            lblGridViewQuantidadeTotal.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            lblGridViewQuantidadeTotal.ForeColor = Color.OrangeRed;
            lblGridViewQuantidadeTotal.Location = new Point(8, 42);
            lblGridViewQuantidadeTotal.Name = "lblGridViewQuantidadeTotal";
            lblGridViewQuantidadeTotal.Size = new Size(118, 17);
            lblGridViewQuantidadeTotal.TabIndex = 9;
            lblGridViewQuantidadeTotal.Text = "Quantidade Itens:";
            // 
            // lblEfetuarPagamentoCategoria
            // 
            lblEfetuarPagamentoCategoria.AutoSize = true;
            lblEfetuarPagamentoCategoria.Location = new Point(354, 17);
            lblEfetuarPagamentoCategoria.Name = "lblEfetuarPagamentoCategoria";
            lblEfetuarPagamentoCategoria.Size = new Size(61, 15);
            lblEfetuarPagamentoCategoria.TabIndex = 7;
            lblEfetuarPagamentoCategoria.Text = "Categoria:";
            // 
            // cboEfetuarPagamentoCategoria
            // 
            cboEfetuarPagamentoCategoria.FormattingEnabled = true;
            cboEfetuarPagamentoCategoria.Location = new Point(421, 14);
            cboEfetuarPagamentoCategoria.Name = "cboEfetuarPagamentoCategoria";
            cboEfetuarPagamentoCategoria.Size = new Size(188, 23);
            cboEfetuarPagamentoCategoria.TabIndex = 8;
            cboEfetuarPagamentoCategoria.SelectedValueChanged += CboEfetuarPagamentoCategoria_SelectedValueChanged;
            // 
            // lblEfetuarPagamentoInformativoDuploCliqueGrid
            // 
            lblEfetuarPagamentoInformativoDuploCliqueGrid.AutoSize = true;
            lblEfetuarPagamentoInformativoDuploCliqueGrid.Location = new Point(1020, 42);
            lblEfetuarPagamentoInformativoDuploCliqueGrid.Name = "lblEfetuarPagamentoInformativoDuploCliqueGrid";
            lblEfetuarPagamentoInformativoDuploCliqueGrid.Size = new Size(334, 15);
            lblEfetuarPagamentoInformativoDuploCliqueGrid.TabIndex = 6;
            lblEfetuarPagamentoInformativoDuploCliqueGrid.Text = "Ao efetuar duplo clique na linha do Grid abre para Pagamento";
            // 
            // lblPagamentoInfo
            // 
            lblPagamentoInfo.AutoSize = true;
            lblPagamentoInfo.Location = new Point(1069, 15);
            lblPagamentoInfo.Name = "lblPagamentoInfo";
            lblPagamentoInfo.Size = new Size(287, 15);
            lblPagamentoInfo.TabIndex = 5;
            lblPagamentoInfo.Text = "Ideal p/ Pagamento em Massa, Ex.: Cartão de Crédito";
            // 
            // btnPagamentoAvulso
            // 
            btnPagamentoAvulso.Location = new Point(951, 3);
            btnPagamentoAvulso.Name = "btnPagamentoAvulso";
            btnPagamentoAvulso.Size = new Size(112, 38);
            btnPagamentoAvulso.TabIndex = 4;
            btnPagamentoAvulso.Text = "Novo Pagamento\r\nAvulso";
            btnPagamentoAvulso.UseVisualStyleBackColor = true;
            btnPagamentoAvulso.Click += BtnPagamentoAvulso_Click;
            // 
            // btnEfetuarPagamentoBuscar
            // 
            btnEfetuarPagamentoBuscar.Location = new Point(244, 14);
            btnEfetuarPagamentoBuscar.Name = "btnEfetuarPagamentoBuscar";
            btnEfetuarPagamentoBuscar.Size = new Size(99, 23);
            btnEfetuarPagamentoBuscar.TabIndex = 3;
            btnEfetuarPagamentoBuscar.Text = "Buscar Dados";
            btnEfetuarPagamentoBuscar.UseVisualStyleBackColor = true;
            btnEfetuarPagamentoBuscar.Click += BtnEfetuarPagamentoBuscar_Click;
            // 
            // lblEfetuarPagamentoAnoMes
            // 
            lblEfetuarPagamentoAnoMes.AutoSize = true;
            lblEfetuarPagamentoAnoMes.Location = new Point(8, 18);
            lblEfetuarPagamentoAnoMes.Name = "lblEfetuarPagamentoAnoMes";
            lblEfetuarPagamentoAnoMes.Size = new Size(59, 15);
            lblEfetuarPagamentoAnoMes.TabIndex = 2;
            lblEfetuarPagamentoAnoMes.Text = "Ano/Mês:";
            // 
            // cboEfetuarPagamentoAnoMes
            // 
            cboEfetuarPagamentoAnoMes.FormattingEnabled = true;
            cboEfetuarPagamentoAnoMes.Location = new Point(73, 15);
            cboEfetuarPagamentoAnoMes.Name = "cboEfetuarPagamentoAnoMes";
            cboEfetuarPagamentoAnoMes.Size = new Size(156, 23);
            cboEfetuarPagamentoAnoMes.TabIndex = 1;
            // 
            // dgvEfetuarPagamentoListagem
            // 
            dgvEfetuarPagamentoListagem.AllowUserToOrderColumns = true;
            dgvEfetuarPagamentoListagem.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgvEfetuarPagamentoListagem.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgvEfetuarPagamentoListagem.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgvEfetuarPagamentoListagem.DefaultCellStyle = dataGridViewCellStyle1;
            dgvEfetuarPagamentoListagem.Location = new Point(8, 60);
            dgvEfetuarPagamentoListagem.Name = "dgvEfetuarPagamentoListagem";
            dgvEfetuarPagamentoListagem.RowTemplate.Height = 25;
            dgvEfetuarPagamentoListagem.Size = new Size(1418, 448);
            dgvEfetuarPagamentoListagem.TabIndex = 0;
            dgvEfetuarPagamentoListagem.CellDoubleClick += DgvEfetuarPagamentoListagem_CellDoubleClick;
            dgvEfetuarPagamentoListagem.CellMouseDown += DgvEfetuarPagamentoListagem_CellMouseDown;
            // 
            // Initial
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1370, 544);
            Controls.Add(tbcInitial);
            Name = "Initial";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Tela inicial - Organização Financeira";
            Load += Initial_Load;
            tbcInitial.ResumeLayout(false);
            tbpContaPagarLivre.ResumeLayout(false);
            grbTemplateContaPagar.ResumeLayout(false);
            grbTemplateContaPagar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvContaPagar).EndInit();
            tbpEfetuarPagamento.ResumeLayout(false);
            tbpEfetuarPagamento.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEfetuarPagamentoListagem).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private TabControl tbcInitial;
        private TabPage tbpContaPagarLivre;
        private TabPage tbpContaPagarCartaoCredito;
        private GroupBox grbTemplateContaPagar;
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
        private Label lblContaPagarAnoMesInicial;
        private ComboBox cboContaPagarTipoConta;
        private DataGridView dgvContaPagar;
        private DateTimePicker dtpContaPagarDataCompra;
        private Label lblContaPagarDataCompra;
        private Label lblContaPagarMelhorDiaPagamento;
        private ComboBox cboContaPagarMelhorDiaPagamento;
        private ComboBox cboContaPagarFrequencia;
        private Label lblContaPagarFrequencia;
        private ComboBox cboContaPagarTipoCadastro;
        private Label lblContaPagarTipoCadastro;
        private RichTextBox rtbContaPagarMensagemAdicional;
        private Label lblContaPagarMensagemAdicional;
        private Label lblContaPagarDataCriacao;
        private GroupBox grbContaPagarHistorico;
        private Button btnContaPagarCadastrar;
        private TabPage tbpEfetuarPagamento;
        private DataGridView dgvEfetuarPagamentoListagem;
        private Label lblEfetuarPagamentoAnoMes;
        private ComboBox cboEfetuarPagamentoAnoMes;
        private Button btnEfetuarPagamentoBuscar;
        private Label lblPagamentoInfo;
        private Button btnPagamentoAvulso;
        private Label lblEfetuarPagamentoInformativoDuploCliqueGrid;
        private Label lblEfetuarPagamentoCategoria;
        private ComboBox cboEfetuarPagamentoCategoria;
        private Label lblGridViewValorTotal;
        private Label lblGridViewQuantidadeTotal;
    }
}