using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace VKClient.VkApi
{
    public class WpfAutorization
    {
        //Окно для авторизации
        private readonly Window _wOAuth = new Window();

        //WebBrowser для авторизации приложения
        private readonly WebBrowser _wbMain = new WebBrowser();

        private readonly string _connectString;

        public WpfAutorization(string connectString)
        {
            _connectString = connectString;
            _wbMain.LoadCompleted += wbMain_LoadCompleted;
        }

        //Загрузка документа
        private void wbMain_LoadCompleted(object sender, NavigationEventArgs e)
        {
            var uri = e.Uri.ToString();

            //Авторизация отклонена
            if (uri.IndexOf("access_denied", StringComparison.Ordinal) != -1)
            {
                throw new Exception("Error. Access denied!");
            }

            //Авторизация успешна
            if (uri.IndexOf("access_token", StringComparison.Ordinal) == -1) return;
            //Разбор данных с ответа
            var myReg = new Regex(@"(?<name>[\w\d\x5f]+)=(?<value>[^\x26\s]+)",
                                  RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match m in myReg.Matches(uri))
            {
                //Токен
                switch (m.Groups["name"].Value)
                {
                    case "access_token":
                        StaticObjects.Token = m.Groups["value"].Value;
                        break;
                    case "expires_in":
                        StaticObjects.ExpiresIn = Convert.ToInt32(m.Groups["value"].Value);
                        break;
                    case "user_id":
                        StaticObjects.UserId = Convert.ToInt32(m.Groups["value"].Value);
                        break;
                }
            }
            _wOAuth.Close();
        }

        //Отображение окна авторизации
        public void ShowAutorized()
        {
            _wOAuth.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _wOAuth.ResizeMode = ResizeMode.CanMinimize;
            _wOAuth.Title = "Авторизация";
            _wOAuth.Width = 610;
            _wOAuth.Height = 400;
            _wOAuth.Content = _wbMain;
            _wOAuth.Loaded += wOAuth_Loaded;
            _wOAuth.ShowDialog();
        }

        //Запуск в браузере адреса для авторизации
        private void wOAuth_Loaded(object sender, RoutedEventArgs e)
        {
            _wbMain.Navigate(_connectString);
        }
    }
}
