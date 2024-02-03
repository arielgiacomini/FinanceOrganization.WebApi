﻿using App.Forms.Config;
using App.Forms.DataSource;
using App.Forms.Enums;
using App.Forms.Forms.Edição;
using App.Forms.Forms.Pay;
using App.Forms.Services;
using App.Forms.Services.Output;
using App.Forms.ViewModel;
using Domain.Entities;
using Domain.Utils;
using Newtonsoft.Json;
using System.Data;

namespace App.Forms.Forms
{
    public partial class Initial : Form
    {
        private const string TAB_PAGE_LIVRE = "tbpContaPagarLivre";
        private const string TAB_PAGE_CARTAO_CREDITO = "tbpContaPagarCartaoCredito";
        private const string TAB_PAGE_PAGAMENTO = "tbpEfetuarPagamento";
        private const string DESCRICAO_GROUP_BOX = "Cadastro de Conta a Pagar";
        private const string EH_CARTAO_CREDITO_NAIRA = "Cartão de Crédito Nubank Naíra";
        private readonly Dictionary<int, CreateBillToPayViewModel> _createBillToPayViewModels = new Dictionary<int, CreateBillToPayViewModel>();
        private IList<DgvEfetuarPagamentoListagemDataSource> _dgvEfetuarPagamentoListagemDataSource = new List<DgvEfetuarPagamentoListagemDataSource>();
        public static int CurrentIndex { get; set; } = 0;
        public decimal ValorContaPagarDigitadoTextBox { get; set; } = 0;
        public int Identifier { get; set; } = 0;
        public InfoHeader InfoHeader { get; set; } = new InfoHeader();

        public Initial(InfoHeader? infoHeader)
        {
            if (infoHeader != null)
            {
                InfoHeader = infoHeader;
            }

            InitializeComponent();
        }

        private async void Initial_Load(object sender, EventArgs e)
        {
            lblVersion.Text = InfoHeader.Version;
            lblInfoHeader.Text = AdjusteInfoHeader();
            PreencherLabelDataCriacao();
            PreencherComboBoxContaPagarCategoria();
            PreencherComboBoxContaPagarTipoConta();
            PreencherComboBoxAnoMes();
            RegraCamposAnoMes();
            CampoValor();
            TabPageIndexOne();
            PreencherContaPagarMelhorDiaPagamento();
            PreencherContaPagarFrequencia();
            PreencherContaPagarTipoCadastro();
            await BuscarListaPagamentos();
            tbcInitial.SelectedTab = tbcInitial.TabPages[(tbcInitial.TabCount) - 1];
        }

        private string AdjusteInfoHeader(DateTime? lastUpdate = null)
        {
            string? lblInfoHeaderIntern;

            if (InfoHeader.IsProductionEnvironment)
            {
                lblInfoHeader.BackColor = Color.OrangeRed;
                lblInfoHeader.ForeColor = Color.White;
                lblVersion.BackColor = Color.OrangeRed;
                lblVersion.ForeColor = Color.White;
                lblInfoHeaderIntern = string.Concat("CFM - PRODUÇÃO", " - ", InfoHeader.Url, " - ", "Última Busca: ", lastUpdate ?? DateTime.Now);
            }
            else
            {
                lblInfoHeader.BackColor = Color.DarkGreen;
                lblInfoHeader.ForeColor = Color.White;
                lblVersion.BackColor = Color.DarkGreen;
                lblVersion.ForeColor = Color.White;
                lblInfoHeaderIntern = string.Concat("CFM - HOMOLOGAÇÃO", " - ", InfoHeader.Url, " - ", "Última Busca: ", lastUpdate ?? DateTime.Now);
            }

            return lblInfoHeaderIntern;
        }

