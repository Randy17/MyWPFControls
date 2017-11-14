using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfControlsLibrary.CustomizableContextMenu.Models;
using WpfControlsLibrary.Members;
using WpfDemoApp.Infrastructure;

namespace WpfDemoApp.Models
{
    public class DemoContextMenuCommand : ContextMenuCommand
    {
        public DemoContextMenuCommand(string name, string pathToVectorIcon)
        {
            //ID = Guid.NewGuid();
            Name = name;

            if (File.Exists(pathToVectorIcon) && System.IO.Path.GetExtension(pathToVectorIcon) == ".svg")
            {
                VectorIconPath = pathToVectorIcon;
            }

            Command = new Command(o => { });
        }
    }
}
