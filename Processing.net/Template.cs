using Processing.API;
using System.Windows;
using System.Windows.Media;

public class Program : IProgram
{
    private int _i = 0;
    public void Draw(DrawingContext ctx)
    {
        _i++;
        ctx.DrawEllipse(_i % 2 == 0 ? Brushes.Red : Brushes.Blue, null, new Point(256, 256), 100, 100);
    }
}