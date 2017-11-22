using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static MbJsonToYaml.Constants;

namespace MbJsonToYaml.Utils
{
    /// <summary>
    /// Convert styles from 
    /// https://github.com/mapbox/mapbox-gl-styles/tree/master/styles
    /// in Mapbox Format  https://www.mapbox.com/mapbox-gl-style-spec/
    /// to Mapzen Tangram Format https://mapzen.com/documentation/tangram/
    /// </summary>    
    public class Converter
    {
        public static bool Debug = true;        

        private StringBuilder _debugSb;

        private StringBuilder _sb;

        private int _indent;
        private bool _includeSprites = true;

        private static Converter _instance;
        public string ForcedUrl { get; set; }

        public bool IncludeSprites
        {
            get { return _includeSprites; }
            set { _includeSprites = value; }
        }

        public static Converter GetInstance()
        {
            if (_instance == null)
                _instance = new Converter();
            return _instance;
        }

        public string Convert(string style, string jsonIn, out string debug)
        {
            _sb = new StringBuilder();
            _debugSb = new StringBuilder();

            _indent = 0;

            MapboxJson styleJson = JsonConvert.DeserializeObject<MapboxJson>(jsonIn);

            AppendLine($"#'{styleJson.Name}' Version:{styleJson.Version} (Converted: {DateTime.Now.ToShortDateString()} { DateTime.Now.ToShortTimeString() })" );

            CreateGlobals(style);
            CreateScene(styleJson);            
            CreateSources(styleJson);
            CreateLayers(styleJson);

            debug = _debugSb.ToString();

            return _sb.ToString();
        }

        private void CreateLayers(MapboxJson styleJson)
        {
            var layers = styleJson.Layers;

            if (layers != null && layers.Count > 0)
            {
                AppendLine("layers:");
                _indent = 1;

                List<TangramLayerGroup> tangramLayers = LayerUtils.ProcessLayers(_replacedSources, layers);

                foreach (TangramLayerGroup group in tangramLayers)
                {
                    var lines = group.CreateLines();
                    var split = lines.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                    foreach (string str in split)
                        if (str.Trim().Length > 0)
                            AppendLine(str);
                }

                _indent = 0;
            }
        }

        private Dictionary<string, string> _replacedSources = new Dictionary<string, string>();

        private void CreateSources(MapboxJson styleJson)
        {
            _replacedSources.Clear();

            JObject sources = styleJson.Sources as JObject;
            if (sources != null)
            {
                AppendLine("sources:");
                _indent++;

                int i = 0;

                JProperty source1 = (JProperty) sources.First;

                foreach (var inSource in sources)
                {
                    i++;

                    JProperty source = (JProperty) inSource.Value.Parent;

                    if (!source.Name.StartsWith("mapbox://"))
                        AppendLine(source.Name + ":");
                    else
                    {
                        string newName = "source-" + i;
                        AppendLine(newName + ":");
                        _replacedSources.Add(source.Name, newName);
                    }

                    _indent++;

                    foreach (JProperty valEntry in source.Value)
                    {
                        string prop = valEntry.Name;
                        string val = valEntry.Value.ToString();

                        if (prop == "type")
                        {
                            string type = val == "vector" ? "MVT" : "?";
                            AppendLine($"type: '{type}'");
                        }
                        else if (prop == "url")
                        {
                            if (val.Contains("mapbox.mapbox-streets"))
                            {
                                if (ForcedUrl != null)
                                    val = ForcedUrl;
                                else
                                    val = MapboxUrl;
                            }
                            else if (val.Contains("mapbox://") && ForcedUrl != null)
                            {
                                val = ForcedUrl;
                            }                            

                            AppendLine($"url: '{val}'");
                        }
                    }

                    _indent--;
                }                
            }
            _indent = 0;
        }

        private void CreateScene(MapboxJson styleJson)
        {
            var layers = styleJson.Layers;
            if (layers != null && styleJson.Layers.Count > 0)
            {
                var backgroundLayer = layers.FirstOrDefault(item => item.Id == Background);
                if (backgroundLayer != null)
                {
                    AppendLine("scene:");
                    _indent = 1;
                    AppendLine($"{Background}:");
                    _indent = 2;

                    // background colors don't do stops
                    string color = backgroundLayer.Paint.BackgroundColor as string;
                    if (backgroundLayer.Paint.BackgroundColor is JObject)                    
                        color = ((JObject)backgroundLayer.Paint.BackgroundColor).Last.Last.Last.Last.ToString();                    
                    AppendLine($"color: {Helper.ProcessColor(color, null)}");
                }
            }

            _indent = 0;
        }

