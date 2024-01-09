using App.Forms.ViewModel;
using Domain.Utils;

namespace App.Forms.Forms
{
    public partial class Initial : Form
    {
        private const string TAB_PAGE_LIVRE = "tbpLivre";
        private const string DESCRICAO_GROUP_BOX = "Cadastro de Conta a Pagar";

        public Initial()
        {
            InitializeComponent();
        }

        private void Initial_Load(object sender, EventArgs e)
        {
            PreencherLabelDataCriacao();
            PreencherComboBoxContaPagarCategory();
            PreencherComboBoxContaPagarTipoConta();
            PreencherComboBoxAnoMes();
            RegraCamposAnoMes();
            CampoValor();
            TabPageIndexOne();
            PreencherContaPagarMelhorDiaPagamento();
            PreencherContaPagarFrequencia();
        }

        private void BtnContaPagarCadastrar_Click(object sender, EventArgs e)
        {
            BillToPayViewModel billToPayViewModel = new();
            billToPayViewModel.Name = txtContaPagarNameDescription.Text;
            billToPayViewModel.Account = cboContaPagarTipoConta.Text;
            billToPayViewModel.Frequence = cboContaPagarFrequencia.Text;
            billToPayViewModel.RegistrationType = cboContaPagarTipoCadastro.Text;
            billToPayViewModel.InitialMonthYear = cboContaPagarAnoMesInicial.Text;
            billToPayViewModel.FynallyMonthYear = cboContaPagarAnoMesFinal.Text;
            billToPayViewModel.Category = cboContaPagarCategory.Text;
            billToPayViewModel.Value = Convert.ToDecimal(txtContaPagarValor.Text.Replace("R$ ", ""));
            billToPayViewModel.PurchaseDate = Convert.ToDateTime(dtpContaPagarDataCompra.Text);
            billToPayViewModel.BestPayDay = Convert.ToInt32(cboContaPagarMelhorDiaPagamento.Text);
            billToPayViewModel.AdditionalMessage = rtbContaPagarMensagemAdicional.Text;
            billToPayViewModel.CreationDate = DateTime.Now;
            billToPayViewModel.LastChangeDate = null;
        }

        private void PreencherLabelDataCriacao()
        {
            string texto = "Data de Criação: ";
            lblContaPagarDataCriacao.Text = string.Concat(texto, DateTime.Now);
        }

        private void PreencherComboBoxContaPagarCategory(string tabPageName = null, string categorySelected = null)
        {
            Dictionary<int, string> categoriasContaPagar = new()
            {
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
                { 24, "Transporte:Escolar" }
            };

            foreach (var item in categoriasContaPagar)
            {
                cboContaPagarCategory.Items.Add(item.Value);
            }

            if (categorySelected == null)
            {
                cboContaPagarCategory.SelectedItem = categoriasContaPagar.FirstOrDefault().Value;
            }
            else
            {
                var theChoise = categoriasContaPagar.FirstOrDefault(x => x.Value == categorySelected);

                if (theChoise.Value.Length > 0)
                {
                    cboContaPagarCategory.SelectedItem = theChoise.Value;
                }
                else
                {
                    cboContaPagarCategory.SelectedItem = categoriasContaPagar.FirstOrDefault().Value;
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
            var dateTimeCorteInitial = DateTime.Now.AddMonths(-3);

            DateTime firstDate = new(dateTimeCorteInitial.Year, dateTimeCorteInitial.Month, 1, 0, 0, 0, DateTimeKind.Local);

            Dictionary<int, string> yearMonths = new();

            for (int i = 0; i < 27; i++)
            {
                var dateAdd = firstDate.AddMonths(i);

                var yearMonthAdd = DateServiceUtils.GetYearMonthPortugueseByDateTime(dateAdd);

                yearMonths.Add(i, yearMonthAdd);

                cboContaPagarAnoMesInicial.Items.Add(yearMonthAdd);
                cboContaPagarAnoMesFinal.Items.Add(yearMonthAdd);
            }

            _ = yearMonths.TryGetValue(3, out string currentYearMont);

            cboContaPagarAnoMesInicial.SelectedItem = currentYearMont;
            cboContaPagarAnoMesFinal.SelectedItem = currentYearMont;
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

        private void CboContaPagarAnoMesInicial_Leave_1(object sender, EventArgs e)
        {
            RegraCamposAnoMes();
        }

        private void CboContaPagarAnoMesInicial_SelectedValueChanged_1(object sender, EventArgs e)
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
                case "tbpCartaoCredito":
                    SetParameters(tabPageCurrentText, "Alimentação:Café da Manhã", "Cartão de Crédito", "Mensal");
                    grbTemplateContaPagar.Text = string.Concat(DESCRICAO_GROUP_BOX, " - ", tabPageCurrentText);
                    break;
                default:
                    break;
            }

            PreencherLabelDataCriacao();
            tbcInitial.TabPages[tbcInitial.SelectedIndex].Controls.Add(grbTemplateContaPagar);
        }

        private void TabPageIndexOne()
        {
            tbcInitial.TabPages[tbcInitial.SelectedIndex].Controls.Add(grbTemplateContaPagar);
        }

        private void SetParameters(string tabPageName, string category, string account, string frequencia)
        {
            cboContaPagarCategory.Items.Clear();
            cboContaPagarTipoConta.Items.Clear();
            PreencherComboBoxContaPagarCategory(tabPageName, category);
            PreencherComboBoxContaPagarTipoConta(tabPageName, account);
            PreencherContaPagarFrequencia(tabPageName, frequencia);
        }
    }
}