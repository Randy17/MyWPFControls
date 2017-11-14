using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfControlsLibrary.CustomizableContextMenu.Models
{
    public class ContextMenuGroup
    {
        #region Properties
        public Guid ID
        {
            get;
        }
        public string Name
        {
            get;set;
        }
        public List<ContextMenuSubgroup> ContextSubgroups
        {
            get;
        }
        #endregion

        public ContextMenuGroup()
        {
            ContextSubgroups = new List<ContextMenuSubgroup>();
        }
    }
}
