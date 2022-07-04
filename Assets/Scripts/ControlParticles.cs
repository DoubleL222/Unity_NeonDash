using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlParticles : MonoBehaviour
{

    private ParticleSystem ps;
    public float psStartSize = 1.0f;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        var main = ps.main;
        main.startSize = psStartSize;
    }

}
