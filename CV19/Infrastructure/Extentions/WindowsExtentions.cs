//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Windows;

/* Изменим пространство имён: */
//namespace CV19.Infrastructure.Extentions
using CV19.Infrastructure.Extentions;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace System.Windows
{
    static class WindowsExtentions
    {
        /* Нам понадобится вызвать библиотеку "user32.dll" из Windows API: */
        private const string user32 = "user32.dll";

        /* Для этого определяем внешнюю функцию. Здесь важно её название, потому что именно по этому названию эта функция будет обнаружена в библиотеке user32:*/
        // Укажем финкции, где она находится.

        // Разные перегрузки методов:
        [DllImport(user32, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        /* IntPtr - это безопасный указатель на память, определяемый размерностью операционной системы (32 или 64 bit). Используется в Windows API. Дополнительно можно почитать
         * про функцию SendMessage в библиотеке user32.dll, которая предназначена для отправки сообщений windows.*/
        [DllImport(user32, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);
        public static IntPtr SendMessage(this Window window, WM Msg, IntPtr wParam, IntPtr lParam) =>
            SendMessage(window.GetWindowHandle(), Msg, wParam, lParam);

        /* Все сообщения windows скопировыны готовыми из проекта преподавателя (данные классы можно найти в интернете). Для этого в папе Extentions создаём классы WM.cs и SC.cs.
         * (на сайте Pinvoke.net собрана вся информация для взаимодействия с различными библиотеками win32 (ядро операционной системы Windows)). Сообщения же - это по сути некоторый
         * числовой код, уоторый йотправляется в функцию SendMessage, и операционная система уже дальше сама знает, что с ним делать. С помощью этого метода можно рассылать
         * различным окнам различные сообщения Windows: */
        public static IntPtr SendMessage(this Window window, WM Msg, SC wParam, IntPtr lParam = default)
        {
            return SendMessage(
                hWnd: window.GetWindowHandle(),
                (uint)Msg, (IntPtr)wParam,
                lParam == default ? (IntPtr)' ' : lParam);
        }

        /* По данной функции коментариев не было: */
        public static IntPtr GetWindowHandle(this Window window)
        {
            var helper = new WindowInteropHelper(window);
            return helper.Handle;
        }
    }
}
