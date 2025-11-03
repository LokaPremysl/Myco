namespace Mush.WinForms
{
    public class InputDialog : Form
    {
        private TextBox txtInput;
        public string Value => txtInput.Text.Trim();

        public InputDialog(string title, string prompt)
        {
            Text = title;
            Width = 350;
            Height = 150;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;

            var lbl = new Label { Text = prompt, Dock = DockStyle.Top, Padding = new Padding(10) };
            txtInput = new TextBox { Dock = DockStyle.Top, Margin = new Padding(10) };
            var btnOk = new Button { Text = "OK", DialogResult = DialogResult.OK, Dock = DockStyle.Bottom };
            var btnCancel = new Button { Text = "Zrušit", DialogResult = DialogResult.Cancel, Dock = DockStyle.Bottom };

            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Controls.Add(txtInput);
            Controls.Add(lbl);

            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }
    }
}