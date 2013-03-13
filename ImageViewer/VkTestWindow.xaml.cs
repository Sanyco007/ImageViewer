using System;
using System.Windows;
using AsyncLibrary;
using VKClient.VkApi;

namespace ImageViewer
{
    /// <summary>
    /// Логика взаимодействия для VkTestWindow.xaml
    /// </summary>
    public partial class VkTestWindow
    {
        private Vk _vk;

        public VkTestWindow()
        {
            InitializeComponent();
        }

        private void bAutorization_Click(object sender, RoutedEventArgs e)
        {
            _vk = new Vk();
            _vk.Autorization();
        }

        private void bLoadPhotos_Click(object sender, RoutedEventArgs e)
        {
            string[] files =
                {
                    "D:\\1.jpg", 
                    "D:\\22.jpg", 
                    "D:\\3.jpg"
                };
            var albums = _vk.Photos.GetAlbums();
            var tmp = new PhotosAsync(_vk);
            tmp.LoadPhotoToAlbum(albums[0].Aid, files, CallBack);
            progressBar1.IsIndeterminate = true;
        }

        private void CallBack(int count, int total)
        {

                    lProgress.Content = count + " from " + total;
                    if (count == total)
                    {
                        progressBar1.IsIndeterminate = false;
                        lProgress.Content = "Загрузка завершена!";
                    }

        }
    }
}
