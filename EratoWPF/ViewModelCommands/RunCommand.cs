using System;
using System.Windows.Input;

namespace EratoWPF.ViewModelCommands
{
    class RunCommand :ICommand
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

        public RunCommand(ViewModel viewModel)
        {
            if(viewModel == null)
            {
                throw new ArgumentNullException("no viewModel defined");
            }
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            if((viewModel.ValidationRangeStart==viewModel.validationOKSource)&&(viewModel.ValidationRangeEnd==viewModel.validationOKSource)&&(!viewModel.Running))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Execute(object parameter)
        {
            viewModel.Processing();
        }
    }
}
