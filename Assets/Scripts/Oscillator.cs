using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(0,0,0);
    [SerializeField] float period;

    [Range(-1, 1)] [SerializeField] float movementFactor;

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
        if (period <= Mathf.Epsilon) return;

        var cycles = Time.time / period;
        var rawSinWave = Mathf.Sin(cycles * tau);

        //movementFactor = (rawSinWave / 2f) + 0.5f; // (0,1)
        movementFactor = rawSinWave; //(-1,1)
        var offset = movementVector * movementFactor;
        transform.position = startingPos + offset;

    }
}
