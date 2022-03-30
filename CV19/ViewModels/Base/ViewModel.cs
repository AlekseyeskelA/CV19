using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace CV19.ViewModels.Base
{
    internal abstract class ViewModel : INotifyPropertyChanged
    {
        // Событие:
        public event PropertyChangedEventHandler PropertyChanged;



        //// Методы: Средства генерации собятия, чтобы все наследники смогли им воспользоваться, чтобы не генерировать собитие вручную:

        // Метод 1: В нём важно передать имя свойства PropertyName и сгенерировать внутри событие:
        // Синтаксический сахар для упрощения генерации события (Это позволяет вызывать метод, не указывая в параметрах имя свойства. В этом случае компилятор автоматически подставит
        // в переменную PropertyName имя мотода, из которого вызывается данная процедура):
        // [CallerMemberName] - атрубут для компилятора.
        // PropertyName = null.        
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            // Предложил VisualStudio:
            //PropertyChangedEventHandler handler = PropertyChanged;

            // Из обучения:
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }


        // Метод 2. Обновление значение свойства, для которого определено поле, в котором это свойство хранит свои данные.
        // Задача метода - разрешить кольцевые изменения свойств, которые могут возникать. То есть, когда у нас изменяется одно свойство, то система сможет автоматически обновлять
        // второе свойство, а второе свойство может порождать обновление третьего свойства, а третье свойство может обновлять первое свойство. Флаги false или true в последствии
        // позволят определять, что если свойство действительно изменилось, то мы можем выполнить ещё работу по обнослению других свойств, которые связаны с этим свойством.
        // Чтобы эти кольцевые обновления не зацикливались и не происходило переполнение стека.
        // ref T field - сюда попадает ссылка на поле свойства.
        // T value - сюда будем передавать новое значение, которое хотим установить.
        // [CallerMemberName] string PeopsertName = null - этот параметр будет самостоятельно определяться компилятором, чтобы не указывать его вручную. Это будет имя нашего свойства,
        // которое мы передадим в метод OnPropertyChanged.
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) return false;             // Если значение поля, которое мы хотим обновить уже соответсвует передаваемому значению, то мы просто возвращаем false.
            field = value;                                      // В противном случае обновляем поле и...
            OnPropertyChanged(PropertyName);                    // генерируем OnPropertyChanged события и...
            return true;                                        // возвращаем true.
        }
    }
}
