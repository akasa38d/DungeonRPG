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
        public int x;
        public int y;

        public MyVector2() { }
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

    /// <summary>
    /// GameObjectの拡張
    /// </summary>
    public static class MyExtensions
    {
        public static bool checkOneDistanceE(this GameObject obj, GameObject target)
        {
            //距離０なら
            if (checkDistanceME(obj, target, 0)) { return false; }

            //マンハッタン距離１なら
            if (checkDistanceME(obj, target, 1)) { return true; }

            //カウント用変数
            int count = 0;
            //オブジェクトを調べて
            foreach (GameObject square in ObjectManager.Instance.square)
            {
                //ob1のマンハッタン距離が１かつ
                if (checkDistanceME(obj, square, 1) && target.checkDistanceME(square, 1))
                {
                    //カウントを増やす
                    count++;
                }
            }
            //カウントが２だったら
            if (count == 2) { return true; }
            
            //ここまでの条件に当てはまらなかったら
            return false;
        }

        //チェビシェフ距離（簡易）
        public static bool checkDistanceCE(this GameObject obj, GameObject target, int i)
        {
            if (Mathf.Abs(obj.transform.position.x - target.transform.position.x) <= i * 10)
            {
                if (Mathf.Abs(obj.transform.position.z - target.transform.position.z) <= i * 10)
                {
                    return true;
                }
            }
            return false;
        }
        //マンハッタン距離（簡易）
        public static bool checkDistanceME(this GameObject obj, GameObject target, int i)
        {
            if (Mathf.Abs(obj.transform.position.x - target.transform.position.x)
                +Mathf.Abs(obj.transform.position.z - target.transform.position.z) <= i * 10)
            {
                return true;
            }
            return false;
        }
    }

    //方角
    public enum Direction { up = 0, down, left, right }
}
