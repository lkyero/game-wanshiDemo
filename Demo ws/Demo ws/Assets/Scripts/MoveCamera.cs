using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private float horizontalInput;
    private float forwardInput;
    private float scale;
    public float speed = 20;
    public float wheelSpeed = 500;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");
        scale = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(horizontalInput * Time.deltaTime * speed, 0, forwardInput * Time.deltaTime * speed, Space.World);
        transform.Translate(Vector3.forward * Time.deltaTime * wheelSpeed * scale);
    }
}
