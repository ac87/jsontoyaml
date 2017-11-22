using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace MbJsonToYaml.Utils
{
    public class Helper
    {
        public const int CasingZoomAdjust = 5;

        public static string ProcessWidth(StoppedDouble widthValue, bool isCasing) // if it casing fudge to make them smaller
        {
            if (widthValue != null)
            {
                if (widthValue.Stops != null)
                {
                    if (widthValue.Stops.Count > 1)
                    {
                        List<string> values = new List<string>();

                        if (widthValue.Base > 1 && widthValue.Stops.Count >= 2)
                            CalculateAllStops(widthValue.Base, widthValue.Stops, values, isCasing);
                        else
                        {
                            foreach (var stop in widthValue.Stops)
                            {
                                int zoom;
                                double value;
                                ParseZoomAndValue(stop, out zoom, out value);

                                if (isCasing)
                                    zoom = zoom + CasingZoomAdjust;

                                values.Add(CreateArrayString(zoom, value));
                            }
                        }
                        return JoinValueArray(values);
                    }
                    var width = widthValue.Stops[widthValue.Stops.Count - 1];
                    return $"'{width[width.Count - 1]}px'";
                }

                if (widthValue.SingleVal != double.MinValue)
                {
                    return $"{widthValue.SingleVal.ToString("F2")}px";
                }                
            }

            Converter.AppendDebug($"Did not process - ProcessWidth", widthValue);
            return "'30px'";
        }

        public static string ProcessColor(object colorValue, StoppedDouble opacity)
        {
            if (colorValue != null)
            {
                if (colorValue is string)
                    return ColorUtils.ProcessColorString(colorValue, opacity);            
                if (colorValue is JObject)
                {
                    JObject val = (JObject)colorValue;

                    if (val.First.Path == "base")
                    {
                        //double scale = double.Parse(((JValue)val.First.Last).Value.ToString());
                        List<string> values = new List<string>();
                        foreach (JArray stop in val.Last.Last)
                        {
                            List<object> stopAsList = stop.ToObject<List<object>>();
                            int zoom = (int) Math.Ceiling(double.Parse(stopAsList[0].ToString()));
                            string color = ColorUtils.ProcessColorString(stopAsList[1].ToString(), opacity);

                            values.Add(CreateArrayString(zoom, color));
                        }                        
                        return JoinValueArray(values);                        
                    }

                    return ColorUtils.ProcessColorString(val.Last.Last.Last.Last.ToString(), opacity);
                }

                Converter.AppendDebug($"Did not process - ProcessColor", colorValue);
                return "#3f3f3f";
            }

            Converter.AppendDebug($"Did not process - ProcessColor", colorValue);
            return "";
        }

        public static string ProcessTextSize(object sizeValue)
        {
            if (sizeValue != null)
            {
                if (sizeValue is string)
                    return $"'{(string)sizeValue}'";
                if (sizeValue is long || sizeValue is int)
                    return $"'{sizeValue.ToString()}'";
                if (sizeValue is double || sizeValue is float)
                    return $"'{Math.Floor(double.Parse(sizeValue.ToString()))}'";
                if (sizeValue is JObject)
                {
                    JObject val = (JObject)sizeValue;

                    string result = GetStopsFromJObject(val, false);
                    if (result != null)
                        return result;

                    return val.Last.Last.ToString().Replace("\r\n", "").Replace(" ", "");
                }
                return "'16px'";
            }

            return "";
        }

        private static string CreateArrayString(int zoom, double value)
        {
            return $"[{zoom},{value.ToString("0.##")}px]";
        }

        private static string CreateArrayString(int zoom, string value)
        {
            return $"[{zoom},{value}]";
        }

        private static string JoinValueArray(List<string> values)
        {
            return $"[{ string.Join(",", values)}]";
        }

        private static void CalculateAllStops(double scale, IList<IList<object>> stops, List<string> values, bool isCasing)
        {
            List<Tuple<int, double>> zoomValues = new List<Tuple<int, double>>();

            foreach (var stop in stops)
            {
                int zoom;
                double value;
                ParseZoomAndValue(stop, out zoom, out value);
                zoomValues.Add(new Tuple<int, double>(zoom, value));
            }

            Tuple<int, double> first = zoomValues[0];
            Tuple<int, double> last = zoomValues[zoomValues.Count - 1];

            int minZoom = first.Item1;
            double minZoomValue = first.Item2;

            int maxZoom = last.Item1;
            double maxZoomValue = last.Item2;

            for (int i = minZoom + 1; i < maxZoom; i++)
            {
                if (zoomValues.Find(s => s.Item1 == i) == null)
                {
                    double val = GetValueForZoomLevel(i, scale, minZoom, minZoomValue, maxZoom, maxZoomValue);
                    zoomValues.Insert(zoomValues.Count - 1, new Tuple<int, double>(i, val));
                }
            }

            if (zoomValues.Count > 2)
            {
                zoomValues.Sort((x, y) => x.Item1.CompareTo(y.Item1));
            }

            foreach (var zoomValue in zoomValues)
            {
                int zoom = zoomValue.Item1;
                if (isCasing)
                    zoom = zoom + CasingZoomAdjust;
                string value = CreateArrayString(zoom, zoomValue.Item2);
                values.Add(value);
            }
        }

        private static void ParseZoomAndValue(IList<object> stop, out int zoom, out double value)
        {
            zoom = (int) Math.Ceiling( stop[0] is double ? (double)stop[0] : double.Parse(stop[0].ToString()) );
            value = stop[1] is double ? (double)stop[1] : double.Parse(stop[1].ToString());
        }

        private static double GetValueForZoomLevel(int targetZoom, double scale, int minZoom, double minZoomValue, int maxZoom, double maxZoomValue)
        {
            return (double)
                minZoomValue +
                            (Math.Pow(scale, targetZoom - minZoom) - 1) * (maxZoomValue - minZoomValue) /
                            Math.Pow(scale, maxZoom - minZoom) + 0.5;
        }

        private static string GetStopsFromJObject(JObject val, bool isCasing)
        {
            string result = null;
            if (val.First.Path == "base")
            {
                double scale = double.Parse(((JValue)val.First.Last).Value.ToString());

                if (scale < 1)
                    scale = 1;

                if (scale == 1)
                {
                    List<string> values = new List<string>();
                    foreach (JArray stop in val.Last.Last)
                    {
                        List<object> stopAsList = stop.ToObject<List<object>>();
                        int zoom;
                        double value;
                        ParseZoomAndValue(stopAsList, out zoom, out value);
                        values.Add(CreateArrayString(zoom, value));
                    }
                    result = JoinValueArray(values);
                }
                else
                {                    
                    IList<IList<object>> outerList = new List<IList<object>>();

                    foreach (JArray stop in val.Last.Last)
                    {
                        List<object> stopAsList = stop.ToObject<List<object>>();
                        outerList.Add(stopAsList);
                    }
                    List<string> values = new List<string>();
                    CalculateAllStops(scale, outerList, values, isCasing);
                    result = JoinValueArray(values);                    
                }
            }

            return result;
        }

        public static string ProcessStoppable(object value)
        {
            if (value is string)
            {
                return $"{value}";
            }
            List<string> values = new List<string>();

            if (value is JObject)
            {
                Converter.AppendDebug($"Part processed - ProcessStoppable", value);
                JObject val = (JObject)value;
                return $"{val.Last.Last.Last.Last}";
            }

            Converter.AppendDebug($"Did not process - ProcessStoppable", value);
            return "";
        }

        public static string ProcessFont(object textFont)
        {
            if (textFont is string)
                return textFont.ToString();

            if (textFont is JArray)
            {
                JArray val = (JArray)textFont;
                List<object> list = val.ToObject<List<object>>();
                return $"[{String.Join(",", list)}]";
            }

            if (textFont is JObject)
            {
                return ProcessStoppable(textFont);
            }

            return "";
        }
    }
}
