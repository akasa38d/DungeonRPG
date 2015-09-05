using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]

public class DestroyedAfterParticleSystem : MonoBehaviour
{
    void Start()
    {
        var particleSystem = GetComponent<ParticleSystem>();
        Destroy(this.gameObject, particleSystem.duration);
    }
}