using System.Windows.Input;
using System.ComponentModel;
using EratoWPF.ViewModelCommands;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using System.Collections.Generic;

namespace EratoWPF
{
    public class ViewModel : INotifyPropertyChanged
    {
        //instance of model (PrimeNumbers class)
        private PrimeNumbers model;
        private async Task<int> InitializeModel()
        {
            int start = int.Parse(InputRangeStart);
            int end = int.Parse(InputRangeEnd);
            await Task.Run(() => model = new PrimeNumbers(start, end));
            return model.ProcessEnd;
        }
        
        //implementation of INotifyPropertyChanged - BEGINS
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(params string[] propertiesChanged)
        {
            if(PropertyChanged != null)
            {
                foreach(string property in propertiesChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(property));
                }
            }
        }
        //implementation of INotifyPropertyChanged - ENDS

        private string consoleText = Resources.TextFile.consoleReady;
        public string ConsoleText
        {
            get
            {
                return consoleText;
            }
            set
            {
                consoleText = value;
                OnPropertyChanged("consoleText");
            }
        }

        private string inputRangeStart;
        public string InputRangeStart   
        {
            get
            {
                return inputRangeStart; 
            }
            set
            {
                inputRangeStart = value;
                ValidationRangeStart = ValidateField(value);
                OnPropertyChanged("inputRangeStart");
            }
        }

        private string inputRangeEnd;
        public string InputRangeEnd
        {
            get
            {
                return inputRangeEnd;
            }
            set
            {
                inputRangeEnd = value;
                ValidationRangeEnd = ValidateField(value);
                OnPropertyChanged("inputRangeEnd");
            }
        }

        public readonly string validationOKSource = @"/EratoWPF;component/Resources/Correct.png";
        private readonly string validationFailedSource = @"/EratoWPF;component/Resources/Incorrect.png";

        private string validationRangeStart;
        public string ValidationRangeStart
        {
            get
            {
                return validationRangeStart;
            }
            set
            {
                validationRangeStart = value;
                OnPropertyChanged("validationRangeStart");
            }
        }

        private string validationRangeEnd;

        public string ValidationRangeEnd
        {
            get
            {
                return validationRangeEnd;
            }
            set
            {
                validationRangeEnd = value;
                OnPropertyChanged("validationRangeEnd");
            }
        }

        public ViewModel()
        {
            validationRangeStart = validationOKSource;
            validationRangeEnd = validationOKSource;
            inputRangeStart = "2";
            inputRangeEnd = "10";
        }

        private string ValidateField(string content)
        {
            int output;
            if((int.TryParse(content, out output))&&(output > 1))
            {
                //MessageBox.Show("validate OK");
                return validationOKSource;
            }
            else
            {
                //MessageBox.Show("validate failed");
                return validationFailedSource;
            }
        }

        private void UpdateConsole(string text)
        {
            consoleText += text + "\n";
        }

        private int maxProgress = 100;
        public int MaxProgress
        {
            get
            {                               
                return maxProgress;
            }
            set
            {
                maxProgress = value;
                OnPropertyChanged("maxProgress");
            }                           
        }

        private int currentProgress = 0;
        public int CurrentProgress
        {
            get
            {
                return currentProgress;
            }
            set
            {
                currentProgress = value;
                OnPropertyChanged("currentProgress");
            }
        }
                                
        private bool enabledProgress = false;
        public bool EnabledProgress
        {
            get
            {
                return enabledProgress;
            }
            set
            {
                enabledProgress = value;
                OnPropertyChanged("enabledProgress");
            }
        }

        private bool running = false;
        public bool Running
        {
            get
            {
                return running;
            }
            set
            {
                running = value;
            }                               
        }

        private bool inputsEnabled = true;
        public bool InputsEnabled
        {
            get
            {
                return inputsEnabled;
            }
            set
            {
                inputsEnabled = value;
                OnPropertyChanged("inputsEnabled");
            }
        }


        //ResetCommand definition
        private ICommand resetCommand;

        public ICommand Reset
        {
            get
            {
                if(resetCommand == null)
                {
                    resetCommand = new ResetCommand(this);
                }
                return resetCommand;
            }
        }
        //End of ResetCommand definition

