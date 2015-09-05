using UnityEngine;
using System.Collections;

public class DestroyParticle : MonoBehaviour {

    ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

	// Update is called once per frame
	void Update ()
    {
        if (particleSystem.duration == 0)
        {
            Destroy(this.gameObject);
        }	
	}
}
