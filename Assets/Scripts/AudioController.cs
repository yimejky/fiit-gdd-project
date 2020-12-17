using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip jumpSound;
    public AudioClip hitSound;

    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
    }

    public void PlayJump()
    {
        source.clip = jumpSound;
        source.Play();
    }

    public void PlayHit()
    {
        source.clip = hitSound;
        source.Play();
    }

}
