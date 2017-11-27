using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MbJsonToYaml.Utils;

namespace MbJsonToYaml
{
    public partial class FormMain : Form
    {
        private readonly Converter _converter = Converter.GetInstance();
        private string _style;

        public FormMain()
        {
            InitializeComponent();
        }

        private string BrightFile = "bright-v9";
        private string BasicFile = "basic-v9";
        private string LibertyFile = "osm-liberty";

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxInput.SelectedIndex = 0;
        }

        private void ReadFile(string style)
        {
            _style = style;

            var lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\Styles\" + _style + "\\" + _style + ".json");
            textBoxIn.Lines = lines;
            Process();
        }

        private void Process()
        {
            string debugLines = null;

            string inText = textBoxIn.Text;

            textBoxOut.Text = _converter.Convert(_style, inText, out debugLines);
            textBoxDebug.Text = debugLines;
        }

        static string ConvertStringArrayToString(string[] array)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.AppendLine(value);                
            }
            return builder.ToString();
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBoxOut.Text);
        }

        private void buttonConvert_Click(object sender, EventArgs e)
        {
            Process();
        }

        private void checkBoxForceUrl_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxForceUrl.Checked)            
                _converter.ForcedUrl = "http://localhost:8765/{x}/{y}/{z}.mvt";            
            else            
                _converter.ForcedUrl = null;
            
            Process();
        }

        private void checkBoxExcludeCommonParts_CheckedChanged(object sender, EventArgs e)
        {
            // I have some of these things defined in a common import yaml 
            _converter.ExcludeCommon = checkBoxExcludeCommonParts.Checked;
            Process();
        }

        private void checkBoxIncludeSprites_CheckedChanged(object sender, EventArgs e)
        {
            _converter.IncludeSprites = checkBoxIncludeSprites.Checked;
        }

        private void comboBoxInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxInput.SelectedIndex)
            {
                case 0:
                    ReadFile(BasicFile);
                    break;
                case 1:
                    ReadFile(BrightFile);
                    break;
                case 2:
                    ReadFile(LibertyFile);
                    break;
            }
        }        
    }
}
