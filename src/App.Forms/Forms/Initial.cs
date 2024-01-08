using Domain.Utils;

namespace App.Forms.Forms
{
    public partial class Initial : Form
    {
        private const string TAB_PAGE_LIVRE = "tbpLivre";

        public Initial()
        {
            InitializeComponent();
        }

        private void Initial_Load(object sender, EventArgs e)
        {
            PreencherComboBoxContaPagarCategory();
            PreencherComboBoxContaPagarTipoConta();
            PreencherComboBoxAnoMes();
            RegraCamposAnoMes();
            CampoValor();
            TabPageIndexOne();
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
                    SetParametersDefaultPorTipoDeContaECategoria(tabPageCurrentText, "Dizimo", "Cartão de Débito");
                    break;
                case "tbpCartaoCredito":
                    SetParametersDefaultPorTipoDeContaECategoria(tabPageCurrentText, "Alimentação:Café da Manhã", "Cartão de Crédito");
                    break;
                default:
                    break;
            }

            tbcInitial.TabPages[tbcInitial.SelectedIndex].Controls.Add(grbTemplateContaPagar);
        }

        private void TabPageIndexOne()
        {
            tbcInitial.TabPages[tbcInitial.SelectedIndex].Controls.Add(grbTemplateContaPagar);
        }

        private void SetParametersDefaultPorTipoDeContaECategoria(string tabPageName, string category, string account)
        {
            PreencherComboBoxContaPagarCategory(tabPageName, category);
            PreencherComboBoxContaPagarTipoConta(tabPageName, account);
        }
    }
}