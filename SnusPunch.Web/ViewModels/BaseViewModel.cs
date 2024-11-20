using SnusPunch.Shared.Models.Errors;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SnusPunch.Web.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region Fields
        private bool mIsBusy = false;
        private List<ErrorModel> mErrors = new List<ErrorModel>(); 
        private List<string> mSuccessMessages = new List<string>();
        #endregion

        #region Properties
        public bool IsBusy
        {
            get
            {
                return mIsBusy;
            }
            set
            {
                mIsBusy = value;
                OnPropertyChanged();
            }
        }

        public List<ErrorModel> Errors
        {
            get
            {
                return mErrors;
            }
            set
            {
                mErrors = value;
                OnPropertyChanged();
            }
        }

        public List<string> SuccessMessages
        {
            get
            {
                return mSuccessMessages;
            }
            set
            {
                mSuccessMessages = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? aPropertyName = null)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName));
        }
        protected void SetValue<T>(ref T backingFiled, T value, [CallerMemberName] string? aPropertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingFiled, value)) return;
            backingFiled = value;
            OnPropertyChanged(aPropertyName);
        }
        #endregion

        #region Methods
        public void AddError(string aErrorText)
        {
            Errors.Add(new ErrorModel {  ErrorText = aErrorText });
        }
        #endregion
    }
}
