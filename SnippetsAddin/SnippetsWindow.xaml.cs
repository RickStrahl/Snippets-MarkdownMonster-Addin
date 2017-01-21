﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MarkdownMonster;

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
            {
                Model.Configuration.Snippets =
                    new ObservableCollection<Snippet>(Model.Configuration.Snippets.OrderBy(snip => snip.Name));
                if (Model.Configuration.Snippets.Count > 0)
                    Model.ActiveSnippet = Model.Configuration.Snippets[0];
            }

            Loaded += SnippetsWindow_Loaded;
            Unloaded += SnippetsWindow_Unloaded;

            WebBrowserSnippet.Visibility = Visibility.Hidden;

            DataContext = Model;            
        }

      


        private MarkdownEditorSimple editor;

        private void SnippetsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string initialValue = null;
            if (Model.Configuration.Snippets.Count > 0)
            {
                ListSnippets.SelectedItem = Model.Configuration.Snippets[0];
                initialValue = Model.Configuration.Snippets[0].SnippetText;
            }

            ListSnippets.Focus();


            editor = new MarkdownEditorSimple(WebBrowserSnippet, initialValue);
            editor.IsDirtyAction =  () =>
            { 
                string val = editor.GetMarkdown();
                if (val != null && Model.ActiveSnippet != null)
                    Model.ActiveSnippet.SnippetText = val;

                return true;
            };
        }


        private void SnippetsWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            SnippetsAddinConfiguration.Current.Write();
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
            var snippet = ListSnippets.SelectedItem as Snippet;
            if (snippet == null)
                return;
            SnippetsAddinConfiguration.Current.Snippets.Remove(snippet);
        }

        private void ListSnippets_KeyUp(object sender, KeyEventArgs e)
        {
            
            if (e.Key == Key.Return || e.Key == Key.Space)
            {
                var snippet = ListSnippets.SelectedItem as Snippet;
                if (snippet != null)
                    Model.Addin.InsertSnippet(snippet);
            }
        }

        private void ListSnippets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var snippet = ListSnippets.SelectedItem as Snippet;
            if (snippet != null)
                editor?.SetMarkdown(snippet.SnippetText);
        }

        private void ListScriptModes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Model.ActiveSnippet == null)
                return;

            //if (Model.ActiveSnippet.ScriptMode == ScriptModes.CSharpExpressions)
            //    editor?.SetEditorSyntax("markdown");
            //else
            //    editor?.SetEditorSyntax("razor");

        }
    }
}
