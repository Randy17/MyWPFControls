using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfControlsLibrary.CustomizableContextMenu.Configuration;
using WpfControlsLibrary.CustomizableContextMenu.Models;
using WpfControlsLibrary.Infrastrucrure;

namespace WpfControlsLibrary.CustomizableContextMenu.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private const int MAX_ROW_ITEMS_COUNT = 10;
        private const int MIN_ROW_ITEMS_COUNT = 9;
        private const int MAX_ITEMS_IN_FAVORITES = MAX_ROW_ITEMS_COUNT * 2;

        #region Fields
        private ContextMenuGroup _selectedGroup;
        private int _maxItemsInMainArea = 9;
        private List<ContextMenuGroup> _groups;
        private IEnumerable<ContextMenuCommand> _commands;
        private List<ContextMenuCommand> _suggestionsSource;
        private ObservableCollection<ContextMenuCommand> _favorites;
        private ObservableCollection<ContextMenuCommand> _suggestions;
        private bool _isMenuOpen = true;
        private bool _isExtendedOpen;
        private double _verticalOffset;
        private double _horizontalOffset;

        private Command _addToFavoritesCmd;
        private Command _removeFromFavoritesCmd;
        private Command _executeContextCommandCmd;        
        #endregion

        #region Properties
        public ObservableCollection<ContextMenuCommand> Favorites
        {
            get
            {
                return _favorites;
            }
            private set
            {
                _favorites = value;
                RaisePropertyChanged("Favorites");
            }
        }
        public ObservableCollection<ContextMenuCommand> Suggestions
        {
            get
            {
                return _suggestions;
            }
            private set
            {
                _suggestions = value;
                RaisePropertyChanged("Suggestions");
            }
        }
        public ObservableCollection<ContextMenuGroup> Groups
        {
            get;
        }
        public ContextMenuGroup SelectedGroup
        {
            get
            {
                return _selectedGroup;
            }
            set
            {
                _selectedGroup = value;
                RaisePropertyChanged("SelectedGroup");
            }
        }
        public int MaxItemsInMainArea
        {
            get
            {
                return _maxItemsInMainArea;
            }
            private set
            {
                if(_maxItemsInMainArea != value)
                {
                    if (value > MAX_ROW_ITEMS_COUNT)
                        _maxItemsInMainArea = MAX_ROW_ITEMS_COUNT;
                    else if (value < MIN_ROW_ITEMS_COUNT)
                        _maxItemsInMainArea = MIN_ROW_ITEMS_COUNT;
                    else
                        _maxItemsInMainArea = value;

                    RaisePropertyChanged("MaxItemsInMainArea");
                }
            }
        }
        public bool IsMenuOpen
        {
            get
            {
                return _isMenuOpen;
            }
            set
            {
                _isMenuOpen = value;
                RaisePropertyChanged("IsMenuOpen");
                if(!_isMenuOpen)
                {
                    IsExtendedOpen = false;
                }
            }
        }
        public bool IsExtendedOpen
        {
            get
            {
                return _isExtendedOpen;
            }
            private set
            {
                _isExtendedOpen = value;                
                RaisePropertyChanged("IsExtendedOpen");
            }
        }
        #endregion

        #region Commands
        public Command AddToFavoritesCmd
        {
            get
            {
                if(_addToFavoritesCmd == null)
                {
                    _addToFavoritesCmd = new Command(AddToFavorites, o => Favorites.Count < MAX_ITEMS_IN_FAVORITES);
                }
                return _addToFavoritesCmd;
            }
        }
        public Command RemoveFromFavoritesCmd
        {
            get
            {
                if (_removeFromFavoritesCmd == null)
                {
                    _removeFromFavoritesCmd = new Command(RemoveFromFavorites);
                }
                return _removeFromFavoritesCmd;
            }
        }        
        public Command ExecuteContextCommandCmd
        {
            get
            {
                if (_executeContextCommandCmd == null)
                    _executeContextCommandCmd = new Command(ExecuteContextCommand);
                return _executeContextCommandCmd;
            }
        }        
        #endregion

        internal MainViewModel(List<ContextMenuGroup> groups)
        {
            _groups = groups;

            _commands = _groups.SelectMany(group => group.ContextSubgroups.SelectMany(sg => sg.ContextCommands));
            BuildMenu(_commands);

            Groups = new ObservableCollection<ContextMenuGroup>(groups);
            SelectedGroup = Groups.FirstOrDefault();
        }

        #region Methods
        private void BuildMenu(IEnumerable<ContextMenuCommand> commands)
        {
            var favorites = commands.Where(c => c.IsFavorite).OrderBy(c => c.PositionInFavorites).Take(MAX_ROW_ITEMS_COUNT * 2);            
            Favorites = new ObservableCollection<ContextMenuCommand>(favorites);
            for (int i = 0; i < Favorites.Count; i++)
            {
                Favorites[i].PositionInFavorites = i + 1;
            }

            _suggestionsSource = commands.Where(c => c.IsFavorite == false).ToList();
            RefreshSuggestions();            

            MaxItemsInMainArea = Math.Max(Favorites.Count, Suggestions.Count);
        }
        private void RefreshSuggestions()
        {
            Suggestions = new ObservableCollection<ContextMenuCommand>(_suggestionsSource.OrderByDescending(c => c.UsesCount).ThenByDescending(c => c.Priority).Take(MAX_ROW_ITEMS_COUNT));
        }
        internal void LoadConfiguration(ContextMenuConfiguration configuration)
        {
            if (configuration == null)
                return;

            foreach (var cmdConfig in configuration._commands)
            {
                var cmd = _commands.FirstOrDefault(c => c.ID == cmdConfig.ID);
                if(cmd != null)
                {
                    cmd.IsFavorite = cmdConfig.IsFavorite;
                    cmd.UsesCount = cmdConfig.UsesCount;
                    cmd.PositionInFavorites = cmdConfig.PositionInFavorites;
                }
            }

            BuildMenu(_commands);
        }

        private void AddToFavorites(object obj)
        {
            ContextMenuCommand cmdToAdd = obj as ContextMenuCommand;
            if (cmdToAdd == null)
                return;

            if (!Favorites.Contains(cmdToAdd))
            {
                cmdToAdd.PositionInFavorites = Favorites.Count + 1;
                Favorites.Add(cmdToAdd);
                cmdToAdd.IsFavorite = true;

                if (Suggestions.Contains(cmdToAdd))
                {
                    _suggestionsSource.Remove(cmdToAdd);
                    RefreshSuggestions();
                }
                else
                {
                    _suggestionsSource.Remove(cmdToAdd);
                }

                MaxItemsInMainArea = Math.Max(Favorites.Count, Suggestions.Count);

                AddToFavoritesCmd.RaiseCanExecuteChanged();
            }
        }
        private void RemoveFromFavorites(object obj)
        {
            ContextMenuCommand cmdToRemove = obj as ContextMenuCommand;
            if (cmdToRemove == null)
                return;

            if (Favorites.Contains(cmdToRemove))
            {
                Favorites.Remove(cmdToRemove);
                cmdToRemove.PositionInFavorites = 0;
                for (int i = 0; i < Favorites.Count(); i++)
                {
                    Favorites[i].PositionInFavorites = i + 1;
                }
                cmdToRemove.IsFavorite = false;

                _suggestionsSource.Add(cmdToRemove);
                RefreshSuggestions();

                MaxItemsInMainArea = Math.Max(Favorites.Count, Suggestions.Count);
            }
        }
        private void ExecuteContextCommand(object obj)
        {
            ContextMenuCommand cmdToExecute = obj as ContextMenuCommand;
            if (cmdToExecute == null)
                return;

            if (cmdToExecute.Command != null)
            {
                cmdToExecute.Command.Execute(cmdToExecute.CommandParameter);
                cmdToExecute.UsesCount++;
            }

            IsMenuOpen = false;
            RefreshSuggestions();

        }
        #endregion
    }
}
