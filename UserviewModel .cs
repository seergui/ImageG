using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ImageG
{
    internal class UserviewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public UserviewModel() { }
        private double data;
        public double X(double x)
        {
            try
            {
                data = x;
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("请输入数据!");
            }
            return data;
        }
    }
}
