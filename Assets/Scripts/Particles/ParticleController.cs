using UnityEngine;
using System.Collections;

public class ParticleController : MonoBehaviour {

    public GameObject[] Particles;

    public void ParticleItRandomly(Vector3 position)
    {
        Debug.Log(Particles.Length);
        int random = Random.Range(0, Particles.Length);
        Instantiate(Particles[random], position + (Vector3.up * 1.25f), Quaternion.identity);
    }
}
