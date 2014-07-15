using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace CustomChart.Helpers
{

    public class StockMarketDataSample : StockMarketDataCollection
    {

    }
    public class StockMarketDataCollection : ObservableCollection<StockMarketDataPoint>
    {
        public StockMarketDataCollection()
        {
            _stockSettings = new StockMarketSettings();
            this.Generate();
        }
        public void Generate()
        {
            this.Clear();
            StockMarketGenerator.Settings = this.Settings;
            StockMarketGenerator.Generate();
            foreach (StockMarketDataPoint dp in StockMarketGenerator.Data)
            {
                this.Add(dp);
            }
        }
        private StockMarketSettings _stockSettings;
        /// <summary>
        /// Settings used to generate StockMarketDataPoint objects
        /// </summary>
        public StockMarketSettings Settings
        {
            get { return _stockSettings; }
            set
            {
                _stockSettings = value;
                this.Generate();
                OnPropertyChanged(new PropertyChangedEventArgs("Settings"));
            }
        }

    }
    public class StockMarketSettings : DataSettings
    {
        public StockMarketSettings()
        {
            DataPoints = 50;

            VolumeStart = 2000;
            VolumeRange = 50;
            VolumeSample = 4;

            PriceStart = 1000;
            PriceRange = 50;
            PriceSample = 4;

            DateStart = new DateTime(2010, 1, 1);
            DateInterval = TimeSpan.FromDays(1);
        }

        #region Properties
        /// <summary>
        /// Determines range for used to generate random Volume value 
        /// </summary>
        public int VolumeRange { get; set; }
        /// <summary>
        /// Determines number of samples used to generate random Volume values
        /// </summary>
        public int VolumeSample { get; set; }
        /// <summary>
        /// Determines starting Volume value for the first StockMarketDataPoint object
        /// </summary>
        public double VolumeStart { get; set; }

        /// <summary>
        /// Determines range used to generate random Open, High, Low, and Close values
        /// </summary>
        public int PriceRange { get; set; }
        /// <summary>
        /// Determines number of samples used value used to generate random Price values
        /// </summary>
        public int PriceSample { get; set; }
        /// <summary>
        /// Determines starting Price value for the first StockMarketDataPoint object
        /// </summary>
        public double PriceStart { get; set; }

        /// <summary>
        /// Determines time intervals between StockMarketDataPoint objects
        /// </summary>
        public TimeSpan DateInterval { get; set; }
        /// <summary>
        /// Determines starting date value for the first StockMarketDataPoint object
        /// </summary>
        public DateTime DateStart { get; set; }
        #endregion
    }
    public class StockMarketDataPoint : ObservableModel
    {
        #region Constructors
        public StockMarketDataPoint()
            : this(new DateTime(2010, 1, 1), 100, 120, 90, 110, 1000)
        { }

        public StockMarketDataPoint(DateTime date, double open, double high, double low, double close, double volume)
        {
            _date = date;
            _open = open;
            _high = high;
            _low = low;
            _close = close;
            _volume = volume;
        }
        public StockMarketDataPoint(string date, double open, double high, double low, double close, double volume)
            : this(DateTime.Parse(date, System.Globalization.CultureInfo.InvariantCulture),
                open, high, low, close, volume)
        { }

        public StockMarketDataPoint(StockMarketDataPoint dataPoint)
        {
            _date = dataPoint.Date;
            _open = dataPoint.Open;
            _high = dataPoint.High;
            _low = dataPoint.Low;
            _close = dataPoint.Close;
            _volume = dataPoint.Volume;
            _index = dataPoint.Index;
        }
        #endregion

        #region Properties
        private double _open;
        private double _high;
        private double _low;
        private double _close;
        private double _volume;
        private DateTime _date;
        private int _index;
        private string _label;

        public int Index { get { return _index; } set { if (_index == value) return; _index = value; OnPropertyChanged("Index"); } }
        public double Open { get { return _open; } set { if (_open == value) return; _open = value; OnPropertyChanged("Open"); } }
        public double High { get { return _high; } set { if (_high == value) return; _high = value; OnPropertyChanged("High"); } }
        public double Low { get { return _low; } set { if (_low == value) return; _low = value; OnPropertyChanged("Low"); } }
        public double Close { get { return _close; } set { if (_close == value) return; _close = value; OnPropertyChanged("Close"); } }
        public double Volume { get { return _volume; } set { if (_volume == value) return; _volume = value; OnPropertyChanged("Volume"); } }
        public DateTime Date { get { return _date; } set { if (_date == value) return; _date = value; OnPropertyChanged("Date"); } }

        public string Category { get { return _label; } set { if (_label == value) return; _label = value; OnPropertyChanged("Category"); } }




        /// <summary>
        /// returns the difference between Close and Open values
        /// </summary>
        public double Change
        {
            get { return Close - Open; }
        }
        public double ChangePerCent
        {
            get { return (Change / Open) * 100; }
        }
        #endregion

        public new string ToString()
        {
            return String.Format(
                "Index {0}, Open {1}, High {2}, Low {3}, Close {4}, Change {5}, Volume {6}, Date {7}",
                Index, Open, High, Low, Close, Change, Volume, Date);
        }

    }
    public static class StockMarketGenerator
    {
        static StockMarketGenerator()
        {
            Generator = new Random();
            Settings = new StockMarketSettings();
            Data = GenerateData();
        }
        #region Properties
        public static StockMarketSettings Settings { get; set; }
        public static List<StockMarketDataPoint> Data { get; set; }
        public static StockMarketDataPoint LastDataPoint { get; set; }

        #endregion
        #region Fields
        private static readonly Random Generator;
        #endregion
        #region Methods
        public static void Generate()
        {
            Data = GenerateData();
        }
        public static DateTime GenerateStockDate(DateTime lastDate)
        {
            return lastDate.Add(Settings.DateInterval);
        }

        #region Methods for Random GeneratorMode


        public static double GenerateStockVolume(double preVolume)
        {
            double sum = 0;
            int min = (int)preVolume - Settings.VolumeRange;
            int max = (int)preVolume + Settings.VolumeRange;
            for (int i = 0; i < Settings.VolumeSample; i++)
            {
                sum += (double)Generator.Next(min, max);
            }
            double volume = sum / Settings.VolumeSample;
            return System.Math.Abs(volume);
        }

        public static double CurrentRandomValue;
        public static double GenerateStockRandom()
        {
            double nextRandomValue = Generator.NextDouble();
            if (nextRandomValue > .5)
                CurrentRandomValue += nextRandomValue * Settings.PriceRange;
            else
                CurrentRandomValue -= nextRandomValue * Settings.PriceRange;
            return CurrentRandomValue;
        }
        public static double GenerateStockOpen(double preClose)
        {
            //open value always equals to previous close value
            return preClose;
        }
        public static double GenerateStockOpen(double low, double high)
        {
            int min = (int)System.Math.Floor(System.Math.Min(low, high));
            int max = (int)System.Math.Ceiling(System.Math.Max(low, high));
            return System.Math.Abs((double)Generator.Next(min, max));
            // or return (low + high) / 2.0;
        }
        public static double GenerateStockHigh(double open)
        {
            double sum = 0;
            int min = (int)open;
            int max = (int)open + Settings.PriceRange;
            for (int i = 0; i < Settings.PriceSample; i++)
            {
                sum += (double)Generator.Next(min, max);
            }
            return System.Math.Abs(sum / Settings.PriceSample);
        }
        public static double GenerateStockLow(double open)
        {
            double sum = 0;
            int min = (int)open - Settings.PriceRange;
            int max = (int)open;
            for (int i = 0; i < Settings.PriceSample; i++)
            {
                sum += (double)Generator.Next(min, max);
            }
            return System.Math.Abs(sum / Settings.PriceSample);
        }
        public static double GenerateStockClose(double low, double high)
        {
            int min = (int)System.Math.Floor(System.Math.Min(low, high));
            int max = (int)System.Math.Ceiling(System.Math.Max(low, high));
            return System.Math.Abs((double)Generator.Next(min, max));
            // or return (low + high) / 2.0;
        }


        #endregion

        /// <summary>
        /// Generate new StockMarketDataPoint based on the passed StockMarketDataPoint
        /// </summary>
        /// <param name="dataPoint"></param>
        /// <returns></returns>
        public static StockMarketDataPoint GenerateDataPoint(StockMarketDataPoint dataPoint)
        {
            double open = GenerateStockOpen(dataPoint.Close);
            //double open = GenerateStockOpen(dataPoint.Low, dataPoint.High);
            //double open = GenerateStockRandom();
            double high = GenerateStockHigh(open);
            double low = GenerateStockLow(open);
            double close = GenerateStockClose(low, high);

            double volume = GenerateStockVolume(dataPoint.Volume);
            DateTime date = dataPoint.Date.Add(Settings.DateInterval);
            int index = dataPoint.Index + 1;

            LastDataPoint = new StockMarketDataPoint
            {
                Index = index,
                Date = date,
                Open = open,
                Close = close,
                High = high,
                Low = low,
                Volume = volume
            };
            return LastDataPoint;
        }

        /// <summary>
        /// Generate new StockMarketDataPoint based on the last StockMarketDataPoint in List of StockMarketDataPoint 
        /// </summary>
        /// <returns></returns>
        public static StockMarketDataPoint GenerateDataPoint()
        {
            StockMarketDataPoint newDataPoint = GenerateDataPoint(LastDataPoint);
            return newDataPoint;
        }

        /// <summary>
        /// Generate List of StockMarketDataPoint based on the Settings
        /// </summary>
        /// <returns></returns>
        public static List<StockMarketDataPoint> GenerateData()
        {
            List<StockMarketDataPoint> data = new List<StockMarketDataPoint>();

            StockMarketDataPoint dataPoint = new StockMarketDataPoint
            {
                Index = -1,
                Close = Settings.PriceStart,
                Volume = Settings.VolumeStart,
                Date = Settings.DateStart
            };
            CurrentRandomValue = Settings.PriceStart;

            for (int i = 0; i < Settings.DataPoints; i++)
            {
                dataPoint = GenerateDataPoint(dataPoint);
                data.Add(dataPoint);
            }
            return data;
        }

        /// <summary>
        /// Append new StockMarketDataPoint to existing StockMarketData
        /// </summary>
        public static void AppendDataPoint()
        {
            Data.Add(GenerateDataPoint());
        }

        #endregion

    }


    public abstract class ObservableModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class DataSettings : ObservableModel
    {
        public DataSettings()
        {
            DataPoints = 100;
        }
        public int DataPoints { get; set; }

    }
}
