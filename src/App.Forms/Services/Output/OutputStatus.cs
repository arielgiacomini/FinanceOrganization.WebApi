namespace App.Forms.Services.Output
{
    public enum OutputStatus
    {
        Success = 0,
        HasValidationIssue = 1,
        HasInternalError = 2,
        EntityNotFound = 3,
        NoContent = 4
    }
}