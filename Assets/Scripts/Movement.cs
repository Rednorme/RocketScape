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

    // Start is called before the first frame update
    void Start()
    {
        GetComponents();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void GetComponents()
    {
        rigidBody = GetComponent<Rigidbody>();
        transformVariable = GetComponent<Transform>();
        m_MyAudioSource = GetComponent<AudioSource>();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    void StartThrusting()
    {
        ApplyThrust(thrustSpeed);
        PlayEngineAudio();
        if (!rocketJetParticles.isPlaying)
        {
            rocketJetParticles.Play();
        }
    }

    void StopThrusting()
    {
        m_MyAudioSource.Stop();
        rocketJetParticles.Stop();
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }
    }

    void RotateLeft()
    {
        ApplyRotation(rotationValue);

        if (!leftThrusterParticles.isPlaying)
        {
            leftThrusterParticles.Play();
        }
    }

    void RotateRight()
    {
        ApplyRotation(-rotationValue);

        if (!rightThrusterParticles.isPlaying)
        {
            rightThrusterParticles.Play();
        }
    }

    void StopRotating()
    {
        leftThrusterParticles.Stop();
        rightThrusterParticles.Stop();
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
    
    void PlayEngineAudio()
    {
        if (!m_MyAudioSource.isPlaying)
        {
            m_MyAudioSource.PlayOneShot(mainEngine);
        }
    }
}
