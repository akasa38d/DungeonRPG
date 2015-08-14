﻿using UnityEngine;
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
		for (int i=0; i < 6; i++){
			deckCard.Add(new SwordItem()); 
		}
		for (int i=0; i < 6; i++){
			deckCard.Add(new FlowerItem()); 
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

	public void shuffule(int handNumber)
	{
		StartCoroutine ("shuffulCoroutine",handNumber);
	}

	IEnumerator shuffulCoroutine(int handNumber)
	{
		if(trashCard.Count() == 0)
		{
			Debug.Log("なんてことだぜ！");
		}
		
		if(trashCard.Count() != 0)
		{
			Debug.Log("仕切り直しだぜ！");

			foreach (var n in trashCard)
			{
				deckCard.Add(n);
				deckCount.GetComponent<Text> ().text = deckCard.Count ().ToString();
				yield return null;
				trashCard.Remove(n);
				trashCount.GetComponent<Text> ().text = trashCard.Count ().ToString();
				yield return null;
			}
			yield return null;

			draw (handNumber);
		}
		yield return null;
	}

	public void use(int handNumber)
	{
		Debug.Log ("このカードを使うぜ！");
		
		if (handCard [handNumber].id == 0)
		{
			Debug.Log ("カードがないぜ！");
		}
		
		if (handCard [handNumber].id != 0)
		{			
			usingNumber = handNumber;
			
			handCard[handNumber].buttonEvent();			
		}
	}



	public void draw (int handNumber)
	{
		if (deckCard.Count == 0)
		{
			shuffule (handNumber);
		}

		if (deckCard.Count != 0)
		{
			Debug.Log ("ドローするぜ！");
			var n = deckCard.ElementAt (Random.Range (0, deckCard.Count ()));
			handCard [handNumber] = n;
			deckCard.Remove(n);
			deckCount.GetComponent<Text> ().text = deckCard.Count ().ToString();

			var image = cardUI[handNumber].GetComponent<Image> ();
			image.sprite = handCard [handNumber].sprite;
		}


	}


	public void preTurnEnd()
	{
		Debug.Log ("終了前だぜ！");
		if (usingNumber != -1) {
			//使用済みカードに追加
			usedCard.Add (handCard [usingNumber]);
			usedCount.GetComponent<Text> ().text = usedCard.Count ().ToString ();
		
			//使用したカードをnullカードに変更
			handCard [usingNumber] = new NullItem ();
			var image = cardUI [usingNumber].GetComponent<Image> ();
			image.sprite = handCard [usingNumber].sprite;

			usingNumber = -1;
		}
	}

	public void turnEnd()
	{
		StartCoroutine ("turnEndCoroutine");
	}

	IEnumerator turnEndCoroutine()
	{
		Debug.Log ("使用したカードを墓地に送るぜ！");
		//使用したカードをトラッシュに送る
		trashCard.AddRange (usedCard);
		trashCount.GetComponent<Text> ().text = trashCard.Count ().ToString();
		yield return null;

		//使用したカードをクリア
		usedCard.Clear ();
		usedCount.GetComponent<Text> ().text = usedCard.Count ().ToString();
		yield return null;

		for (int i=0; i<6; i++)
		{
			if(handCard[i].id == 0)
			{
				Debug.Log ("補充だぜ！");
				draw (i);
				yield return null;
			}
		}
	}
}
