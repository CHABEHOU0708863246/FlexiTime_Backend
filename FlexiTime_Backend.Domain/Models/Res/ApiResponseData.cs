namespace FlexiTime_Backend.Domain.Models.Res
{
    public class ApiResponseData<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponseData(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