        //ClearConsoleCommand definition
        private ICommand clearConsoleCommand;

        public ICommand ClearConsole
        {
            get
            {
                if(clearConsoleCommand == null)
                {
                    clearConsoleCommand = new ClearConsoleCommand(this);
                }
                return clearConsoleCommand;
            }
        }
        //End of ClearConsoleCommand definition

        //RunCommand definition
        private ICommand runCommand;

        public ICommand Run
        {
            get
            {
                if(runCommand == null)
                {
                    runCommand = new RunCommand(this);
                }
                return runCommand;
            }
        }
        //End of RunCommand definition

        //processing
        private Stopwatch timer;
        private List<int> results = new List<int>();

        public async void Processing()
        {
            timer = new Stopwatch();
            timer.Start();
            Running = true;
            InputsEnabled = false;
            EnabledProgress = true;
            //initialize model instnce in view model and UI progress bar
            ConsoleText += "\n"+Resources.TextFile.consoleDataSet;
            Task<int> createDataTask = InitializeModel();
            MaxProgress = await createDataTask;
            //MessageBox.Show(createDataTask.Status.ToString());
            //setting 'running' flag to true;
            ConsoleText += "\n"+Resources.TextFile.consoleRunning+"\n";
            CurrentProgress = 0;
            Task processTask = new Task(() => model.Sieve(UpdateProgress));
            Task continueTask = processTask.ContinueWith((a) => Completed());
            processTask.Start();
        }

        private void UpdateProgress(int i)
        {
            CurrentProgress = i;
        }

        private void Completed()
        {
            timer.Stop();
            string days = string.Format("{0:D1}", timer.Elapsed.Days);
            string dayStr;
            if(timer.Elapsed.Days == 1)
            {
                dayStr = Resources.TextFile.consoleDay;
            }
            else
            {
                dayStr = Resources.TextFile.consoleDays;
            }
            string hours = string.Format("{0:D2}", timer.Elapsed.Hours);
            string mins = string.Format("{0:D2}", timer.Elapsed.Minutes);
            string secs = string.Format("{0:D2}", timer.Elapsed.Seconds);
            string mils = string.Format("{0:D4}", timer.Elapsed.Milliseconds);
            MessageBox.Show(Resources.TextFile.completedText, Resources.TextFile.completedMsg,MessageBoxButton.OK,MessageBoxImage.Information);
            ConsoleText += Resources.TextFile.consoleProcessTime + " " + days + " " + dayStr + ", " + hours + ":" + mins + ":" + secs + ":" + mils;
            ConsoleText += "\n"+Resources.TextFile.consoleFound+" ";
            foreach(int i in model.numbers)
            {
                ConsoleText += i + " ";
            }
            ConsoleText += "\n**************************************\n";
            ConsoleText += Resources.TextFile.consoleReady;
            Running = false;
            InputsEnabled = true;
            CurrentProgress = 0;
        }

        //SaveConsoleCommand definition
        private ICommand saveConsoleCommand;

        public ICommand SaveConsole
        {
            get
            {
                if(saveConsoleCommand == null)
                {
                    saveConsoleCommand = new SaveConsoleCommand(this);
                }
                return saveConsoleCommand;
            }
        }
        //End of SaveConsoleCommand definition

        //CloseCommand definition
        private ICommand closeCommand;

        public ICommand Close
        {
            get
            {
                if(closeCommand == null)
                {
                    closeCommand = new CloseCommand();
                }
                return closeCommand;
            }
        }
        //End of CloseCommand definition

        //WWWCommand definition
        private ICommand wwwCommand;

        public ICommand WWW
        {
            get
            {
                if(wwwCommand == null)
                {
                    wwwCommand = new WWWCommand();
                }
                return wwwCommand;
            }
        }
        //End of WWWCommand definition

        //AboutCommand definition
        private ICommand aboutCommand;

        public ICommand About
        {
            get
            {
                if(aboutCommand == null)
                {
                    aboutCommand = new AboutCommand();
                }
                return aboutCommand;
            }
        }
        //End of AboutCommand definition
    }
}
