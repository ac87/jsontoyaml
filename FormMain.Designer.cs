namespace MbJsonToYaml
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxIn = new System.Windows.Forms.TextBox();
            this.textBoxOut = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxIncludeSprites = new System.Windows.Forms.CheckBox();
            this.comboBoxInput = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.button5 = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.button4 = new System.Windows.Forms.Button();
            this.checkBoxForceUrl = new System.Windows.Forms.CheckBox();
            this.textBoxDebug = new System.Windows.Forms.TextBox();
            this.checkBoxExcludeCommonParts = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxIn
            // 
            this.textBoxIn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxIn.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxIn.Location = new System.Drawing.Point(3, 32);
            this.textBoxIn.Multiline = true;
            this.textBoxIn.Name = "textBoxIn";
            this.textBoxIn.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxIn.Size = new System.Drawing.Size(294, 533);
            this.textBoxIn.TabIndex = 1;
            // 
            // textBoxOut
            // 
            this.textBoxOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOut.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxOut.Location = new System.Drawing.Point(3, 33);
            this.textBoxOut.Multiline = true;
            this.textBoxOut.Name = "textBoxOut";
            this.textBoxOut.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxOut.Size = new System.Drawing.Size(621, 395);
            this.textBoxOut.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(211, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer1.Panel2.Controls.Add(this.button2);
            this.splitContainer1.Size = new System.Drawing.Size(931, 568);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 5;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxIn, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263158F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.73684F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(300, 568);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.checkBoxIncludeSprites, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.comboBoxInput, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(300, 29);
            this.tableLayoutPanel3.TabIndex = 6;
            // 
            // checkBoxIncludeSprites
            // 
            this.checkBoxIncludeSprites.AutoSize = true;
            this.checkBoxIncludeSprites.Checked = true;
            this.checkBoxIncludeSprites.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxIncludeSprites.Location = new System.Drawing.Point(209, 3);
            this.checkBoxIncludeSprites.Name = "checkBoxIncludeSprites";
            this.checkBoxIncludeSprites.Size = new System.Drawing.Size(78, 17);
            this.checkBoxIncludeSprites.TabIndex = 3;
            this.checkBoxIncludeSprites.Text = "inc. Sprites";
            this.checkBoxIncludeSprites.UseVisualStyleBackColor = true;
            this.checkBoxIncludeSprites.CheckedChanged += new System.EventHandler(this.checkBoxIncludeSprites_CheckedChanged);
            // 
            // comboBoxInput
            // 
            this.comboBoxInput.FormattingEnabled = true;
            this.comboBoxInput.Items.AddRange(new object[] {
            "Basic (MapBox)",
            "Bright (MapBox)",
            "OSM-Liberty (OpenMapTiles)"});
            this.comboBoxInput.Location = new System.Drawing.Point(3, 3);
            this.comboBoxInput.Name = "comboBoxInput";
            this.comboBoxInput.Size = new System.Drawing.Size(200, 21);
            this.comboBoxInput.TabIndex = 4;
            this.comboBoxInput.SelectedIndexChanged += new System.EventHandler(this.comboBoxInput_SelectedIndexChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.button5, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.textBoxOut, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBoxDebug, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 78.94736F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 21.05263F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(627, 568);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button5.Location = new System.Drawing.Point(447, 542);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(177, 23);
            this.button5.TabIndex = 4;
            this.button5.Text = "Copy to Clipboard";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.63636F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.18182F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.18182F));
            this.tableLayoutPanel4.Controls.Add(this.button4, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxForceUrl, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxExcludeCommonParts, 2, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(627, 30);
            this.tableLayoutPanel4.TabIndex = 5;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(3, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "Convert";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.buttonConvert_Click);
            // 
            // checkBoxForceUrl
            // 
            this.checkBoxForceUrl.AutoSize = true;
            this.checkBoxForceUrl.BackColor = System.Drawing.SystemColors.Control;
            this.checkBoxForceUrl.Location = new System.Drawing.Point(88, 3);
            this.checkBoxForceUrl.Name = "checkBoxForceUrl";
            this.checkBoxForceUrl.Size = new System.Drawing.Size(109, 17);
            this.checkBoxForceUrl.TabIndex = 5;
            this.checkBoxForceUrl.Text = "Force offline URL";
            this.checkBoxForceUrl.UseVisualStyleBackColor = false;
            this.checkBoxForceUrl.CheckedChanged += new System.EventHandler(this.checkBoxForceUrl_CheckedChanged);
            // 
            // textBoxDebug
            // 
            this.textBoxDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDebug.Location = new System.Drawing.Point(3, 434);
            this.textBoxDebug.Multiline = true;
            this.textBoxDebug.Name = "textBoxDebug";
            this.textBoxDebug.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDebug.Size = new System.Drawing.Size(621, 100);
            this.textBoxDebug.TabIndex = 6;
            // 
            // checkBoxExcludeCommonParts
            // 
            this.checkBoxExcludeCommonParts.AutoSize = true;
            this.checkBoxExcludeCommonParts.Location = new System.Drawing.Point(358, 3);
            this.checkBoxExcludeCommonParts.Name = "checkBoxExcludeCommonParts";
            this.checkBoxExcludeCommonParts.Size = new System.Drawing.Size(108, 17);
            this.checkBoxExcludeCommonParts.TabIndex = 6;
            this.checkBoxExcludeCommonParts.Text = "Exclude Common";
            this.checkBoxExcludeCommonParts.UseVisualStyleBackColor = true;
            this.checkBoxExcludeCommonParts.CheckedChanged += new System.EventHandler(this.checkBoxExcludeCommonParts_CheckedChanged);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 568);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormMain";
            this.Text = "JSON to YAML Converter";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxIn;
        private System.Windows.Forms.TextBox textBoxOut;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckBox checkBoxForceUrl;
        private System.Windows.Forms.TextBox textBoxDebug;
        private System.Windows.Forms.CheckBox checkBoxIncludeSprites;
        private System.Windows.Forms.ComboBox comboBoxInput;
        private System.Windows.Forms.CheckBox checkBoxExcludeCommonParts;
    }
}

