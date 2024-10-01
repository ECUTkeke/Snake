using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip eatAppleClip;
    [SerializeField] private AudioClip failedClip;

    private void OnEnable() {
        SnakeController.OnEatApple += Snake_OnEatApple;
        SnakeController.OnGameOver += Snake_OnFailed;
    } 

    private void OnDisable() {
        SnakeController.OnEatApple -= Snake_OnEatApple;     
        SnakeController.OnGameOver -= Snake_OnFailed;
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
