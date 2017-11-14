using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfControlsLibrary.CustomizableContextMenu.Models;

namespace WpfControlsLibrary.CustomizableContextMenu.Configuration
{
    internal class ContextCommandConfig : ContextMenuCommand
    {


        public ContextCommandConfig()
        { }
        public ContextCommandConfig(ContextMenuCommand command)
        {
            ID = command.ID;
            IsFavorite = command.IsFavorite;
            UsesCount = command.UsesCount;
            PositionInFavorites = command.PositionInFavorites;
        }
    }
}
