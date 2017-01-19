using System;
using System.IO;
using System.Linq;
using System.Windows;
using FontAwesome.WPF;
using MarkdownMonster;
using MarkdownMonster.AddIns;

namespace SnippetsAddin
{
    public class SnippetsAddin : MarkdownMonster.AddIns.MarkdownMonsterAddin
    {
        private SnippetsWindow snippetsWindow;

        public override void OnApplicationStart()
        {
            base.OnApplicationStart();

            Id = " SnippetsAddin";

            // by passing in the add in you automatically
            // hook up OnExecute/OnExecuteConfiguration/OnCanExecute
            var menuItem = new AddInMenuItem(this)
            {
                Caption = " SnippetsAddin",

                // if an icon is specified it shows on the toolbar
                // if not the add-in only shows in the add-ins menu
                FontawesomeIcon = FontAwesomeIcon.PencilSquareOutline
            };

            // if you don't want to display config or main menu item clear handler
            //menuItem.ExecuteConfiguration = null;

            // Must add the menu to the collection to display menu and toolbar items            
            this.MenuItems.Add(menuItem);
        }

        public override void OnExecute(object sender)
        {
            if (snippetsWindow == null || !snippetsWindow.IsLoaded)
            {
                snippetsWindow = new SnippetsWindow(this);

                snippetsWindow.Top = Model.Window.Top;
                snippetsWindow.Left = Model.Window.Left + Model.Window.Width -
                                      Model.Configuration.WindowPosition.SplitterPosition;
            }
            snippetsWindow.Show();
            snippetsWindow.Activate();
        }

        public override void OnExecuteConfiguration(object sender)
        {
            Model.Window.OpenTab(Path.Combine(mmApp.Configuration.CommonFolder, "snippetsaddin.json"));         
        }

        public override bool OnCanExecute(object sender)
        {
            return true;
        }


        /// <summary>
        /// Helper function that returns the snippet text that's to be inserted
        /// </summary>
        /// <param name="snippet"></param>
        /// <returns></returns>
        public string GetEvaluatedSnippetText(Snippet snippet)
        {
            string snippetText = snippet.SnippetText;


            if (snippet.ScriptMode == ScriptModes.CSharpExpressions && snippetText.Contains("{{"))
            {
                var parser = new ScriptParser();
                snippetText = parser.EvaluateScript(snippetText, Model);
                if (snippetText == null)
                {
                    MessageBox.Show("Snippet execution failed:  " + parser.ErrorMessage);
                    return null;
                }
            }
            else if (snippet.ScriptMode == ScriptModes.Razor && snippetText.Contains("@"))
            {
                var parser = new ScriptParser();
                snippetText = parser.EvaluateRazorScript(snippetText, Model);
                if (snippetText == null)
                {
                    MessageBox.Show("Snippet execution failed:  " + parser.ErrorMessage);
                    return null;
                }
            }

            return snippetText;
        }

        /// <summary>
        /// Inserts a selected snippet into the current document
        /// </summary>
        /// <param name="snippet"></param>
        public void InsertSnippet(Snippet snippet)
        {
            var snippetText = GetEvaluatedSnippetText(snippet);
            if (string.IsNullOrEmpty(snippetText))
                return;
            
            SetSelection(snippetText);
                                                
        }

        public override void OnDocumentUpdated()
        {
            base.OnDocumentUpdated();

            var editor = GetMarkdownEditor();
            string line = editor.GetCurrentLine();
            if (string.IsNullOrEmpty(line))
                return;            
            
            var snippet = SnippetsAddinConfiguration.Current.Snippets.FirstOrDefault(sn => sn.ShortCut != null && line.Trim().EndsWith(sn.ShortCut));
            if (snippet != null)
            {
                var snippetText = GetEvaluatedSnippetText(snippet);
                editor.FindAndReplaceTextInCurrentLine(snippet.ShortCut, snippetText);                
            }
        }

        //public override void OnDocumentUpdated()
        //{
        //    base.OnDocumentUpdated();

        //    var editor = GetMarkdownEditor();
        //    var doc = editor.MarkdownDocument;

        //    if (doc.CurrentText.Contains("ecb"))
        //    {
        //        doc.CurrentText = doc.CurrentText.Replace("ecb", "Eeel can Blink!");
        //        editor.SetMarkdown();
        //    }

        //////        selectionRange = editor.getSelectionRange();

        //////startLine = selectionRange.start.row;
        //////endLine = selectionRange.end.row;

        //////content = editor.session.getTextRange(selectionRange);

        //}
    }
}
