using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAuth0App.ViewModels
{
    public class GraphTagPageViewModel
    {

        public static ObservableCollection<DateTimePoint> Data { get; set; }

        public ISeries[] Series { get; set; } = {
            new LineSeries<DateTimePoint>() {
                TooltipLabelFormatter = (point) => $"{new DateTime((long) point.SecondaryValue):dd/MM/yy HH:mm:ss} » {point.PrimaryValue}",
                LineSmoothness = 0,
                Values = Data,
                Stroke = new SolidColorPaint(SKColors.MediumPurple) { StrokeThickness = 6 },
                GeometryStroke = new SolidColorPaint(SKColors.MediumPurple) { StrokeThickness = 6 },
                Fill = new SolidColorPaint(SKColors.MediumPurple.WithAlpha(100))
            }
        };

        public Axis[] XAxes { get; set; } = {
            new Axis() {
                Labeler = value => new DateTime((long) value).ToString("dd/MM/yy HH:mm:ss"),
                LabelsRotation = 20,
                UnitWidth = TimeSpan.FromSeconds(1).Ticks,
                MinStep = TimeSpan.FromSeconds(1).Ticks,
                Name = "Orario"
            }
        };

        public Axis[] YAxes { get; set; } = {
            new Axis() {
                Name = "Valore"
            }
        };

    }
}
