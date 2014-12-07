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
using System.Windows.Shapes;

namespace Softfluent.Articles.NVO.DNASpecializer.HMI
{
    /// <summary>
    /// Interaction logic for MyDialog.xaml
    /// </summary>
    public partial class NewRandomPopulation : Window
    {
        public NewRandomPopulation()
        {
            InitializeComponent();
        }

        public int Response
        {
            get { return Int32.Parse( ResponseTextBox.Text); }
            set { ResponseTextBox.Text = value.ToString(); }
        }

        private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int result;
            if (Int32.TryParse(ResponseTextBox.Text, out result) && result >= 10)
                DialogResult = true;
        }
    }
}
