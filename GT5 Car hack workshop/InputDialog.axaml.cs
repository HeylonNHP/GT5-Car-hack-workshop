using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace GT5_Car_hack_workshop
{
    public partial class InputDialog : Window
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
            get => LblPrompt.Text ?? string.Empty;
            set => LblPrompt.Text = value;
        }

        /// <summary>
        /// The current value of the input box.
        /// </summary>
        public string InputValue
        {
            get => TxtInput.Text ?? string.Empty;
            set => TxtInput.Text = value;
        }

        /// <summary>
        /// Displays the dialog and returns what the user typed (or null on cancel).
        /// </summary>
        public static async System.Threading.Tasks.Task<string?> Show(Window owner, string prompt, string title = "Input", string defaultValue = "")
        {
            var dialog = new InputDialog
            {
                Title = title,
                Prompt = prompt,
                InputValue = defaultValue
            };

            // Focus and select text when shown
            dialog.Opened += (s, e) =>
            {
                dialog.TxtInput.SelectAll();
                dialog.TxtInput.Focus();
            };

            var result = await dialog.ShowDialog<bool>(owner);
            return result ? dialog.InputValue : null;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Close(true);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close(false);
        }
    }
}
