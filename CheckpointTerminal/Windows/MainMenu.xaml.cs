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
using CheckpointTerminal.Classes;
using CheckpointTerminal.Converters;
using CheckpointTerminal.Http;
using CheckpointTerminal.Models;
using CheckpointTerminal.Singletons;

namespace CheckpointTerminal.Windows
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        private UpdateData up;
        public MainMenu()
        {
            InitializeComponent();
            Refresh();
        }
        private async void Refresh()
        {
            List<CheckpointWithOfficeName> allCheckpoints;
            HttpQuery httpManager = new HttpQuery();
            allCheckpoints = await httpManager.GetAllCheckpoints();
            CheckpointLV.ItemsSource = allCheckpoints;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button ChooseButton && ChooseButton.DataContext is Checkpointt selectedcheckpoint)
            {
                var QRCode = new QRCodeWindow(selectedcheckpoint);
                QRCode.Show();
                this.Close();
            }

        }
    }
}