        private async void BtnContaPagarCadastrar_Click(object sender, EventArgs e)
        {
            Identifier++;

            var createBillToPay = new CreateBillToPayViewModel
            {
                Name = txtContaPagarNameDescription.Text,
                Account = cboContaPagarTipoConta.Text,
                Frequence = cboContaPagarFrequencia.Text,
                RegistrationType = cboContaPagarTipoCadastro.Text,
                InitialMonthYear = cboContaPagarAnoMesInicial.Text,
                FynallyMonthYear = cboContaPagarAnoMesFinal.Text,
                Category = cboContaPagarCategory.Text,
                Value = Convert.ToDecimal(txtContaPagarValor.Text.Replace("R$ ", "")),
                PurchaseDate = Convert.ToDateTime(dtpContaPagarDataCompra.Text),
                BestPayDay = Convert.ToInt32(cboContaPagarMelhorDiaPagamento.Text),
                AdditionalMessage = rtbContaPagarMensagemAdicional.Text,
                CreationDate = DateTime.Now,
                LastChangeDate = null,
                Status = RegistrationStatus.AwaitRequestAPI
            };

            _createBillToPayViewModels.Add(Identifier, createBillToPay);

            UpdateDataGridView();

            var result = await BillToPayServices.CreateBillToPay(createBillToPay);

            _createBillToPayViewModels[Identifier].Status = RegistrationStatus.AwaitResponseAPI;

            UpdateDataGridView();

            TratamentoOutput(Identifier, result);
        }

