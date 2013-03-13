using System;
using System.IO;
using System.Threading;
using System.Windows.Threading;
using VKClient.VkApi;

namespace AsyncLibrary
{
    public class PhotosAsync : DispatcherObject
    {
        public delegate void LoadStateChange(int count, int total);

        private readonly Vk _vk;
        private bool _loadFiles;

        public PhotosAsync(Vk vk)
        {
            _vk = vk;
        }

        public void StopLoad()
        {
            _loadFiles = false;
        }

        public void LoadPhotoToAlbum(int aid, string[] files, LoadStateChange callBack)
        {
            new Thread( () =>
            {
                _loadFiles = true;
                for (var i = 0; i < files.Length; i++)
                {
                    if (!_loadFiles) return;
                    var fileName = files[i];
                    if (!File.Exists(fileName)) continue;
                    _vk.Photos.LoadPhotoToAlbum(aid, fileName);
                    if (callBack != null)
                    {
                        var i1 = i;
                        Dispatcher.Invoke((Action) (() => callBack(i1 + 1, files.Length)));
                    }
                }
            }).Start();
        }

    }
}
