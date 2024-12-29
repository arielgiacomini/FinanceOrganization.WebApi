namespace Application.Feature
{
    public class Output<T>
    {
        public int Quantidade { get; set; } = 0;
        public object? Data { get; set; } = new object();
        public Dictionary<string, string> Validations { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Errors { get; private set; } = new Dictionary<string, string>();
        public OutputStatus Status { get; private set; } = OutputStatus.Success;
        public string? Message { get; set; }

        public static Output<T> Success(string message, object data, int quantidade = 0)
        {
            return new Output<T>
            {
                Status = OutputStatus.Success,
                Message = message,
                Data = data,
                Quantidade = quantidade
            };
        }

        public static Output<T> NoContent(string message)
        {
            return new Output<T>
            {
                Status = OutputStatus.NoContent,
                Message = message,
                Data = null,
                Quantidade = 0
            };
        }

        public static Output<T> Validation(string message, Dictionary<string, string> validations, int quantidade = 0)
        {
            return new Output<T>
            {
                Status = OutputStatus.HasValidationIssue,
                Message = message,
                Validations = validations,
                Quantidade = quantidade
            };
        }

        public static Output<T> Error(string message, Dictionary<string, string> errors, int quantidade = 0)
        {
            return new Output<T>
            {
                Status = OutputStatus.HasInternalError,
                Message = message,
                Errors = errors,
                Quantidade = quantidade
            };
        }
    }
}