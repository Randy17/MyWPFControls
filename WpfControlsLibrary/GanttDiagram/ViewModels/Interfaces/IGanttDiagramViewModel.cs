using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WpfControlsLibrary.GanttDiagram.Models;
using WpfControlsLibrary.Infrastrucrure;

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
        bool IsAllRowsShrinked { get; set; }

        #endregion

        Command ShrinkAllRowsCmd { get; }
        Command UnshrinkAllRowsCmd { get; }

        #region Methods

        void CalculateScaleValues(double graphWidth);

        bool TrySetItems(IList newItems);
        bool TrySetThresholdLines(IList newLines);

        void AddItem(IGanttItem newItem);
        void RemoveItem(IGanttItem item);

        void AddThresholdLine(IThresholdLine newItem);
        void RemoveThresholdLine(IThresholdLine item);

        #endregion
    }
}
