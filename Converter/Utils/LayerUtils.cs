using System;
using System.Collections.Generic;
using System.Linq;

using static MbJsonToYaml.Constants;

namespace MbJsonToYaml.Utils
{
    public class LayerUtils
    {
        private const int RoadOrderDefault = 40;

        private const int ServiceLink_OrderOffset = 1;        
        private const int TertiaryLink_OrderOffset = ServiceLink_OrderOffset + 1;
        private const int SecondaryLink_OrderOffset = TertiaryLink_OrderOffset + 1;
        private const int PrimaryLink_OrderOffset = SecondaryLink_OrderOffset + 1;
        private const int MotorwayLink_OrderOffset = PrimaryLink_OrderOffset + 1;
        private const int TrunkLink_OrderOffset = MotorwayLink_OrderOffset + 1;

        private const int Service_OrderOffset = TrunkLink_OrderOffset + 1;        
        private const int Tertiary_OrderOffset = Service_OrderOffset + 1;               
        private const int Secondary_OrderOffset = Tertiary_OrderOffset + 1;
        private const int Primary_OrderOffset = Secondary_OrderOffset + 1;                                    
        private const int Motorway_OrderOffset = Primary_OrderOffset + 1;
        private const int Trunk_OrderOffset = Motorway_OrderOffset + 1;

        public static int GetLayerOrder(Layer layer, bool isCasing = false)
        {
            var id = layer.Id;

            bool isBridge = id.Contains(Bridge);

            bool isCase = (IsCasing(id) || isCasing);
                       
            if (id.Contains(Motorway))
                return GetRoadOrder(layer, id.Contains(Link) ? MotorwayLink_OrderOffset : Motorway_OrderOffset, isBridge, isCase);
                        
            if (id.Contains(Trunk))
                return GetRoadOrder(layer, id.Contains(Link) ? TrunkLink_OrderOffset : Trunk_OrderOffset, isBridge, isCase);      

            if (id.Contains(Primary))
                return GetRoadOrder(layer, id.Contains(Link) ? PrimaryLink_OrderOffset : Primary_OrderOffset, isBridge, isCase);

            if (id.Contains(Secondary))
                return GetRoadOrder(layer, id.Contains(Link) ? SecondaryLink_OrderOffset : Secondary_OrderOffset, isBridge, isCase);

            if (id.Contains(Tertiary))
                return GetRoadOrder(layer, id.Contains(Link) ? TertiaryLink_OrderOffset : Tertiary_OrderOffset, isBridge, isCase);

            if (id.Contains("service"))
                return GetRoadOrder(layer, id.Contains(Link) ? ServiceLink_OrderOffset : Service_OrderOffset, isBridge, isCase);
            
            if (id.Contains(Ferry))
                return 30;
            
            if (layer.Type == MapboxLines && id.Contains(Bridge))
                return RoadOrderDefault + 20;

            if (id.StartsWith(Road) || layer.SourceLayer == Road)
            {
                if (id.Contains("major"))
                    return RoadOrderDefault + 5;
                return RoadOrderDefault;
            }                        

            if (layer.SourceLayer == Water)
                return 5;

            if (id.StartsWith(Water)) // waterways and rivers should be below water
                return 4;

            if (id.EndsWith("park")) // parks have to be below water.
                return 3;

            if (id == "school")
                return 1;

            if (layer.SourceLayer == "landuse" || id.StartsWith("landcover_"))
                return 2;

            if (id.Contains("building"))
                return 30;

            if (layer.Type == MapboxLines)
                return 25;

            if (layer.Type == MapboxPolygons)
                return 5;

            return 10;
        }

        public static string NormaliseName(string id)
        {
            return id.Replace(CasingSuffix, "").Replace(CaseSuffix, "").Replace(" ", "_").ToLower();
        }

        public static List<TangramLayerGroup> ProcessLayers(Dictionary<string, string> replacedSources, IList<Layer> layers)
        {
            List<Layer> layerWithNoSourceList = layers.Where(item => item.Type == null && !IsLayerIgnored(item)).ToList();
            List<Layer> layerWithSourceList = layers.Where(item => item.Type != null && !IsLayerIgnored(item)).ToList();

            List<Layer> reSourcedList = new List<Layer>();
            List<Layer> usedLayers = new List<Layer>();

            List<TangramLayerGroup> tangramLayerGroups = new List<TangramLayerGroup>();
                        
            ProcessLayers(replacedSources, layerWithNoSourceList, layerWithSourceList, reSourcedList, tangramLayerGroups, usedLayers);

            if (reSourcedList.Count > 0)
            {
                layerWithSourceList.Clear();
                layerWithSourceList.AddRange(reSourcedList);
                reSourcedList.Clear();
                ProcessLayers(replacedSources, layerWithNoSourceList, layerWithSourceList, reSourcedList, tangramLayerGroups, usedLayers);
            }

            foreach (Layer layer in usedLayers)
            {
                TangramLayer layerFound = null;

                foreach (var layerGroup in tangramLayerGroups)
                {
                    foreach (var layerGroupLayer in layerGroup)
                    {
                        if (layerGroupLayer.LayerWithSource == layer)
                            layerFound = layerGroupLayer;
                    }

                    if (layerFound != null)
                    {
                        layerGroup.Remove(layerFound);
                        break;
                    }
                }
            }

            foreach (var layerMissed in layerWithNoSourceList)
                Converter.AppendDebug("missed layer", layerMissed.Id);

            return tangramLayerGroups;
        }

