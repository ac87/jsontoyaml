using System;
using System.Collections.Generic;
using System.Text;

using static MbJsonToYaml.Constants;

namespace MbJsonToYaml
{
    public class TangramLayerGroup : List<TangramLayer>
    {
        private readonly Dictionary<string, string> _replacedSources;
        private StringBuilder _sb;
        private int _indent;

        public string SourceLayer { get; internal set; }
        public bool IsCasing { get; internal set; }

        public TangramLayerGroup(Dictionary<string, string> replacedSources, string sourceLayer, bool isCasing)
        {
            _replacedSources = replacedSources;
            SourceLayer = sourceLayer;
            IsCasing = isCasing;
        }

        public string CreateLines()
        {
            if (this.Count == 0)
                return "";

            _sb = new StringBuilder();

            AppendLine($"{SourceLayer}{(IsCasing ? "-case" : "")}:");
            _indent++;

            string source = this[0].LayerWithSource.Source;
            if (_replacedSources != null && _replacedSources.ContainsKey(source))
                source = _replacedSources[source];

            if (IsCasing)
                AppendLine($"data: {{ source: { source }, layer: {SourceLayer} }}");
            else
                AppendLine($"data: {{ source: { source } }}");

            //_indent++;
            foreach (TangramLayer layer in this)
            {
                if (layer.LayerWithSource.Id.EndsWith("2" + CaseSuffix) || layer.LayerWithSource.Id.EndsWith("2" + CasingSuffix) || layer.LayerWithSource.Id.EndsWith("-2"))
                    continue;

                //if (!(layer.LayerWithSource.Id.Contains(Motorway) || layer.LayerWithSource.Id.Contains(Trunk)) || layer.LayerWithSource.Id.Contains(Bridge) || layer.LayerWithSource.Id.Contains("tunnel"))
                //    continue;

                var lines = layer.CreateLines(IsCasing);
                var split = lines.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                foreach (string str in split)
                    if (str.Trim().Length > 0)
                        AppendLine(str);
            }
            //_indent--;

            return _sb.ToString();
        }

        private void AppendLine(string txt)
        {
            for (int i = 0; i < _indent; i++)
                _sb.Append("    ");
            _sb.AppendLine(txt);
        }
    }
}
