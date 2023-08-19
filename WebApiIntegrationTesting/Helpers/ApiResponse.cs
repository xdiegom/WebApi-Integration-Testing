namespace WebApiIntegrationTesting.Helpers
{
    public class ApiResponse<TData>
    {
        public TData? Data { get; set; }
        public string? Message { get; set; }
        public string? Code { get; set; }
        public string? Trace { get; set; }

        public static ApiResponse<TData> CreateApiResponse(
            TData? data,
            string? message = null,
            string? code = null,
            string? trace = null
        )
        {
            return new ApiResponse<TData>
            {
                Data = data,
                Message = message,
                Code = code,
                Trace = trace
            };
        }
    }
}
