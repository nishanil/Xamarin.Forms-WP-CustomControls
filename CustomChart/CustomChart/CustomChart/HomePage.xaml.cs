using CustomChart.CustomControls;
using CustomChart.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomChart
{
	public partial class HomePage
	{
        private StockMarketServiceClient _client;
        private StockMarketDataSample _data;
		public HomePage ()
		{
			InitializeComponent ();

            chartPicker.SelectedIndexChanged += chartPicker_SelectedIndexChanged;
            StartButton.Clicked += StartButton_Clicked;
            StopButton.Clicked += StopButton_Clicked;
            ShowSplineSwitch.Toggled += ShowSplineSwitch_Toggled;
            _client = new StockMarketServiceClient();

            chart.ItemSource = _data = new StockMarketDataSample();

            foreach (var chartType in Enum.GetValues(typeof(PriceDisplayType)))
            {
                chartPicker.Items.Add(chartType.ToString());
            }
            chartPicker.SelectedIndex = 0;
		}

        void ShowSplineSwitch_Toggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            chart.ShowSpline = e.Value;
        }

        void OnStockMarketDataReceived(object sender, StockMarketDataReceivedEventArgs e)
        {
            _data.RemoveAt(0);
            // add the new StockMarketDataPoint to the collection of StockMarketDataPoint objects
            _data.Add(e.NewDataPoint);
        }

        void StopButton_Clicked(object sender, EventArgs e)
        {
            _client.Stop();
            _client.StockMarketDataReceived -= OnStockMarketDataReceived;
        }

        void StartButton_Clicked(object sender, EventArgs e)
        {
            _client.StockMarketDataReceived += OnStockMarketDataReceived;
            _client.Start();
        }

        void chartPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            PriceDisplayType priceDisplayType;
            if (Enum.TryParse<PriceDisplayType>(chartPicker.Items[chartPicker.SelectedIndex], out priceDisplayType))
                chart.PriceDisplayType = priceDisplayType;
        }
	}
}
