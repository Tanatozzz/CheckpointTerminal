using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
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
using System.Windows.Threading;
using CheckpointTerminal.Models;
using CheckpointTerminal.Singletons;
using QRCoder;

namespace CheckpointTerminal.Windows
{
    /// <summary>
    /// Interaction logic for QRCodeWindow.xaml
    /// </summary>
    public partial class QRCodeWindow : Window
    {
        Checkpointt check = new Checkpointt();
        CheckpointWithOfficeName checkTitleOffice = new CheckpointWithOfficeName();
        private DispatcherTimer timer;
        public QRCodeWindow(Checkpointt selectedCheckpoint)
        {
            InitializeComponent();
            this.check = selectedCheckpoint;
            GenerateQRCode();
            StartTimer();
            IDTextBlock.Text = check.ID.ToString();
            TitleTextBlock.Text = check.Title.ToString();
            IDOfficeTextBlock.Text = checkTitleOffice.OfficeTitle.ToString();
            if (check.IsActive == true)
            {
                IsActiveTextBlock.Text = "Активен";
            }
            else
            {
                IsActiveTextBlock.Text = "Не активен";
            }
            
        }
        private void StartTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            GenerateQRCode();
        }
        private void GenerateQRCode()
        {
            string id = check.ID.ToString();
            string title = check.Title.ToString();
            string officeID = check.IDOffice.ToString();
            bool isActive = check.IsActive;

            // Формируем информацию о проходе с текущей датой и временем
            string passageInfo = $"ID: {id}\nTitle: {title}\nIDOffice: {officeID}\nIsActive: {isActive}\nDateTime: {DateTime.Now}";

            // Создаем экземпляр генератора QR-кода
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(passageInfo, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            // Генерируем изображение QR-кода
            System.Drawing.Bitmap qrCodeImage = qrCode.GetGraphic(20); // 20 - размер пикселей для каждого модуля QR-кода

            // Конвертируем изображение в тип BitmapImage для отображения в элементе Image
            using (MemoryStream memoryStream = new MemoryStream())
            {
                qrCodeImage.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();

                // Устанавливаем сгенерированное изображение QR-кода в элемент Image
                qrCodeImageElement.Source = bitmapImage;
            }
        }

    }
}
