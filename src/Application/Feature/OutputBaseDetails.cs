﻿namespace Application.Feature
{
    public class OutputBaseDetails
    {
        public object Data { get; set; } = new object();
        public Dictionary<string, string> Validations { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Errors { get; private set; } = new Dictionary<string, string>();
        public OutputStatus Status { get; private set; } = OutputStatus.Success;
        public string? Message { get; set; }

        public static OutputBaseDetails Success(string message, object data)
        {
            return new OutputBaseDetails
            {
                Status = OutputStatus.Success,
                Message = message,
                Data = data
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