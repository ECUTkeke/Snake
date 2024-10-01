using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip eatAppleClip;
    [SerializeField] private AudioClip failedClip;

    private void OnEnable() {
        SnakeModel.OnEatApple += Snake_OnEatApple;
        SnakeModel.OnFailed += Snake_OnFailed;
    } 

    private void OnDisable() {
        SnakeModel.OnEatApple -= Snake_OnEatApple;     
        SnakeModel.OnFailed -= Snake_OnFailed;
    }

    private void PlaySound(AudioClip audioClip){
        var obj = new GameObject("Audio");
        var audioSource = obj.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
        Destroy(obj, audioClip.length);
    }

    private void Snake_OnEatApple()
    {
        PlaySound(eatAppleClip);
    }
    private void Snake_OnFailed()
    {
        PlaySound(failedClip);
    }
}
