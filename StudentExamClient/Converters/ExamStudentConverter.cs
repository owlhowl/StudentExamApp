using ModelsApi;
using StudentExamClient.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StudentExamClient.Converters
{
    internal class ExamStudentConverter : IValueConverter
    {
        private List<Student> allStudents;

        public ExamStudentConverter() => Initialize();

        private void Initialize() => allStudents = Task.Run(() => Api.GetListAsync<List<Student>>("Student")).Result;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var id = (int)value;
            var student = allStudents.Find(e => e.Id == id);
            return student.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
