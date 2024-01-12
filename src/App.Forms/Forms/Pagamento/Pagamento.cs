using App.Forms.Enums;
using Domain.Utils;

namespace App.Forms.Forms.Pay
{
    public partial class FrmPagamento : Form
    {
        public Guid IdentificadorUnicoContaPagar { get; set; } = Guid.Empty;
        public string? Nome { get; set; } = string.Empty;

        public FrmPagamento()
        {
            InitializeComponent();
            PreencherComboBoxAnoMes();
            PreencherComboBoxContaPagarTipoConta();
            RegraApresentarInfoPreenchidas();
        }

        private void PreencherComboBoxAnoMes()
        {
            var yearMonths = DateServiceUtils.GetListYearMonthsByThreeMonthsBeforeAndTwentyFourAfter();

            cboPagamentoMesAno.Items.AddRange(yearMonths.Values.ToArray());

            _ = yearMonths.TryGetValue(3, out string? currentYearMont);

            cboPagamentoMesAno.SelectedItem = currentYearMont;
        }

        private void PreencherComboBoxContaPagarTipoConta(string accountSelected = null)
        {
            var tipoConta = TipoConta.GetTipoContaEnable();

            foreach (var item in tipoConta)
            {
                cboPagamentoConta.Items.Add(item.Value);
            }

            if (accountSelected == null)
            {
                cboPagamentoConta.SelectedItem = tipoConta.FirstOrDefault().Value;
            }
            else
            {
                var theChoise = tipoConta.FirstOrDefault(x => x.Value == accountSelected);

                if (theChoise.Value.Length > 0)
                {
                    cboPagamentoConta.SelectedItem = theChoise.Value;
                }
                else
                {
                    cboPagamentoConta.SelectedItem = tipoConta.FirstOrDefault().Value;
                }
            }
        }

        private void FrmPagamento_Load(object sender, EventArgs e)
        {
            PreencherComboBoxAnoMes();
            PreencherComboBoxContaPagarTipoConta();
            RegraApresentarInfoPreenchidas();

        }

        private void RegraApresentarInfoPreenchidas()
        {
            if (IdentificadorUnicoContaPagar != Guid.Empty)
            {
                txtPagamentoIdContaPagar.Enabled = false;
                txtPagamentoIdContaPagar.Text = IdentificadorUnicoContaPagar.ToString();
            }

            if (!string.IsNullOrWhiteSpace(Nome))
            {
                lblPagamentoNome.Text = Nome;
            }
            else
            {
                lblPagamentoNome.Text = string.Empty;
            }
        }
    }
}