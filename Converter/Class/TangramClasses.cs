using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MbJsonToYaml
{
    public class Stroke
    {
        public string Width { get; set; }
        public string Color { get; set; }
    }

    public class Font
    {
        public string Family { get; set; }
        public string Size { get; set; }
        public string Fill { get; set; }
        public Stroke Stroke { get; set; }
        public string Transform { get; set; }
    }
}
