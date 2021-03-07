namespace DepsWebApp.Models
{
     /// <summary>
     /// Exception model
     /// </summary>
    public class CustomExceptionModel
    {
        /// <summary>
        /// Exception message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Exception code
        /// </summary>
        public int Code { get; set; }

#pragma warning disable CS1591 
        public CustomExceptionModel(string error, int statusCode)
        {
            Message = error;
            Code = statusCode;
        }

        public static readonly CustomExceptionModel FailedRegistration = new CustomExceptionModel(
            "Sorry, we cannot register you right now. Please, try again later", 1);
        
        public static readonly CustomExceptionModel InternalServerError = new CustomExceptionModel(
            "Internal server error occured", 2);
        
        public static readonly CustomExceptionModel BadRequest = new CustomExceptionModel(
            "Something went wrong :( Fix request parameters and try again", 3);
        
        public static readonly CustomExceptionModel UnknownError = new CustomExceptionModel(
            "Unknown error occured. Please, contact our administrator", 100);
#pragma warning restore CS1591

    }
}