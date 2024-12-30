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
using Wpf.Ui.Controls;

namespace HAMZA_PFE.Dialogs
{
    /// <summary>
    /// Interaction logic for QuickScanDialog.xaml
    /// </summary>
    public partial class QuickScanDialog : FluentWindow
    {
        public QuickScanDialog()
        {
            InitializeComponent();
            FileEnScan.Content = ".....";
            TimeEstimation.Content = ".....";
            Progress.Value = 0;

        }
    }
}
