using ModelsApi;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace StudentExamClient.Core
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public delegate void AnyHandler();
    }
}
