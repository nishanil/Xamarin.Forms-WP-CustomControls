using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace CustomChart.Helpers
{

    public class SimpleDataCollection : ObservableCollection<SimpleDataPoint>
    {
        public SimpleDataCollection()
        {
            this.Add(new SimpleDataPoint() { Label = "1", Value = 3.0 });
            this.Add(new SimpleDataPoint() { Label = "2", Value = 2.0 });
            this.Add(new SimpleDataPoint() { Label = "3", Value = 3.0 });
            this.Add(new SimpleDataPoint() { Label = "4", Value = 4.0 });
            this.Add(new SimpleDataPoint() { Label = "5", Value = 5.0 });
            this.Add(new SimpleDataPoint() { Label = "6", Value = 6.0 });
            this.Add(new SimpleDataPoint() { Label = "7", Value = 5.0 });

        }
    }
    /// <summary>
    /// Simple storage class for pair of string and double value
    /// </summary>
    public class SimpleDataPoint
    {
        public double Value { get; set; }
        public string Label { get; set; }
    }

}
