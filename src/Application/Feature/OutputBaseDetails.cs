namespace Application.Feature
{
    public class OutputBaseDetails
    {
        public int Quantidade { get; set; } = 0;
        public object Data { get; set; } = new object();
        public Dictionary<string, string> Validations { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Errors { get; private set; } = new Dictionary<string, string>();
        public OutputStatus Status { get; private set; } = OutputStatus.Success;
        public string? Message { get; set; }

        public static OutputBaseDetails Success(string message, object data, int quantidade = 0)
        {
            return new OutputBaseDetails
            {
                Status = OutputStatus.Success,
                Message = message,
                Data = data,
                Quantidade = quantidade
            };
        }

        public static OutputBaseDetails Validation(string message, Dictionary<string, string> validations, int quantidade = 0)
        {
            return new OutputBaseDetails
            {
                Status = OutputStatus.HasValidationIssue,
                Message = message,
                Validations = validations,
                Quantidade = quantidade
            };
        }

        public static OutputBaseDetails Error(string message, Dictionary<string, string> errors, int quantidade = 0)
        {
            return new OutputBaseDetails
            {
                Status = OutputStatus.HasInternalError,
                Message = message,
                Errors = errors,
                Quantidade = quantidade
            };
        }

        public enum OutputStatus : ushort
        {
            Success = 0,
            HasValidationIssue = 1,
            HasInternalError = 2,
            EntityNotFound = 3,
            NoContent = 4
        }
    }
}