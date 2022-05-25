using ModelsApi;
using StudentExamClient.ViewModels;
using System.Windows.Controls;

namespace StudentExamClient.Views
{
    public partial class MyStatsPage : Page
    {
        public MyStatsPage(MainViewModel mainVM)
        {
            DataContext = new StatsViewModel(mainVM);
            InitializeComponent();
        }
    }
}
