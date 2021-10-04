
namespace HTMLElementSelect
{
    partial class frm_DiagramAction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_DiagramAction));
            this.bt_DeleteFunction = new System.Windows.Forms.Button();
            this.bt_EditFunction = new System.Windows.Forms.Button();
            this.lbl_Message = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bt_DeleteFunction
            // 
            this.bt_DeleteFunction.Location = new System.Drawing.Point(74, 76);
            this.bt_DeleteFunction.Name = "bt_DeleteFunction";
            this.bt_DeleteFunction.Size = new System.Drawing.Size(104, 23);
            this.bt_DeleteFunction.TabIndex = 0;
            this.bt_DeleteFunction.Text = "Delete Function";
            this.bt_DeleteFunction.UseVisualStyleBackColor = true;
            this.bt_DeleteFunction.Click += new System.EventHandler(this.bt_DeleteFunction_Click);
            // 
            // bt_EditFunction
            // 
            this.bt_EditFunction.Location = new System.Drawing.Point(197, 76);
            this.bt_EditFunction.Name = "bt_EditFunction";
            this.bt_EditFunction.Size = new System.Drawing.Size(105, 23);
            this.bt_EditFunction.TabIndex = 1;
            this.bt_EditFunction.Text = "Edit Function";
            this.bt_EditFunction.UseVisualStyleBackColor = true;
            this.bt_EditFunction.Click += new System.EventHandler(this.bt_EditFunction_Click);
            // 
            // lbl_Message
            // 
            this.lbl_Message.AutoSize = true;
            this.lbl_Message.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Message.Location = new System.Drawing.Point(12, 32);
            this.lbl_Message.Name = "lbl_Message";
            this.lbl_Message.Size = new System.Drawing.Size(182, 13);
            this.lbl_Message.TabIndex = 2;
            this.lbl_Message.Text = "What are you going to do with ";
            // 
            // frm_DiagramAction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 129);
            this.Controls.Add(this.lbl_Message);
            this.Controls.Add(this.bt_EditFunction);
            this.Controls.Add(this.bt_DeleteFunction);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_DiagramAction";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Workflow Action";
            this.Load += new System.EventHandler(this.frm_DiagramAction_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_DeleteFunction;
        private System.Windows.Forms.Button bt_EditFunction;
        private System.Windows.Forms.Label lbl_Message;
    }
}