using UnityEngine;

public class coin : MonoBehaviour
{
    public GameObject particles;
    AudioSource myAudioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        myAudioSource.Play();
        GameObject newP = Instantiate(particles, collision.contacts[0].point, Quaternion.identity);
    }
}
