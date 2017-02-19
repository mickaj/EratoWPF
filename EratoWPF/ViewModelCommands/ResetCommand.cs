using System;
using System.Windows.Input;

namespace EratoWPF.ViewModelCommands
{
    public class ResetCommand :ICommand
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

        public ResetCommand(ViewModel viewModel)
        {
            if(viewModel == null)
            {
                throw new ArgumentNullException("no viewModel defined");
            }
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            if(((viewModel.InputRangeEnd == "10") && (viewModel.InputRangeStart == "2"))||(viewModel.Running))
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
            viewModel.InputRangeStart = "2";
            viewModel.InputRangeEnd = "10";
        }
    }
}
