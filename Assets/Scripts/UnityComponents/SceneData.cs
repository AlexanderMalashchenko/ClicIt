using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{
    public GameObject BallPrefab;
    public Camera Camera;
    public ParticleSystem Explosion;
    public float CurrentHealth;

    public void ExplosionInstantiate(Vector3 position, Color color)
    {

        var Expl = Instantiate(Explosion, position, Quaternion.identity);
      
        ParticleSystem.MainModule main = Expl.main;
        main.startColor = color;

    }


}
