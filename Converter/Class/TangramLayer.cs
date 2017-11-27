using MbJsonToYaml.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static MbJsonToYaml.Constants;

namespace MbJsonToYaml
{
    public class TangramLayer
    {
        private StringBuilder _sb;
        private int _indent;

        private List<Layer> refLayers;
        
        public Layer LayerWithSource { get; internal set; }

        public Stroke Outline { get; set; }        

        public string Width { get; set; }
        public string Color { get; set; }
        public string Join { get; set; }
        public string Cap { get; set; }

        public Font Font { get; set; }

        public bool? Visible { get; set; }
        public string TextSource { get; set; }

        public string Sprite { get; set; }
        
        public TangramLayer(Layer sourceLayer, List<Layer> refLayers)
        {            
            this.LayerWithSource = sourceLayer;            
            this.refLayers = refLayers;

            if (this.refLayers != null)
            {
                foreach (Layer refLayer in this.refLayers)
                    refLayer.Type = LayerWithSource.Type;
            }

            Process();
        }

        private void Process()
        {
            ProcessSubLayer(LayerWithSource);
            if (this.refLayers != null)
            {
                foreach (Layer refLayer in refLayers)
                    ProcessSubLayer(refLayer);
            }
        }

        private void ProcessSubLayer(Layer layer)
        {
            bool isCasing = LayerUtils.IsCasing(layer.Id);

            Paint paint = layer.Paint;
            Layout layout = layer.Layout;

            if (paint != null)
            {
                if (paint.FillColor != null)
                    ProcessColor(paint.FillColor, paint.FillOpacity, isCasing);

                if (paint.FillOutlineColor != null)
                    ProcessColor(paint.FillOutlineColor, paint.FillOpacity, true);

                if (paint.LineColor != null)
                    ProcessColor(paint.LineColor, paint.LineOpacity, isCasing);
                if (paint.LineWidth != null)
                    ProcessWidth(paint.LineWidth, isCasing);

                if (paint.TextColor != null || paint.TextHaloColor != null || paint.TextHaloWidth != null)
                {
                    if (Font == null)
                        Font = new Font();

                    if (paint.TextColor != null)                    
                        Font.Fill = Helper.ProcessColor(paint.TextColor, paint.TextOpacity);                    

                    if (paint.TextHaloColor != null || paint.TextHaloWidth != null)
                    {
                        if (Font.Stroke == null)
                            Font.Stroke = new Stroke();

                        if (paint.TextHaloColor != null)
                            Font.Stroke.Color = Helper.ProcessColor(paint.TextHaloColor, null);

                        if (paint.TextHaloWidth != null)
                            Font.Stroke.Width = Math.Round((double)paint.TextHaloWidth).ToString();
                    }
                }
			}

            if (layout != null)
            {
                if (layout.Visibility != null)
                    Visible = layout.Visibility == "visible";
                if (layout.LineCap != null)
                    Cap = Helper.ProcessStoppable(layout.LineCap);
                if (layout.LineJoin != null)
                    Join = layout.LineJoin;

                if (layout.TextField != null)
                    TextSource = GetTextField(layout.TextField, layer);

                if ((layout.TextFont != null && layout.TextFont is List<string>) || layout.TextSize != null || layout.TextTransform != null)
                {
                    if (Font == null)
                        Font = new Font();

                    if (layout.TextFont != null)                    
                        Font.Family = Helper.ProcessFont(layout.TextFont);                    
                    if (layout.TextSize != null)
                        Font.Size = Helper.ProcessTextSize(layout.TextSize);
                    if (layout.TextTransform != null)
                        Font.Transform = layout.TextTransform;                    
                }

                if (layer.Layout.IconImage != null)
                    Sprite = GetSprite(layer.Layout.IconImage);                
            }
        }

