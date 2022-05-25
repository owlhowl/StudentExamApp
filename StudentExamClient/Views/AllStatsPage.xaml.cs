using StudentExamClient.ViewModels;
using System.Windows.Controls;

namespace StudentExamClient.Views
{
    public partial class AllStatsPage : Page
    {
        public AllStatsPage(MainViewModel mainVM)
        {
            DataContext = new StatsViewModel(mainVM);
            InitializeComponent();
        }
    }
}
