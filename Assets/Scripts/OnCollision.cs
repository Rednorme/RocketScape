using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class OnCollision : MonoBehaviour
{
    bool isTransitioning = false;
    [SerializeField] AudioClip crashExplosion;
    [SerializeField] AudioClip finishPlatform;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem finishParticles;
    [SerializeField] float respawnTime = 2f;
    [SerializeField] float levelLoadDelay = 1.5f;
    AudioSource audioSource;
    
    void Start() 
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (isTransitioning) { return; }
        
        switch (other.gameObject.tag)
        {
            case "Finish":
                StartSuccessSequence();
                break;
            case "Friendly":
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void LoadNextLevel()
    { 
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
        isTransitioning = false;
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isTransitioning = false;
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(finishPlatform, 0.2F);
        finishParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(crashExplosion, 0.4F);
        crashParticles.Play();
        Invoke("ReloadLevel", respawnTime);
    }
}