        private string GetTextField(object source, Layer layer)
        {
            string textSource = Helper.ProcessStoppable(source);

            if (layer.Id == "road_label" && layer.SourceLayer == "transportation_name")
                return "global.ref_source";

            if (textSource == "{name_en}" || textSource == "{name}")
                return "global.name_source";

            if (textSource.Contains("{"))
            {
                var split = textSource.Split(new[] { '{', '}' });
                if (split.Length == 1)
                    return $"function() {{ return feature.{split[0]}; }}";
                if (split.Length == 3)
                    return $"function() {{ return \"{split[0]}\" + feature.{split[1]} + \"{split[2]}\"; }}";
                if (split.Length == 5)
                    return $"function() {{ return \"{split[0]}\" + feature.{split[1]} + \"{split[2]}\" + feature.{split[3]} + \"{split[4]}\"; }}";

                return "?";
            }

            Converter.AppendDebug("Default TextField ignored", textSource);

            return LayerUtils.IsRefTextLayer(layer) ? "global.ref_source" : "global.default_text_source";
        }

        private void ProcessColor(object colorObj, StoppedDouble opacityObj, bool isCasing)
        {
            if (isCasing && Outline == null)
                Outline = new Stroke();

            string color = Helper.ProcessColor(colorObj, opacityObj);
            if (isCasing)
                Outline.Color = color;
            else
                Color = color;
        }

        private void ProcessWidth(StoppedDouble lineWidth, bool isCasing)
        {
            if (isCasing && Outline == null)
                Outline = new Stroke();

            string width = Helper.ProcessWidth(lineWidth, isCasing);
            if (isCasing)
            {
                if (Outline.Width == null)
                    Outline.Width = width;
            }
            else if (Width == null)
                Width = width;
        }

        public override string ToString()
        {
            return $"{LayerWithSource.ToString()}  :  {(refLayers != null ? refLayers.Count.ToString() : "null")}";
        }

        public string CreateLines(bool isCasing)
        {
            _sb = new StringBuilder();

            string layerType = LayerUtils.GetLayerType(LayerWithSource);

            AppendLine($"{LayerUtils.NormaliseName(LayerWithSource.Id)}:");
            _indent++;

            CreateFilterLines(isCasing);

            Layer casingLayer = null;

            if (refLayers != null && refLayers.Count > 0)
            {                
                if (LayerUtils.IsCasing(LayerWithSource.Id))
                    casingLayer = LayerWithSource;
                else if (refLayers != null)
                {
                    foreach (Layer refLayer in refLayers)
                    {
                        if (LayerUtils.IsCasing(refLayer.Id))
                            casingLayer = refLayer;
                    }

                    if (casingLayer != null)
                    {
                        refLayers.Remove(casingLayer);
                        refLayers.Add(LayerWithSource);
                    }
                }
            }

            CreateDrawLines(layerType, isCasing);

            _indent--;

            return _sb.ToString();
        }       

        private void CreateDrawLines(string type, bool isCasing)
        {         
            AppendLine("draw:");
            _indent++;

            

			if (Color != null)
			{
			    int colorStringLength = 0;
                bool forceBlended = LayerUtils.IsBlendedLayer(LayerWithSource.Id);
			    if (!forceBlended)
			    {
			        if (!Color.Contains(","))
			            colorStringLength = Color.Length;
			        else
			        {
			            // todo split this kind of thing to get the length of the color strings inside.  "[[5,'#F2934A'],[6,'#fc8']]"
			        }
			    }

			    if (forceBlended || colorStringLength > 7) // i.e. longer than #FFFFFF so it has an A component.
			    {
			        if (type == "polygons")
			            type = "polys-blended";
			        else if (type == "lines")
			            type = "lines-blended";
			    }
			}
            
            AppendLine(type + ":");
            _indent++;

            if (type != TangramText)
                AppendLine("order: " + LayerUtils.GetLayerOrder(LayerWithSource, isCasing || Outline != null));
            else            
                AppendLine("priority: " + LayerUtils.GetTextLayerPriority(LayerWithSource));            

            //if (LayerWithSource.Id.StartsWith("road"))
               // AppendLine("collide: false");

            if (Outline != null)
            {
                AppendLine("outline:");
                _indent++;
                if (Outline.Width != null)
                    AppendLine("width: " + Outline.Width);
                if (Outline.Color != null)
                    AppendLine("color: " + Outline.Color);
                _indent--;
            }

            if (Color != null)            
                AppendLine("color: " + Color);
                //AppendLine("color: " + (!isCasing ? Color : "'#FF0000'"));
            if (Width != null)
                AppendLine("width: " + Width);

            if (!isCasing)
            {
                if (Join != null)
                    AppendLine("join: " + Join);
                if (Cap != null && !LayerWithSource.Id.StartsWith("bridge"))
                    AppendLine("cap: " + Cap);
            }            

            if (Font != null && type != TangramIcons)            
                AppendFont();            

            if (Sprite != null && !Sprite.Contains("motorway-exit"))
            {
                AppendLine("sprite: " + Sprite);

                //AppendLine("blend_order: 1");
                //AppendLine("priority: 21");
                //AppendLine("order: 100");
                //AppendLine("size: 24px");

                bool preciseIcon = Sprite.Contains("dot");

                AppendLine("collide: false");
                if (!preciseIcon)
                    AppendLine("anchor: top");

                _indent--;

                if (type == TangramIcons && Sprite != null)
                {
                    DrawIconText(preciseIcon);
                }
            }
            else if (type == TangramIcons && Font != null)
            {
                _indent--;
                DrawIconText(false);
                _indent++;
            }
                

            _indent--;
        }

