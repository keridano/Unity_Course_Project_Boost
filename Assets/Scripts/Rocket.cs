﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float levelLoadDelay = 3f;

    [SerializeField] AudioClip rocketThrust;
    [SerializeField] AudioClip collisionSound;
    [SerializeField] AudioClip victorySound;

    [SerializeField] ParticleSystem rocketThrustParticles;
    [SerializeField] ParticleSystem collisionParticles;
    [SerializeField] ParticleSystem victoryParticles;


    AudioSource audioSource;
    Rigidbody rigidBody;

    bool isTransitioning;

    private bool collisionsOff;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        isTransitioning = false;
    }

    // Update is called once per frame
    void Update()
    {
        DebugMode();

        if (!isTransitioning)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionsOff) return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //Do nothing
                break;

            case "Finish":
                StartSuccessSequence();
                break;

            default:
                StartDeathSequence();
                break;
        }
    }


    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
            ApplyThrust();
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
            rocketThrustParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
            StopSoundAndPlayOneShot(rocketThrust);
        rocketThrustParticles.Play();
    }

    private void RespondToRotateInput()
    {
        rigidBody.angularVelocity = Vector3.zero; //reset angular velocity due to physics

        var rotateThisFrame = rcsThrust * Time.deltaTime;
        var fwdRotateThisFrame = Vector3.forward * rotateThisFrame;

        if (Input.GetKey(KeyCode.D))
            transform.Rotate(-fwdRotateThisFrame);
        else if (Input.GetKey(KeyCode.A))
            transform.Rotate(fwdRotateThisFrame);
    }

    private void StartSuccessSequence()
    {
        isTransitioning = true;
        StopSoundAndPlayOneShot(victorySound);
        StartVictoryParticles();
        Invoke("HitFinish", levelLoadDelay);
    }

    private void StartVictoryParticles()
    {
        rocketThrustParticles.Stop();
        victoryParticles.Play();
    }

    private void StartDeathSequence()
    {
        isTransitioning = true;
        StopSoundAndPlayOneShot(collisionSound);
        StartCollisionParticles();
        Invoke("HitDeadlyObject", 1f);
    }

    private void StartCollisionParticles()
    {
        rocketThrustParticles.Stop();
        collisionParticles.Play();
    }

    private void StopSoundAndPlayOneShot(AudioClip audioClip)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(audioClip);
    }

#pragma warning disable IDE0051

    private void HitDeadlyObject()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void HitFinish()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int maxIndex = SceneManager.sceneCountInBuildSettings - 1;

        if (currentIndex < maxIndex)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(0);
    }

#pragma warning restore IDE0051

    private void DebugMode()
    {
        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.L))
                HitFinish();
            else if (Input.GetKeyDown(KeyCode.C))
                collisionsOff = !collisionsOff;
        }
    }

}