        private void TratamentoOutput(int identifier, CreateBillToPayOutput result)
        {
            if (result.Output.Status == OutputStatus.Success)
            {
                MessageBox.Show(result.Output.Message,
                    "Cadastro de Conta a Pagar Realizado com Sucesso.",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                _createBillToPayViewModels[identifier].Status = RegistrationStatus.Created;
                UpdateDataGridView();
            }
            else
            {
                _createBillToPayViewModels[identifier].Status = RegistrationStatus.RegistrationError;
                UpdateDataGridView();

                var information = string.Empty;

                var errors = result.Output.Errors;
                var validations = result.Output.Validations;

                foreach (var error in errors)
                {
                    information = string
                        .Concat(information, error.Key, " - ", error.Value, " | ");
                }

                foreach (var validation in validations)
                {
                    information = string
                        .Concat(information, validation.Key, " - ", validation.Value, " | ");
                }

                MessageBox.Show(information, "Erro ao tentar cadastrar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateDataGridView()
        {
            dgvContaPagar.DataSource = MapCreateBillToPayViewModelToDataSource(_createBillToPayViewModels);
            dgvContaPagar.Columns[0].HeaderText = "Descrição";
            dgvContaPagar.Columns[1].HeaderText = "Conta";
            dgvContaPagar.Columns[2].HeaderText = "Frequência";
            dgvContaPagar.Columns[3].HeaderText = "Tipo";
            dgvContaPagar.Columns[4].HeaderText = "Inicial";
            dgvContaPagar.Columns[5].HeaderText = "Final";
            dgvContaPagar.Columns[6].HeaderText = "Categoria";
            dgvContaPagar.Columns[7].HeaderText = "Valor";
            dgvContaPagar.Columns[8].HeaderText = "Data de Compra";
            dgvContaPagar.Columns[9].HeaderText = "Melhor Dia";
            dgvContaPagar.Columns[10].HeaderText = "Mensagem";
            dgvContaPagar.Columns[11].HeaderText = "Status";
        }

        private static IList<DgvContaPagarDataSource> MapCreateBillToPayViewModelToDataSource(Dictionary<int, CreateBillToPayViewModel> billToPayViewModel)
        {
            IList<DgvContaPagarDataSource> list = new List<DgvContaPagarDataSource>();
            foreach (var item in billToPayViewModel.Values)
            {
                var dgvContaPagarDataSource = new DgvContaPagarDataSource()
                {
                    Name = item.Name,
                    Account = item.Account,
                    Frequence = item.Frequence,
                    RegistrationType = item.RegistrationType,
                    InitialMonthYear = item.InitialMonthYear,
                    FynallyMonthYear = item.FynallyMonthYear,
                    Category = item.Category,
                    Value = item.Value,
                    PurchaseDate = item.PurchaseDate,
                    BestPayDay = item.BestPayDay,
                    AdditionalMessage = item.AdditionalMessage,
                    Status = item.Status.ToString()
                };

                list.Add(dgvContaPagarDataSource);
            }

            return list;
        }

        private void PreencherLabelDataCriacao()
        {
            string texto = "Data de Criação: ";
            lblContaPagarDataCriacao.Text = string.Concat(texto, DateTime.Now);
        }

        private void PreencherComboBoxContaPagarCategoria(string tabPageName = null, string categorySelected = null)
        {
            Dictionary<int, string> categoriasContaPagar = new()
            {
                { 0, "Nenhum" },
                { 1, "Alimentação:Almoço" },
                { 2, "Alimentação:Besteiras" },
                { 3, "Alimentação:Café da Manhã" },
                { 4, "Alimentação:Feira" },
                { 5, "Alimentação:Jantar" },
                { 6, "Alimentação:Mercado" },
                { 7, "Automóvel:Combustível" },
                { 8, "Automóvel:Estacionamento" },
                { 9, "Automóvel:Garagem" },
                { 10, "Automóvel:Seguro" },
                { 11, "Casa:Essencial" },
                { 12, "Casa:Financiamento" },
                { 13, "Casa:Internet" },
                { 14, "Casa:Segurança" },
                { 15, "Dizimo" },
                { 16, "Esporte:Helena" },
                { 17, "Esporte:TaxaMatricula" },
                { 18, "Filhos:Helena" },
                { 19, "Filhos:Leo" },
                { 20, "Gasto de Terceiros" },
                { 21, "Serviço:Cloud" },
                { 22, "Serviço:Produtividade" },
                { 23, "Serviço:Streaming" },
                { 24, "Transporte:Escolar" },
                { 25, "Alimentação:Açougue" },
                { 26, "Automóvel:Pedágio" },
                { 27, "Viagem" },
                { 28, "Automóvel:Documentação" },
                { 29, "Cabeleleiro:Ariel" },
                { 30, "Cabeleleiro:Naira" },
                { 31, "Cabeleleiro:Helena" },
                { 32, "Cabeleleiro:Leo" }
            };

            var categoriasContaPagarOrderBy = categoriasContaPagar
                .OrderBy(x => x.Value)
                .ToList();

            foreach (var item in categoriasContaPagarOrderBy)
            {
                cboContaPagarCategory.Items.Add(item.Value);
                cboEfetuarPagamentoCategoria.Items.Add(item.Value);
            }

            if (categorySelected == null)
            {
                cboContaPagarCategory.SelectedItem = categoriasContaPagarOrderBy.FirstOrDefault().Value;
                cboEfetuarPagamentoCategoria.SelectedItem = categoriasContaPagarOrderBy.FirstOrDefault().Value;
            }
            else
            {
                var theChoise = categoriasContaPagarOrderBy
                    .FirstOrDefault(x => x.Value == categorySelected);

                if (theChoise.Value.Length > 0)
                {
                    cboContaPagarCategory.SelectedItem = theChoise.Value;
                    cboEfetuarPagamentoCategoria.SelectedItem = theChoise.Value;
                }
                else
                {
                    var dado = categoriasContaPagarOrderBy
                        .FirstOrDefault().Value;

                    cboContaPagarCategory.SelectedItem = dado;
                    cboEfetuarPagamentoCategoria.SelectedItem = dado;
                }
            }
        }

        private void PreencherComboBoxContaPagarTipoConta(string tabPageName = null, string accountSelected = null)
        {
            Dictionary<int, string> tipoConta = new()
            {
                { 1, "Cartão de Crédito" },
                { 2, "Cartão de Débito" },
                { 3, "Cartão VA" },
                { 4, "Cartão VR" },
                { 5, "Itaú" }
            };

            foreach (var item in tipoConta)
            {
                cboContaPagarTipoConta.Items.Add(item.Value);
            }

            if (accountSelected == null)
            {
                cboContaPagarTipoConta.SelectedItem = tipoConta.FirstOrDefault().Value;
            }
            else
            {
                var theChoise = tipoConta.FirstOrDefault(x => x.Value == accountSelected);

                if (theChoise.Value.Length > 0)
                {
                    cboContaPagarTipoConta.SelectedItem = theChoise.Value;
                }
                else
                {
                    cboContaPagarTipoConta.SelectedItem = tipoConta.FirstOrDefault().Value;
                }
            }
        }

        private void PreencherComboBoxAnoMes()
        {
            var yearMonths = DateServiceUtils.GetListYearMonthsByThreeMonthsBeforeAndTwentyFourAfter();
            var yearMonthsArray = yearMonths.Values.ToArray();

            cboContaPagarAnoMesInicial.Items.AddRange(yearMonthsArray);
            cboContaPagarAnoMesFinal.Items.AddRange(yearMonthsArray);
            cboEfetuarPagamentoAnoMes.Items.AddRange(yearMonthsArray);

            _ = yearMonths.TryGetValue(3, out string? currentYearMont);

            cboContaPagarAnoMesInicial.SelectedItem = currentYearMont;
            cboContaPagarAnoMesFinal.SelectedItem = currentYearMont;
            cboEfetuarPagamentoAnoMes.SelectedItem = currentYearMont;
        }

        private void PreencherContaPagarMelhorDiaPagamento()
        {
            IList<int> bestPayDay = new List<int>();

            for (int day = 1; day <= 31; day++)
            {
                bestPayDay.Add(day);
                cboContaPagarMelhorDiaPagamento.Items.Add(day);
            }

            cboContaPagarMelhorDiaPagamento.SelectedItem = DateTime.Now.Day;
        }

        private void PreencherContaPagarFrequencia(string tabPageName = null, string frequenciaSelected = null)
        {
            Dictionary<int, string> frequencia = new()
            {
                { 1, "Livre" },
                { 2, "Mensal" },
                { 3, "Mensal:Recorrente" }
            };

            foreach (var item in frequencia)
            {
                cboContaPagarFrequencia.Items.Add(item.Value);
            }

            if (frequenciaSelected == null)
            {
                cboContaPagarFrequencia.SelectedItem = frequencia.FirstOrDefault().Value;
            }
            else
            {
                var theChoise = frequencia.FirstOrDefault(x => x.Value == frequenciaSelected);

                if (theChoise.Value.Length > 0)
                {
                    cboContaPagarFrequencia.SelectedItem = theChoise.Value;
                }
                else
                {
                    cboContaPagarFrequencia.SelectedItem = frequencia.FirstOrDefault().Value;
                }
            }
        }

        private void PreencherContaPagarTipoCadastro(string tabPageName = null, string tipoCadastroSelected = null)
        {
            Dictionary<int, string> tipoCadastro = new()
            {
                { 1, "Compra Livre" },
                { 2, "Conta/Fatura Fixa" }
            };

            foreach (var item in tipoCadastro)
            {
                cboContaPagarTipoCadastro.Items.Add(item.Value);
            }

            if (tipoCadastroSelected == null)
            {
                cboContaPagarTipoCadastro.SelectedItem = tipoCadastro.FirstOrDefault().Value;
            }
            else
            {
                var theChoise = tipoCadastro.FirstOrDefault(x => x.Value == tipoCadastroSelected);

                if (theChoise.Value.Length > 0)
                {
                    cboContaPagarTipoCadastro.SelectedItem = theChoise.Value;
                }
                else
                {
                    cboContaPagarTipoCadastro.SelectedItem = tipoCadastro.FirstOrDefault().Value;
                }
            }
        }

        private void CkbContaPagarConsideraMesmoMes_CheckedChanged(object sender, EventArgs e)
        {
            RegraCamposAnoMes();
        }

        private void RegraCamposAnoMes()
        {
            if (ckbContaPagarConsideraMesmoMes.Checked)
            {
                cboContaPagarAnoMesFinal.SelectedItem = cboContaPagarAnoMesInicial.SelectedItem;
                cboContaPagarAnoMesFinal.Enabled = false;
            }
            else
            {
                cboContaPagarAnoMesFinal.Enabled = true;
            }
        }

        private void CampoValor()
        {
            txtContaPagarValor.Text = Convert.ToDecimal("0").ToString("C");
        }

        private void CboContaPagarAnoMesInicial_Leave(object sender, EventArgs e)
        {
            RegraCamposAnoMes();
        }

        private void CboContaPagarAnoMesInicial_SelectedValueChanged(object sender, EventArgs e)
        {
            RegraCamposAnoMes();
        }

        private void TbcInitial_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tabPageCurrent = tbcInitial.SelectedTab;
            var tabPageCurrentText = tabPageCurrent.Text;
            switch (tabPageCurrent.Name)
            {
                case TAB_PAGE_LIVRE:
                    SetParameters(tabPageCurrentText, "Dizimo", "Cartão de Débito", "Livre");
                    grbTemplateContaPagar.Text = string.Concat(DESCRICAO_GROUP_BOX, " - ", tabPageCurrentText);
                    break;
                case TAB_PAGE_CARTAO_CREDITO:
                    SetParameters(tabPageCurrentText, "Alimentação:Café da Manhã", "Cartão de Crédito", "Mensal");
                    grbTemplateContaPagar.Text = string.Concat(DESCRICAO_GROUP_BOX, " - ", tabPageCurrentText);
                    break;
                case TAB_PAGE_PAGAMENTO:
                    SetParameters(tabPageCurrentText, "Nenhum");
                    break;
                default:
                    break;
            }

            if (TabPageCurrentIsFormWithTemplate())
            {
                PreencherLabelDataCriacao();
                tbcInitial.TabPages[tbcInitial.SelectedIndex].Controls.Add(grbTemplateContaPagar);
            }
        }

        private bool TabPageCurrentIsFormWithTemplate()
        {
            return
                   tbcInitial.TabPages[tbcInitial.SelectedIndex].Name == TAB_PAGE_LIVRE
                || tbcInitial.TabPages[tbcInitial.SelectedIndex].Name == TAB_PAGE_CARTAO_CREDITO;
        }

        private void TabPageIndexOne()
        {
            tbcInitial.TabPages[tbcInitial.SelectedIndex].Controls.Add(grbTemplateContaPagar);
        }

        private void SetParameters(string tabPageName, string? category = null, string? account = null, string? frequencia = null)
        {
            cboContaPagarCategory.Items.Clear();
            cboContaPagarTipoConta.Items.Clear();
            PreencherComboBoxContaPagarCategoria(tabPageName, category);
            PreencherComboBoxContaPagarTipoConta(tabPageName, account);
        }

        private async void BtnEfetuarPagamentoBuscar_Click(object sender, EventArgs e)
        {
            lblInfoHeader.Text = AdjusteInfoHeader(DateTime.Now);
            await BuscarListaPagamentos();
        }

        private async Task BuscarListaPagamentos()
        {
            _dgvEfetuarPagamentoListagemDataSource.Clear();
            cboEfetuarPagamentoCategoria.SelectedItem = "Nenhum";

            SearchBillToPayViewModel search = new()
            {
                YearMonth = cboEfetuarPagamentoAnoMes.Text
            };

            await PreencherEfetuarPagamentoDataGridViewHistory(search);
        }

        private async Task PreencherEfetuarPagamentoDataGridViewHistory(SearchBillToPayViewModel search)
        {
            var resultSearch = await BillToPayServices.SearchBillToPay(search);

            var dataSource = MapSearchResultToDataSource(resultSearch);

            var dataSourceOrderBy = dataSource
                .OrderBy(hasPay => hasPay.HasPay)
                .ThenBy(creditCard => creditCard.Account == Account.CARTAO_CREDITO)
                .ThenBy(dueDate => dueDate.DueDate)
                .ThenByDescending(purchase => purchase.PurchaseDate)
                .ToList();

            PreecherDataGridViewEfetuarPagamento(dataSourceOrderBy);
        }

        private void PreecherDataGridViewEfetuarPagamento(IList<DgvEfetuarPagamentoListagemDataSource> dataSourceOrderBy)
        {
            Consolidate(dataSourceOrderBy);

            _dgvEfetuarPagamentoListagemDataSource = dataSourceOrderBy;

            dgvEfetuarPagamentoListagem.DataSource = dataSourceOrderBy;
            dgvEfetuarPagamentoListagem.Columns[0].HeaderText = "Id";
            dgvEfetuarPagamentoListagem.Columns[0].Visible = false;
            dgvEfetuarPagamentoListagem.Columns[1].HeaderText = "Id da tabela pai";
            dgvEfetuarPagamentoListagem.Columns[1].Visible = false;
            dgvEfetuarPagamentoListagem.Columns[2].HeaderText = "Conta";
            dgvEfetuarPagamentoListagem.Columns[3].HeaderText = "Descrição";
            dgvEfetuarPagamentoListagem.Columns[4].HeaderText = "Categoria";
            dgvEfetuarPagamentoListagem.Columns[5].HeaderText = "Valor";
            dgvEfetuarPagamentoListagem.Columns[5].DefaultCellStyle.Format = "C2";
            dgvEfetuarPagamentoListagem.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvEfetuarPagamentoListagem.Columns[6].HeaderText = "Data de Compra";
            dgvEfetuarPagamentoListagem.Columns[7].HeaderText = "Vencimento";
            dgvEfetuarPagamentoListagem.Columns[8].HeaderText = "Mês/Ano";
            dgvEfetuarPagamentoListagem.Columns[9].HeaderText = "Frequência";
            dgvEfetuarPagamentoListagem.Columns[10].HeaderText = "Tipo";
            dgvEfetuarPagamentoListagem.Columns[11].HeaderText = "Data de Pagamento";
            dgvEfetuarPagamentoListagem.Columns[12].HeaderText = "Pago?";
            dgvEfetuarPagamentoListagem.Columns[13].HeaderText = "Mensagem";
            dgvEfetuarPagamentoListagem.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvEfetuarPagamentoListagem.Columns[14].HeaderText = "Data de Criação";
            dgvEfetuarPagamentoListagem.Columns[14].Visible = false;
            dgvEfetuarPagamentoListagem.Columns[15].HeaderText = "Data de Alteração";
            dgvEfetuarPagamentoListagem.Columns[15].Visible = false;
        }

        private void Consolidate(IList<DgvEfetuarPagamentoListagemDataSource> dataSourceOrderBy)
        {
            lblGridViewTotais.Text = string
                .Concat("Total: ", dataSourceOrderBy.Count, " - ", "R$ ", string
                .Format("{0:#,##0.00}", Convert.ToDecimal(dataSourceOrderBy.Sum(x => x.Value))));

            lblGridViewTotalPago.Text = string
                .Concat("Pago: ", dataSourceOrderBy.Count(pay => pay.HasPay), " - ", "R$ ", string
                .Format("{0:#,##0.00}", Convert.ToDecimal(dataSourceOrderBy
                .Where(pay => pay.HasPay)
                .Sum(x => x.Value))));

            lblGridViewCartaoCreditoFamilia.Text = string
                .Concat("Cartão de Crédito Família: ", dataSourceOrderBy
                .Count(creditCardFamily => creditCardFamily.Account == Account.CARTAO_CREDITO
                    && !(creditCardFamily.AdditionalMessage != null
                     && creditCardFamily.AdditionalMessage.ToString().StartsWith(EH_CARTAO_CREDITO_NAIRA))), " - ", "R$ ", string
                .Format("{0:#,##0.00}", Convert
                .ToDecimal(dataSourceOrderBy
                .Where(creditCardFamily => creditCardFamily.Account == Account.CARTAO_CREDITO
                    && !(creditCardFamily.AdditionalMessage != null
                     && creditCardFamily.AdditionalMessage.ToString().StartsWith(EH_CARTAO_CREDITO_NAIRA)))
                .Sum(x => x.Value))));

            lblGridViewCartaoCreditoNaira.Text = string
                .Concat("Cartão de Crédito Naíra: ", dataSourceOrderBy
                .Count(creditCardNaira => creditCardNaira.Account == Account.CARTAO_CREDITO
                    && creditCardNaira.AdditionalMessage != null && creditCardNaira.AdditionalMessage.ToString()
                .StartsWith(EH_CARTAO_CREDITO_NAIRA)), " - ", "R$ ", string
                .Format("{0:#,##0.00}", Convert
                .ToDecimal(dataSourceOrderBy
                .Where(creditCardNaira => creditCardNaira.Account == Account.CARTAO_CREDITO
                    && creditCardNaira.AdditionalMessage != null && creditCardNaira.AdditionalMessage.ToString()
                .StartsWith(EH_CARTAO_CREDITO_NAIRA))
                .Sum(x => x.Value))));
        }

        private static IList<DgvEfetuarPagamentoListagemDataSource> MapSearchResultToDataSource(SearchBillToPayOutput searchBillToPayOutput)
        {
            IList<DgvEfetuarPagamentoListagemDataSource> dgvEfetuarPagamentoListagemDataSources = new List<DgvEfetuarPagamentoListagemDataSource>();

            if (searchBillToPayOutput.Output == null || searchBillToPayOutput.Output.Data == null)
            {
                return dgvEfetuarPagamentoListagemDataSources;
            }

            var dados = searchBillToPayOutput.Output.Data;

            var json = JsonConvert.SerializeObject(dados);

            var conversion = JsonConvert.DeserializeObject<IList<DgvEfetuarPagamentoListagemDataSource>>(json);

            foreach (var item in conversion!)
            {
                dgvEfetuarPagamentoListagemDataSources.Add(item);
            }

            return dgvEfetuarPagamentoListagemDataSources;
        }

        private void DgvEfetuarPagamentoListagem_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _ = Guid.TryParse(dgvEfetuarPagamentoListagem.Rows[e.RowIndex].Cells[0].Value.ToString(), out Guid identificadorContaPagar);
                var conta = dgvEfetuarPagamentoListagem.Rows[e.RowIndex].Cells[2].Value.ToString();
                var descricao = dgvEfetuarPagamentoListagem.Rows[e.RowIndex].Cells[3].Value.ToString();
                var valorOfDgv = dgvEfetuarPagamentoListagem.Rows[e.RowIndex].Cells[5].Value?.ToString()?.Replace("R$ ", "") ?? "0";
                var valor = Convert.ToDecimal(valorOfDgv);
                var mesAno = dgvEfetuarPagamentoListagem.Rows[e.RowIndex].Cells[8].Value.ToString();
                var additionalMessage = dgvEfetuarPagamentoListagem.Rows[e.RowIndex].Cells[13].Value.ToString();

                FrmPagamento frmPagamento = new()
                {
                    IdentificadorUnicoContaPagar = identificadorContaPagar,
                    Nome = descricao,
                    Conta = conta,
                    AnoMes = mesAno,
                    Valor = valor,
                    AdditionalMessage = additionalMessage
                };

                frmPagamento.ShowDialog();
            }
        }

        private void BtnPagamentoAvulso_Click(object sender, EventArgs e)
        {
            FrmPagamento frmPagamento = new();
            frmPagamento.ShowDialog();
        }

        private void CboEfetuarPagamentoCategoria_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboEfetuarPagamentoCategoria.Text != "Nenhum")
            {
                var filterByCategory = _dgvEfetuarPagamentoListagemDataSource
                    .Where(x => x.Category == cboEfetuarPagamentoCategoria.Text)
                    .ToList();

                PreecherDataGridViewEfetuarPagamento(filterByCategory);
            }
            else
            {
                PreecherDataGridViewEfetuarPagamento(_dgvEfetuarPagamentoListagemDataSource);
            }
        }

