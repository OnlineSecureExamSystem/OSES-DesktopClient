using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;
using System;
using System.Collections.Generic;
using LiveChartsCore.Measure;

namespace DesktopClient.ViewModels
{
    public class TestViewModel : ViewModelBase, IRoutableViewModel
    {
        IEnumerable<ISeries> Data { get; set; }

        private readonly Random _random = new();
        public string? UrlPathSegment => throw new NotImplementedException();
        public ObservableValue ObservableValue1 { get; set; }

        public IScreen HostScreen { get; }

        public TestViewModel(IScreen screen)
        {
            ObservableValue1 = new ObservableValue { Value = 4.56 };

            HostScreen = screen;
            Data = new GaugeBuilder()
            {
                OffsetRadius = 5,
                LabelsPosition = PolarLabelsPosition.ChartCenter,
                LabelsSize = 70,
            }.AddValue(ObservableValue1, "Speed")
            .BuildSeries();
        }
    }
}
