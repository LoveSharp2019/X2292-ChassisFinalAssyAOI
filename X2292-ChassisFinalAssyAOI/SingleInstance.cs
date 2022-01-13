using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  X2292_ChassisFinalAssyAOI
{
    public abstract class SingleInstance<T> where T : new()
    {
        private static readonly T _instance = default(T);

        //init here,so no need lock 
        static SingleInstance()
        {
            _instance = new T();
        }
        public static T Instance
        {
            get
            {
                return _instance;
            }
        }

    }
}
