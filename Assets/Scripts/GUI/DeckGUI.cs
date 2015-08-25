using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeckGUI : MonoBehaviour {

    [SerializeField]
    GameObject player;
    ItemManager sPlayer;

    [SerializeField]
    GameObject usedGUI;
    Text tUsedGUI;

    [SerializeField]
    GameObject trashGUI;
    Text tTrashGUI;

    [SerializeField]
    GameObject deckGUI;
    Text tDeckGUI;

	// Use this for initialization
	void Start ()
    {
        sPlayer = player.GetComponent<ItemManager>();
        tUsedGUI = usedGUI.GetComponent<Text>();
        tTrashGUI = trashGUI.GetComponent<Text>();
        tDeckGUI = deckGUI.GetComponent<Text>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
        tUsedGUI.text = sPlayer.usedCard.Count.ToString();
        tTrashGUI.text = sPlayer.trashCard.Count.ToString();
        tDeckGUI.text = sPlayer.deckCard.Count.ToString(); 
	}
}
