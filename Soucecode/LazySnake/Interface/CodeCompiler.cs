using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LazySnake.Interface
{
    class CodeCompiler<T>
    {
        private Assembly compiledeAssembly;
        private List<string> classes = new List<string>();

        public void AddClass(string classe)
        {
            classes.Add(classe);
        }

        public void Compile(string code)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();

            string totalClass = "";
            foreach (string cc in classes)
            {
                totalClass += cc + @" ";
            }

            CompilerResults results = codeProvider.CompileAssemblyFromSource(new CompilerParameters(), new string[]
            {
                string.Format(@"
                    namespace CodeCompilerNamespace
                    {{
                        {0}
                        public class CodeCompilerClass
                        {{
                            {1}
                        }}
                    }}", totalClass, code)
             });

            try
            {
                compiledeAssembly = results.CompiledAssembly;
            }
            catch
            {

            }
        }

        public T Run()
        {
            if(compiledeAssembly != null)
            {
                try
                {
                    dynamic evaluator = Activator.CreateInstance(compiledeAssembly.GetType("CodeCompilerNamespace.CodeCompilerClass"));
                    return evaluator.Run();
                }
                catch
                {
                    return default(T);
                }
            }
            return default(T);
        }
    }
}
