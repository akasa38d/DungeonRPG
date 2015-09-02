using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OpenButton : MonoBehaviour {
	
    [SerializeField]
    Text buttonText;
    [SerializeField]
    GameObject bottomGroup;

    bool onClick;
	public bool OnClick
	{
		set 
		{
            Debug.Log(value);
			if(value == true)
			{
				bottomGroup.GetComponent<Animator>().ResetTrigger("isOpen");
                buttonText.text = "OPEN";
			}
			if(value == false)
			{
				bottomGroup.GetComponent<Animator>().SetTrigger("isOpen");
                buttonText.text = "CLOSE";
			}
		}
	}
}
