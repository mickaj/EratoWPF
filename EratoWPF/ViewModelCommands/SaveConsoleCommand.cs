using Microsoft.Win32;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EratoWPF.ViewModelCommands
{
    class SaveConsoleCommand :ICommand
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

        public SaveConsoleCommand(ViewModel viewModel)
        {
            if(viewModel == null)
            {
                throw new ArgumentNullException("no viewModel defined");
            }
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            if(viewModel.Running)
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
            //MessageBox.Show("Saving txt");
            SaveFileDialog saveTXT = new SaveFileDialog();
            saveTXT.FileName = "PrimeNumbers.txt";
            saveTXT.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            Nullable<bool> dialogRes = saveTXT.ShowDialog();
            if(dialogRes == true)
            {
                using(StreamWriter StreamWrite = new StreamWriter(saveTXT.FileName))
                    StreamWrite.WriteLine(viewModel.ConsoleText + "\r\nPrime numbers generated with EratoWPF by Michal Kajzer");
            }
        }
    }
}
                        