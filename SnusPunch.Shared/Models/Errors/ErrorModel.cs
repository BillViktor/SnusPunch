namespace SnusPunch.Shared.Models.Errors
{
    public class ErrorModel
    {
        public string ErrorText { get; set; }
        public string ExceptionDetails { get; set; }
        public string ExceptionStackTrace { get; set; }
    }   
}
