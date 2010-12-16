using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Processing.API;

namespace Processing.net
{
    internal class OutputWindow : Window
    {
        private readonly IProgram _program;

        public OutputWindow(IProgram program)
        {
            _program = program;
            Width = 512;
            Height = 512;
            Background = Brushes.Transparent;
            var timer = new DispatcherTimer(TimeSpan.FromMilliseconds(50), DispatcherPriority.Render, OnTimer,
                                            Dispatcher);
            timer.Start();
        }

        private void OnTimer(object sender, EventArgs e)
        {
            _program.Update();
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            _program.Draw(drawingContext);
        }

        #region Nested type: VoidDelegate

        private delegate void VoidDelegate();

        #endregion
    }
}