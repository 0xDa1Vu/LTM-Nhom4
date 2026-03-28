namespace CoTuongOnline.Client
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(686, 600);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Cờ Tướng Online";
            Load += Form1_Load;
            ResumeLayout(false);
        }
    }
}