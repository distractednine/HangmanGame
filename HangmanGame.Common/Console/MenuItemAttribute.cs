using System;

namespace HangmanGame.Common.Console
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MenuItemAttribute : Attribute
    {
        public MenuItemAttribute(string actionName, string key)
        {
            ActionName = actionName;
            Key = key;
        }

        public string ActionName { get; private set; }

        public string Key { get; private set; }
    }
}
