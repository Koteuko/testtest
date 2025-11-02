using UnityEngine;

public class FireworkTrigger : MonoBehaviour
{
    public ParticleSystem[] fireworksEffect;
    [SerializeField] private GameObject text;

    private void OnTriggerEnter(Collider other)
    {
        foreach (var ps in fireworksEffect)
            ps.Play();
        
        text.SetActive(true);
    }
}