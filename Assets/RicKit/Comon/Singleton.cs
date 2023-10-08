namespace RicKit.Comon
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static T instance;
        public static T I => instance ??= new T();
    }
}