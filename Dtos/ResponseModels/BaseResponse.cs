namespace LocusIDBackend.Dtos
{
    public class BaseResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public BaseResponse() { }

        public BaseResponse(bool success, string? message = null, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static BaseResponse<T> SuccessResponse(T? data, string message = "Operation successful")
        {
            return new BaseResponse<T>(true, message, data);
        }

        public static BaseResponse<T> FailureResponse(string message = "Operation failed")
        {
            return new BaseResponse<T>(false, message);
        }
    }
}
