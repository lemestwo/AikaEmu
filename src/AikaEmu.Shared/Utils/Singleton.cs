using System.Reflection;

namespace AikaEmu.Shared.Utils
{
    public abstract class Singleton<T> where T : class
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                OnInit();

                return _instance;
            }
        }

        private static void OnInit()
        {
            if (_instance != null)
                return;
            lock (typeof(T))
            {
                _instance = typeof(T).InvokeMember(typeof(T).Name,
                    BindingFlags.CreateInstance |
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic,
                    null, null, null) as T;
            }
        }
    }
}