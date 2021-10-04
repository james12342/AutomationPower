
namespace PowerAutomationTest
{
    partial class Form1
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
            this.txt_XPath2 = new System.Windows.Forms.TextBox();
            this.lbl_XP2 = new System.Windows.Forms.Label();
            this.ck_DynamicElement = new System.Windows.Forms.CheckBox();
            this.bt_TestLatest = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_XPath = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txt_XPath2
            // 
            this.txt_XPath2.Location = new System.Drawing.Point(16, 125);
            this.txt_XPath2.Name = "txt_XPath2";
            this.txt_XPath2.Size = new System.Drawing.Size(772, 20);
            this.txt_XPath2.TabIndex = 50;
            this.txt_XPath2.Visible = false;
            // 
            // lbl_XP2
            // 
            this.lbl_XP2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_XP2.AutoSize = true;
            this.lbl_XP2.Location = new System.Drawing.Point(15, 109);
            this.lbl_XP2.Name = "lbl_XP2";
            this.lbl_XP2.Size = new System.Drawing.Size(282, 13);
            this.lbl_XP2.TabIndex = 49;
            this.lbl_XP2.Text = "Dynamic Element XPath ( After refresh the page and get it)";
            this.lbl_XP2.Visible = false;
            // 
            // ck_DynamicElement
            // 
            this.ck_DynamicElement.AutoSize = true;
            this.ck_DynamicElement.Location = new System.Drawing.Point(163, 29);
            this.ck_DynamicElement.Name = "ck_DynamicElement";
            this.ck_DynamicElement.Size = new System.Drawing.Size(108, 17);
            this.ck_DynamicElement.TabIndex = 48;
            this.ck_DynamicElement.Text = "Dynamic Element";
            this.ck_DynamicElement.UseVisualStyleBackColor = true;
            // 
            // bt_TestLatest
            // 
            this.bt_TestLatest.Location = new System.Drawing.Point(15, 151);
            this.bt_TestLatest.Name = "bt_TestLatest";
            this.bt_TestLatest.Size = new System.Drawing.Size(100, 30);
            this.bt_TestLatest.TabIndex = 47;
            this.bt_TestLatest.Text = "Run  Automation";
            this.bt_TestLatest.UseVisualStyleBackColor = true;
            this.bt_TestLatest.Click += new System.EventHandler(this.bt_TestLatest_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 13);
            this.label2.TabIndex = 46;
            this.label2.Text = "Current Element XPath";
            // 
            // txt_XPath
            // 
            this.txt_XPath.Location = new System.Drawing.Point(18, 86);
            this.txt_XPath.Name = "txt_XPath";
            this.txt_XPath.Size = new System.Drawing.Size(770, 20);
            this.txt_XPath.TabIndex = 45;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "TagName",
            "ClassName",
            "Id",
            "Name",
            "XPath",
            "CssSelector"});
            this.comboBox1.Location = new System.Drawing.Point(277, 29);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 51;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.txt_XPath2);
            this.Controls.Add(this.lbl_XP2);
            this.Controls.Add(this.ck_DynamicElement);
            this.Controls.Add(this.bt_TestLatest);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_XPath);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_XPath2;
        private System.Windows.Forms.Label lbl_XP2;
        private System.Windows.Forms.CheckBox ck_DynamicElement;
        private System.Windows.Forms.Button bt_TestLatest;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_XPath;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}

