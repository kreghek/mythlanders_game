using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Client.Engine;

public static class LineHelper
{
    public static IReadOnlyList<Point> GetBrokenLine(int x1, int y1, int x2, int y2, BrokenLineOptions opts)
    {
        var list = new List<Point>
        {
            new(x1, y1)
        };

        var marginPoint = new Point(x1, y1 + opts.MinimalMargin);

        list.Add(marginPoint);

        if (x1 < x2)
        {
            var yDiff = y2 - marginPoint.Y;
            var anglePoint = new Point(x2 - yDiff, y2);
            list.Add(anglePoint);
        }
        else
        {
            var yDiff = y2 - marginPoint.Y;
            var anglePoint = new Point(x2 + yDiff, y2);
            list.Add(anglePoint);
        }

        list.Add(new Point(x2, y2));

        return list;
    }

    public sealed class BrokenLineOptions
    {
        public int MinimalMargin { get; init; }
    }
}