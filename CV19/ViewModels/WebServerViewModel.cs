using CV19.Infrastructure.Commands;
using CV19.Services.Interfaces;
using System.Windows.Input;

namespace CV19.ViewModels.Base
{
    internal class WebServerViewModel : ViewModel
    {
        private readonly IWebServerService _Server;
        public WebServerViewModel(IWebServerService Server)
        {
            _Server = Server;
        }


        #region Enabled
        //private bool _Enabled;
        //public bool Enabled { get => _Enabled; set => Set(ref _Enabled, value); }

        public bool Enabled
        { 
            get => _Server.Enabled;
            set
            {
                _Server.Enabled = value;
                OnPropertyChanged();
            }
        }
        #endregion


        #region StartCommand
        private ICommand _StartCommand;
        public ICommand StartCommand => _StartCommand
            ??= new LambdaCommand(OnStartComandExecuted, CanStartCommandExecute);
        public bool CanStartCommandExecute(object p) => /*!_Enabled*/ !Enabled;      // Запустить сервер можно только когдатон не включён.
        public void OnStartComandExecuted(object p)
        {
            //Enabled = true;
            _Server.Start();
            OnPropertyChanged(nameof(Enabled));     // При старте сервера нам необходимо сгенерировать событие Enabled, чтобы интерфейс его перечитал.
        }
        #endregion


        #region StopCommand
        private ICommand _StopCommand;
        public ICommand StopCommand => _StopCommand
            ??= new LambdaCommand(OnStopComandExecuted, CanStopCommandExecute);
        public bool CanStopCommandExecute(object p) => Enabled;        // Остановить сервер можно только когдатон включён.
        public void OnStopComandExecuted(object p)
        {
            //Enabled = false;
            _Server.Stop();
            OnPropertyChanged(nameof(Enabled));
        }
        #endregion
    }
}
