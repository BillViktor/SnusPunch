using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SnusPunch.Web.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region Fields
        private bool mIsBusy = false;
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
                IsBusy = value;
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
    }
}
