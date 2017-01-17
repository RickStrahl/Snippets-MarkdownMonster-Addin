using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SnippetsAddin.Annotations;

namespace SnippetsAddin
{
    public class Snippet : INotifyPropertyChanged
    {
        
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }
        private string _name;


        public string SnippetText
        {
            get { return _snippetText; }
            set
            {
                if (value == _snippetText) return;
                _snippetText = value;
                OnPropertyChanged();
            }
        }
        private string _snippetText;

        public string ShortCut
        {
            get { return _shortCut; }
            set
            {
                if (value == _shortCut) return;
                _shortCut = value;
                OnPropertyChanged();
            }
        }
        private string _shortCut;

        public string ExpansionShortCut { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
   
}
