using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    AudioSource rocketThrust;
    AudioSource collisionSound;
    AudioSource victorySound;

    Rigidbody rigidBody;
    Vector3 originalPos;
    Quaternion originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = gameObject.transform.position;
        originalRotation = gameObject.transform.rotation;

        rigidBody = GetComponent<Rigidbody>();

        var audioSources = GetComponents<AudioSource>();
        rocketThrust = audioSources[0];
        collisionSound = audioSources[1];
        victorySound = audioSources[2];
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;

            case "Finish":
                if(!victorySound.isPlaying)
                    victorySound.Play();
                break;

            default:
                if (!collisionSound.isPlaying)
                    collisionSound.Play();
                gameObject.transform.position = originalPos;
                gameObject.transform.rotation = originalRotation;
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
}
