using System;
using System.Windows;
using System.Windows.Input;

namespace ImageViewer
{
	public partial class MainWindow
	{
	    readonly ImagesList _allImage;
		
        public MainWindow()
		{
            //Закрыть приложение если оно было запущено без аргумента ком. стр. с именем файла
            //1-й параметр - полное имя запущенного приложения
            if (Environment.GetCommandLineArgs().Length <= 1)
            {
                Application.Current.Shutdown();
                return;
            }

            InitializeComponent();

            //Получаем список всех изображений в текущей папке
            //и показываем запущенную
            _allImage = new ImagesList(Environment.GetCommandLineArgs()[1]);
            image.Source = _allImage.Current;

            //Установка размеров формы в соответствии с рабочим столом
            Width = SystemParameters.MaximizedPrimaryScreenWidth;
            Height = SystemParameters.MaximizedPrimaryScreenHeight;
		}

        private void NextImage()
        {
            image.Source = _allImage.Next;
        }

        private void PrevImage()
        {
            image.Source = _allImage.Prev;
        }

        private void ImageMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NextImage();
        }

        private void BtnPrevImageMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PrevImage();
        }

        private void BtnCloseMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
	}
}