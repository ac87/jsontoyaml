using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using static MbJsonToYaml.Constants;

namespace MbJsonToYaml.Utils
{
    public class Filters
    {        
        public static List<string> GetLines(Layer layer, bool isParentCasingGroup)
        {
            IList<object> filterList = layer.Filter;

            string qualifier = "?";
            List<Filter> filters = new List<Filter>();
            List<Filter> notFilters = new List<Filter>();

            if (filterList != null && filterList.Count > 0)
            {
                if (filterList[0] is String)
                {
                    ProcessFilter(filterList, filters, notFilters);

                    if (filterList[0] is String && filterList[filterList.Count - 1] is JArray)
                    {
                        qualifier = (string)filterList[0];

                        foreach (object subFilter in filterList)
                        {
                            if (subFilter is String)
                                qualifier = subFilter.ToString();

                            if (subFilter is JArray)
                            {
                                JArray array = (JArray)subFilter;
                                List<object> list = array.ToObject<List<object>>();

                                if (list[0] is String && !(list[1] is string))
                                {
                                    qualifier = (string)list[0];

                                    for (int i = 1; i < list.Count; i++)
                                    {
                                        JArray array2 = (JArray)list[i];
                                        List<object> list2 = array2.ToObject<List<object>>();
                                        ProcessFilter(list2, filters, notFilters);
                                    }
                                }
                                else
                                    ProcessFilter(list, filters, notFilters);
                            }
                        }
                    }
                }
            }

            List<string> strings = new List<string>();

            if (notFilters.Count + filters.Count == 1)
            {
                if (filters.Count == 1)
                    strings = new List<string> { GetFilterString(filters[0]) };
                if (notFilters.Count == 1)
                    strings = new List<string> { $"not: {{ {GetFilterString(notFilters[0])} }}" };
            }
            else
            {
                if (filters.Count > 0 || notFilters.Count > 0)
                {
                    strings = new List<string>();
                    strings.Add(qualifier + ":");

                    foreach (Filter f in filters)
                        strings.Add("    - " + GetFilterString(f));

                    if (notFilters.Count == 1)
                        strings.Add("    - not: { " + GetFilterString(notFilters[0]) + " }");
                    else if (notFilters.Count > 1)
                    {
                        strings.Add("not:");
                        foreach (Filter f in notFilters)
                            strings.Add("    - " + GetFilterString(f));
                    }
                }
            }

            AdjustZoomLevel(layer, isParentCasingGroup);

            if (layer.Minzoom != null || layer.Maxzoom != null)
            {
                string zoomStr = $"$zoom: {{ {LayerUtils.GetMinMaxZoom(layer)} }}";

                if (strings.Count > 1)
                    strings.Insert(1, "    - " + zoomStr);
                else
                    strings.Add(zoomStr);
            }

            return strings;
        }       

        private static void ProcessFilter(IList<object> filter, List<Filter> filters, List<Filter> notFilters)
        {
            if (filter[0] is String)
            {
                string filterOperator = (string)filter[0];
                if (filter.Count == 3 && filter[1] is string)
                {
                    string prop = "";
                    string val = null;
                    GetPropertyAndValue(filter, out prop, out val);

                    if (filterOperator == "==")
                    {                        
                        AddFilter(filters, prop, val);
                    }
                    else if (filterOperator == "!=")
                    {
                        AddFilter(notFilters, prop, val);
                    }
                    else
                    {                        
                        if (prop == "area")
                        {
                            Converter.AppendDebug("Area - Not processed - ProcessFilter", filter[0]);
                        }
                        else if (prop == "class")
                        {
                            AddFilter(filters, prop, val);                            
                        }
                        else
                        {
                            var vals = GetThanValues(val, filterOperator);
                            if (vals != null && (filterOperator == "<" || filterOperator == "<="))                            
                                AddFilter(filters, prop, vals);                            
                            else if (vals != null && (filterOperator == ">" || filterOperator == ">="))                            
                                AddFilter(notFilters, prop, vals);                            
                            else   
                                Converter.AppendDebug("Not processed - ProcessFilter", filter[0]);
                        }
                    }
                }
                else if (filterOperator == "in" && filter.Count >= 4)
                {
                    AddInOutFilter(filter, filters);
                }
                else if (filterOperator == "!in" && filter.Count >= 4)
                {
                    AddInOutFilter(filter, notFilters);
                }                             
            }

        }

        private static List<string> GetThanValues(string val, string filterOperator)
        {
            string newValue = null;
            bool includeStartValue = filterOperator == "<=" || filterOperator == ">";
            int startVal = int.Parse(val);
            List<string> list = new List<string>(); 
            for (int i = 0; i<startVal; i++)            
                list.Add(i.ToString());            
            if (includeStartValue)
                list.Add(startVal.ToString());
            if (list.Count > 0)          
                return list;            

            return null;
        }

        private static void AddInOutFilter(IList<object> filter, List<Filter> filterList)
        {
            string prop = "";
            string val = null;
            GetPropertyAndValue(filter, out prop, out val);
            Filter newFilter = AddFilter(filterList, prop, val);
            if (newFilter != null)
            {
                for (int i = 3; i < filter.Count; i++)                
                    newFilter.Values.Add(filter[i].ToString());                
            }
        }

