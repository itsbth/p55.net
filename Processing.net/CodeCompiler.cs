using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using Processing.API;

namespace Processing.net
{
    internal class CodeCompiler
    {
        public static void Compile(string code)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            var compilerParameters = new CompilerParameters {GenerateInMemory = true, TreatWarningsAsErrors = true};
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    string location = assembly.Location;
                    if (!String.IsNullOrEmpty(location))
                    {
                        compilerParameters.ReferencedAssemblies.Add(location);
                    }
                }
                catch (NotSupportedException)
                {
                    // this happens for dynamic assemblies, so just ignore it.
                }
            }
            CompilerResults res = provider.CompileAssemblyFromSource(compilerParameters, new[] {code});
            if (res.Errors.Count <= 0)
            {
                Type program =
                    res.CompiledAssembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof (IProgram))).First();
                
                var prog = (IProgram) program.GetConstructor(new Type[] {}).Invoke(new object[] {});
                var wnd = new OutputWindow(prog);
                wnd.ShowDialog();
            }
            else
            {
                foreach (object error in res.Errors)
                {
                    MessageBox.Show(error.ToString());
                }
            }
        }
    }

    internal class MyMessageBox : IMessageBox
    {
        #region IMessageBox Members

        public void Show(string message)
        {
            MessageBox.Show(message);
        }

        #endregion
    }
}