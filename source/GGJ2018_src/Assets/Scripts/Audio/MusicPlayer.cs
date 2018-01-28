using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    // I'm sortof a singleton
    public static MusicPlayer Current;

    AudioSource audioSource;

    public List<AudioClip> menuMusic = new List<AudioClip>();
    public List<AudioClip> levelMusic = new List<AudioClip>();

    

    void Awake()
    {
        if (Current)
        {
            // there's already a music player going on
            Destroy(gameObject);
            return;
        }

        Current = this;
        GameObject.DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        CheckOrStartMusic();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CheckOrStartMusic()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene != null)
        {
            string sceneNameLower = currentScene.name.ToLower();
            if (sceneNameLower.Equals("mainmenu") || sceneNameLower.Equals("levelselect") ||
                sceneNameLower.Equals("intro")
                )
            {
                SetMusic(menuMusic);
            }
            else
            {
                SetMusic(levelMusic);
            }
        }
    }

    public void OnLevelWasLoaded(int level)
    {
        CheckOrStartMusic();
    }

    void SetMusic(List<AudioClip> i_music)
    {
        AudioClip newClip = i_music[Random.Range(0, i_music.Count)];

        if (audioSource && newClip != audioSource.clip)
        {
            audioSource.clip = newClip;

            audioSource.Play();
        }
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutRoutine(audioSource, 3f));
    }

    private IEnumerator FadeOutRoutine(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    private IEnumerator FadeInRoutine(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume < 1)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = 1;
    }
}
