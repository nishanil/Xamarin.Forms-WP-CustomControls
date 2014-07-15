using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;

namespace CustomChart.Helpers
{
    /// <summary>
    /// StockMarketServiceClient simulate receiviving StockMarketDataPoint objects
    /// in intervals equal to value StockRequestInterval property
    /// </summary>
    public class StockMarketServiceClient
    {
        public StockMarketServiceClient()
        {
            this.StockRequestInterval = TimeSpan.FromMilliseconds(100);
            _lastDataPoint = StockMarketGenerator.LastDataPoint;
        }

        private DispatcherTimer _timer;
        private StockMarketDataPoint _lastDataPoint;

        public TimeSpan StockRequestInterval { get; set; }

        public void Start()
        {
            _timer = new DispatcherTimer { Interval = StockRequestInterval };
            _timer.Tick += OnTimerTick;
            _timer.Start();
        }

        public void Stop()
        {
            if (_timer == null) return;
            _timer.Stop();
            _timer = null;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            // generate new StockMarketData using StockMarketGenerator
            _lastDataPoint = StockMarketGenerator.GenerateDataPoint(_lastDataPoint);
            OnStockMarketDataReceived(_lastDataPoint);
        }

        public delegate void StockMarketDataReceivedEventHandler(object sender, StockMarketDataReceivedEventArgs e);
        public event StockMarketDataReceivedEventHandler StockMarketDataReceived;

        public void OnStockMarketDataReceived(StockMarketDataPoint data)
        {
            if (StockMarketDataReceived != null)
            {
                StockMarketDataReceived(this, new StockMarketDataReceivedEventArgs(data));
            }
        }
    }

    public class StockMarketDataReceivedEventArgs : EventArgs
    {
        public StockMarketDataReceivedEventArgs(StockMarketDataPoint data)
        {
            this.NewDataPoint = data;
        }

        public StockMarketDataPoint NewDataPoint { get; set; }
    }
}
