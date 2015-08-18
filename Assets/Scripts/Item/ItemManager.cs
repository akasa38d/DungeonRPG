using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class ItemManager : MonoBehaviour
{
    //手札
    public Item[] handCard = new Item[6];
    //デッキ
    public List<Item> deckCard = new List<Item>();
    //墓地
    public List<Item> trashCard = new List<Item>();
    //仕様済み
    public List<Item> usedCard = new List<Item>();

    public int usingNumber = -1;

    //手札のUI
    public GameObject[] cardUI = new GameObject[6];
    public GameObject moveUI;

    //カウント
    public GameObject usedCount;
    public GameObject trashCount;
    public GameObject deckCount;

    //準備中
    public bool inProcess = false;

    public void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            deckCard.Add(new SwordItem());
        }
        for (int i = 0; i < 6; i++)
        {
            deckCard.Add(new FlowerItem());
        }
        for (int i = 0; i < 2; i++)
        {
            deckCard.Add(new BombItem());
        }

        draw(0);
        draw(1);
        draw(2);
        draw(3);
        draw(4);
        setCard(5, new NullItem());

        usedCount.GetComponent<Text>().text = usedCard.Count().ToString();
        trashCount.GetComponent<Text>().text = trashCard.Count().ToString();
        deckCount.GetComponent<Text>().text = deckCard.Count().ToString();
    }

    public void shuffule(int handNumber)
    {
        StartCoroutine("shuffulCoroutine", handNumber);
    }

    IEnumerator shuffulCoroutine(int handNumber)
    {
        if (trashCard.Count() == 0)
        {
            Debug.Log("なんてことだぜ！");
        }

        if (trashCard.Count() != 0)
        {
            Debug.Log("仕切り直しだぜ！");

            deckCard.AddRange(trashCard);
            deckCount.GetComponent<Text>().text = deckCard.Count().ToString();
            yield return null;

            trashCard.Clear();
            trashCount.GetComponent<Text>().text = trashCard.Count().ToString();
            yield return null;
        }
        yield return null;
    }


	//カードの選択
	public bool[] isSelect = new bool[6] {false,false,false,false,false,false};
	public void select(int handNumber)
	{
		if (handCard[handNumber].id == 0)
		{
			Debug.Log("カードがないぜ！");
		}

		if (handCard [handNumber].id != 0) {
			if (!isSelect [handNumber]) {
				use (handNumber);
				return;
			}
			if (isSelect [handNumber]) {
				change (handNumber);
				return;
			}
		}
	}

    void use(int handNumber)
    {
		switchingSelect (handNumber, true);

        Debug.Log("このカードを使うぜ！");
        
        usingNumber = handNumber;

        handCard[handNumber].buttonEvent();
    }

	void change(int handNumber)
	{
		switchingSelect (handNumber, false);

		usingNumber = handNumber;

		handCard [handNumber].changeOperation ();
	}



    public void draw(int handNumber)
    {
        StartCoroutine("drawCoroutine", handNumber);
    }

    IEnumerator drawCoroutine(int handNumber)
    {
        if (deckCard.Count == 0)
        {
            shuffule(handNumber);
        }
        yield return null;

        Debug.Log("ドローするぜ！");
        var n = deckCard.ElementAt(Random.Range(0, deckCard.Count()));
        handCard[handNumber] = n;
        deckCard.Remove(n);
        deckCount.GetComponent<Text>().text = deckCard.Count().ToString();

        var image = cardUI[handNumber].GetComponent<Image>();
        image.sprite = handCard[handNumber].sprite;
        yield return null;
    }


    public void preTurnEnd()
    {
        Debug.Log("終了前だぜ！");
        if (usingNumber != -1)
        {
            //使用済みカードに追加
            usedCard.Add(handCard[usingNumber]);
            usedCount.GetComponent<Text>().text = usedCard.Count().ToString();

            //使用したカードをnullカードに変更
            setCard(usingNumber, new NullItem());

            usingNumber = -1;
        }
    }

    public void turnEnd()
    {
        Debug.Log("使用したカードを墓地に送るぜ！");
        //使用したカードをトラッシュに送る
        trashCard.AddRange(usedCard);
        trashCount.GetComponent<Text>().text = trashCard.Count().ToString();

        //使用したカードをクリア
        usedCard.Clear();
        usedCount.GetComponent<Text>().text = usedCard.Count().ToString();

		for (int i = 0; i < 6; i ++)
		{
			switchingSelect(i, false);
		}

        for (int i = 0; i < 5; i++)
        {
            if (handCard[i].id == 0)
            {
                Debug.Log("補充だぜ！");
                draw(i);
            }
        }
    }

    public void Update()
    {
        if (this.gameObject.GetComponent<PlayerObject>().process != AbstractCharacterObject.Process.Main)
        {
            foreach (var n in cardUI)
            {
				n.GetComponent<Button>().interactable = false;
            }
        }

        if (this.gameObject.GetComponent<PlayerObject>().process == AbstractCharacterObject.Process.Main)
        {
            for (int i = 0; i < 6; i++)
            {
                cardUI[i].GetComponent<Button>().interactable = true;
            }
        }
    }

    //カードの取得
    public void setCard(int handNumber, Item item)
    {
        handCard[handNumber] = item;
        var image = cardUI[handNumber].GetComponent<Image>();
        image.sprite = handCard[handNumber].sprite;
    }

	public void switchingSelect(int number, bool flag)
	{
		isSelect[number] = flag;
		var child = cardUI[number].transform.FindChild("change");
		child.GetComponent<Text> ().enabled = flag;
	}
	public void ResetSelect(int handNumber)
	{
		switchingSelect(handNumber, false);
	}
}
