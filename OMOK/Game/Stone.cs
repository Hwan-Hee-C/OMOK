using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OMOK.Game
{
    public struct Coord
    {
        public int X;
        public int Y;

        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }

        static public Point CoordToScreenPoint(in Coord c)
        {
            int x = Constants.BoardMargin + Constants.BoardInterval * c.X;
            int y = Constants.BoardMargin + Constants.BoardInterval * c.Y;

            return new Point(x, y);
        }

        static public Coord ScreenPointToCoord(in Point p)
        {
            int x = (p.X - Constants.BoardMargin + (Constants.BoardInterval / 2)) / Constants.BoardInterval;
            int y = (p.Y - Constants.BoardMargin+ (Constants.BoardInterval / 2)) / Constants.BoardInterval;
            return new Coord(x, y);
        }
    }

    public enum StoneType
    {
        None = 0,
        Black,
        White
    }

    public struct Stone
    {
        public StoneType Type { get; private set; }
        public Coord Pos { get; private set; }
        public Rectangle Rect { get; private set; }

        public Stone(StoneType type, Coord p)
        {
            Type = type;
            Pos = p;
            int s = Constants.StoneSize;
            Point point = Coord.CoordToScreenPoint(p);
            Rect = new Rectangle(point.X - s / 2, point.Y - s / 2, s, s);
        }
    }
}
