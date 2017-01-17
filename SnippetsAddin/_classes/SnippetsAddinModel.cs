using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MarkdownMonster;
using Newtonsoft.Json;
using SnippetsAddin.Annotations;


namespace SnippetsAddin
{
    public class SnippetsAddinModel : INotifyPropertyChanged
    {
    
        public SnippetsAddin Addin { get; set;  }


        
        public MainWindow Window { get; set; }

        [JsonIgnore]
        public AppModel AppModel
        {
            get { return _appModel; }
            set
            {
                if (Equals(value, _appModel)) return;
                _appModel = value;
                OnPropertyChanged();
            }
        }
        private AppModel _appModel;

        
        public Snippet ActiveSnippet
        {
            get { return _activeSnippet; }
            set
            {
                if (Equals(value, _activeSnippet)) return;
                _activeSnippet = value;
                OnPropertyChanged();
            }
        }
        private Snippet _activeSnippet;


        public SnippetsAddinConfiguration Configuration { get; set; }

        private ObservableCollection<Snippet> _snippets = new ObservableCollection<Snippet>();


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
