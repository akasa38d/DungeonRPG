using UnityEngine;
using System.Collections;

/// <summary>
/// Singleton.
/// </summary>
namespace MyUtility
{
    /// <summary>
    /// Singleton.
    /// </summary>
    public abstract class Singleton<T> where T : class, new()
    {
        static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
    }

    /// <summary>
    /// 二つの値を扱うクラス
    /// ジェネリックからintのみに変更
    /// </summary>
    public class MyVector2
    {
        public readonly int x;
        public readonly int y;

        public MyVector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public MyVector2(MyVector2 myVector2)
        {
            this.x = myVector2.x;
            this.y = myVector2.y;
        }
        public bool isEqual(MyVector2 myVector2)
        {
            if (this.x == myVector2.x && this.y == myVector2.y)
            {
                return true;
            }
            return false;
        }
    }
}
