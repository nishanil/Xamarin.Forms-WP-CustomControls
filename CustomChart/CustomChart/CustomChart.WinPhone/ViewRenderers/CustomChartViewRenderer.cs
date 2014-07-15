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

[assembly: ExportRenderer((typeof(CustomChartView)), typeof(CustomChart.WinPhone.ViewRenderers.CustomChartViewRenderer))]

namespace CustomChart.WinPhone.ViewRenderers
{
    public class CustomChartViewRenderer : ViewRenderer<CustomChartView, XamDataChart>
    {
        XamDataChart DataChart;

        CategoryXAxis DateXAxis;
        NumericYAxis PriceYAxis;
        FinancialPriceSeries series;

        public CustomChartViewRenderer()
        {
            DataChart = new XamDataChart();
            DataChart.Margin = new System.Windows.Thickness(5);


            DateXAxis = new CategoryXAxis();
            DateXAxis.Label = "{}{Date:MM/yyyy}";

            DateXAxis.LabelSettings = new AxisLabelSettings();
            DateXAxis.LabelSettings.Visibility = System.Windows.Visibility.Collapsed;
            DateXAxis.LabelSettings.Foreground = (System.Windows.Media.Brush)System.Windows.Application.Current.Resources["PhoneForegroundBrush"];
            DateXAxis.LabelSettings.Location = AxisLabelsLocation.OutsideBottom;


            // YAxis
            PriceYAxis = new NumericYAxis();
            PriceYAxis.LabelSettings = new AxisLabelSettings();
            PriceYAxis.LabelSettings.Foreground = (System.Windows.Media.Brush)System.Windows.Application.Current.Resources["PhoneForegroundBrush"];

            PriceYAxis.LabelSettings.Extent = 40;
            PriceYAxis.LabelSettings.Location = AxisLabelsLocation.OutsideLeft;
            series = new FinancialPriceSeries();
            series.Thickness = 0.5;
            series. OpenMemberPath="Open";
            series.CloseMemberPath="Close";
            series.HighMemberPath="High";
            series. LowMemberPath="Low";
            series.VolumeMemberPath = "Volume";
            series.Brush = (System.Windows.Media.Brush)System.Windows.Application.Current.Resources["DataChartLightBrush1"];
            series.NegativeBrush = (System.Windows.Media.Brush)System.Windows.Application.Current.Resources["DataChartLightBrush12"];
            series.Outline = (System.Windows.Media.Brush)System.Windows.Application.Current.Resources["DataChartLightBrush2"];

            var legend = new Legend()
            {
                Name = "PriceLegend",
                VerticalAlignment = System.Windows.VerticalAlignment.Bottom,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                Margin = new System.Windows.Thickness(58, 15, 0, 18)
            }; 

            
       
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
            
            DataChart.Axes.Clear();
            DataChart.Axes.Add(DateXAxis);
            DataChart.Axes.Add(PriceYAxis);
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
            if (e.PropertyName == CustomChart.CustomControls.CustomChartView.ItemSourceProperty.PropertyName)
                UpdateChart();

        }

    }
}
