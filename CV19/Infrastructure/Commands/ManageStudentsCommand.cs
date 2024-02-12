using CV19.Infrastructure.Commands.Base;
using CV19.Views.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CV19.Infrastructure.Commands
{
    class ManageStudentsCommand : Command
    {
        private StudentsManagementWindow _Window;

        /* Команду можно испольнить только в том случеа, если окно у нас не задано: */
        public override bool CanExecute(object parameter) => _Window == null;

        public override void Execute(object parameter)
        {
            var window = new StudentsManagementWindow()
            {
                /* Сделаем родительским окном нашего окна главное окно (не знаю, зачем это нужно): */
                Owner = Application.Current.MainWindow
            };
            _Window = window;

            /* Сейчас мы сможем открыть наше окно только один раз, и больше открыть его не сможем. Чтобы это исправить, сделаем обработчик события.
             * Однако, по-моему можно просто после строчки window.ShowDialog(); написть _Window = null; и всё. И не надо нкаких обработчиков: */
            window.Closed += OnWindowClosed;

            /* Будем вызывать метод ShowDialog(), который заблокирует главное окно: */
            window.ShowDialog();
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            /* В случае перехвата этого события, отписываемся от него на всякий случай: */
            ((Window)sender).Closed -= OnWindowClosed;
            
            /* И затираем ссылку: */
            _Window = null;
        }
    }
}