        private static string GetFilterString(Filter filter)
        {
            string value;
            if (filter.Values.Count == 1)            
                value = $"{filter.Values[0]}";            
            else            
                value = "[ " + string.Join(",", filter.Values) + " ]";            

            return $"{filter.Property}: {value}";
        }

        private static string[] IgnoredFilters = { "layer", "underground" }; // shield   maki

        private static Filter AddFilter(List<Filter> filterList, string prop, List<string> vals)
        {            
            Filter filter = AddFilter(filterList, prop, vals[0]);
            vals.RemoveAt(0);
            foreach (string val in vals)
                filter.Values.Add(val);
            return filter;
        }

        private static Filter AddFilter(List<Filter> filterList, string prop, string val)
        {
            if (IgnoredFilters.Contains(prop))
                return null;

            if (prop == "structure" && val == "none")
                return null;

            Filter filter = filterList.Find(s => s.Property == prop);
            if (filter == null)
            {
                filter = new Filter(prop, val);
                filterList.Add(filter);
            }
            else
                filter.Values.Add(val);

            return filter;
        }

        private static void GetPropertyAndValue(IList<object> filter, out string prop, out string val)
        {
            prop = (string)filter[1];
            val = filter[2].ToString();

            if (prop == "$type")
            {
                prop = "$geometry";
                val = val.ToLower();
                if (val == "linestring")
                    val = "line";
            }

            // should have added a comment here. 
            // this breaks OpenMapTiles but was probably needed for MapBox?
            /*if (prop == "class" && val == "trunk"
                || val == "primary" || val == "secondary" || val == "tertiary")
            {
                prop = "type";
            }*/
        }

        private static void AdjustZoomLevel(Layer layer, bool isParentCasingGroup)
        {
            string id = layer.Id;

            // don't adjust these
            if (id.Contains("city") && id.Contains("label"))
                return;

            if (id.Contains("country") && id.Contains("label"))           
                return;

            if (id.Contains("poi") && id.Contains("label"))
                return;

            if (id.Contains("road") && id.Contains("label"))
                return;

            if (id.StartsWith("building"))
            {                
                if (layer.Minzoom == 13 && layer.Maxzoom == 14)
                {
                    // OpenMapTiles has a 3d building layer to appear after 14 but we are ignoring it.                    
                    layer.Maxzoom = null;
                }
                return;
            }                           
                

            if (id.StartsWith(Motorway) && id.Contains("junction"))
                return;

            if (id.StartsWith("road") && (id.Contains("motorway") || id.Contains("service") || id.Contains("link") || id.Contains("track")))
                return;

            // move all down 3 zooms

            if (layer.Minzoom != null && layer.Minzoom <= 10)
            {
                layer.Minzoom = layer.Minzoom + 3;
                if (layer.Maxzoom != null)
                    layer.Maxzoom = layer.Maxzoom + 3;
            }

            if (layer.Id.StartsWith("poi_z") && layer.Minzoom < 18) // openmaptiles pois appear way to soon
            {
                layer.Minzoom = layer.Minzoom + 2;
            }
            else if (layer.Id == "poi_transit" && (layer.Minzoom == null || layer.Minzoom < 17)) // openmaptiles pois appear way to soon
            {
                layer.Minzoom = 17;
            }

            // if no min

            if (layer.Minzoom == null)
            {
                if (IsRoad(layer))
                    layer.Minzoom = id.Contains(Tertiary) ? 9 : 8;

                else if (id.StartsWith("place"))
                {
                    int min = 7;

                    if (id.Contains("city"))
                    {
                        if (id.Contains("sm"))
                            min = 11;
                        else if (id.Contains("md"))
                            min = 9;
                        else
                            min = 7;
                    }
                    else if (id.Contains("town"))
                    {
                        if (id.Contains("sm"))
                            min = 12;
                        else if (id.Contains("md"))
                            min = 10;
                        else
                            min = 8;
                    }

                    layer.Minzoom = min;
                }
                else if (id.StartsWith("road"))
                {
                    if (id.Contains("label"))
                    {
                        int min = 15;

                        if (id.Contains("large"))
                        {
                            if (id.Contains("large"))
                                min = 10;
                            else if (id.Contains("small"))
                                min = 14;
                        }

                        layer.Minzoom = min;
                    }
                    else if (id.Contains("shield"))
                    {
                        layer.Minzoom = 15;
                    }
                }

                else if (id.StartsWith("poi"))
                {
                    layer.Minzoom = 8;
                }

                else if (id == "national_park")
                    layer.Minzoom = 7;                
            }

            // test to find ones too far
            //if (layer.Minzoom > 15)
            //    layer.Minzoom = 15;

            if (isParentCasingGroup && layer.Minzoom != null)
                layer.Minzoom = layer.Minzoom + 1;
        }

        private static bool IsRoad(Layer layer)
        {
            return layer.Id.Contains(Primary) || layer.Id.Contains(Secondary) || layer.Id.Contains(Tertiary) || layer.Id.Contains(Trunk) || layer.Id.Contains(Motorway);
        }

        public class Filter
        {
            public string Property;
            public List<string> Values;

            public Filter(string prop, string val)
            {
                Property = prop;
                Values = new List<String> { val };
            }
        }
    }
}
