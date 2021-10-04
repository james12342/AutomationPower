
namespace HTMLElementSelect
{
    partial class frm_AutoDigram
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
            this.components = new System.ComponentModel.Container();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sendTextSELIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getTextSELIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.designer1 = new Dalssoft.DiagramNet.Designer();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendTextSELIToolStripMenuItem,
            this.getTextSELIToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(152, 48);
            // 
            // sendTextSELIToolStripMenuItem
            // 
            this.sendTextSELIToolStripMenuItem.Name = "sendTextSELIToolStripMenuItem";
            this.sendTextSELIToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.sendTextSELIToolStripMenuItem.Text = "Send Text |SELI";
            // 
            // getTextSELIToolStripMenuItem
            // 
            this.getTextSELIToolStripMenuItem.Name = "getTextSELIToolStripMenuItem";
            this.getTextSELIToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.getTextSELIToolStripMenuItem.Text = "Get Text |SELI";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBox1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1114, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox1.Items.AddRange(new object[] {
            "Send Text |SEL",
            "Get Text |SEL",
            "Click Something |SEL"});
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 25);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.designer1);
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1114, 595);
            this.panel1.TabIndex = 4;
            // 
            // designer1
            // 
            this.designer1.AutoScroll = true;
            this.designer1.BackColor = System.Drawing.SystemColors.Window;
            this.designer1.Location = new System.Drawing.Point(0, 3);
            this.designer1.Name = "designer1";
            this.designer1.Size = new System.Drawing.Size(1114, 592);
            this.designer1.TabIndex = 0;
            // 
            // frm_AutoDigram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 622);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "frm_AutoDigram";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Automation Digram";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.ToolStripMenuItem sendTextSELIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getTextSELIToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private Dalssoft.DiagramNet.Designer designer1;
    }
}