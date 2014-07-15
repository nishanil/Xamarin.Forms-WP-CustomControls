using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomChart.WinPhone.ViewRenderers
{
    public static class Helper
    {
        public static Infragistics.Controls.Charts.PriceDisplayType ToIGPriceType(this CustomChart.CustomControls.PriceDisplayType priceType)
        {

            if (priceType == CustomChart.CustomControls.PriceDisplayType.OHLC)
                return Infragistics.Controls.Charts.PriceDisplayType.OHLC;

            return Infragistics.Controls.Charts.PriceDisplayType.Candlestick;
        }
    }
}
