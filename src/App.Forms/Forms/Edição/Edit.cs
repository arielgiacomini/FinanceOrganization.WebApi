using App.Forms.Services;
using App.Forms.Services.Output;
using App.Forms.ViewModel;
using Domain.Utils;

namespace App.Forms.Forms.Edição
{
    public partial class FrmEdit : Form
    {
        public EditBillToPayViewModel EditBillToPayViewModel { get; set; } = new EditBillToPayViewModel();

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
            cboContaPagarAnoMesFinal.Text = EditBillToPayViewModel.YearMonth;
            cboContaPagarCategory.Text = EditBillToPayViewModel.Category;
            txtContaPagarValor.Text = EditBillToPayViewModel.Value.ToString("C");
            dtpContaPagarDataCompra.Text = EditBillToPayViewModel.PurchaseDate.ToString();
            cboContaPagarMelhorDiaPagamento.Text = EditBillToPayViewModel.DueDate.Day.ToString();
            rtbContaPagarMensagemAdicional.Text = EditBillToPayViewModel.AdditionalMessage;
            lblContaPagarDataCriacao.Text = EditBillToPayViewModel.LastChangeDate.ToString();
        }

        private async void BtnContaPagarEditar_Click(object sender, EventArgs e)
        {
            MapFormToViewModel();

            var result = await BillToPayServices.EditBillToPay(EditBillToPayViewModel);

            TratamentoOutput(result);
        }

        private static void TratamentoOutput(EditBillToPayOutput result)
        {
            if (result.Output.Status == OutputStatus.Success)
            {
                MessageBox.Show(result.Output.Data.ToString(),
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
            EditBillToPayViewModel.PurchaseDate = DateServiceUtils.GetDateTimeOfString(dtpContaPagarDataCompra.Text);
            //EditBillToPayViewModel.DueDate = // Data de Vencimento;
            EditBillToPayViewModel.AdditionalMessage = rtbContaPagarMensagemAdicional.Text;
            EditBillToPayViewModel.LastChangeDate = DateServiceUtils.GetDateTimeOfString(lblContaPagarDataCriacao.Text) ?? DateTime.Now;
        }
    }
}