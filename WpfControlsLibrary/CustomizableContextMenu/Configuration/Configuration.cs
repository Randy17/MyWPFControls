using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfControlsLibrary.CustomizableContextMenu.Models;

namespace WpfControlsLibrary.CustomizableContextMenu.Configuration
{
    public class ContextMenuConfiguration
    {
        [JsonProperty]
        internal IEnumerable<ContextCommandConfig> _commands
        {
            get;
            set;
        }

        /// <summary>
        /// Save customizable context menu config to JSON string
        /// </summary>
        /// <param name="commandsGroups">Groups with commands that bind to Context menu</param>
        /// <returns></returns>
        public static string SaveConfigToJson(IEnumerable<ContextMenuGroup> commandsGroups)
        {
            ContextMenuConfiguration config = new ContextMenuConfiguration();
            config._commands = commandsGroups.SelectMany(cg => cg.ContextSubgroups.SelectMany(csg => csg.ContextCommands)).Select(cmd => new ContextCommandConfig(cmd));

            return JsonConvert.SerializeObject(config);
        }
        /// <summary>
        /// Load customizable context menu config from JSON string
        /// </summary>
        /// <param name="jsonString">JSON string with the configuration</param>
        /// <returns></returns>
        public static ContextMenuConfiguration LoadConfig(string jsonString)
        {
            try
            {
                return (ContextMenuConfiguration)JsonConvert.DeserializeObject(jsonString, typeof(ContextMenuConfiguration));
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
