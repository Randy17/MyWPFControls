using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfControlsLibrary.CustomizableContextMenu.Models
{
    public class ContextMenuSubgroup
    {
        #region Properties
        public Guid ID
        {
            get;
        }
        public string Name
        {
            get; set;
        }
        public List<ContextMenuCommand> ContextCommands
        {
            get;
        }
        #endregion

        public ContextMenuSubgroup()
        {
            ContextCommands = new List<ContextMenuCommand>();
        }
    }
}
