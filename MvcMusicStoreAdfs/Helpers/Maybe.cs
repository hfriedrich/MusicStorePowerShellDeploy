namespace MvcMusicStoreAdfs.Helpers
{
    public class Maybe<T> where T : class
    {
        private readonly T _instance;

        public T Value { get { return _instance; } }
        public bool HasValue { get { return _instance != null; } }
        public bool ValueMissing { get { return _instance == null; } }

        public Maybe(T instance)
        {
            _instance = instance;
        }
    }
}