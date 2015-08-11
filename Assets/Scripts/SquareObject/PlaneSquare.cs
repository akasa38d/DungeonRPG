using UnityEngine;
using System.Collections;

/// <summary>
/// 普通の床のクラス
/// 処理はAbstractSquareを参照
/// </summary>
public class PlaneSquare : AbstractSquare
{
    public void Start() { type = Type.Normal; }
}
