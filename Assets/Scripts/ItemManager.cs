using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour {

	public enum State{hand, used, trash, deck}
	State state;

	public Dictionary<GameObject, State> itemDictionary = new Dictionary<GameObject, State>();

}