        private void TxtContaPagarValor_Enter(object sender, EventArgs e)
        {
            txtContaPagarValor.Text = "";
        }

        private void TxtContaPagarValor_Leave(object sender, EventArgs e)
        {
            ValorContaPagarDigitadoTextBox = StringDecimalUtils
                .TranslateStringEmDecimal(txtContaPagarValor.Text);

            txtContaPagarValor.Text = StringDecimalUtils
                .TranslateValorEmStringDinheiro(txtContaPagarValor.Text);
        }

        private void DgvEfetuarPagamentoListagem_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                _ = Guid.TryParse(dgvEfetuarPagamentoListagem.Rows[e.RowIndex].Cells[0].Value.ToString(), out Guid identificadorContaPagar);

                if (identificadorContaPagar == Guid.Empty)
                {
                    MessageBox.Show("Não encontramos o Identificador da Conta pagar conseguir editar.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var editBillToPayViewModel = new EditBillToPayViewModel
                {
                    Id = identificadorContaPagar,
                    IdFixedInvoice = Convert.ToInt32(dgvEfetuarPagamentoListagem.Rows[e.RowIndex].Cells[1].Value?.ToString()),
                    Account = dgvEfetuarPagamentoListagem.Rows[e.RowIndex].Cells[2].Value?.ToString(),
                    Name = dgvEfetuarPagamentoListagem.Rows[e.RowIndex].Cells[3].Value?.ToString(),
                    Category = dgvEfetuarPagamentoListagem.Rows[e.RowIndex].Cells[4].Value?.ToString(),
                    Value = Convert.ToDecimal(dgvEfetuarPagamentoListagem.Rows[e.RowIndex].Cells[5].Value?.ToString()?.Replace("R$ ", "") ?? "0"),
                    PurchaseDate = DateServiceUtils.GetDateTimeOfString(dgvEfetuarPagamentoListagem.Rows[e.RowIndex].Cells[6].Value?.ToString()),
                    DueDate = DateServiceUtils.GetDateTimeOfString(dgvEfetuarPagamentoListagem.Rows[e.RowIndex].Cells[7].Value?.ToString())!.Value,
                    YearMonth = dgvEfetuarPagamentoListagem.Rows[e.RowIndex].Cells[8].Value?.ToString(),
                    Frequence = dgvEfetuarPagamentoListagem.Rows[e.RowIndex].Cells[9].Value?.ToString(),
                    RegistrationType = dgvEfetuarPagamentoListagem.Rows[e.RowIndex].Cells[10].Value?.ToString(),
                    PayDay = dgvEfetuarPagamentoListagem.Rows[e.RowIndex].Cells[11].Value?.ToString(),
                    HasPay = Convert.ToBoolean(dgvEfetuarPagamentoListagem.Rows[e.RowIndex].Cells[12].Value?.ToString()),
                    AdditionalMessage = dgvEfetuarPagamentoListagem.Rows[e.RowIndex].Cells[13].Value?.ToString(),
                    LastChangeDate = DateTime.Now
                };

                FrmEdit frmPagamento = new()
                {
                    EditBillToPayViewModel = editBillToPayViewModel
                };

                frmPagamento.ShowDialog();
            }
        }

