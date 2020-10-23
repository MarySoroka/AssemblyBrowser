using System.Collections.Generic;
using System.ComponentModel;
using AssemblyLibrary;

namespace AssemblyBrowser
{
    class ModelView :INotifyPropertyChanged
    {

        private Comands openFile;

        public Comands OpenFile
        {
            get
            {
                return openFile ?? (openFile = new Comands(obj =>
                {
                    IDialogService dialog = new DialogService();
                    if (!dialog.OpenFileDialog()) return;
                    Path = dialog.FilePath;
                    var browser = new BrowserLibrary();
                    var result = browser.GetResult(Path);
                    AssemblyName = result.AssemblyName;
                    AssemblyInfo = result.Namespaces;
                }));
            }
        }

        private string assemblyName;
        public string AssemblyName
        {
            get => assemblyName;

            set
            {
                assemblyName = value;
                OnPropertyChanged("AssemblyName");
                
            }
        }

        private List<AssemblyNamespace> assemblyInfo;
        public List<AssemblyNamespace> AssemblyInfo
        {
            get => assemblyInfo;
            set
            {
                assemblyInfo = value;
                OnPropertyChanged("AssemblyInfo");
            }
        }

        private string path;
        public string Path
        {
            get => path;
            set
            {
                path = value;
                OnPropertyChanged("Path");
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        }

    }
}