        private void CreateGlobals(string style)
        {
            AppendLine("labels-global:");
            _indent++;
            AppendLine("    - &text_visible         true");
            AppendLine("    - &debug_visible        true");
            _indent--;

            AppendLine("global:");
            _indent++;

            //AppendLine("default_order: function() { return feature.sort_key || 0; }");

            // AppendLine("language: en");

            AppendLine("default_text_source: |");
            _indent++;
            AppendLine("function() {");
            _indent++;
            //AppendLine("return (global.language && feature['name_' + global.language]) || feature.name;");
            AppendLine("if (feature.ref && feature.name_en ) { return feature.ref +\" \" + feature.name_en; } else if (feature.name_en) { return feature.name_en; } else { return feature.ref; }");
            _indent--;
            AppendLine("}");
            _indent--;

            AppendLine("name_source: |");
            _indent++;
            AppendLine("function() {");
            _indent++;
            //AppendLine("return (global.language && feature['name_' + global.language]) || feature.name;");
            AppendLine("{ return feature.name_en; }");
            _indent--;
            AppendLine("}");
            _indent--;

            AppendLine("ref_source: |");
            _indent++;
            AppendLine("function() {");
            _indent++;
            //AppendLine("return (global.language && feature['name_' + global.language]) || feature.name;");
            AppendLine("if (feature.ref) { return feature.ref; } else if (feature.name) { return feature.name; } else { return \"No Ref\"; }");
            _indent--;
            AppendLine("}");
            _indent--;

            _indent = 0;

            AppendLine("styles:");
            _indent++;
            AppendLine(RailLinesDash+":");
            _indent++;
            AppendLine("base: lines");
            AppendLine("texcoords: true");
            AppendLine("shaders:");
            _indent++;
            AppendLine("defines:");
            _indent++;
            AppendLine("DASH_SIZE: .9");
            AppendLine("DASH_SCALE: .6");
            _indent--;
            AppendLine("blocks:");
            _indent++;
            AppendLine("filter: |");
            _indent++;
            AppendLine("if (step(DASH_SIZE, fract(v_texcoord.y * DASH_SCALE)) == 0.)");
            AppendLine("{");
            AppendLine("discard;");
            AppendLine("}");
            _indent--;
            AppendLine(CustomLinesDash+":");
            _indent++;
            AppendLine("base: lines");
            AppendLine("dash: [2,2]");

            _indent = 0;
            _indent++;
            AppendLine("icons:");
            _indent++;
            AppendLine("base: points");
            AppendLine("texture: pois");
            AppendLine("interactive: true");
            //AppendLine("blend_order: 1");
            
            _indent = 0;
            _indent++;
            AppendLine("lines-blended:");
            _indent++;
            AppendLine("base: lines");
            AppendLine("blend: inlay");

            _indent = 0;
            _indent++;
            AppendLine("polys-blended:");
            _indent++;
            AppendLine("base: polygons");
            AppendLine("blend: inlay");

            _indent = 0;

            AppendLine("cameras:");
            _indent++;
            AppendLine("perspective:");
            _indent++;
            AppendLine("type: perspective");            
            AppendLine("vanishing_point: [0, -500]");
            _indent = 0;
            /*AppendLine("lights:");
            _indent++;
            AppendLine("directional1:");
            _indent++;
            AppendLine("type: directional");
            AppendLine("direction: [.1, .5, -1]");
            AppendLine("diffuse: .7");
            AppendLine("ambient: .5");
            _indent = 0;*/

            if (IncludeSprites)
            {
                string variant = ""; // @2x

                AppendLine("textures:");
                _indent++;
                AppendLine("pois:");
                _indent++;
                AppendLine($"url: images/{style}{variant}.png");
                AppendLine("filtering: mipmap");
                AppendLine("sprites:");
                _indent++;

                var sprites = SpriteConverter.GetSprites($"{AppDomain.CurrentDomain.BaseDirectory}\\Styles\\{style}\\sprite\\{style}{variant}.json");
                foreach (var sprite in sprites)                
                    AppendLine($"{sprite.Name}: [{sprite.X},{sprite.Y},{sprite.Width},{sprite.Height}]");

                _indent = 0;
            }
        }        
              
        private void AppendLine(string txt)
        {
            for (int i = 0; i < _indent; i++)
                _sb.Append("    ");
            _sb.AppendLine(txt);
        }

        public static void AppendDebug(string line, object value)
        {
            _instance.AppendDebug(line + " - " + value.ToString().Replace("\r\n", ""));
        }

        public void AppendDebug(string line)
        {            
            _debugSb.AppendLine(line);
        }
    }

    public class StoppedDoubleConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(StoppedDouble).IsAssignableFrom(objectType) || typeof(int).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader,
            Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Object)
            {
                return token.ToObject<StoppedDouble>();
            }
            return new StoppedDouble() { SingleVal = token.Value<float>()};
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
