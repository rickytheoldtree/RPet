using System;
using Object = UnityEngine.Object;

namespace RicKit.Pool
{
    public class MonoPool<T> : Pool<T> where T : Object
    {
        private readonly Action<T> mResetMethod;

        public MonoPool(Func<T> factoryMethod, Action<T> resetMethod = null, int initCount = 0)
        {
            mFactory = new CustomObjectFactory<T>(factoryMethod);
            mResetMethod = resetMethod;

            for (var i = 0; i < initCount; i++) mCacheStack.Push(mFactory.Create());
        }

        public override T Allocate()
        {
            if (mCacheStack.Count == 0)
                return mFactory.Create();
            var item = mCacheStack.Pop();
            if (item) return item;
            while (mCacheStack.Count > 0)
            {
                item = mCacheStack.Pop();
                if (item) return item;
            }

            return mFactory.Create();
        }

        public override bool Recycle(T obj)
        {
            if (!obj) return false;
            mResetMethod?.Invoke(obj);
            mCacheStack.Push(obj);
            return true;
        }
    }
}