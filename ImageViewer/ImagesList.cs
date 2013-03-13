using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;

namespace ImageViewer
{
    class ImagesList
    {
        List<string> files;
        private string directory;
        private int currentImageIndex;


        public ImagesList(string fileName)
        {
            directory = Path.GetDirectoryName(fileName);
            //получение имен всех файлов изображения в текущей папке
            files =  Directory.GetFiles(directory, "*.jpg").ToList();
            //Устанавливаем индекс в соответствии с параметром запущенного изображения
            currentImageIndex = files.IndexOf(fileName);
        }
        //Возврат тукущего изображения
        public BitmapImage Current
        {
            get
            {
                while (!File.Exists(files[currentImageIndex]))
                {
                    files.RemoveAt(currentImageIndex);
                    if (currentImageIndex >= files.Count) currentImageIndex = files.Count - 1;
                    if (currentImageIndex < 0)
                    {
                        Application.Current.Shutdown();
                        return null;
                    }
                }
                try
                {
                    return new BitmapImage(new Uri(files[currentImageIndex]));
                }
                catch (Exception e) 
                { 
                    //Тут нужно будет возвращать изображение
                    //ФАЙЛ НЕВОЗМОЖНО ОТКРЫТЬ
                    return null; 
                }
            }
        }
        //Возврат следующего изображения
        public BitmapImage Next
        {
            get
            {
                //Увеличение индекса и проверка на выход из диапазона
                if (++currentImageIndex >= files.Count) currentImageIndex = 0;
                return this.Current;
            }
        }
        //Возврат предидущего изображения
        public BitmapImage Prev
        {
            get
            {
                //Уменьшение индекса и проверка на выход из диапазона
                if (--currentImageIndex < 0) currentImageIndex = files.Count-1;
                return this.Current;
            }
        }
    }
}
