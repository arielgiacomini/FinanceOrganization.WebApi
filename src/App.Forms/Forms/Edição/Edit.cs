using App.Forms.Services;
using App.Forms.Services.Output;
using App.Forms.ViewModel;
using Domain.Utils;

namespace App.Forms.Forms.Edição
{
    public partial class FrmEdit : Form
    {
        public EditBillToPayViewModel EditBillToPayViewModel { get; set; } = new EditBillToPayViewModel();
        public decimal valorContaPagarDigitadoTextBox = 0;
        public string? Environment { get; set; }

        public FrmEdit()
        {
            InitializeComponent();
        }

        private void FrmEdit_Load(object sender, EventArgs e)
        {
            PreencherCampos();
        }

        private void PreencherCampos()
        {
            txtContaPagarNameDescription.Text = EditBillToPayViewModel.Name;
            cboContaPagarTipoConta.Text = EditBillToPayViewModel.Account;
            cboContaPagarFrequencia.Text = EditBillToPayViewModel.Frequence;
            cboContaPagarTipoCadastro.Text = EditBillToPayViewModel.RegistrationType;
            cboContaPagarAnoMesInicial.Text = EditBillToPayViewModel.YearMonth;
            cboContaPagarCategory.Text = EditBillToPayViewModel.Category;
            txtContaPagarValor.Text = EditBillToPayViewModel.Value.ToString("C");

            if (EditBillToPayViewModel.PurchaseDate == null)
            {
                dtpContaPagarDataCompra.Enabled = false;
                dtpContaPagarDataCompra.Text = null;
            }
            else
            {
                cboHabilitarDataCompra.Checked = true;
                dtpContaPagarDataCompra.Text = EditBillToPayViewModel.PurchaseDate.ToString();
            }

            dtpContaPagarDataVencimento.Text = EditBillToPayViewModel.DueDate.ToString();
            txtContaPagarDataPagamento.Text = EditBillToPayViewModel.PayDay?.ToString();
            rdbPagamentoPago.Checked = EditBillToPayViewModel.HasPay;
            rdbPagamentoNaoPago.Checked = !EditBillToPayViewModel.HasPay;
            rtbContaPagarMensagemAdicional.Text = EditBillToPayViewModel.AdditionalMessage;
            lblContaPagarDataCriacao.Text = EditBillToPayViewModel.LastChangeDate.ToString();
        }

        private async void BtnContaPagarEditar_Click(object sender, EventArgs e)
        {
            MapFormToViewModel();

            BillToPayServices.Environment = Environment;
            var result = await BillToPayServices.EditBillToPay(EditBillToPayViewModel);

            OutputMapper(result);
        }

        private static void OutputMapper(EditBillToPayOutput result)
        {
            if (result.Output.Status == OutputStatus.Success)
            {
                MessageBox.Show(result.Output.Message,
                    "Edição de Conta Realizado com Sucesso.",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
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

        private void MapFormToViewModel()
        {
            EditBillToPayViewModel.Name = txtContaPagarNameDescription.Text;
            EditBillToPayViewModel.Account = cboContaPagarTipoConta.Text;
            EditBillToPayViewModel.Frequence = cboContaPagarFrequencia.Text;
            EditBillToPayViewModel.RegistrationType = cboContaPagarTipoCadastro.Text;
            EditBillToPayViewModel.YearMonth = cboContaPagarAnoMesInicial.Text;
            EditBillToPayViewModel.Category = cboContaPagarCategory.Text;
            EditBillToPayViewModel.Value = Convert.ToDecimal(txtContaPagarValor.Text.Replace("R$ ", ""));
            EditBillToPayViewModel.PurchaseDate = cboHabilitarDataCompra.Checked ? DateServiceUtils.GetDateTimeOfString(dtpContaPagarDataCompra.Text) : null;
            EditBillToPayViewModel.PayDay = string.IsNullOrWhiteSpace(txtContaPagarDataPagamento.Text) ? null : txtContaPagarDataPagamento.Text;
            EditBillToPayViewModel.HasPay = rdbPagamentoPago.Checked;
            EditBillToPayViewModel.DueDate = DateServiceUtils.GetDateTimeOfString(dtpContaPagarDataVencimento.Text) ?? DateTime.Now;
            EditBillToPayViewModel.AdditionalMessage = rtbContaPagarMensagemAdicional.Text;
            EditBillToPayViewModel.LastChangeDate = DateServiceUtils.GetDateTimeOfString(lblContaPagarDataCriacao.Text) ?? DateTime.Now;
        }

        private void TxtContaPagarValor_Leave(object sender, EventArgs e)
        {
            valorContaPagarDigitadoTextBox = StringDecimalUtils
            .TranslateStringEmDecimal(txtContaPagarValor.Text);

            txtContaPagarValor.Text = StringDecimalUtils
                .TranslateValorEmStringDinheiro(txtContaPagarValor.Text);
        }

        private void TxtContaPagarValor_Enter(object sender, EventArgs e)
        {
            txtContaPagarValor.Text = "";
        }

        private void CboHabilitarDataCompra_CheckedChanged(object sender, EventArgs e)
        {
            if (cboHabilitarDataCompra.Checked)
            {
                dtpContaPagarDataCompra.Enabled = true;
            }
            else
            {
                dtpContaPagarDataCompra.Text = string.Empty;
                dtpContaPagarDataCompra.Enabled = false;
            }
        }
    }
}