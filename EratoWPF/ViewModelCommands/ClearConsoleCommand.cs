using System;
using System.Windows.Input;

namespace EratoWPF.ViewModelCommands
{
    class ClearConsoleCommand :ICommand
    {

        private readonly ViewModel viewModel;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public ClearConsoleCommand(ViewModel viewModel)
        {
            if(viewModel == null)
            {
                throw new ArgumentNullException("no viewModel defined");
            }
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            if((viewModel.ConsoleText == "Ready...")||(viewModel.Running))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Execute(object parameter)
        {
            viewModel.ConsoleText = "Ready...";
        }
    }
}
