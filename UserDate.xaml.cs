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
namespace ImageG
{
    /// <summary>
    /// UserDate.xaml 的交互逻辑
    /// </summary>
    public partial class UserDate : Window
    {
        public UserDate()
        {
            InitializeComponent();
        }
        private void Commit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
