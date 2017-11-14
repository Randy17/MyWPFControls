using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfControlsLibrary.Members
{
    public static class DrawingGroupExtension
    {
        public static void RemoveWhiteGeometryDrawings(this DrawingGroup drawingGroup)
        {
            var geometries = drawingGroup.GetGeometryDrawings();
            var whiteGeometries = geometries.Where(g => (g.Brush is SolidColorBrush) && (g.Brush as SolidColorBrush).Color == Colors.White).ToList();
            foreach (var wg in whiteGeometries)
            {
                drawingGroup.RemoveGeometryDrawing(wg);
            }
        }

        public static IEnumerable<GeometryDrawing> GetGeometryDrawings(this DrawingGroup drawingGroup)
        {
            var result = new List<GeometryDrawing>();

            Action<Drawing> handleDrawing = null;
            handleDrawing = aDrawing =>
            {
                if (aDrawing is DrawingGroup)
                    foreach (Drawing d in ((DrawingGroup)aDrawing).Children)
                    {
                        handleDrawing(d);
                    }
                if (aDrawing is GeometryDrawing)
                {
                    result.Add((GeometryDrawing)aDrawing);

                }
            };
            handleDrawing(drawingGroup);

            return result;
        }

        public static void RemoveGeometryDrawing(this DrawingGroup drawingGroup, GeometryDrawing geometryDrawing)
        {
            Action<Drawing> handleDrawing = null;
            handleDrawing = aDrawing =>
            {
                if (aDrawing is DrawingGroup)
                {
                    if (((DrawingGroup)aDrawing).Children.Contains(geometryDrawing))
                    {
                        ((DrawingGroup)aDrawing).Children.Remove(geometryDrawing);
                        return;
                    }
                    foreach (Drawing d in ((DrawingGroup)aDrawing).Children)
                    {
                        handleDrawing(d);
                    }
                }
            };
            handleDrawing(drawingGroup);
        }

        public static void PaintAllGemetryDrawings(this DrawingGroup drawingGroup, Color color)
        {
            var geometries = drawingGroup.GetGeometryDrawings();
            foreach (var g in geometries)
            {
                g.Brush = new SolidColorBrush(color);
            }
        }

        public static void PaintAllGemetryDrawings(this DrawingGroup drawingGroup, Brush brush)
        {
            var geometries = drawingGroup.GetGeometryDrawings();
            foreach (var g in geometries)
            {
                g.Brush = brush;
            }
        }
    }
}
