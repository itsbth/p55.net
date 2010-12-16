using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Processing.API;

namespace Processing.Examples
{
    public class GameOfLifeProgram : IProgram
    {
        private const int Size = 128;
        private readonly bool[,,] _grid = new bool[2,Size,Size];

        private readonly int[][] _neighbours = new[]
                                          {
                                              new[] {-1, -1},
                                              new[] {-1, 0},
                                              new[] {-1, 1},
                                              new[] {0, -1},
                                              new[] {0, 1},
                                              new[] {1, -1},
                                              new[] {1, 0},
                                              new[] {1, 1}
                                          };

        private int _state;

        public GameOfLifeProgram()
        {
            var r = new Random();
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    _grid[_state, x, y] = r.NextDouble() > 0.5;
                }
            }
        }

        #region IProgram Members

        public void Update()
        {
            int os = _state;
            _state = _state == 0 ? 1 : 0;
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    bool ns = _grid[os, x, y];
                    var neighbours = Neighbours(os, x, y);
                    if (neighbours < 2) ns = false;
                    else if (neighbours > 3) ns = false;
                    else if(ns == false && neighbours == 3) ns = true;

                    _grid[_state, x, y] = ns;
                }
            }
        }

        public void Draw(DrawingContext ctx)
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    //ctx.DrawRectangle(_grid[_state, x, y] ? Brushes.Wheat : Brushes.Black, null, new Rect(x * 8, y * 8, 8, 8));
                    if (_grid[_state, x, y]) ctx.DrawEllipse(Brushes.SteelBlue, null, new Point(x * 8, y * 8) + new Vector(4, 4), 4, 4);
                }
            }
    }

        #endregion

        private int Neighbours(int state, int x, int y)
        {
            return (from neighbour in _neighbours
                       let nx = x + neighbour[0]
                       let ny = y + neighbour[1]
                       where ((nx >= 0 && nx < Size) && ny >= 0) && ny < Size
                       where _grid[state, nx, ny]
                       select nx).Count();
        }
    }
}