using System;
using System.Xml;
using System.Net;
using System.IO;
using System.Web;
using System.Text;
using VKClient.VkApi.VkHelpClasses;

namespace VKClient.VkApi.VkPhotos
{
    public sealed class Photos
    {
        /// <summary>
        /// Возвращает массив альбомов пользователя
        /// </summary>
        public Album[] GetAlbums()
        {
            string url = String.Format(StaticObjects.Request,
                "photos.getAlbums.xml", "need_covers=1", StaticObjects.Token);
            var xml = new XmlDocument();
            xml.Load(url);
            var list = xml.GetElementsByTagName("album");
            var albums = new Album[list.Count];
            for (var i = 0; i < list.Count; i++)
            {
                albums[i] = new Album();
                var item = (XmlElement)list[i];

                var aid = item.GetElementsByTagName("aid")[0].InnerText;
                albums[i].Aid = Convert.ToInt32(aid);

                var thumbId = item.GetElementsByTagName("thumb_id")[0].InnerText;
                albums[i].ThumbId = Convert.ToInt32(thumbId);

                var ownerId = item.GetElementsByTagName("owner_id")[0].InnerText;
                albums[i].OwnerId = Convert.ToInt32(ownerId);

                albums[i].Title = item.GetElementsByTagName("title")[0].InnerText;

                albums[i].Description = item.GetElementsByTagName("description")[0].InnerText;

                //var created = item.GetElementsByTagName("created")[0].InnerText;
                //var updated = item.GetElementsByTagName("updated")[0].InnerText;

                var size = item.GetElementsByTagName("size")[0].InnerText;
                albums[i].Size = Convert.ToInt32(size);

                var privacy = item.GetElementsByTagName("privacy")[0].InnerText;
                albums[i].Privacy = Convert.ToInt32(privacy);

                albums[i].ThumbSrc = item.GetElementsByTagName("thumb_src")[0].InnerText;
            }
            return albums;
        }

        public Photo[] GetProfile(int uid)
        {
            var url = String.Format(StaticObjects.Request, 
                "photos.getProfile.xml", "uid=" + uid +
                "&extended=1", StaticObjects.Token);
            var xml = new XmlDocument();
            xml.Load(url);
            var list = xml.GetElementsByTagName("photo");
            var photos = new Photo[list.Count];
            for (var i = 0; i < list.Count; i++)
            {
                photos[i] = new Photo();
                var item = (XmlElement)list[i];

                photos[i].Pid = item.GetElementsByTagName("pid")[0].InnerText;
                photos[i].Aid = item.GetElementsByTagName("aid")[0].InnerText;
                photos[i].OwnerId = item.GetElementsByTagName("owner_id")[0].InnerText;
                photos[i].Src = item.GetElementsByTagName("src")[0].InnerText;
                photos[i].SrcSmall = item.GetElementsByTagName("src_small")[0].InnerText;
                photos[i].SrcBig = item.GetElementsByTagName("src_big")[0].InnerText;
                photos[i].Text = item.GetElementsByTagName("text")[0].InnerText;
                photos[i].Created = item.GetElementsByTagName("created")[0].InnerText;

            }
            return photos;
        }

        //Получение адреса сервера
        private static UploadServer GetUploadServer(int aid)
        {
            var url = String.Format(StaticObjects.Request,
                "photos.getUploadServer.xml", "aid=" + aid, StaticObjects.Token);
            var xml = new XmlDocument();
            xml.Load(url);
            var uServer = new UploadServer
                {
                    UploadUrl = xml.GetElementsByTagName("upload_url")[0].InnerText
                };
            var xaid = xml.GetElementsByTagName("aid")[0].InnerText;
            uServer.Aid = Convert.ToInt32(xaid);
            return uServer;
        }

        //Сохранение загруженных картинок на сервере
        private static void PhotosSave(ServerInfo serverInfo, string caption = "")
        {
            var param = "aid=" + HttpUtility.UrlEncode(serverInfo.Aid)
                + "&server=" + HttpUtility.UrlEncode(serverInfo.Server) +
                "&photos_list=" + HttpUtility.UrlEncode(serverInfo.PhotosList) +
                "&hash=" + HttpUtility.UrlEncode(serverInfo.Hash) +
                "&caption=\"" + HttpUtility.UrlEncode(caption) + "\"";
            var url = String.Format(StaticObjects.Request, "photos.save.xml", 
                param, StaticObjects.Token);
            var xml = new XmlDocument();
            xml.Load(url);
        }

        private static ServerInfo ParseServerJson(string text)
        {
            var sInfo = new ServerInfo();
            text = text.Substring(10);
            var pos = text.IndexOf(",", StringComparison.Ordinal);
            sInfo.Server = text.Substring(0, pos);
            pos = text.IndexOf(":", StringComparison.Ordinal);
            text = text.Substring(pos + 1);
            pos = text.IndexOf(",\"aid\":", StringComparison.Ordinal);
            sInfo.PhotosList = text.Substring(1, pos - 2);
            text = text.Substring(pos + 7);
            pos = text.IndexOf(",", StringComparison.Ordinal);
            sInfo.Aid = text.Substring(0, pos);
            pos = text.IndexOf(":", StringComparison.Ordinal);
            text = text.Substring(pos + 1);
            sInfo.Hash = text.Substring(1, text.Length - 3);
            return sInfo;
        }

        //Загрузка файла на сервер
        public void LoadPhotoToAlbum(int aid, string file)
        {
            var uploadUrl = GetUploadServer(aid).UploadUrl;

            //Разделитель в теле POST запроса
            const string boundary = "----------ei4Ef1ei4cH2GI3gL6KM7cH2gL6ae0";

            //Шаблоны заголовков
            const string headerTemplate = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; " +
                  "filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n";
            const string endTemplate = "--{0}--\r\n\r\n";

            //Создание и настройка POST запроса
            var request = (HttpWebRequest)WebRequest.Create(uploadUrl);
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = "POST";
            request.ContentType = String.Format("multipart/form-data; boundary={0}", boundary);
            var stream = request.GetRequestStream();

            var picture = File.ReadAllBytes(file);

            string filePath = "block_1.png";
            const string fileType = "application/octet-stream";
            const string fileName = "file1";

            var contentFile = Encoding.ASCII.GetBytes(
                String.Format(headerTemplate, boundary, fileName, filePath, fileType));

            //Запись заголовка в поток запроса
            stream.Write(contentFile, 0, contentFile.Length);

            //Запись файла в поток запроса 
            stream.Write(picture, 0, picture.Length);

            //Запись символов новой строки в поток запроса
            var lineFeed = Encoding.ASCII.GetBytes("\r\n");
            stream.Write(lineFeed, 0, lineFeed.Length);

            //Запись признака конца запроса в поток запроса
            byte[] contentEnd = Encoding.ASCII.GetBytes(String.Format(endTemplate, boundary));
            stream.Write(contentEnd, 0, contentEnd.Length);
            stream.Flush();
            stream.Close();

            //Выполнение запроса с получением ответа
            var webResponse = (HttpWebResponse)request.GetResponse();
            var rstream = webResponse.GetResponseStream();
            if (rstream == null) return;
            var read = new StreamReader(rstream);

            //Ответ сервера
            var answer = read.ReadToEnd();
            answer = answer.Replace("\\\"", "\"");
            var serverInfo = ParseServerJson(answer);
            PhotosSave(serverInfo);
        }
    }
}
