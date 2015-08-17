using UnityEngine;
using System.Collections;

public class FieldItem : MonoBehaviour
{
    //取得するアイテム
    public Item item;
    //場所
    public Vector3 position;

    public FieldItem(Item item)
    {
        this.item = item;
    }
    public FieldItem(Item item, Vector3 vector3)
    {
        this.item = item;
        this.position = vector3;
    }

    public void operation()
    {
        Debug.Log(this.item + "を入手");
        var player = ObjectManager.Instance.character[0];
        player.GetComponent<ItemManager>().setCard(5, this.item);
        Destroy(this.gameObject);
    }
}
