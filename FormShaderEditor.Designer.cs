namespace ugl
{
    partial class FormShaderEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormShaderEditor));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.glControl1 = new ugl.GLControl();
            this.textBoxEnter1 = new ugl.TextBoxEnter();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.glControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBoxEnter1);
            this.splitContainer1.Size = new System.Drawing.Size(1719, 832);
            this.splitContainer1.SplitterDistance = 801;
            this.splitContainer1.TabIndex = 0;
            // 
            // glControl1
            // 
            this.glControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl1.Location = new System.Drawing.Point(0, 0);
            this.glControl1.Name = "glControl1";
            this.glControl1.Running = true;
            this.glControl1.Size = new System.Drawing.Size(801, 832);
            this.glControl1.TabIndex = 0;
            this.glControl1.Text = "glControl1";
            // 
            // textBoxEnter1
            // 
            this.textBoxEnter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEnter1.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEnter1.Location = new System.Drawing.Point(0, 0);
            this.textBoxEnter1.Multiline = true;
            this.textBoxEnter1.Name = "textBoxEnter1";
            this.textBoxEnter1.Size = new System.Drawing.Size(914, 832);
            this.textBoxEnter1.TabIndex = 0;
            this.textBoxEnter1.Text = resources.GetString("textBoxEnter1.Text");
            this.textBoxEnter1.CtrlReturn += new System.EventHandler(this.textBoxEnter1_CtrlReturn);
            // 
            // FormShaderEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1719, 832);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormShaderEditor";
            this.Text = "Shader editor";
            this.Load += new System.EventHandler(this.FormShaderEditor_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private GLControl glControl1;
        private TextBoxEnter textBoxEnter1;
    }
}