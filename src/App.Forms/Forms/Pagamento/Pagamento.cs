using App.Forms.Enums;
using App.Forms.Services;
using App.Forms.Services.Output;
using App.Forms.ViewModel;
using Domain.Utils;

namespace App.Forms.Forms.Pay
{
    public partial class FrmPagamento : Form
    {
        private const string EH_CARTAO_CREDITO_NAIRA = "Cartão de Crédito Nubank Naíra";

        public Guid IdentificadorUnicoContaPagar { get; set; } = Guid.Empty;
        public string? Nome { get; set; } = string.Empty;
        public string? Conta { get; set; } = string.Empty;
        public string AdditionalMessage { get; set; } = string.Empty;
        public string? AnoMes { get; set; } = string.Empty;
        public decimal? Valor { get; set; } = 0;
        public string? Environment { get; set; }

        public FrmPagamento()
        {
            InitializeComponent();
        }

        private void PreencherComboBoxAnoMes(string selectedItem = null)
        {
            var yearMonths = DateServiceUtils.GetListYearMonthsByThreeMonthsBeforeAndTwentyFourAfter();

            cboPagamentoMesAno.Items.AddRange(yearMonths.Values.ToArray());

            _ = yearMonths.TryGetValue(3, out string? currentYearMont);

            cboPagamentoMesAno.SelectedItem = currentYearMont;

            if (!string.IsNullOrWhiteSpace(selectedItem))
            {
                cboPagamentoMesAno.SelectedItem = selectedItem;
            }
        }

        private void PreencherComboBoxContaPagarTipoConta(string accountSelected = null)
        {
            var tipoConta = TipoConta.GetTipoContaEnable();

            foreach (var item in tipoConta)
            {
                cboPagamentoConta.Items.Add(item.Value);
            }

            if (string.IsNullOrWhiteSpace(accountSelected))
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
            PreencherComboBoxAnoMes(AnoMes);
            PreencherComboBoxContaPagarTipoConta(Conta);
            RegraApresentarInfoPreenchidas();
        }

        private void RegraApresentarInfoPreenchidas()
        {
            if (IdentificadorUnicoContaPagar != Guid.Empty)
            {
                txtPagamentoIdContaPagar.Enabled = false;
                cboPagamentoConta.Enabled = false;
                cboPagamentoMesAno.Enabled = false;
                txtPagamentoIdContaPagar.Text = IdentificadorUnicoContaPagar.ToString();
            }

            if (!string.IsNullOrWhiteSpace(Nome))
            {
                lblPagamentoNome.Text = Nome;
            }

            if (Valor != null && Valor.Value > 0)
            {
                lblPagamentoValor.Text = Valor.Value.ToString("C");
            }
        }

        private async void BtnPagamentoPagar_Click(object sender, EventArgs e)
        {
            PayBillToPayOutput output;
            PayBillToPayViewModel request;

            if (cboPagamentoConta.Text == "Cartão de Crédito" && !(AdditionalMessage.StartsWith(EH_CARTAO_CREDITO_NAIRA)))
            {
                if (!string.IsNullOrWhiteSpace(txtPagamentoIdContaPagar.Text))
                {
                    MessageBox.Show(
                        $"Se você está efetuando um pagamento em massa da conta: " +
                        $"{cboPagamentoConta.Text} não é possível informar um ID de conta para pagamento.",
                        "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                request = MapPayBillToPayToRequest();
            }
            else
            {
                _ = Guid.TryParse(txtPagamentoIdContaPagar.Text, out Guid idContaPagar);

                request = MapPayBillToPayToRequest(idContaPagar, false);
            }

            BillToPayServices.Environment = Environment;
            output = await BillToPayServices.PayBillToPay(request);

            SetOutput(output);
        }

        private static void SetOutput(PayBillToPayOutput? output)
        {
            if (output == null)
            {
                MessageBox.Show("Ocorreu um erro ao tentar efetuar o pagamento.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (output!.Output!.Status == OutputStatus.Success)
            {
                MessageBox.Show($"{output!.Output!.Message}", "Pagamento realizado com sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var validations = output!.Output?.Validations;

            if (validations != null && validations!.Count > 0)
            {
                var mensagem = string.Empty;
                foreach (var validation in validations)
                {
                    mensagem += string.Concat(validation.Key, "-", validation.Value);
                }

                MessageBox.Show(mensagem, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            var errors = output!.Output?.Errors;
            if (errors != null && errors!.Count > 0)
            {
                var mensagem = string.Empty;
                foreach (var error in errors)
                {
                    mensagem += string.Concat(error.Key, "-", error.Value);
                }

                MessageBox.Show(mensagem, "Erros", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private PayBillToPayViewModel MapPayBillToPayToRequest(Guid? idContaPagar = null, bool sendYearMonthAndAccount = true)
        {
            var request = new PayBillToPayViewModel()
            {
                Id = idContaPagar,
                PayDay = txtPagamentoData.Text,
                HasPay = rdbPagamentoPago.Checked ? rdbPagamentoPago.Checked : rdbPagamentoNaoPago.Checked,
                LastChangeDate = DateTime.Now,
                YearMonth = cboPagamentoMesAno.Text,
                Account = cboPagamentoConta.Text
            };

            if (!sendYearMonthAndAccount)
            {
                request.Account = null;
                request.YearMonth = null;
            }

            return request;
        }
    }
}