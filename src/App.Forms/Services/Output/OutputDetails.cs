namespace App.Forms.Services.Output
{
    public class OutputDetails
    {
        public int Quantidade { get; set; } = 0;
        public object Data { get; set; } = new object();
        public Dictionary<string, string> Validations { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Errors { get; private set; } = new Dictionary<string, string>();
        public OutputStatus Status { get; private set; } = OutputStatus.Success;
        public string? Message { get; set; }
    }
}