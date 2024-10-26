using SnusPunch.Shared.Models.Errors;

namespace SnusPunch.Shared.Models.ResultModel
{
    public class ResultModel
    {
        public bool Success { get; set; } = true;
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();

        #region Errors
        public void AddError(string aErrorMessage)
        {
            ErrorModel sErrorModel = new ErrorModel
            {
                ErrorText = aErrorMessage
            };

            Errors.Add(sErrorModel);
        }

        public void AddExceptionError(Exception aException)
        {
            ErrorModel sErrorModel = new ErrorModel
            {
                ErrorText = $"Exception in {(new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().ReflectedType.FullName}.{(new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name}",
                ExceptionDetails = aException.Message,
                ExceptionStackTrace = aException.StackTrace
            };

            Errors.Add(sErrorModel);
        }

        public void AppendErrors(List<ErrorModel> aErrorModelList)
        {
            Errors.AddRange(aErrorModelList);
        }
        #endregion
    }

    public class ResultModel<T> : ResultModel
    {
        public T ResultObject { get; set; }
    }
}
