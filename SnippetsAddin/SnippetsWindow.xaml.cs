using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MarkdownMonster;
using Newtonsoft.Json;
using Westwind.Utilities;

namespace SnippetsAddin
{
    /// <summary>
    /// Interaction logic for PasteHref.xaml
    /// </summary>
    public partial class SnippetsWindow
    {
        public SnippetsAddinModel Model { get; set; }
        
        public SnippetsWindow(SnippetsAddin addin)
        {
            InitializeComponent();
            mmApp.SetThemeWindowOverride(this);


            Model = new SnippetsAddinModel()
            {
                Configuration = SnippetsAddinConfiguration.Current,
                Window = addin.Model.Window,
                AppModel = addin.Model.Window.Model,                
                Addin = addin                               
            };


            if (Model.Configuration.Snippets == null)
                Model.Configuration.Snippets = new System.Collections.ObjectModel.ObservableCollection<Snippet>();
            else
                Model.Configuration.Snippets =
                    new ObservableCollection<Snippet>(Model.Configuration.Snippets.OrderBy(snip => snip.Name));
            
            Loaded += SnippetsWindow_Loaded;
            Unloaded += SnippetsWindow_Unloaded;

            

            DataContext = Model;            
        }

        private void SnippetsWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            SnippetsAddinConfiguration.Current.Write();
        }

        private void SnippetsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Model.Configuration.Snippets.Count > 0)
                ListSnippets.SelectedItem = Model.Configuration.Snippets[0];

            ListSnippets.Focus();
        }
        

        private void ListSnippets_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var snippet = ListSnippets.SelectedItem as Snippet;
            if (snippet == null)
                return;

            Model.Addin.InsertSnippet(snippet);            
        }

        private void ToolButtonNewSnippet_Click(object sender, RoutedEventArgs e)
        {
            Model.Configuration.Snippets.Insert(0,new Snippet() {Name = "New Snippet"});
            ListSnippets.SelectedItem = Model.Configuration.Snippets[0];
        }

        private void ToolButtonRemoveSnippet_Click(object sender, RoutedEventArgs e)
        {


        }

        private void ListSnippets_KeyUp(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.Key);
            if (e.Key == Key.Return || e.Key == Key.Space)
            {
                var snippet = ListSnippets.SelectedItem as Snippet;
                if (snippet != null)
                    Model.Addin.InsertSnippet(snippet);
            }
        }
    }
}
