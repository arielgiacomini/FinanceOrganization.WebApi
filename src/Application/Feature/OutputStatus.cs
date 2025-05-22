﻿namespace Application.Feature
{
    public enum OutputStatus : ushort
    {
        Success = 0,
        HasValidationIssue = 1,
        HasInternalError = 2,
        EntityNotFound = 3,
        NoContent = 4
    }
}