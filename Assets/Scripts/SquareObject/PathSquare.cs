using UnityEngine;
using System.Collections;

public class PathSquare : AbstractSquare
{
    public DungeonManager.Room room;

    public enum Direction { up, down, left, right }
    public Direction direction = Direction.down;

    [SerializeField]
    Position nextPosition;

    public PathSquare()
    {
        type = Type.Path;
    }

    public void setPathSquare(int row, int column, int adjustX, int adjustY, Direction a)
    {
        position = Position.createPosition(row, column);
		nextPosition = Position.createPosition(row + adjustX, column + adjustY);
        direction = a;
    }

    public override void enterThis()
    {
		StartCoroutine (enterThisCoroutine());
    }

	IEnumerator enterThisCoroutine()
	{
		var floor = DungeonManager.Floor.Instance;
		
		floor.inPreparation = true;
		
		FadeManager.Instance.LoadLevel(1);
		Debug.Log(direction + "に移動！");
		floor.destroyPrevious(position.row, position.column);
		floor.createNext(nextPosition.row, nextPosition.column);
		
		floor.randomizeToSquare(nextPosition.row, nextPosition.column,DungeonManager.Instance.player);
		TurnManager.Instance.turnCount = 0;
		floor.inPreparation = false;
		yield return null;
	}



}
