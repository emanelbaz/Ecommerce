namespace Ecommece.API.Errors
{
    public class ApiValidationerrorResponse : ApiResponse
    {
        public ApiValidationerrorResponse(int statusCode, string message = null) : base(statusCode, message)
        {
        }
    }
}
