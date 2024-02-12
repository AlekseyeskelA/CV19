using System;
using System.Collections.Generic;
using System.Text;

namespace CV19.Services.Interfaces
{
    /* Здесь будем описывать всё, что нам необходимо делать для взаимодействия с пользователем: */
    interface IUserDialogService
    {
        // Мы должны иметь возможность что-то редактировать:
        bool Edit(object item);

        // Отображение сообщений:
        void ShowInformation(string Information, string Caption);

        // Отображение предупреждений:
        void ShowWarning(string Message, string Caption);

        // Отображение ошибок:
        void ShowError(string Message, string Caption);

        // Спросим пользователя, согласен он  с чем-то или нет:
        bool Confirm(string Message, string Caption, bool Exclamation = false);

        /* Далее сделаем реализацию этого сервиса WindowsUserDialogService в папке Services. */

        /* Для наглядности удобства полученной структуры добавим текстовое сообжение и соответствующее диалоговое окно StringValueDialogWindow: */
        string GetStringValue(string Message, string Caption, string DefaultValue = null);
    }
}
