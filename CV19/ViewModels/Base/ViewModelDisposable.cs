using System;
using System.Collections.Generic;
using System.ComponentModel;            
using System.Text;
using System.Runtime.CompilerServices;

namespace CV19.ViewModels.Base
{
    // Класс для примера, чтобы понять, как можно реализовать интерфе с IDisposable.
    // IDisposable плох тем, что обращает на сабя внимание сборщика мусора, отнимая его ресурсы.
    internal class ViewModelDisposable : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }


        // Для IDisposable:
        
        public void Dispose()
        {
            Dispose(true);
        }

        // Если вдруг у нас появится деструктор, то метод Dispose нужно вызывать с параметром false:
        //~ViewModelDisposable()
        //{
        //    Dispose(false);
        //}

        private bool _Disposed;
        protected virtual void Dispose(bool Disposing)
        {
            if (!Disposing || _Disposed) return;
            _Disposed = true;
            // Освобождение управляемых ресурсов
        }
    }
}
