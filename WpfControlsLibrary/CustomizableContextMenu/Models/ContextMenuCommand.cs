using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfControlsLibrary.CustomizableContextMenu.Models
{
    public abstract class ContextMenuCommand
    {
        #region Properties
        public Guid ID
        {
            get;
            set;
        }
        [JsonIgnore]
        public string Name
        {
            get;set;
        }
        [JsonIgnore]
        public string VectorIconPath
        {
            get;set;
        }
        [JsonIgnore]
        public ICommand Command
        {
            get;set;
        }
        [JsonIgnore]
        public object CommandParameter
        {
            get;set;
        }
        [JsonIgnore]
        public int Priority
        {
            get;set;
        }
        public int UsesCount
        {
            get;set;
        }
        public bool IsFavorite
        {
            get;set;
        }
        [JsonProperty]
        internal int PositionInFavorites
        {
            get;set;
        }
        #endregion
    }
}
