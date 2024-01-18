using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Open.Trl.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool _initializing;
        private string _lastTextChanged = "";

        public MainWindow()
        {
            _initializing = true;
            InitializeComponent();
            _initializing = false;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            KeyText.Text = "";
            GuidText.Text = "";
            _lastTextChanged = "";

            GuidText.Background = Brushes.White;
            KeyText.Background = Brushes.White;
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            if ((!String.IsNullOrWhiteSpace(_lastTextChanged)) && (String.Compare(_lastTextChanged, "Guid", StringComparison.CurrentCultureIgnoreCase) == 0))
            {
                // Convert GUID to Identity Key
                Guid guidValue;
                bool wasGuidValid = Guid.TryParse(GuidText.Text, out guidValue);
                if (wasGuidValid)
                {
                    Open.Trl.TeKey.EncryptionPassphrase = PassphraseText.Text.Trim();

                    Open.Trl.TeKey teKey = new Open.Trl.TeKey(guidValue);
                    KeyText.Text = teKey.ToString();

                    GuidText.Background = Brushes.White;
                    KeyText.Background = Brushes.LightGreen;
                }
                else
                {
                    GuidText.Background = Brushes.Red;
                    KeyText.Background = Brushes.LightCoral;
                }
            }
            else if ((!String.IsNullOrWhiteSpace(_lastTextChanged)) && (String.Compare(_lastTextChanged, "Key", StringComparison.CurrentCultureIgnoreCase) == 0))
            {
                Open.Trl.TeKey.EncryptionPassphrase = PassphraseText.Text.Trim();

                // Convert Identity Key to GUID
                Open.Trl.TeKey teKey = Open.Trl.TeKey.Parse(KeyText.Text);
                if (teKey.KeyType == Open.Trl.TeKeyType.Guid)
                {
                    GuidText.Text = teKey.GuidValue.ToString();

                    KeyText.Background = Brushes.White;
                    GuidText.Background = Brushes.LightGreen;
                }
            }
        }

        private void GuidText_Changed(object sender, TextChangedEventArgs e)
        {
            if (!_initializing)
            {
                _lastTextChanged = "Guid";
                GuidText.Background = Brushes.White;
                KeyText.Background = Brushes.LightCoral;
            }
        }

        private void KeyText_Changed(object sender, TextChangedEventArgs e)
        {
            if (!_initializing)
            {
                _lastTextChanged = "Key";
                KeyText.Background = Brushes.White;
                GuidText.Background = Brushes.LightCoral;
            }
        }
    }
}
