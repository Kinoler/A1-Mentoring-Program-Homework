using System;

namespace WelcоmeHandler
{
    public class WelcomeHandler
    {
        public string WelcomeWrapper(string name)
        {
            return $"{DateTime.Now} Hello, {name}!";
        }
    }
}
