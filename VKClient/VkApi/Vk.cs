using System;
using VKClient.VkApi.VkPhotos;

namespace VKClient.VkApi
{
    public sealed class Vk
    {
        //Индетификатор приложения
        private const int Id = 3256100;

        //Разрешает доступ к фотографиям и уведомлениям
        private const string Scope = "notify,photos";

        //Адрес ответа сервера (в текущем случае - ответ приложению)
        private const string RedirectUri = "http://oauth.vk.com/blank.html";

        //Внешний вид окна авторизации
        private const string Display = "popup";

        //Адрес для авторизации приложения
        private static string ConnectString
        {
            get
            {
                var res = String.Format("https://oauth.vk.com/authorize?client_id={0}" +
                    "&scope={1}&redirect_uri={2}&display={3}& response_type=token", 
                    Id, Scope, RedirectUri, Display);
                return res;
            }
        }

        private Photos _photos;
        public Photos Photos
        {
            get 
            { 
                if (_photos == null)
                {
                    if (StaticObjects.Token == null)
                    {
                        ThrowNoAutorizedException();
                    }
                    else
                    {
                        _photos = new Photos();
                    }
                }
                return _photos; 
            }
        }

        public void Autorization()
        {
            new WpfAutorization(ConnectString).ShowAutorized();
        }

        private static void ThrowNoAutorizedException()
        {
            throw new Exception("User is not autorized!");
        }
    }
}
