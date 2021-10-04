
namespace HTMLElementSelect
{
    partial class frm_PageTopBottom
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_PageTopBottom));
            this.lbl_PageTop_Bottom = new System.Windows.Forms.Label();
            this.bt_PageTopBottom = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_PageTop_Bottom
            // 
            this.lbl_PageTop_Bottom.AutoSize = true;
            this.lbl_PageTop_Bottom.Location = new System.Drawing.Point(23, 35);
            this.lbl_PageTop_Bottom.Name = "lbl_PageTop_Bottom";
            this.lbl_PageTop_Bottom.Size = new System.Drawing.Size(75, 13);
            this.lbl_PageTop_Bottom.TabIndex = 0;
            this.lbl_PageTop_Bottom.Text = "Going to Page";
            // 
            // bt_PageTopBottom
            // 
            this.bt_PageTopBottom.Location = new System.Drawing.Point(102, 88);
            this.bt_PageTopBottom.Name = "bt_PageTopBottom";
            this.bt_PageTopBottom.Size = new System.Drawing.Size(98, 23);
            this.bt_PageTopBottom.TabIndex = 1;
            this.bt_PageTopBottom.Text = "OK";
            this.bt_PageTopBottom.UseVisualStyleBackColor = true;
            this.bt_PageTopBottom.Click += new System.EventHandler(this.bt_PageTopBottom_Click);
            // 
            // frm_PageTopBottom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 147);
            this.Controls.Add(this.bt_PageTopBottom);
            this.Controls.Add(this.lbl_PageTop_Bottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_PageTopBottom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Go Page";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_PageTop_Bottom;
        private System.Windows.Forms.Button bt_PageTopBottom;
    }
}