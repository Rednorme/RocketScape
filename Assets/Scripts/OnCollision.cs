using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class OnCollision : MonoBehaviour
{
    bool isTransitioning = false;

    bool collisionDisabled = false;

    AudioSource audioSource;
    [SerializeField] AudioClip crashExplosion;
    [SerializeField] AudioClip finishPlatform;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem finishParticles;

    [SerializeField] float respawnTime = 2f;
    [SerializeField] float levelLoadDelay = 1.5f;
    
    void Start() 
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() 
    {
        CheatDebugKeys();
    }

    void OnCollisionEnter(Collision other) 
    {
        if (isTransitioning || collisionDisabled) { return; }
        
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

    public void LoadNextLevel()
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

    void CheatDebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            LoadNextLevel();
        }

        if (Input.GetKey(KeyCode.C))
        {
            if (!collisionDisabled)
            {
                collisionDisabled = true;
            }
            else
            {
                collisionDisabled = false;
            }
        }
    }
}
