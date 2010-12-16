using System.Windows.Media;

namespace Processing.API
{
    public interface IProgram
    {
        void Update();
        void Draw(DrawingContext ctx);
    }
}