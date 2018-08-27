using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfControlsLibrary.CustomizableContextMenu;
using WpfControlsLibrary.CustomizableContextMenu.Configuration;
using WpfControlsLibrary.CustomizableContextMenu.Models;
using WpfControlsLibrary.GanttDiagram.Models;
using WpfControlsLibrary.GanttDiagram.ViewModels.TimeGantt;
using WpfDemoApp.Infrastructure;
using WpfDemoApp.Models;

namespace WpfDemoApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private const string CONFIG_FILE_PATH = "menuConfig.json";

        private TimeGanttItem _selectedGanttItem;
        private ObservableCollection<TimeGanttItem> _ganttItems;

        public List<ContextMenuGroup> CommandsGroups
        {
            get;set;
        }

        public ContextMenuConfiguration MenuConfig
        {
            get;set;
        }

        public ObservableCollection<TimeGanttItem> GanttItems
        {
            get { return _ganttItems; }
            set
            {
                _ganttItems = value;
                RaisePropertyChanged(nameof(GanttItems));
            }
        }

        public TimeGanttItem SelectedGanttItem
        {
            get { return _selectedGanttItem; }
            set
            {
                _selectedGanttItem = value;
                RaisePropertyChanged(nameof(SelectedGanttItem));
            }
        }

        public Command RefreshGanttCmd
        {
            get
            {
                return new Command(RefreshGantt);
            }
        }

        public MainViewModel()
        {
            CommandsGroups = new List<ContextMenuGroup>();
            var group1 = new ContextMenuGroup() { Name = "Group 1" };
            var subgroup1 = new ContextMenuSubgroup();
            subgroup1.ContextCommands.Add(new DemoContextMenuCommand("Command 1", @"Icons/Icon_test.svg") { ID = Guid.Parse("{301BE655-8D65-4F87-A703-598E05E709D7}"), Priority = 1 });
            subgroup1.ContextCommands.Add(new DemoContextMenuCommand("Command 2", @"Icons/039.svg") { ID = Guid.Parse("{87F00755-7A54-4D6E-A2BC-AECD446E5606}"), Priority = 2 });
            subgroup1.ContextCommands.Add(new DemoContextMenuCommand("Command 3", @"Icons/052.svg") { ID = Guid.Parse("{EA1AD1C2-6979-497E-87AF-CDA6C5AF4BFA}"), Priority = 3 });
            group1.ContextSubgroups.Add(subgroup1);
            CommandsGroups.Add(group1);

            var group2 = new ContextMenuGroup() { Name = "Group 2" };
            var subgroup2 = new ContextMenuSubgroup() { Name = "Subgroup 2" };
            subgroup2.ContextCommands.Add(new DemoContextMenuCommand("Command 4", @"Icons/069.svg") { ID = Guid.Parse("{331EBF3B-1EAD-4466-A57B-0C4199FF82B3}") });
            subgroup2.ContextCommands.Add(new DemoContextMenuCommand("Command 5", @"Icons/095.svg") { ID = Guid.Parse("{BB78CF73-CD81-4654-A09F-BEF33BA39AFD}") });
            subgroup2.ContextCommands.Add(new DemoContextMenuCommand("Command 6", @"Icons/102.svg") { ID = Guid.Parse("{5139E352-F3FF-4ED6-9D71-313B7F45E21D}") });
            subgroup2.ContextCommands.Add(new DemoContextMenuCommand("Command 7", @"Icons/125.svg") { ID = Guid.Parse("{524A6C4B-BBD1-4D59-971C-5B1D5F544C37}") });
            group2.ContextSubgroups.Add(subgroup2);

            var subgroup3 = new ContextMenuSubgroup() { Name = "Subgroup 3" };
            subgroup3.ContextCommands.Add(new DemoContextMenuCommand("Command 8", @"Icons/136.svg") { ID = Guid.Parse("{41C54A49-E152-498C-8AAE-90162F1E0880}") });
            subgroup3.ContextCommands.Add(new DemoContextMenuCommand("Command 9", @"Icons/155.svg") { ID = Guid.Parse("{F112062A-CD06-407F-8BFE-57EF9CB78E5A}") });
            subgroup3.ContextCommands.Add(new DemoContextMenuCommand("Command 10", @"Icons/157.svg") { ID = Guid.Parse("{31D338B4-65A8-4E04-8A48-27B2FD5C2EB5}") });
            subgroup3.ContextCommands.Add(new DemoContextMenuCommand("Command 11", @"Icons/180.svg") { ID = Guid.Parse("{722B2AD6-0E77-4007-A0F4-71DFC8EF9240}") });
            group2.ContextSubgroups.Add(subgroup3);
            CommandsGroups.Add(group2);

            var group3 = new ContextMenuGroup() { Name = "Group 3" };
            var subgroup4 = new ContextMenuSubgroup() { Name = "Subgroup 4" };
            subgroup4.ContextCommands.Add(new DemoContextMenuCommand("Command 1", @"Icons/Icon_test.svg") { ID = Guid.Parse("{97DD9FA4-92B7-4FD9-94BE-65652953EA66}"), Priority = 1 });
            subgroup4.ContextCommands.Add(new DemoContextMenuCommand("Command 2", @"Icons/039.svg") { ID = Guid.Parse("{A431FDE6-3FD9-40EF-A2A7-0EC1E0EAED50}"), Priority = 2 });
            subgroup4.ContextCommands.Add(new DemoContextMenuCommand("Command 3", @"Icons/052.svg") { ID = Guid.Parse("{B3B8E99E-EFDB-466B-B130-4EE6B56C5583}"), Priority = 3 });
            group3.ContextSubgroups.Add(subgroup4);
            CommandsGroups.Add(group3);

            var group4 = new ContextMenuGroup() { Name = "Group 4" };
            var subgroup5= new ContextMenuSubgroup() { Name = "Subgroup 5" };
            subgroup5.ContextCommands.Add(new DemoContextMenuCommand("Command 4", @"Icons/069.svg") { ID = Guid.Parse("{49F82CA2-4A3B-4952-8C5E-28CA87CC072B}") });
            subgroup5.ContextCommands.Add(new DemoContextMenuCommand("Command 5", @"Icons/095.svg") { ID = Guid.Parse("{8D37CFA9-538A-4BB7-94A0-8BAD97AF1F09}") });
            subgroup5.ContextCommands.Add(new DemoContextMenuCommand("Command 6", @"Icons/102.svg") { ID = Guid.Parse("{49E5A19C-9992-4C38-B68C-8E96CE515760}") });
            subgroup5.ContextCommands.Add(new DemoContextMenuCommand("Command 7", @"Icons/125.svg") { ID = Guid.Parse("{2D372D74-F712-4513-B9A7-BB092FA35660}") });
            subgroup5.ContextCommands.Add(new DemoContextMenuCommand("Command 8", @"Icons/136.svg") { ID = Guid.Parse("{9FAC2805-6C5C-409C-A98B-23FFFAF634CC}") });
            subgroup5.ContextCommands.Add(new DemoContextMenuCommand("Command 9", @"Icons/155.svg") { ID = Guid.Parse("{0AF6208F-6FC2-4EC3-86C9-D09943797655}") });
            subgroup5.ContextCommands.Add(new DemoContextMenuCommand("Command 10", @"Icons/157.svg") { ID = Guid.Parse("{312E2FC7-8266-4B51-B516-5225AAB80DCD}") });
            subgroup5.ContextCommands.Add(new DemoContextMenuCommand("Command 11", @"Icons/180.svg") { ID = Guid.Parse("{76644A5C-AF5E-4D19-A0BA-4F2E8D9125DA}") });
            subgroup5.ContextCommands.Add(new DemoContextMenuCommand("Command 9", @"Icons/155.svg") { ID = Guid.Parse("{79789C57-7B5F-4875-B6F4-52D977D01F4B}") });
            subgroup5.ContextCommands.Add(new DemoContextMenuCommand("Command 10", @"Icons/157.svg") { ID = Guid.Parse("{7E4C64AD-A712-4EE3-9365-867B332FAA48}") });
            subgroup5.ContextCommands.Add(new DemoContextMenuCommand("Command 11", @"Icons/180.svg") { ID = Guid.Parse("{486B0EE2-F866-4016-AB31-BD300E7AA3A7}") });
            group4.ContextSubgroups.Add(subgroup5);

            var subgroup6 = new ContextMenuSubgroup() { Name = "Subgroup 6" };
            subgroup6.ContextCommands.Add(new DemoContextMenuCommand("Command 12", @"Icons/102.svg") { ID = Guid.Parse("{49E5A19C-9992-4C38-B68C-8E96CE512750}") });
            subgroup6.ContextCommands.Add(new DemoContextMenuCommand("Command 13", @"Icons/125.svg") { ID = Guid.Parse("{2D372D74-F712-4513-B9A7-BB092FA15660}") });
            group4.ContextSubgroups.Add(subgroup6);

            var subgroup7 = new ContextMenuSubgroup() { Name = "Subgroup 7" };
            subgroup7.ContextCommands.Add(new DemoContextMenuCommand("Command 14", @"Icons/102.svg") { ID = Guid.Parse("{49E5A19C-9992-4C38-B63C-8E96CE517750}") });
            subgroup7.ContextCommands.Add(new DemoContextMenuCommand("Command 15", @"Icons/125.svg") { ID = Guid.Parse("{2D372D74-F712-4513-B2A7-BB092FA15660}") });
            group4.ContextSubgroups.Add(subgroup7);

            var subgroup8 = new ContextMenuSubgroup() { Name = "Subgroup 8" };
            subgroup8.ContextCommands.Add(new DemoContextMenuCommand("Command 14", @"Icons/102.svg") { ID = Guid.Parse("{49E5A19C-9992-4C38-B63C-8E96CE519750}") });
            subgroup8.ContextCommands.Add(new DemoContextMenuCommand("Command 15", @"Icons/125.svg") { ID = Guid.Parse("{2D372D74-F712-4513-B2A7-BB092FA15660}") });
            group4.ContextSubgroups.Add(subgroup8);

            CommandsGroups.Add(group4);

            //var group5 = new ContextMenuGroup() { Name = "Group 5" };
            //CommandsGroups.Add(group5);
            //var group6 = new ContextMenuGroup() { Name = "Group 6" };
            //CommandsGroups.Add(group6);
            //var group7 = new ContextMenuGroup() { Name = "Group 7" };
            //CommandsGroups.Add(group7);
            //var group8 = new ContextMenuGroup() { Name = "Group 8" };
            //CommandsGroups.Add(group8);
            //var group9 = new ContextMenuGroup() { Name = "Group 9" };
            //CommandsGroups.Add(group9);
            //var group10 = new ContextMenuGroup() { Name = "Group 10" };
            //CommandsGroups.Add(group10);
            //CommandsGroups.Add(group1);

            if (File.Exists(CONFIG_FILE_PATH))
                MenuConfig = ContextMenuConfiguration.LoadConfig(File.ReadAllText(CONFIG_FILE_PATH));

            InitGantt();

        }

        internal void SaveContextMenuConfig()
        {
            File.WriteAllText(CONFIG_FILE_PATH, ContextMenuConfiguration.SaveConfigToJson(CommandsGroups));
        }

        private void InitGantt()
        {
            GanttItems = new ObservableCollection<TimeGanttItem>
            {
                new TimeGanttItem(DateTime.Now, DateTime.Now.AddMinutes(10), "Row 1", "Item 1", GanttItemInRowPosition.BottomHalf),
                new TimeGanttItem(DateTime.Now.AddMinutes(20), DateTime.Now.AddMinutes(30), "Row 1", "Item 2", GanttItemInRowPosition.UpperHalf),
                new TimeGanttItem(DateTime.Now, DateTime.Now.AddMinutes(30), "Row 2", "Item 3"),
                new TimeGanttItem(DateTime.Now, DateTime.Now.AddMinutes(10), "Row 3", "Item 4"),
                new TimeGanttItem(DateTime.Now, DateTime.Now.AddMinutes(10), "Row 4", "Item 5"),
                new TimeGanttItem(DateTime.Now, DateTime.Now.AddMinutes(10), "Row 5", "Item 5"),
                new TimeGanttItem(DateTime.Now, DateTime.Now.AddMinutes(10), "Row 6", "Item 5"),
                new TimeGanttItem(DateTime.Now, DateTime.Now.AddMinutes(10), "Row 7", "Item 5"),
                new TimeGanttItem(DateTime.Now, DateTime.Now.AddMinutes(10), "Row 8", "Item 5"),
                new TimeGanttItem(DateTime.Now, DateTime.Now.AddMinutes(10), "Row 9", "Item 5"),
                new TimeGanttItem(DateTime.Now, DateTime.Now.AddMinutes(10), "Row 10", "Item 5"),
                new TimeGanttItem(DateTime.Now, DateTime.Now.AddMinutes(10), "Row 11", "Item 5"),
                new TimeGanttItem(DateTime.Now, DateTime.Now.AddMinutes(100), "Row 12", "Item 5")
            };
        }

        private void RefreshGantt(object obj)
        {
            //InitGantt();
            //GanttItems = new ObservableCollection<ScheduleResultItem>();
            //GanttViewModel.Items = GanttItems;

            GanttItems.Add(new TimeGanttItem(DateTime.Now.Subtract(TimeSpan.FromMinutes(15)), GanttItems.FirstOrDefault().StartTime, "Row 4", "Item 6"));
            //RaisePropertyChanged(nameof(GanttItems));
        }
    }
}
