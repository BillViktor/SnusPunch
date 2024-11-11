namespace SnusPunch.Shared.Models.Errors
{
    public class ErrorModel
    {
        public string ErrorText { get; set; }
        public string ExceptionDetails { get; set; }
        public string ExceptionStackTrace { get; set; }

        public string GetDetailedErrorString()
        {
            string sString = "";

            if(!string.IsNullOrEmpty(ErrorText))
            {
                sString += $"ErrorText: {ErrorText}\n";
            }
            if (!string.IsNullOrEmpty(ExceptionDetails))
            {
                sString += $"ExceptionDetails: {ExceptionDetails}\n";
            }
            if (!string.IsNullOrEmpty(ExceptionStackTrace))
            {
                sString += $"ExceptionStackTrace: {ExceptionStackTrace}\n";
            }

            return sString.TrimEnd('\n');
        }
    }   
}
