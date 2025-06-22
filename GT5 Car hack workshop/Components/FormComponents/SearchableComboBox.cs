using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace YourNamespace.Controls
{
    public partial class SearchableComboBox : UserControl
    {
        private List<object> _originalItems = new List<object>();
        private bool _isFiltering = false;

        public SearchableComboBox()
        {
            InitializeComponent();
            SetupEventHandlers();
        }

        #region Public Properties

        [Browsable(true)]
        [Category("Data")]
        [Description("The items displayed in the combo box")]
        public ComboBox.ObjectCollection Items => comboBox1.Items;

        [Browsable(true)]
        [Category("Appearance")]
        [Description("The selected item in the combo box")]
        public object SelectedItem
        {
            get => comboBox1.SelectedItem;
            set => comboBox1.SelectedItem = value;
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("The index of the selected item")]
        public int SelectedIndex
        {
            get => comboBox1.SelectedIndex;
            set => comboBox1.SelectedIndex = value;
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("The text displayed in the combo box")]
        public override string Text
        {
            get => comboBox1.Text;
            set => comboBox1.Text = value;
        }

        [Browsable(true)]
        [Category("Behavior")]
        [Description("Determines if the search is case sensitive")]
        public bool CaseSensitiveSearch { get; set; } = false;

        [Browsable(true)]
        [Category("Data")]
        [Description("The property to display for the items in the combo box")]
        public string DisplayMember
        {
            get => comboBox1.DisplayMember;
            set => comboBox1.DisplayMember = value;
        }

        [Browsable(true)]
        [Category("Data")]
        [Description("The property to use as the value for the items in the combo box")]
        public string ValueMember
        {
            get => comboBox1.ValueMember;
            set => comboBox1.ValueMember = value;
        }

        #endregion

        #region Public Events

        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when the selected index changes")]
        public event EventHandler SelectedIndexChanged;

        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when the text changes")]
        public new event EventHandler TextChanged;

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds an item to the combo box
        /// </summary>
        public void AddItem(object item)
        {
            _originalItems.Add(item);
            if (!_isFiltering)
            {
                comboBox1.Items.Add(item);
            }
        }

        /// <summary>
        /// Removes an item from the combo box
        /// </summary>
        public void RemoveItem(object item)
        {
            _originalItems.Remove(item);
            comboBox1.Items.Remove(item);
        }

        /// <summary>
        /// Clears all items from the combo box
        /// </summary>
        public void ClearItems()
        {
            _originalItems.Clear();
            comboBox1.Items.Clear();
        }

        /// <summary>
        /// Refreshes the original items list from current combo box items
        /// </summary>
        public void RefreshOriginalItems()
        {
            _originalItems.Clear();
            foreach (object item in comboBox1.Items)
            {
                _originalItems.Add(item);
            }
        }

        #endregion

        #region Private Methods

        private void SetupEventHandlers()
        {
            comboBox1.TextChanged += ComboBox_TextChanged;
            comboBox1.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            comboBox1.DropDown += ComboBox_DropDown;
        }

        private void ComboBox_TextChanged(object sender, EventArgs e)
        {
            if (_isFiltering) return;

            // Only filter items if the dropdown is already open
            if (comboBox1.DroppedDown)
            {
                FilterItems();
            }

            TextChanged?.Invoke(this, e);
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedIndexChanged?.Invoke(this, e);
        }

        private void ComboBox_DropDown(object sender, EventArgs e)
        {
            // Always show all items when dropdown is opened
            RestoreAllItems();
        }

        private void FilterItems()
        {
            string searchText = comboBox1.Text;

            if (string.IsNullOrEmpty(searchText))
            {
                RestoreAllItems();
                return;
            }

            _isFiltering = true;

            // Store current cursor position
            int cursorPosition = comboBox1.SelectionStart;

            // Filter items based on search text
            var filteredItems = _originalItems.Where(item =>
                ContainsSearchText(item?.ToString() ?? string.Empty, searchText)).ToList();

            // Clear and repopulate combo box
            comboBox1.Items.Clear();
            foreach (var item in filteredItems)
            {
                comboBox1.Items.Add(item);
            }

            // Restore text and cursor position
            comboBox1.Text = searchText;
            comboBox1.SelectionStart = cursorPosition;

            // Show dropdown if there are filtered results
            if (filteredItems.Count > 0 && !comboBox1.DroppedDown)
            {
                comboBox1.DroppedDown = true;
            }

            _isFiltering = false;
        }

        private bool ContainsSearchText(string itemText, string searchText)
        {
            if (CaseSensitiveSearch)
            {
                return itemText.Contains(searchText);
            }

            return itemText.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void RestoreAllItems()
        {
            if (_originalItems.Count == comboBox1.Items.Count) return;

            _isFiltering = true;
            comboBox1.Items.Clear();
            foreach (var item in _originalItems)
            {
                comboBox1.Items.Add(item);
            }

            _isFiltering = false;
        }

        #endregion

        #region Designer Support

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Initialize original items list from designer-added items
            if (_originalItems.Count == 0 && comboBox1.Items.Count > 0)
            {
                RefreshOriginalItems();
            }
        }

        #endregion
    }
}