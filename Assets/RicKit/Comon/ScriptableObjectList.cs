using System.Collections.Generic;
using UnityEngine;

namespace RicKit.Comon
{
    public abstract class ScriptableObjectList<T> : ScriptableObject
    {
        public List<T> list = new List<T>();
    }
}