using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GT5_Car_hack_workshop.Components.Forms
{
    partial class InputDialog
    {
        private IContainer components = null;
        private Label lblPrompt;
        private TextBox txtInput;
        private Button btnOk;
        private Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new Container();
            this.lblPrompt = new Label();
            this.txtInput = new TextBox();
            this.btnOk = new Button();
            this.btnCancel = new Button();
            SuspendLayout();
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Location = new Point(12, 15);
            this.lblPrompt.MaximumSize = new Size(360, 0);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new Size(50, 13);
            this.lblPrompt.Text = "Prompt:";
            // 
            // txtInput
            // 
            this.txtInput.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.txtInput.Location = new Point(15, 40);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new Size(375, 20);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnOk.DialogResult = DialogResult.OK;
            this.btnOk.Location = new Point(234, 80);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(75, 23);
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(315, 80);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(75, 23);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // InputDialog
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new Size(404, 115);
            this.Controls.Add(this.lblPrompt);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputDialog";
            this.Padding = new Padding(10);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Input";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}