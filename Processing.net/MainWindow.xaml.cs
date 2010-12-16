using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Indentation.CSharp;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;

namespace Processing.net
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
            _editor.TextArea.IndentationStrategy = new CSharpIndentationStrategy();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            CodeCompiler.Compile(_editor.Document.Text);
        }

        private void Parse_Click(object sender, RoutedEventArgs e)
        {
            IParser par = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader(_editor.Document.Text));
            par.Parse();
            if (par.Errors.Count > 0)
            {
                MessageBox.Show(par.Errors.ErrorOutput);
            }
            foreach (var child in par.CompilationUnit.Children)
            {
                MessageBox.Show(child.ToString());
                if (child is NamespaceDeclaration)
                {
                    foreach (var nested in child.Children)
                    {
                        if (nested is TypeDeclaration)
                        {
                            MessageBox.Show(nested.ToString());
                            foreach (var member in nested.Children)
                            {
                                MessageBox.Show(member.ToString());
                            }
                        }
                    }
                }
            }
        }
    }
}
