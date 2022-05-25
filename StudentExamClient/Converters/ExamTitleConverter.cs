using ModelsApi;
using StudentExamClient.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StudentExamClient.Converters
{
    internal class ExamTitleConverter : IValueConverter
    {
        private List<Exam> allExams;

        public ExamTitleConverter() => Initialize();

        private void Initialize() => allExams = Task.Run(() => Api.GetListAsync<List<Exam>>("Exam")).Result;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var id = (int)value;
            var exam = allExams.Find(e => e.Id == id);
            return exam.Title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
