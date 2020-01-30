using ChartJs.Blazor.ChartJS.Common.Time;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSK.Online.Model
{
    public class LineDataXpress
    {
        [JsonProperty("w")]
        public double Wave { get; set; }

        /// <summary>
        /// The y-value for this <see cref="TimeTuple{TData}"/>.
        /// </summary>
        [JsonProperty("y")]
        public double YValue { get; set; }

    }
    public class LineDataItem<TData>
    {
        
        public LineDataItem(double wave, TData yValue)
        {
            Wave = wave;
            YValue = yValue;
        }

        /// <summary>
        /// The time-/x-value for this <see cref="TimeTuple{TData}"/>.
        /// </summary>
        [JsonProperty("w")]
        public double Wave { get; set; }

        /// <summary>
        /// The y-value for this <see cref="TimeTuple{TData}"/>.
        /// </summary>
        [JsonProperty("y")]
        public TData YValue { get; set; }
    }
}
