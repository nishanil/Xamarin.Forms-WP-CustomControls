using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using CustomChart.CustomControls;
using Infragistics.Controls.Charts;
using CustomChart.Helpers;
using System.Windows.Controls;

[assembly: ExportRenderer((typeof(CustomChartView)), typeof(CustomChart.WinPhone.ViewRenderers.CustomChartViewRenderer))]

namespace CustomChart.WinPhone.ViewRenderers
{
    public class CustomChartViewRenderer : ViewRenderer<CustomChartView, XamDataChart>
    {
        XamDataChart DataChart;

        CategoryXAxis DateXAxis;
        NumericYAxis PriceYAxis, VolumeYAxis;
        FinancialPriceSeries series;
        SplineAreaSeries splineSeries;
        public CustomChartViewRenderer()
        {
            DataChart = new XamDataChart();
            DataChart.Margin = new System.Windows.Thickness(5);

            //XAxis
            DateXAxis = new CategoryXAxis();
            DateXAxis.Label = "{}{Date:MM/yyyy}";

            DateXAxis.LabelSettings = new AxisLabelSettings();
            DateXAxis.LabelSettings.Visibility = System.Windows.Visibility.Collapsed;
            DateXAxis.LabelSettings.Foreground = (System.Windows.Media.Brush)System.Windows.Application.Current.Resources["PhoneForegroundBrush"];
            DateXAxis.LabelSettings.Location = AxisLabelsLocation.OutsideBottom;


            // YAxis
            PriceYAxis = new NumericYAxis();
            PriceYAxis.LabelSettings = new AxisLabelSettings();
            PriceYAxis.LabelSettings.Extent = 40;
            PriceYAxis.LabelSettings.Location = AxisLabelsLocation.OutsideLeft;
            PriceYAxis.LabelSettings.Foreground = (System.Windows.Media.Brush)System.Windows.Application.Current.Resources["PhoneForegroundBrush"];

            VolumeYAxis = new NumericYAxis();
            VolumeYAxis.MajorStroke = (System.Windows.Media.Brush)System.Windows.Application.Current.Resources["TransparentBrush"];

            VolumeYAxis.LabelSettings = new AxisLabelSettings();
            VolumeYAxis.LabelSettings.Extent = 40;
            VolumeYAxis.LabelSettings.Location = AxisLabelsLocation.OutsideRight;
            VolumeYAxis.LabelSettings.Foreground = (System.Windows.Media.Brush)System.Windows.Application.Current.Resources["PhoneForegroundBrush"];

            VolumeYAxis.LabelSettings.Visibility = System.Windows.Visibility.Visible;
           

      
            series = new FinancialPriceSeries();
            
            series.Thickness = 0.5;
            series. OpenMemberPath="Open";
            series.CloseMemberPath="Close";
            series.HighMemberPath="High";
            series. LowMemberPath="Low";
            series.VolumeMemberPath = "Volume";
            series.Brush = (System.Windows.Media.Brush)System.Windows.Application.Current.Resources["MetroThemeAccentColor"];
            series.NegativeBrush = (System.Windows.Media.Brush)System.Windows.Application.Current.Resources["DataChartLightBrush12"];
            series.Outline = (System.Windows.Media.Brush)System.Windows.Application.Current.Resources["LightForegroundBrush"];

            splineSeries = new SplineAreaSeries();
            splineSeries.ValueMemberPath = "Volume";
        
            series.SetValue(Canvas.ZIndexProperty, 1);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CustomChartView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || this.Element == null)
                return;
            UpdateChart();
            SetNativeControl(DataChart);
        }

        private void UpdateChart()
        {
            DataChart.DataContext = this.Element.ItemSource;

            DateXAxis.ItemsSource = this.Element.ItemSource;
            series.XAxis = DateXAxis;
            series.YAxis = PriceYAxis;
            
            series.ItemsSource = this.Element.ItemSource;
            series.DisplayType = this.Element.PriceDisplayType.ToIGPriceType();


            splineSeries.ItemsSource = this.Element.ItemSource;
            splineSeries.XAxis = DateXAxis;
            splineSeries.YAxis = VolumeYAxis;
            

            DataChart.Axes.Clear();
            DataChart.Axes.Add(DateXAxis);
            DataChart.Axes.Add(PriceYAxis);
            DataChart.Axes.Add(VolumeYAxis);

            DataChart.Series.Clear();
            DataChart.Series.Add(series);

            
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null || Element == null)
                return;
            if (e.PropertyName == CustomChart.CustomControls.CustomChartView.PriceDisplayTypeProperty.PropertyName)
                series.DisplayType = Element.PriceDisplayType.ToIGPriceType();
            if (e.PropertyName == CustomChart.CustomControls.CustomChartView.ShowSplineProperty.PropertyName)
                ShowHideSpline();

        }

        private void ShowHideSpline()
        {
            if (this.Element.ShowSpline)
                DataChart.Series.Add(splineSeries);
            else if (DataChart.Series.Contains(splineSeries))
                DataChart.Series.Remove(splineSeries);

        }

    }
}
