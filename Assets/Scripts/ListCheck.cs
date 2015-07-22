using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ListCheck : MonoBehaviour {

	[SerializeField]
	public Text uiText;

	// Update is called once per frame
	void Update()
	{
		uiText.text = ObjectManager.Instance.square.Count.ToString();
	}
}
