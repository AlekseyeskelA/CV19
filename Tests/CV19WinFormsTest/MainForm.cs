using System;
using System.Threading;
using System.Windows.Forms;

namespace CV19WinFormsTest
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            new Thread(ComputeValue).Start();
        }



        // Вариант кода 1:
        //private void ComputeValue()
        //{            
        //    var value = LongProcess(DateTime.Now);

        //    // При такой записи произоёдет ошибка из-за разным номеров потоков:
        //    //ResultLabel.Text = value;

        //    // В WinForms есть аналог диспетчера WPF:
        //    if (ResultLabel.InvokeRequired)                                         // Если требуется вызов, ...
        //        ResultLabel.Invoke(new Action(() => ResultLabel.Text = value));     // То делаем вызов.
        //    else                                                                    // Если вызов не требуется, ...
        //        ResultLabel.Text = value;                                           // Т вызов не делаем.
        //}



        // Вариант кода 2:
        private void ComputeValue()
        {
            var value = LongProcess(DateTime.Now);

            // При такой записи произоёдет ошибка из-за разным номеров потоков:
            //ResultLabel.Text = value;

            SetResultValue(value);

        }

        private void SetResultValue(string Value)
        {
            if (ResultLabel.InvokeRequired)                                         // Если требуется вызов, то...
                ResultLabel.Invoke(new Action<string>(SetResultValue), Value);      // то вызываем этот же метод, но уже в контексте самого элемента.
            else                                                                    // Если вызов не требуется, ...
                ResultLabel.Text = Value;                                           // то просто устанавливаем значение.

        }



        private static string LongProcess(DateTime Time)
        {
            Thread.Sleep(3000);

            return $"Value: {Time}";
        }
    }
}
