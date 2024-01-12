namespace App.Forms.Services.Output
{
    public class OutputDetails
    {
        public int Quantidade { get; set; } = 0;
        public object Data { get; set; } = new object();
        public Dictionary<string, string> Validations { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
        public OutputStatus Status { get; set; }
        public string? Message { get; set; }
    }
}