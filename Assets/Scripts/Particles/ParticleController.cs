using UnityEngine;
using System.Collections;

public class ParticleController : MonoBehaviour {

    public GameObject[] ParticleEffects;
    public static ParticleController Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void ParticleItRandomly(Vector3 position)
    {
        int random = Random.Range(0, ParticleEffects.Length);
        Instantiate(ParticleEffects[random], position + (Vector3.up * 1.25f), Quaternion.identity);
    }
}
