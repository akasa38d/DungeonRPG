using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    Player player;

    public Item[] handCard = new Item[6];
	public int handNum()
	{
		Debug.Log (handCard[5].name);
		if (handCard [5].id == 0)
		{
			return 5;
		}
		return 6;
	}
    //デッキccc
    public List<Item> deckCard = new List<Item>();
    //墓地
    public List<Item> trashCard = new List<Item>();
    //仕様済み
    public List<Item> usedCard = new List<Item>();

    public int usingNumber = -1;

    //手札のUI
    public GameObject[] cardUI = new GameObject[6];
    public GameObject moveUI;

    //準備中
    public bool inProcess = false;

    //*****discription関係*****//
    [SerializeField]
    GameObject discription;

    [SerializeField]
    GameObject cardImage;

    [SerializeField]
    Text cardText;

    public void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            deckCard.Add(Item.getItemData(1));  //knife
        }
        for (int i = 0; i < 2; i++)
        {
            deckCard.Add(Item.getItemData(11)); //hatchet
        }
        for (int i = 0; i < 2; i++)
        {
            deckCard.Add(Item.getItemData(41)); //bomb
        }
		for (int i = 0; i < 4; i++)
		{
            deckCard.Add(Item.getItemData(61)); //flower
		}
        deckCard.Add(Item.getItemData(52)); //bread

        draw(0);
        draw(1);
        draw(2);
        draw(3);
        draw(4);
        setCard(5, new NullItem());
    }

    public void shuffule(int handNumber)
    {
        StartCoroutine("shuffulCoroutine", handNumber);
    }

    IEnumerator shuffulCoroutine(int handNumber)
    {
        if (trashCard.Count() != 0)
        {
            deckCard.AddRange(trashCard);
            yield return null;

            trashCard.Clear();
            yield return null;
        }
        yield return null;
    }


	//カードの選択
	public bool[] isSelect = new bool[6] {false,false,false,false,false,false};
	public void select(int handNumber)
	{
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
        if (deckCard.Count == 0) { shuffule(handNumber); }
        yield return null;

        var n = deckCard.ElementAt(Random.Range(0, deckCard.Count()));
        handCard[handNumber] = n;
        deckCard.Remove(n);

        var image = cardUI[handNumber].GetComponent<Image>();
        image.sprite = handCard[handNumber].sprite;
        yield return null;
    }

    public void preTurnEnd()
    {
        closeDiscription();

        if (usingNumber != -1)
        {
            if (handCard [usingNumber].chain)
            {
                player.additionalTurn++;
            }

            //使用済みカードに追加
            usedCard.Add(handCard[usingNumber]);
  
            //使用したカードをnullカードに変更
            setCard(usingNumber, new NullItem());

            usingNumber = -1;
        }
    }

    public void turnEnd()
    {
		StartCoroutine ("turnEndCoroutine");
    }

	IEnumerator turnEndCoroutine()
	{
		//使用したカードをトラッシュに送る
		for (int i = 0; i < usedCard.Count(); i++)
        {
            Debug.Log(usedCard [i].expendable);
            if (usedCard [i].expendable == true && usedCard [i].changed == false)
            {
                usedCard.Remove(usedCard [i]);
            }
            else
            {
                usedCard [i].changed = false;
            }
        }
		yield return null;

		trashCard.AddRange(usedCard);
		yield return null;

		usedCard.Clear();
		yield return null;

		for (int i = 0; i < 6; i ++)
		{
			switchingSelect(i, false);
		}
		yield return null;
		
		for (int i = 0; i < 5; i++)
		{
			if (handCard[i].id == 0)
			{
				draw(i);
			}
		}
		yield return null;
	}

    public void Update()
    {
        if (this.gameObject.GetComponent<Player>().process == AbstractCharacter.Process.Main)
        {
            foreach (var n in cardUI)
            {
                n.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            foreach (var n in cardUI)
            {
                n.GetComponent<Button>().interactable = false;
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

    //unity上で設定
	public void ResetSelect(int handNumber)
	{
		switchingSelect(handNumber, false);
	}

    //unity上で設定
    public void openDiscription(int handNumber)
    {
        if (handCard [handNumber].id != 0)
        {
            discription.GetComponent<Image>().enabled = true;
            cardImage.GetComponent<Image>().enabled = true;
            cardImage.GetComponent<Image>().sprite = handCard [handNumber].sprite;
            cardText.GetComponent<Text>().enabled = true;
            cardText.text = handCard [handNumber].name + "\n" + handCard [handNumber].text;
        }
    }

    //unity上で設定・ターン終了時にも使用
    public void closeDiscription()
    {
        discription.GetComponent<Image>().enabled = false;
        cardImage.GetComponent<Image>().enabled = false;
        cardText.text = "";
        cardText.GetComponent<Text>().enabled = false;
    }
}
