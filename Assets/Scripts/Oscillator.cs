using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;

    //[Range(0, 1)] [SerializeField] float movementFactor;

    Vector3 startingPos;

    const float tau = Mathf.PI * 2;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        var cycles = Time.time / period;
        var rawSinWave = Mathf.Sin(cycles * tau);
          
        var offset = movementVector * rawSinWave;
        transform.position = startingPos + offset;
    }
}
