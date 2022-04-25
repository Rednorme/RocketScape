using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float thrustSpeed = 1;
    [SerializeField] float rotationValue = 1;
    Rigidbody rigidBody;
    Transform transformVariable;
    AudioSource m_MyAudioSource;
    [SerializeField] AudioClip mainEngine;
    
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;
    [SerializeField] ParticleSystem rocketJetParticles;
    bool m_Play;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        transformVariable = GetComponent<Transform>();
        m_MyAudioSource = GetComponent<AudioSource>();
        m_Play = true;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            PlayAudio();
            ApplyThrust(thrustSpeed);
            if (!rocketJetParticles.isPlaying)
            {
                rocketJetParticles.Play();
            }
        }
        else
        {
            m_MyAudioSource.Stop();
            rocketJetParticles.Stop();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationValue);
            
            if (!leftThrusterParticles.isPlaying)
            {
                leftThrusterParticles.Play();
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationValue);

            if (!rightThrusterParticles.isPlaying)
            {
                rightThrusterParticles.Play();
            }
        }
        else
        {
            leftThrusterParticles.Stop();
            rightThrusterParticles.Stop();
        }
    }

    void ApplyThrust(float speedOfThrust)
    {
        rigidBody.AddRelativeForce(Vector3.up * speedOfThrust * Time.deltaTime);
    }

    void ApplyRotation(float rotationSpeed)
    {
        rigidBody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        rigidBody.freezeRotation = false;
    }
    
    void PlayAudio()
    {
        if (!m_MyAudioSource.isPlaying)
        {
            m_MyAudioSource.PlayOneShot(mainEngine);
        }
    }
}
