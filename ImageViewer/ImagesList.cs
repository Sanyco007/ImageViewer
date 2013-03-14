using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;

namespace ImageViewer
{
    class ImagesList
    {
        //Список имен файлов изображений
        readonly List<string> _files = new List<string>();
        //Папка из которой запустили изображение
        private readonly string _directory;
        //Индекс текущего изображения
        private int _currentImageIndex;
        //Список поддерживаемых расширений
        private readonly List<string> _extension = new List<string> { ".jpg", ".png", ".ico", ".bmp" };

        public ImagesList(string fileName)
        {
            //Извлечение директории из файла параметра
            _directory = Path.GetDirectoryName(fileName);
            //получение имен всех файлов в текущей папке
            Debug.Assert(_directory != null, "_directory != null");
            string[] allFiles = Directory.GetFiles(_directory);
            //Выбор изображений из всех файлов
            foreach (string s in allFiles)
            {
                //Если хоть один элемент из списка расширений совпадает с именем текущего файла
                //то добавляем файл в список
                if (_extension.Exists(item=>s.ToLower().EndsWith(item)))
                {
                    _files.Add(s);
                }
            }
            //Устанавливаем индекс в соответствии с параметром запущенного изображения
            _currentImageIndex = _files.IndexOf(fileName);
        }
        //Возврат тукущего изображения
        public BitmapImage Current
        {
            get
            {
                //Удаляем те имена из списка, которых уже нет на диске
                while (!File.Exists(_files[_currentImageIndex]))
                {
                    _files.RemoveAt(_currentImageIndex);
                    if (_currentImageIndex >= _files.Count) _currentImageIndex = _files.Count - 1;
                    //Если все файлы удалены, то закрываем программу
                    if (_currentImageIndex < 0)
                    {
                        Application.Current.Shutdown();
                        return null;
                    }
                }
                try
                {
                    return new BitmapImage(new Uri(_files[_currentImageIndex]));
                }
                catch (Exception) 
                {
                    //Если полученный файл не есть изображением, то выводим картинку-предуприждение
                    //Преобрахование из Bitmap в BitmapImage
                    var result = new BitmapImage();
                    using (var memory = new MemoryStream())
                    {
                        Resource.IncorrectData.Save(memory, ImageFormat.Png);
                        memory.Position = 0;
                        result.BeginInit();
                        result.StreamSource = memory;
                        result.CacheOption = BitmapCacheOption.OnLoad;
                        result.EndInit();
                    }
                    return result;
                }
            }
        }
        //Возврат следующего изображения
        public BitmapImage Next
        {
            get
            {
                //Увеличение индекса и проверка на выход из диапазона
                if (++_currentImageIndex >= _files.Count) _currentImageIndex = 0;
                return Current;
            }
        }
        //Возврат предидущего изображения
        public BitmapImage Prev
        {
            get
            {
                //Уменьшение индекса и проверка на выход из диапазона
                if (--_currentImageIndex < 0) _currentImageIndex = _files.Count-1;
                return Current;
            }
        }
    }
}
