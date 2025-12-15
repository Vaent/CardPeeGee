using UnityEngine;

public class PreloadCards : MonoBehaviour
{
    private bool isTurning = true;
    private Vector3 turnVector = new Vector3(440, 320, 0);

    void Start()
    {
        GetComponent<Rigidbody>().linearVelocity = Vector3.down * 10;
    }

    void Update()
    {
        if (isTurning)
        {
            transform.Rotate(turnVector * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other) {
    	GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        isTurning = false;
        Destroy(GetComponent<Collider>());
    }
}
