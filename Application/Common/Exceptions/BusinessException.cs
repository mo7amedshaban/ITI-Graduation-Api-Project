namespace Application.Common.Exceptions;

public class BusinessException : Exception
{
    public int ErrorCode { get; set; } 
    public string BusinessRuleName { get; set; } 

    public BusinessException(string message, string businessRuleName = null, int errorCode = 400)
        : base(message)
    {
        ErrorCode = errorCode;
        BusinessRuleName = businessRuleName;
    }
}