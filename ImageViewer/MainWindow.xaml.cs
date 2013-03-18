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
            if (!ParameterPassed())
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
            SetFormSize();
		}

	    private void SetFormSize()
	    {
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

        private bool ParameterPassed()
        {
            //1-й параметр - полное имя запущенного приложения
            //2-й - имя файла переданого в качестве параметра
            return (Environment.GetCommandLineArgs().Length >= 2);
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

        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Right:NextImage();
                    break;
                case Key.Left:PrevImage();
                    break;
            }
        }

	}
}