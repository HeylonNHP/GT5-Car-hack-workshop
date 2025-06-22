using System.Windows.Forms;

namespace GT5_Car_hack_workshop.Components.Forms
{
    public partial class InputDialog : Form
    {
        public InputDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The text shown as the prompt.
        /// </summary>
        public string Prompt
        {
            get => lblPrompt.Text;
            set => lblPrompt.Text = value;
        }

        /// <summary>
        /// The current value of the input box.
        /// </summary>
        public string InputValue
        {
            get => txtInput.Text;
            set => txtInput.Text = value;
        }

        /// <summary>
        /// Displays the dialog and returns what the user typed (or null on cancel).
        /// </summary>
        public static string Show(string prompt, string title = "Input", string defaultValue = "")
        {
            using (var dlg = new InputDialog())
            {
                dlg.Prompt = prompt;
                dlg.Text = title;
                dlg.InputValue = defaultValue;
                dlg.txtInput.SelectAll();
                dlg.txtInput.Focus();

                return dlg.ShowDialog() == DialogResult.OK
                    ? dlg.InputValue
                    : null;
            }
        }
    }
}