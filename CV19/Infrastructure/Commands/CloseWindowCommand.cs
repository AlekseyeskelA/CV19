using CV19.Infrastructure.Commands.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CV19.Infrastructure.Commands
{
    // Данная команда просто закрывает окно:
    class CloseWindowCommand : Command
    {
        public override bool CanExecute(object parameter) => parameter is Window;

        public override void Execute(object parameter)
        {
            if (!CanExecute(parameter)) return;

            var window = (Window) parameter;
            window.Close();
        }
    }

    // Данная команда закрывает окно в диалоговом режиме:
    class CloseDialogCommand : Command
    {
        public bool? DialogResult { get; set; }
        public override bool CanExecute(object parameter) => parameter is Window;

        public override void Execute(object parameter)
        {
            if (!CanExecute(parameter)) return;

            var window = (Window)parameter;
            window.DialogResult = DialogResult;
            window.Close();
        }
    }
}
