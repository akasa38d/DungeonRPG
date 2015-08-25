using UnityEngine;
using System.Collections;

public class LockButton : MonoBehaviour {

	bool onClick;
	public bool OnClick
	{
		set 
		{
			if(value == true)
			{
				this.gameObject.GetComponent<Animator>().SetTrigger("isLock");
			}
			if(value == false)
			{
				this.gameObject.GetComponent<Animator>().ResetTrigger("isLock");
			}
		}
	}
}
