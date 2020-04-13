using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    enum State { Alive, Dying, Transcending}

    AudioSource rocketThrust;
    AudioSource collisionSound;
    AudioSource victorySound;

    Rigidbody rigidBody;

    State playerState;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerState = State.Alive;

        var audioSources = GetComponents<AudioSource>();
        rocketThrust = audioSources[0];
        collisionSound = audioSources[1];
        victorySound = audioSources[2];
    }

    // Update is called once per frame
    void Update()
    {
        if(playerState == State.Alive)
        {
            Thrust();
            Rotate();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;

            case "Finish":
                playerState = State.Transcending;

                if (!victorySound.isPlaying)
                    victorySound.Play();

                Invoke("HitFinish", 3f);
                break;

            default:
                playerState = State.Dying;
                if (!collisionSound.isPlaying)
                    collisionSound.Play();
                Invoke("HitDeadlyObject", 1f);
                break;
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!rocketThrust.isPlaying)
                rocketThrust.Play();
        }
        else
        {
            if (rocketThrust.isPlaying)
                rocketThrust.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; //Manually manage rotation

        var rotateThisFrame = rcsThrust * Time.deltaTime;
        var fwdRotateThisFrame = Vector3.forward * rotateThisFrame;

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-fwdRotateThisFrame);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(fwdRotateThisFrame);
        }

        rigidBody.freezeRotation = false; //Physical rotation restored

    }

#pragma warning disable IDE0051

    private void HitDeadlyObject()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void HitFinish()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            SceneManager.LoadScene(1);
        else if (SceneManager.GetActiveScene().buildIndex == 1)
            SceneManager.LoadScene(2);
    }

#pragma warning restore IDE0051 
}
