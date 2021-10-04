namespace HTMLElementSelect
{
    partial class frm_Function_Sel_SendText
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Function_Sel_SendText));
            this.label4 = new System.Windows.Forms.Label();
            this.txt_rediURL = new System.Windows.Forms.TextBox();
            this.ck_isStartURL = new System.Windows.Forms.CheckBox();
            this.bt_SaveStep = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.txt_SendValue = new System.Windows.Forms.TextBox();
            this.txt_WaitT = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_XPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_url = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 209);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 13);
            this.label4.TabIndex = 48;
            this.label4.Text = "After done Redirect URL";
            // 
            // txt_rediURL
            // 
            this.txt_rediURL.Location = new System.Drawing.Point(15, 225);
            this.txt_rediURL.Name = "txt_rediURL";
            this.txt_rediURL.Size = new System.Drawing.Size(516, 20);
            this.txt_rediURL.TabIndex = 47;
            // 
            // ck_isStartURL
            // 
            this.ck_isStartURL.AutoSize = true;
            this.ck_isStartURL.Location = new System.Drawing.Point(15, 51);
            this.ck_isStartURL.Name = "ck_isStartURL";
            this.ck_isStartURL.Size = new System.Drawing.Size(84, 17);
            this.ck_isStartURL.TabIndex = 46;
            this.ck_isStartURL.Text = "Is Start URL";
            this.ck_isStartURL.UseVisualStyleBackColor = true;
            // 
            // bt_SaveStep
            // 
            this.bt_SaveStep.Location = new System.Drawing.Point(212, 251);
            this.bt_SaveStep.Name = "bt_SaveStep";
            this.bt_SaveStep.Size = new System.Drawing.Size(100, 30);
            this.bt_SaveStep.TabIndex = 45;
            this.bt_SaveStep.Text = "Save Step";
            this.bt_SaveStep.UseVisualStyleBackColor = true;
            this.bt_SaveStep.Click += new System.EventHandler(this.bt_SaveStep_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 41;
            this.label5.Text = "Send Value";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(67, 187);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(49, 13);
            this.label45.TabIndex = 44;
            this.label45.Text = "Seconds";
            // 
            // txt_SendValue
            // 
            this.txt_SendValue.Location = new System.Drawing.Point(15, 136);
            this.txt_SendValue.Name = "txt_SendValue";
            this.txt_SendValue.Size = new System.Drawing.Size(513, 20);
            this.txt_SendValue.TabIndex = 40;
            // 
            // txt_WaitT
            // 
            this.txt_WaitT.Location = new System.Drawing.Point(15, 184);
            this.txt_WaitT.Name = "txt_WaitT";
            this.txt_WaitT.Size = new System.Drawing.Size(46, 20);
            this.txt_WaitT.TabIndex = 43;
            this.txt_WaitT.Text = "0";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(15, 168);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(136, 13);
            this.label46.TabIndex = 42;
            this.label46.Text = "Wait Seconds When Finish";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 50;
            this.label1.Text = "XPath value";
            // 
            // txt_XPath
            // 
            this.txt_XPath.Location = new System.Drawing.Point(15, 93);
            this.txt_XPath.Name = "txt_XPath";
            this.txt_XPath.Size = new System.Drawing.Size(513, 20);
            this.txt_XPath.TabIndex = 49;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 52;
            this.label2.Text = "Web URL";
            // 
            // txt_url
            // 
            this.txt_url.Location = new System.Drawing.Point(15, 25);
            this.txt_url.Name = "txt_url";
            this.txt_url.Size = new System.Drawing.Size(513, 20);
            this.txt_url.TabIndex = 51;
            // 
            // frm_Function_Sel_SendText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 292);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_url);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_XPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_rediURL);
            this.Controls.Add(this.ck_isStartURL);
            this.Controls.Add(this.bt_SaveStep);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label45);
            this.Controls.Add(this.txt_SendValue);
            this.Controls.Add(this.txt_WaitT);
            this.Controls.Add(this.label46);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_Function_Sel_SendText";
            this.Text = "Web Send Text";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_rediURL;
        private System.Windows.Forms.CheckBox ck_isStartURL;
        private System.Windows.Forms.Button bt_SaveStep;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.TextBox txt_SendValue;
        private System.Windows.Forms.TextBox txt_WaitT;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_XPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_url;
    }
}