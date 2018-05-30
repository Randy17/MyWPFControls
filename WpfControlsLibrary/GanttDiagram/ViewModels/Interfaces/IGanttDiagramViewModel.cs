using System;
using System.Collections;
using System.Collections.ObjectModel;
using WpfControlsLibrary.GanttDiagram.Models;

namespace WpfControlsLibrary.GanttDiagram.ViewModels.Interfaces
{
    internal delegate void SelectedItemChangedDelegate(IGanttItem selectedItem);

    internal interface IGanttDiagramViewModel
    {
        #region Events

        event EventHandler ScaleStepChanged;
        event EventHandler GraphUpdated;
        event SelectedItemChangedDelegate SelectedItemChanged;

        #endregion

        #region Properties

        bool IsVisible { get; set; }
        string ServiceMessage { get; set; }
        uscGanttDiagram UscGanttDiagram { get; set; }
        int ScaleStep { get; set; }
        int MaxWidth { get; set; }
        ObservableCollection<ScaleValue> ScaleValues { get; set; }
        ObservableCollection<GanttRowViewModelBase> Rows { get; set; }
        string Caption { get; }
        int MinScaleStepValue { get; set; }
        int MaxScaleStepValue { get; set; }
        bool IsRangeSelectorVisible { get; set; }
        double LeftRangeSelectorPosition { get; set; }
        double RangeWidth { get; set; }

        #endregion

        #region Methods

        void CalculateScaleValues(double graphWidth);

        bool TrySetItems(IList newItems);

        void AddItem(IGanttItem newItem);
        void RemoveItem(IGanttItem item);

        #endregion
    }
}
