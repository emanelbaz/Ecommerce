
namespace Ecommece.API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefulteMessage( statusCode);
        
        }

        

        public int StatusCode { get; set; }
        public string Message { get; set; }


        private string GetDefulteMessage( int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                401 => "Not Autriozed",
                404 => "Not Found",
                500 => "Errors",
                _ => null
            };
        }

    }
}
