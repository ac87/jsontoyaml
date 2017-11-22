using System;
using System.Collections.Generic;
using MbJsonToYaml.Utils;
using Newtonsoft.Json;

namespace MbJsonToYaml
{
    public class StoppedDouble
    {
        [JsonProperty("base")]
        public double Base { get; set; }

        [JsonProperty("stops")]
        public IList<IList<object>> Stops { get; set; }

        public double SingleVal { get; set; } = double.MinValue;
    }

    public class Paint
    {

        [JsonProperty("background-color")]
        public object BackgroundColor { get; set; }

        [JsonProperty("fill-color")]
        public object FillColor { get; set; }

        [JsonConverter(typeof(StoppedDoubleConverter))]
        [JsonProperty("fill-opacity")]
        public StoppedDouble FillOpacity { get; set; }

        [JsonProperty("line-color")]
        public object LineColor { get; set; }

        [JsonConverter(typeof(StoppedDoubleConverter))]
        [JsonProperty("line-width")]
        public StoppedDouble LineWidth { get; set; }

        [JsonProperty("fill-translate")]
        public object FillTranslate { get; set; }

        [JsonProperty("fill-pattern")]
        public string FillPattern { get; set; }

        [JsonProperty("fill-outline-color")]
        public object FillOutlineColor { get; set; }

        [JsonProperty("line-dasharray")]
        public object LineDasharray { get; set; }

        [JsonConverter(typeof(StoppedDoubleConverter))]
        [JsonProperty("line-opacity")]
        public StoppedDouble LineOpacity { get; set; }

        [JsonProperty("text-color")]
        public string TextColor { get; set; }

        [JsonProperty("text-halo-width")]
        public double? TextHaloWidth { get; set; }

        [JsonProperty("text-halo-color")]
        public object TextHaloColor { get; set; }

        [JsonProperty("text-halo-blur")]
        public double? TextHaloBlur { get; set; }

        [JsonProperty("fill-antialias")]
        public bool? FillAntialias { get; set; }

        [JsonProperty("fill-translate-anchor")]
        public string FillTranslateAnchor { get; set; }

        [JsonProperty("line-gap-width")]
        public object LineGapWidth { get; set; }

        [JsonProperty("line-blur")]
        public object LineBlur { get; set; }

        [JsonProperty("line-translate")]
        public IList<int> LineTranslate { get; set; }

        [JsonProperty("icon-halo-color")]
        public string IconHaloColor { get; set; }

        [JsonProperty("icon-halo-width")]
        public int? IconHaloWidth { get; set; }

        [JsonConverter(typeof(StoppedDoubleConverter))]
        [JsonProperty("text-opacity")]
        public StoppedDouble TextOpacity { get; set; }

        [JsonProperty("icon-color")]
        public string IconColor { get; set; }

        [JsonProperty("text-translate")]
        public IList<int> TextTranslate { get; set; }

        [JsonConverter(typeof(StoppedDoubleConverter))]
        [JsonProperty("icon-opacity")]
        public StoppedDouble IconOpacity { get; set; }
    }

    public class Layout
    {
        [JsonProperty("line-cap")]
        public object LineCap { get; set; }

        [JsonProperty("line-join")]
        public string LineJoin { get; set; }

        [JsonProperty("visibility")]
        public string Visibility { get; set; }

        [JsonProperty("text-font")]
        public object TextFont { get; set; }

        [JsonProperty("text-field")]
        public object TextField { get; set; }

        [JsonProperty("text-max-width")]
        public object TextMaxWidth { get; set; }

        [JsonProperty("text-size")]
        public object TextSize { get; set; }

        [JsonConverter(typeof(StoppedDoubleConverter))]
        [JsonProperty("text-padding")]
        public StoppedDouble TextPadding { get; set; }

        [JsonProperty("text-offset")]
        public object TextOffset { get; set; }

        [JsonProperty("text-anchor")]
        public object TextAnchor { get; set; }

        [JsonProperty("symbol-placement")]
        public object SymbolPlacement { get; set; }

        [JsonProperty("text-rotation-alignment")]
        public string TextRotationAlignment { get; set; }

        [JsonProperty("icon-rotation-alignment")]
        public string IconRotationAlignment { get; set; }

        [JsonProperty("text-transform")]
        public string TextTransform { get; set; }

        [JsonProperty("text-letter-spacing")]
        public double? TextLetterSpacing { get; set; }

        [JsonProperty("text-line-height")]
        public double? TextLineHeight { get; set; }

        [JsonProperty("icon-image")]
        public object IconImage { get; set; }

        [JsonProperty("symbol-spacing")]
        public object SymbolSpacing { get; set; }

        [JsonProperty("icon-padding")]
        public int? IconPadding { get; set; }

        [JsonProperty("icon-size")]
        [JsonConverter(typeof(StoppedDoubleConverter))]
        public StoppedDouble IconSize { get; set; }

        [JsonProperty("icon-allow-overlap")]
        public bool? IconAllowOverlap { get; set; }

        [JsonProperty("icon-ignore-placement")]
        public bool? IconIgnorePlacement { get; set; }        

        [JsonProperty("text-max-angle")]
        public int? TextMaxAngle { get; set; }
    }

    public class Layer
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("paint")]
        public Paint Paint { get; set; }

        [JsonProperty("interactive")]
        public bool Interactive { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("source-layer")]
        public string SourceLayer { get; set; }

        [JsonProperty("filter")]
        public IList<object> Filter { get; set; }

        //[JsonProperty("metadata")]
        //public Metadata { get; set; }

        [JsonProperty("layout")]
        public Layout Layout { get; set; }

        [JsonProperty("ref")]
        public string Ref { get; set; }

        [JsonProperty("maxzoom")]
        public int? Maxzoom { get; set; }

        [JsonProperty("minzoom")]
        public int? Minzoom { get; set; }

        public override string ToString()
        {
            return Id + " " + Type;
        }
    }

    public class MapboxJson
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        //[JsonProperty("metadata")]
        //public Metadata Metadata { get; set; }

        [JsonProperty("sources")]
        public object Sources { get; set; }

        //[JsonProperty("sprite")]
        //public string Sprite { get; set; }

        [JsonProperty("glyphs")]
        public string Glyphs { get; set; }

        [JsonProperty("layers")]
        public IList<Layer> Layers { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("modified")]
        public DateTime Modified { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("draft")]
        public bool Draft { get; set; }
    }
}
