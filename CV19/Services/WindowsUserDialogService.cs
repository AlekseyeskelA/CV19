using CV19.Models.Decanat;
using CV19.Services.Interfaces;
using CV19.Views.Windows;
using System;
using System.Windows;
using System.Linq;

namespace CV19.Services
{
    class WindowsUserDialogService : IUserDialogService
    {
        /* В любой момент мы можем отказаться от класса IUserDialogService, переписать его иначе, реализовав этот интерфейс другим способом, например, 
         * с использованием web-интерфейса, и диалоговые окна будут показаны уже другим способом. И при этом модели-представления теперь расчитывают только
         * на этот интерфейс для взаимодействия. Т.е., таким образом мы развязали логическую и визуальную части. */
        public bool Edit(object item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            switch (item)
            {
                default: throw new NotSupportedException($"Редактирование объекта типа {item.GetType().Name} не поддерживается");
                case Student student:
                    return EditStudent(student);                
            }
        }
        public void ShowInformation(string Information, string Caption) => MessageBox.Show(Information, Caption, MessageBoxButton.OK, MessageBoxImage.Information);
        public void ShowWarning(string Message, string Caption) => MessageBox.Show(Message, Caption, MessageBoxButton.OK, MessageBoxImage.Warning);
        public void ShowError(string Message, string Caption) => MessageBox.Show(Message, Caption, MessageBoxButton.OK, MessageBoxImage.Error);

        /* При вызове метода Confirm будет отображаться диалоговое окно с помощью класса MessageBox, передавая туда все параметры и  проверяя результат,
         * который выберет пользователь (если он нажмёт кнопку Yes, то он согласился с той информацией, которую мы у него спрашивали).*/
        public bool Confirm(string Message, string Caption, bool Exclamation = false) =>
            MessageBox.Show(
                Message,
                Caption,
                MessageBoxButton.YesNo,
                Exclamation ? MessageBoxImage.Exclamation : MessageBoxImage.Question)
            == MessageBoxResult.Yes;

        private static bool EditStudent(Student student)
        {
            /* Создаём диалоговое окно, заполняем его свойствами - теми данными обекта, который мы хотим отредактировать, и отображаем его пользователю: */
            var dlg = new StudentManagerWindow
            {
                FirstName = student.Name,
                LastName = student.Surename,
                Patronymic = student.Patronymic,
                Rating = student.Rating,
                Birthday = student.Birthday,
                /* Спомощью следующего свойства можно открыть окно диалога по центру родительского. Преподаватель сказал, что с главным окном это очень просто,
                 * а так приходится прибегать к таким вот костылям. Так плохо, потому что приходится привязываться к конкретному окну:*/
                Owner = Application.Current.Windows.OfType<StudentsManagementWindow>().FirstOrDefault(),
                WindowStartupLocation = WindowStartupLocation.CenterOwner
                /* В качестве алтенативы можно передавать отбельный параметр в метод Edit, но преподаватель сказал, что можно ещё подумать. */
            };
            if (dlg.ShowDialog() != true)
                return false;

            /* Если пользователь согласился с теми изменениями, которые он внёс в объект, то после этого выполняем обратное копирование: */
            student.Name = dlg.FirstName;
            student.Surename = dlg.LastName;
            student.Patronymic = dlg.Patronymic;
            student.Rating = dlg.Rating;
            student.Birthday = dlg.Birthday;

            /* И возвращаем положительный результат: */
            return true;
        }
    }
}
