using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class ItemManager : MonoBehaviour {
	//手札
	public Item[] handCard = new Item[6];
	//デッキ
	public List<Item> deckCard = new List<Item> ();
	//墓地
	public List<Item> trashCard = new List<Item> ();
	//仕様済み
	public List<Item> usedCard = new List<Item> ();


	//手札のUI
	public GameObject[] cardUI = new GameObject[6];
	//カウント
	public GameObject usedCount;
	public GameObject trashCount;
	public GameObject deckCount;

	public void Start()
	{
		for (int i=0; i < 20; i++){
			deckCard.Add(new SwordItem()); 
		}

		draw(0);
		draw(1);
		draw(2);
		draw(3);
		draw(4);
		draw(5);

		usedCount.GetComponent<Text> ().text = usedCard.Count ().ToString();
		trashCount.GetComponent<Text> ().text = trashCard.Count ().ToString();
		deckCount.GetComponent<Text> ().text = deckCard.Count ().ToString();
	}

	public void test()
	{
	}


	public void shuffule()
	{
		if(trashCard.Count() != 0)
		{
			Debug.Log("仕切り直しだぜ！");
			foreach (var n in trashCard)
			{
				deckCard.Add(n);
				deckCount.GetComponent<Text> ().text = deckCard.Count ().ToString();
			}
			trashCard.Clear();
			trashCount.GetComponent<Text> ().text = trashCard.Count ().ToString();
		}
		if(trashCard.Count() != 0)
		{
			Debug.Log("なんてことだぜ！");
		}
	}

	public void use(int handNumber)
	{
		Debug.Log ("このカードを使うぜ！");
		if (handCard [handNumber] != null)
		{
			usedCard.Add (handCard [handNumber]);
			usedCount.GetComponent<Text> ().text = usedCard.Count ().ToString ();
			draw (handNumber);
		}
		if (handCard [handNumber] == null)
		{
			Debug.Log ("使えなかったから代わりのカードを引くぜ！");
			draw (handNumber);
		}
	}

	public void draw (int handNumber)
	{
		if (deckCard.Count () != 0)
		{
			Debug.Log ("ドローするぜ！");
			var n = deckCard.ElementAt (Random.Range (0, deckCard.Count ()));
			handCard [handNumber] = n;
			deckCard.Remove(n);
			deckCount.GetComponent<Text> ().text = deckCard.Count ().ToString();

			var image = cardUI[handNumber].GetComponent<Image> ();
			image.sprite = n.sprite;
		}

		if (deckCard.Count () == 0)
		{
			Debug.Log ("ドローできないんだぜ！");
			handCard [handNumber] = null;
			shuffule ();
		}
	}

	public void turnEnd()
	{
		foreach (var n in usedCard)
		{
			trashCard.Add (n);
			trashCount.GetComponent<Text> ().text = trashCard.Count ().ToString();
		}
		usedCard.Clear ();
		usedCount.GetComponent<Text> ().text = usedCard.Count ().ToString();

		for (int i=0; i<6; i++)
		{
			if(handCard[i] == null)
			{
				Debug.Log ("補充だぜ！");
				draw (i);
			}
		}
	}
}
