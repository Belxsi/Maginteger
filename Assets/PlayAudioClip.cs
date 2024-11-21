using UnityEngine;

public class PlayAudioClip : MonoBehaviour
{
    public bool play;
    public AudioSource source;

    // Update is called once per frame
    void Update()
    {
        if (play)
        {
            play = false;
            source.Play();
        }
    }
}
