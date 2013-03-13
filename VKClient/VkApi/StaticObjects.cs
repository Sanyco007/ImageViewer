namespace VKClient.VkApi
{
    public class StaticObjects
    {
        //Токен для работы с VK API
        internal static string Token;

        //Время жизни токена в секундах после получения
        internal static int ExpiresIn;

        //Идентификатор пользователя в системе
        internal static int UserId;

        //Запрос к серверу VK
        internal static string Request;

        static StaticObjects()
        {
            Token = null;
            Request = "https://api.vk.com/method/{0}?{1}&access_token={2}";
        }
    }
}
