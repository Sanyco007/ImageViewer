using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace ImageViewer
{
	public partial class MainWindow : Window
	{
        ImagesList allImage;
		public MainWindow()
		{
            //Закрыть приложение если оно было запущено без аргумента ком. стр. с именем файла
            //1-й параметр - полное имя запущенного приложения
            if (System.Environment.GetCommandLineArgs().Length <= 1)
            {
                Application.Current.Shutdown();
                return;
            }

            this.InitializeComponent();

		    var src = System.Environment.GetCommandLineArgs()[1];

		    if (File.Exists(src))
		    {
		        //Получаем список всех изображений в текущей папке
		        //и показываем запущенную
		        allImage = new ImagesList(src);
		        image.Source = allImage.Current;
		    }

		    //Установка размеров формы в соответствии с рабочим столом
            this.Width = SystemParameters.MaximizedPrimaryScreenWidth;
            this.Height = SystemParameters.MaximizedPrimaryScreenHeight;
		}

        private void NextImage()
        {
            image.Source = allImage.Next;
        }

        private void PrevImage()
        {
            image.Source = allImage.Prev;
        }

        private void btnNextImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NextImage();
        }

        private void btnPrevImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PrevImage();
        }

        private void btnClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.T)
            {
                new VkTestWindow().ShowDialog();
            }
        }
	}
}