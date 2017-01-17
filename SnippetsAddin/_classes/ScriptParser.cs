//if false
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
//using Microsoft.CodeAnalysis.CSharp.Scripting;
//using Microsoft.CodeAnalysis.Scripting;
using Westwind.wwScripting;

namespace SnippetsAddin
{

    /// <summary>
    /// A minimal Script Parser class that uses {{ C# Expressions }} to evaluate
    /// string based templates.
    /// </summary>
    /// <example>
    /// string script = @"Hi {{Name}}! Time is {{DateTime.sNow.ToString(""MMM dd, yyyy HH:mm:ss"")}}...";    
    /// var parser = new ScriptParser();
    /// string result = await parser.EvaluateScriptAsync(script, new Globals { Name = "Rick" });
    /// </example>
    public class ScriptParser
    {        
        // Additional namespaces to add to the script
        public List<string> Namespaces = new List<string>();

        /// <summary>
        ///  Additional references to add beyond MsCoreLib and System 
        /// Pass in a type from a give assembly
        /// </summary>        
        public List<string> References = new List<string>();

        public object ErrorMessage { get; set; }


        public string EvaluateScript(string snippet, object state = null)
        {
            snippet = snippet.Replace("\"","\"\"").Replace("{{", "\" + ").Replace("}}", " + @\"");
            snippet = "@\"" + snippet + "\"";
            

            
            string code = "dynamic Model = Parameters[0];\r\n" +                          
                          "return " + snippet + ";";

            Debug.WriteLine("wwScripting Code: \r\n" + code);
       
            var scripting = new wwScripting();
            string result = scripting.ExecuteCode(code,state) as string;

            
            if (result == null)
                ErrorMessage = scripting.ErrorMessage;
            else
                ErrorMessage = null;

            return result;
        }

        /// <summary>
        /// Executes a script with embedded {{ C# expression }}  syntax.
        /// Anything between double brackets is executed.
        /// 
        /// This async version is the preferred way to call this method.
        /// </summary>
        /// <param name="snippet"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task<string> EvaluateScriptAsync(string snippet, object state = null)
        {
            snippet = snippet.Replace("{{", "\" + ").Replace("}}", " + @\"");
            snippet = "@\"" + snippet + "\"";


            Console.WriteLine(snippet);



            var scripting = new wwScripting();
            string result = scripting.ExecuteCode("return " + snippet + ";" ) as string;

            if (result == null)
                ErrorMessage = scripting.ErrorMessage;
            else
                ErrorMessage = null;

            return result;

#if false
            var options = ScriptOptions.Default;

            //Add reference to mscorlib
            var mscorlib = typeof(System.Object).Assembly;
            var systemCore = typeof(System.Linq.Enumerable).Assembly;

            try
            {
                options = options
                    .AddReferences(mscorlib);
                options = options
                    .AddReferences(systemCore);
                
                options = options
                    .AddImports("System")
                    .AddImports("System.IO")
                    .AddImports("System.Text")
                    .AddImports("System.Net")
                    .AddImports("System.Linq")
                    .AddImports("System.Collections.Generic");


                foreach (string ns in Namespaces)
                {
                    if (!options.Imports.Contains(ns))
                        options.AddImports(ns);
                }
                foreach (string reference in References)
                {
                    options.AddReferences(reference);
                }

                //note: we block here, because we are in Main method, normally we could await as scripting APIs are async
                return CSharpScript.EvaluateAsync<string>(snippet, options, state).Result;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return null;
            }
#endif
        }

    }
}
//#endif