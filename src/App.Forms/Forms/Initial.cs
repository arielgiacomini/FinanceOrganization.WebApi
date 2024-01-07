using Domain.Utils;

namespace App.Forms.Forms
{
    public partial class Initial : Form
    {
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
        }

        private void PreencherComboBoxContaPagarCategory()
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

            var positionOne = categoriasContaPagar.FirstOrDefault();

            cboContaPagarCategory.SelectedItem = positionOne.Value;
        }

        private void PreencherComboBoxContaPagarTipoConta()
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

            var positionOne = tipoConta.FirstOrDefault();

            cboContaPagarTipoConta.SelectedItem = positionOne.Value;
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

        private void ckbContaPagarConsideraMesmoMes_CheckedChanged(object sender, EventArgs e)
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
    }
}