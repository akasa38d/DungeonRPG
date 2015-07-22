using UnityEngine;
using System.Collections;

public class DrawPosition : MonoBehaviour
{
    public GameObject Cube;

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<GUIText>().text = " X : " + Cube.transform.position.x / 10 + " Y : " + Cube.transform.position.z / -10;
    }
}
