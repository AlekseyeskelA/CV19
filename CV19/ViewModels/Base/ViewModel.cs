﻿using System;
using System.Collections.Generic;
using System.ComponentModel;                    // Для INotifyPropertyChanged
using System.Runtime.CompilerServices;          // Для [CallerMemberName]
using System.Text;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Xaml;

namespace CV19.ViewModels.Base
{
    // Базовый класс представления
    // Если проект большой, то базовый класс представления лучше вынести в отдельную библиотеку
    
    // Ещё одно место применения расширения разметки - это модели-представления. Наделим дазовую модель представления базовым классом MarkupExtention.
    // После этого (е реализации метода Provide...) у нас модель стала расширением разметки:
    internal abstract class ViewModel : MarkupExtension, INotifyPropertyChanged/*, IDisposable*/
    // Интерфейс, способный уведомлять о том, что внутри нашего объекта изменилось какое-то свойство.
    // При этом интерфейсная визуальная часть подключится к этому интерфейсу и будет следить за своим свойством, которое егму интересно.
    // В том случае, если это свойство изменилось, оно перечитает его значение и обновит визуальную часть.
    // Внутри этого интерфейса содержится описание всего лишь одного события (PropertyChanged)
    // IDisposable - для примера, чтобы понять, как можно реализовать интерфе с IDisposable.
    // IDisposable плох тем, что обращает на сабя внимание сборщика мусора, отнимая его ресурсы.
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

            // Генерация события (из обучения):
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }


        // Метод 2. Обновление значение свойства, для которого определено поле, в котором это свойство хранит свои данные.
        // Задача метода - разрешить кольцевые изменения свойств, которые могут возникать. То есть, когда у нас изменяется одно свойство, то система сможет автоматически обновлять
        // второе свойство, а второе свойство может порождать обновление третьего свойства, а третье свойство может обновлять первое свойство. Флаги false или true в последствии
        // позволят определять, что если свойство действительно изменилось, то мы можем выполнить ещё работу по обнослению других свойств, которые связаны с этим свойством.
        // Чтобы эти кольцевые обновления не зацикливались и не происходило переполнение стека.
        // ref T field - сюда попадает ссылка на поле свойства.
        // T value - сюда будем передавать новое значение, которое хотим установить.
        // [CallerMemberName] string PeopsertName = null - Если написать так, то имя имя свойства будет определяться и определяться компилятором самостоятельно, чтобы не указывать его вручную.
        // Это будет имя нашего свойства, которое мы передадим в метод OnPropertyChanged.
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) return false;             // Если значение поля, которое мы хотим обновить уже соответсвует передаваемому значению, то мы просто возвращаем false.
            field = value;                                      // В противном случае обновляем поле и...
            OnPropertyChanged(PropertyName);                    // генерируем OnPropertyChanged события и...
            return true;                                        // возвращаем true.
        }


        // Метод 3. Реализуем метод MarkupExtention:
        public override object ProvideValue(IServiceProvider sp)
        {
            /*Параметр IServiceProvider sp позволяет получить доступ к специальным сервисам, которые работают внутри механизма xaml-разметки на этапе её построения. И таким образом
            можно достать из разметки несколько полезных сервисов, с помощью которых получить доступ к таким параметрам, как объект, в который выполняется установки значения (Window),
            имя свойства, для которого кторому применяется это расширение разметки DataContext, а также корневой объект всей разметки (Window). А если бы мы присваивали DataContext на уровне
            DockPanel, то мы бы получили ссылку на DockPanel, свойство DataContext и сам объект WIndow.*/
            // Сервисы, которые позволяют получить доступы к объектам:
            var value_target_service = sp.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;   // Целевой объект, к которому выполняется обращение.
            var root_object_service = sp.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;    // Корень дерева (наше окно)

            OnInitialized(
                value_target_service?.TargetObject,
                value_target_service?.TargetProperty,
                root_object_service?.RootObject);

            return this;
        }
        
        // Здесть сохраним результаты метода OnInitialized.
        // Однако, если мы просто напишем вот так:
        //private object _Target;
        //private object _Root;
        /*то это плохо закончится в плане утечек памяти. Получится так, что создастся модель-представления, она получает ссылку на окно, захватывает её и держит. После этого, если будут
        работать какие-то диалоги, окна будут создаваться, закрываться, то мы будем терять на них ссылку, а модель-представления будет держать ссылку на окно, и в результате сборщик мусора
        не сможет удалить это самое окно, что не очень хорошо в плане памяти. Поэтому мы будем использовать не прямые (жёсткие) ссылки, а будем использовать мягкие ссылки. Это специальная вещь,
        которая позволяет удалять объекты из памяти сборщику мусора, но при этом позволяет нам хранить ссылку на этот объект и получать к нему доступ. Есть специальный класс WeakReference.
        раньше он был не типизированный, не шаблонный (без скобочек <>), в котором приходилось делать приведение типов вручную. Недавно появился класс шаблонного типа WeakReference<>,
        в котором мы можем указывать тип хранимого объекта. Но в нашем случае мы воспользуемся обычным старым классом:*/
        
        private WeakReference _TargetRef;       // Ссылка на целевой объект.
        private WeakReference _RootRef;         // Ссылка на корень.

        // Делаем свойства, которые позволяяют получать доступ к этим объектам через ссылки:
        public object TargetObject => _TargetRef.Target;
        public object RootObject => _RootRef.Target;

        protected virtual void OnInitialized(object Target, object Property, object Root)
        {
            // Сохраняем полноценные ссылки:
            _TargetRef = new WeakReference(Target);
            _RootRef = new WeakReference(Root);
        }

        /*Теперь наша Вью-модель познакомилась с окном, но при этом она с ним связана слабыми ссылками. При этом оно окно не удержит само по себе. Сборщик мусора сможет удалить окно.
         А само окно, как представлени, доступно через свойство RootObject. То есть к нему можно в любой момент обратиться и "грохнуть" его, например, что, конечно, не очень хорошо
        с точки зрения архитектуры, но WPF позволяет это сделать через расширение разметки.
        (см MainWindowViewModel, private void OnCloseApplicationCommandExecuted(object p)), (RootObject as Window)?.Close();*/


        //// Для IDisposable:        
        //public void Dispose()
        //{
        //    Dispose(true);
        //}

        //// Если вдруг у нас появится деструктор, то метод Dispose нужно вызывать с параметром false:
        ////~ViewModelDisposable()
        ////{
        ////    Dispose(false);
        ////}

        //private bool _Disposed;
        //protected virtual void Dispose(bool Disposing)
        //{
        //    if (!Disposing || _Disposed) return;
        //    _Disposed = true;
        //    // Освобождение управляемых ресурсов
        //}
    }
}
