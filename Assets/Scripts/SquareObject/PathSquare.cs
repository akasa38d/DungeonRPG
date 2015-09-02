using UnityEngine;
using System.Collections;
using MyUtility;

/// <summary>
/// 小路のクラス
/// プレイヤーが乗ると次の部屋に移動する
/// </summary>
public class PathSquare : AbstractSquare
{
    public Direction direction = Direction.down;

    public MyVector2 nextSequence;

    //コンストラクタ
    public void Start() { type = Type.Path; }

    public void setPathSquare(int row, int column, int adjustX, int adjustY, Direction a)
    {
        sequence = new MyVector2(row, column);
        nextSequence = new MyVector2(row + adjustX, column + adjustY);
        direction = a;
    }

    public override void enterThis()
    {
        if (isCharacterOn(AbstractCharacter.Type.Player))
        {
            StartCoroutine(enterThisCoroutine());
        }
    }

    IEnumerator enterThisCoroutine()
    {
        FadeManager.Instance.LoadLevel(1, () => pathEvent());
        yield return null;
    }
    public void pathEvent()
    {
        DungeonManager.Instance.destroyPrevious(sequence);
        DungeonManager.Instance.createNext(nextSequence);
        DungeonManager.Instance.randomizeToSquare(nextSequence, ObjectManager.Instance.character[0]);
        TurnManager.Instance.turnCharacter = 0;
    }
}
