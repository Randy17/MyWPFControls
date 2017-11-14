using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;

namespace WpfControlsLibrary.Members
{
    public class SvgConverter
    {
        public DrawingGroup SvgFileToWpfObject(string filepath)
        {
            WpfDrawingSettings wpfDrawingSettings = new WpfDrawingSettings { IncludeRuntime = false, TextAsGeometry = false, OptimizePath = true };
            var reader = new FileSvgReader(wpfDrawingSettings);

            //workaround: error when Id starts with a number
            var doc = XDocument.Load(Path.GetFullPath(filepath));
            FixIds(doc.Root); //id="3d-view-icon" -> id="_3d-view-icon"
            using (var ms = new MemoryStream())
            {
                doc.Save(ms);
                ms.Position = 0;
                reader.Read(ms);
                return reader.Drawing;
            }
        }

        private void FixIds(XElement root)
        {
            var idAttributesStartingWithDigit = root.DescendantsAndSelf()
                .SelectMany(d => d.Attributes())
                .Where(a => string.Equals(a.Name.LocalName, "Id", StringComparison.InvariantCultureIgnoreCase));
            foreach (var attr in idAttributesStartingWithDigit)
            {
                if (char.IsDigit(attr.Value.FirstOrDefault()))
                {
                    attr.Value = "_" + attr.Value;
                }

                attr.Value = attr.Value.Replace("/", "_");
            }
        }
    }
}
