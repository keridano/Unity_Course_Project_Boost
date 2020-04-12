using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    Rigidbody rigidBody;
    AudioSource rocketThrust;
    Vector3 originalPos;
    Quaternion originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rocketThrust = GetComponent<AudioSource>();

        originalPos = gameObject.transform.position;
        originalRotation = gameObject.transform.rotation;
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
            default:
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
