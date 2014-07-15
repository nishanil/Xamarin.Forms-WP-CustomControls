using CustomChart.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CustomChart.CustomControls
{
    public class CustomChartView : View
    {
        public static readonly BindableProperty PriceDisplayTypeProperty = BindableProperty.Create<CustomChartView, PriceDisplayType>(p => p.PriceDisplayType, PriceDisplayType.CandleStick);

        public PriceDisplayType PriceDisplayType
        {
            get { return (PriceDisplayType)GetValue(PriceDisplayTypeProperty); }
            set { SetValue(PriceDisplayTypeProperty, value); }
        }

        public static readonly BindableProperty ItemSourceProperty = BindableProperty.Create<CustomChartView, StockMarketDataSample>(p => p.ItemSource, new StockMarketDataSample());

        public StockMarketDataSample ItemSource
        {
            get { return (StockMarketDataSample)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }

        public void Start()
        { 
        }

        public void Stop()
        {

        }
    }

    public enum PriceDisplayType
    {
        CandleStick = 0,
        OHLC = 1
    }
}
