using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractCharacterObject : MonoBehaviour
{
    //ターン進行
    public enum Process { Start, Main, End, Next };
    public Process process;

    public int id;

    //処理の流れ
    virtual public void operation()
    {
        switch (this.process)
        {
            //スタンバイフェイズ
            case Process.Start:
                this.startOperation();
                break;
            //メインフェイズ
            case Process.Main:
                this.mainOperation();
                break;
            //エンドフェイズ
            case Process.End:
                this.endOperation();
                break;
            case Process.Next:
                this.nextOperation();
                break;
            //例外
            default:
                this.endOperation();
                break;
        }
    }

    //スタンバイフェイズ処理
    virtual protected void startOperation() { }

    virtual protected void getAroundCharacter(List<GameObject> list) { }

    //メインフェイズ処理
    virtual protected void mainOperation() { }

    //エンドフェイズ処理
    virtual protected void endOperation() { }

    //仮
    virtual protected void nextOperation() { }

}