        private void DrawIconText(bool preciseIcon)
        {
            AppendLine("text:");

            _indent++;

            AppendLine("collide: true");
            AppendLine("anchor: bottom");
            if (preciseIcon)
                AppendLine("offset: [0, 10px]");

            AppendLine("priority: " + LayerUtils.GetTextLayerPriority(LayerWithSource));

            //AppendLine("blend_order: 1");
            //AppendLine("order: 99");


            if (Font != null)
                AppendFont();
            else
            {
                AppendLine($"visible: *debug_visible");
                AppendLine("font:");
                _indent++;
                AppendLine("fill: '#3A4CA6'");
                AppendLine("size: [[16, 11],[20,14]]");
            }

            _indent--;
        }

        private void AppendFont()
        {
            if (TextSource != null)
                AppendLine("text_source: " + TextSource);

            AppendLine("font:");
            _indent++;
            //if (Font.Family != null)
            //    AppendLine("family: " + Font.Family); // todo these fonts don't exist
            if (Font.Size != null)
                AppendLine("size: " + Font.Size);
            if (Font.Fill != null)
                AppendLine("fill: " + Font.Fill);
            if (Font.Stroke != null)
            {
                AppendLine("stroke:");
                _indent++;
                if (Font.Stroke.Color != null)
                    AppendLine("color: " + Font.Stroke.Color);
                if (Font.Stroke.Width != null)
                    AppendLine("width: " + Font.Stroke.Width);
                _indent--;
            }

            if (Font.Transform != null)
                AppendLine("transform: " + Font.Transform);            

            _indent--;
        }

        private string GetSprite(object iconImage)
        {            
            string icon = Helper.ProcessStoppable(iconImage);

            Converter.AppendDebug("Default icon", iconImage);

            if (icon.Contains("{"))
            {
                var split = icon.Split(new []{'{','}'});
                if (split.Length == 1)
                    return $"function() {{ return feature.{split[0]}; }}";
                if (split.Length == 3)
                    return $"function() {{ return \"{split[0]}\" + feature.{split[1]} + \"{split[2]}\"; }}";
                if (split.Length == 5)
                    return $"function() {{ return \"{split[0]}\" + feature.{split[1]} + \"{split[2]}\" + feature.{split[3]} + \"{split[4]}\"; }}";

                return "?";
            }
            
            return icon;
        }
    
        private void CreateFilterLines(bool isCasing)
        {
            List<string> filterStrings = new List<string>();

            if ((LayerWithSource.Filter != null && LayerWithSource.Filter.Count > 0) || LayerWithSource.Minzoom != null)
                filterStrings.AddRange(Filters.GetLines(LayerWithSource, isCasing));

            if (filterStrings != null && filterStrings.Count > 0)
            {
                AppendLine("filter:");
                _indent++;

                if (filterStrings.Count > 1 && filterStrings[0] != "all:")
                {
                    AppendLine("all:");
                    foreach (string str in filterStrings)
                        if (!str.StartsWith("#"))
                            AppendLine("    - " + str);
                        else
                            AppendLine("      " + str);
                }
                else
                {
                    foreach (string str in filterStrings)
                        AppendLine(str);
                }

                _indent--;
            }
        }

        private void AppendLine(string txt)
        {
            for (int i = 0; i < _indent; i++)
                _sb.Append("    ");
            _sb.AppendLine(txt);
        }
    }    
}