        private static bool ContaPagarVencida(DataGridViewRow row)
        {
            return Convert.ToDateTime(row.Cells[7].Value) <= DateTime.Now;
        }

        private static bool ContaPagarVencimentoProximo(DataGridViewRow row)
        {
            bool vencimentoProximo = false;
            var now = DateTime.Now;
            var nowMoreFiveDays = now.AddDays(5);
            var dataVencimento = Convert.ToDateTime(row.Cells[7].Value?.ToString());
            var descricao = row.Cells[3].Value?.ToString();
            var index = row.Index;

            if (dataVencimento >= now && dataVencimento <= nowMoreFiveDays)
            {
                vencimentoProximo = true;
            }

            return vencimentoProximo;
        }

        private static void SetColorRows(DataGridViewRow row, Color backColor, Color foreColor)
        {
            var columnsCount = row.Cells.Count;

            for (int i = 0; i < columnsCount; i++)
            {
                row.Cells[i].Style.BackColor = backColor;
                row.Cells[i].Style.ForeColor = foreColor;
            }
        }

        private void DgvEfetuarPagamentoListagem_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            foreach (DataGridViewRow row in dgvEfetuarPagamentoListagem.Rows)
            {
                if (Convert.ToBoolean(row.Cells[12].Value))
                {
                    SetColorRows(row, Color.DarkGreen, Color.White);
                }

                if (row.Cells[2].Value.ToString() == Account.CARTAO_CREDITO && !Convert.ToBoolean(row.Cells[12].Value))
                {
                    SetColorRows(row, Color.DarkOrange, Color.Black);
                }

                if (!string.IsNullOrWhiteSpace(row.Cells[13].Value?.ToString())
                    && row.Cells[13].Value.ToString()!.StartsWith(EH_CARTAO_CREDITO_NAIRA))
                {
                    SetColorRows(row, Color.DimGray, Color.White);
                }
            }
        }

        private void DgvContaPagar_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            foreach (DataGridViewRow row in dgvContaPagar.Rows)
            {
                if (row.Cells[11].Value == RegistrationStatus.AwaitRequestAPI.ToString())
                {
                    SetColorRows(row, Color.Yellow, Color.Black);
                }

                if (row.Cells[11].Value == RegistrationStatus.AwaitResponseAPI.ToString())
                {
                    SetColorRows(row, Color.DarkOrange, Color.Black);
                }

                if (row.Cells[11].Value == RegistrationStatus.Created.ToString())
                {
                    SetColorRows(row, Color.DarkGreen, Color.White);
                }

                if (row.Cells[11].Value == RegistrationStatus.RegistrationError.ToString())
                {
                    SetColorRows(row, Color.DarkRed, Color.White);
                }
            }
        }
    }
}