        private static void ProcessLayers(Dictionary<string, string> replacedSources, List<Layer> layerWithNoSourceList, List<Layer> layerWithSourceList, List<Layer> reSourcedList, List<TangramLayerGroup> tangramLayerGroups, List<Layer> usedLayers)
        {
            foreach (var layerWithSource in layerWithSourceList)
            {
                bool isCasing = IsCasing(layerWithSource.Id);                

                List<Layer> refLayers = null;
                if (layerWithSource.Ref == null)
                {
                    refLayers = layerWithNoSourceList.Where(item => item.Ref == layerWithSource.Id).ToList<Layer>();
                    if (refLayers != null)
                    {
                        foreach (Layer refLayer in refLayers)
                        {
                            layerWithNoSourceList.Remove(refLayer);
                            refLayer.Filter = layerWithSource.Filter;
                            refLayer.Source = layerWithSource.Source;
                            refLayer.SourceLayer = layerWithSource.SourceLayer;

                            if (refLayer.Layout != null)
                            {
                                refLayer.Layout.LineCap = layerWithSource.Layout.LineCap;
                                refLayer.Layout.LineJoin = layerWithSource.Layout.LineJoin;
                            }
                            else
                                refLayer.Layout = layerWithSource.Layout;

                            reSourcedList.Add(refLayer);
                        }
                    }
                    //refLayers.Clear();
                }

                if (layerWithSource.Id.StartsWith("bridge") && (layerWithSource.Id.EndsWith(CaseSuffix) || layerWithSource.Id.EndsWith(CasingSuffix)))
                {
                    var realLayers = layerWithSourceList.Where(item => item.Id == NormaliseName(layerWithSource.Id)).ToList();
                    if (realLayers.Count > 0)
                    {
                        refLayers.Add(realLayers[0]);
                        usedLayers.Add(realLayers[0]);
                    }
                }

                // to join OpenMapTiles casing and regular road layers together as we need to specify width for lines with outlines
                if (layerWithSource.Id.EndsWith("_casing") && refLayers.Count == 0)
                {
                    var lineWidthLayers = layerWithSourceList.Where(item => item.Id == layerWithSource.Id.Replace("_casing", "")).ToList();
                    if (lineWidthLayers.Count == 1)
                        refLayers.Add(lineWidthLayers[0]);
                }

                TangramLayer tangramLayer = new TangramLayer(layerWithSource, refLayers);

                TangramLayerGroup tangramLayerGroup = tangramLayerGroups.FirstOrDefault(item => item.SourceLayer != null && item.SourceLayer.ToString() == layerWithSource.SourceLayer && item.IsCasing == isCasing);

                if (tangramLayerGroup == null)
                {
                    tangramLayerGroup = new TangramLayerGroup(replacedSources, layerWithSource.SourceLayer, isCasing);
                    tangramLayerGroups.Add(tangramLayerGroup);
                }

                tangramLayerGroup.Add(tangramLayer);
            }
        }

        private static bool IsLayerIgnored(Layer layer)
        {
            string id = layer.Id;
            if (IgnoredLayers.Contains(id) || (id.Contains("_") && IgnoredLayers.Contains(id.Substring(0, id.IndexOf("_") + 1))))
                return true;

            if (id.Contains("oneway")) // ignored for now
                return true;

            if (id.Contains("shield")) // just dont work
                return true;

            if (id.Contains("admin") && id.Contains("boundaries") && id.Contains("bg"))
                return true;

            if (id.Contains("water_pattern") || id.Contains("water_offset")) // just dont work
                return true;

            return false;
        }

        private static int GetRoadOrder(Layer layer, int offset, bool isBridge, bool isCasing)
        {
            int order = RoadOrderDefault + offset;

            if (isBridge)
                order = order + 30;

            if (isCasing)
                order = order - 20;
            
            Console.WriteLine(layer.Id + " - Order: " + order);

            return order;
        }
        
        public static string GetLayerType(Layer layer, bool isCasingLayer = false)
        {
            switch (layer.Type)
            {
                case MapboxPolygons:
                    return TangramPolygons;
                case MapboxLines:
                    return layer.Paint.LineDasharray == null ? TangramLines : layer.Id.Contains("rail") ? RailLinesDash: CustomLinesDash;
                case MapboxSymbols:
                    if (layer.Layout.IconImage != null)
                        return TangramIcons;
                    return TangramText;
                default:
                    return isCasingLayer ? TangramLines :"???s";
            }
        }

        public static bool IsCasing(string id)
        {
            return id.EndsWith(CaseSuffix) || id.EndsWith(CasingSuffix);
        }

        public static string GetMinMaxZoom(Layer layer)
        {
            string str = "";
            if (layer.Minzoom != null)
                str = str + $"min: {layer.Minzoom}";            
            if (layer.Maxzoom != null)
            {
                string maxStr = $"max: {layer.Maxzoom}";
                if (str == "")
                    str = maxStr;
                else
                    str = str + ", " + maxStr;
            }
            return str;
        }

        public static bool IsRefTextLayer(Layer layer)
        {
            if (layer.Id.Contains(Motorway) || layer.Id.Contains(Shield))
                return true;

            return false;
        }

        public static int GetTextLayerPriority(Layer layerWithSource)
        {
            if (layerWithSource.Id.Contains("airport"))
                return 1;

            if (layerWithSource.Id.Contains("road"))
                return 1;

            if (layerWithSource.Id.Contains("scalerank1"))
                return 2;

            if (layerWithSource.Id.Contains("scalerank2"))
                return 3;

            if (layerWithSource.Id.Contains("scalerank3"))
                return 4;

            if (layerWithSource.Id.Contains("scalerank4"))
                return 5;

            return 10;
        }

        public static bool IsBlendedLayer(string id)
        {
            //if (id.StartsWith("landcover_") || id.StartsWith("landuse_overlay") || id == "park")
            //    return true;

            return false;
        }
    }
}
