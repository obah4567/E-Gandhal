namespace E_Gandhal.src.Domain.DomainException
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
        public object Data { get; set; }
    }

}
