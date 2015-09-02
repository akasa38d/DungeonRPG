using UnityEngine;
using UnityEngine.UI;

public class StatusGUI : MonoBehaviour {

	[SerializeField]
    GameObject player;
    Player sPlayer;

	[SerializeField]
	GameObject lvGUI;
    Text tLvGUI;

	[SerializeField]
	GameObject hpGUI;
    Text tHpGUI;

	[SerializeField]
	GameObject spGUI;
    Text tSpGUI;

    void Start()
    {
        sPlayer = player.GetComponent<Player>();
        tLvGUI = lvGUI.GetComponent<Text>();
        tHpGUI = hpGUI.GetComponent<Text>();
        tSpGUI = spGUI.GetComponent<Text>();
    }

	void Update()
	{
        tLvGUI.text = sPlayer.parameter.lv.ToString();
        tHpGUI.text = sPlayer.parameter.hp.ToString() + " / " + sPlayer.parameter.maxHp.ToString();
        tSpGUI.text = sPlayer.parameter.sp.ToString("000") + " / " + sPlayer.parameter.maxSp.ToString();
	}
}
