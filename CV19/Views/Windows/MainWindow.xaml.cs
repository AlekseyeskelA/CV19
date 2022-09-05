using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CV19.Models.Decanat;

namespace CV19
{ 
    public partial class MainWindow : Window
    {
        // В идеале здесь не должно быть никаких обрадотчиков событий из разметки. Всё должно быть в том виде, в каком было создано Visual Studio.
        public MainWindow()
        {
            InitializeComponent();
        }

        /* Здесь саное главное - это параметр FilterEventArgs e, который передаётся в обработчик события. Внутри параметра "e" находится сам элемент Item, который подвергается
        фильтрации на каждом моменте работы CollectionViewSource (он каждый раз вызывает это событие)*/
        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Group group)) return;                                                           // Если объект не является группой, то делать ничего не будем.
            if (group.Name is null) return;

            var filter_text = GroupNameFilterText.Text;                                                     // Извлечём строку фильтра из разметки.
            if(filter_text.Length == 0) return;                                                             // Если текст фильтра отсутствует, то пропускаем все элементы.

            if (group.Name.Contains(filter_text, System.StringComparison.OrdinalIgnoreCase)) return;        // Если имя группы содержит искомый текст, то мы ничего не делаем ("ленивая" логика).
            if (group.Description != null
                && group.Description.Contains(filter_text, System.StringComparison.OrdinalIgnoreCase)) return; // Если описание группы не null содержит искомый текст, то мы ничего не делаем.

            // Если группа не содержит ни в имени, ни в описании искомый текст, то в этом случае в свойство Accepted устанавливаем ЛОЖЬ:
            e.Accepted = false;
        }

        //Обработчик событий TextChanged="GroupNameFilterText_TextChanged" будет вызываться всякий раз, когда будет изменяться текст в поле фильтра выбора групп:
        private void GroupNameFilterText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            /* Почему-то по заданному ранее имени не удалось обратиться к CollectionViewSource, поэтому имя в разметке удалим, и попробуем обратиться через поиск ресурсов
            по ключю x:Key="GroupsCollection" с конвертацией в CollectionViewSource: */
            var text_box = (TextBox)sender;                                                                 // получаем объект TextBox с именем "sender" и создаём переменную данного типа.
            var collection = (CollectionViewSource)text_box.FindResource("GroupsCollection");               // через этот TextBox связываемся с разметкой, находим GroupsCollection и создаём переменную данного типа.
            collection.View.Refresh();                                                                      // Обновляем GroupsCollection (с передачей вводённых данных в строку поиска?...).
        }
    }
}
