using System;
using System.IO;
using System.Windows;
using FontAwesome.WPF;
using MarkdownMonster;
using MarkdownMonster.AddIns;

namespace SnippetsAddin
{
    public class SnippetsAddin : MarkdownMonster.AddIns.MarkdownMonsterAddin
    {
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
            var form = new SnippetsWindow(this);

            form.Top = Model.Window.Top;
            form.Left = Model.Window.Left + Model.Window.Width - Model.Configuration.WindowPosition.SplitterPosition;

            form.Show();            
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
        /// Inserts a selected snippet into the current document
        /// </summary>
        /// <param name="snippet"></param>
        public void InsertSnippet(Snippet snippet)
        {

            string snippetText = snippet.SnippetText;
            if (snippetText.Contains("{{"))
            {
                var parser = new ScriptParser();
                snippetText = parser.EvaluateScript(snippetText, Model);
                if (snippetText == null)
                {
                    MessageBox.Show("Snippet execution failed:  " + parser.ErrorMessage);
                    return;
                }
            }

            this.SetSelection(snippetText);
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
