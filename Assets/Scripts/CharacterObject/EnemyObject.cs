using UnityEngine;
using System.Collections;

public class EnemyObject : AbstractCharacterObject
{
    AttackWay myAttack;

    public void Start()
    {
        this.type = Type.Enemy;
        myAttack = new AttackWay("キック", 30, null);
        Debug.Log("名前は" + this.parameter.cName);
        Debug.Log("IDは" + this.parameter.id);
        Debug.Log("攻撃手段：" + myAttack.name);
    }

    //基本処理
    public override void operation() { base.operation(); }

    //スタンバイフェイズ
    protected override void startOperation()
    {
        ObjectManager.Instance.setCharacter();
        ObjectManager.Instance.setSquare();
        base.startOperation();
    }

    //メインフェイズ
    protected override void mainOperation()
    {
        searchTarget();
    }

    //プリエンドフェイズ
    protected override void preEndOperation() { base.preEndOperation(); }

    //エンドフェイズ
    protected override void endOperation() { base.endOperation(); }

    protected override void nextOperation() { base.nextOperation(); }


    //ターゲットを探して行動（メインフェイズ）
    void searchTarget()
    {
        var target = ObjectManager.Instance.character[0];

        if (checkOneDistance(this.gameObject, target))
        {
            attackToTarget(target);
        }
        else
        {
            moveTowardTarget(target);
        }
    }

    //攻撃（仮）
    void attackToTarget(GameObject target)
    {
        base.attackTarget(myAttack, target);
    }

    //移動
    void moveTowardTarget(GameObject target)
    {
        int x = 0;
        int y = 0;

        int tempX = (int)target.transform.position.x - (int)this.transform.position.x;

        int tempY = (int)target.transform.position.z - (int)this.transform.position.z;

        if (tempX != 0) { x = tempX / Mathf.Abs(tempX) * 10; }

        if (tempY != 0) { y = tempY / Mathf.Abs(tempY) * 10; }

        foreach (GameObject obj in ObjectManager.Instance.square)
        {
            if (obj.transform.position.x == this.transform.position.x + x
                && obj.transform.position.z == this.transform.position.z + y)
            {
                if (!obj.GetComponent<AbstractSquare>().isCharacterOn())
                {
                    movePosition(obj);
                    break;
                }


                //ゴリ押し
                if (obj.GetComponent<AbstractSquare>().isCharacterOn())
                {

                    foreach (GameObject obj2 in ObjectManager.Instance.square)
                    {
                        if (obj2.transform.position.x == this.transform.position.x
                            && obj2.transform.position.z == this.transform.position.z + y)
                        {
                            if (!obj2.GetComponent<AbstractSquare>().isCharacterOn())
                            {
                                movePosition(obj2);
                                break;
                            }
                        }
                        if (obj2.transform.position.x == this.transform.position.x + x
                            && obj2.transform.position.z == this.transform.position.z)
                        {
                            if (!obj2.GetComponent<AbstractSquare>().isCharacterOn())
                            {
                                movePosition(obj2);
                                break;
                            }
                        }
                    }
                }


            }

        }



        process = Process.End;
    }
}
