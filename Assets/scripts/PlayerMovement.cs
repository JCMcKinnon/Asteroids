using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    private Vector2 acceleration;
    public float timeToMaxAcceleration;
    public float maxSpeed;
    public float rotateSpeed;
    public Rigidbody2D rb;

    //public GameObject theobj;

    private Camera cam;

    private Vector3 up;
    private Vector3 down;
    private Vector3 left;
    private Vector3 right;

    private Vector3[] verBounds;
    private Vector3[] horBounds;

    void Start()
    {
        up = cam.ViewportToWorldPoint(Vector3.up);
        down = cam.ViewportToWorldPoint(Vector3.zero);
        left = cam.ViewportToWorldPoint(new Vector3(0,0,0));
        right = cam.ViewportToWorldPoint(new Vector3(1,0,0));

        verBounds = new Vector3[2];
        horBounds = new Vector3[2];
        verBounds[0] = up;
        verBounds[1] = down;
        horBounds[0] = left;
        horBounds[1] = right;

/*        for (int i = 0; i < verBounds.Length; i++)
        {
            Instantiate(theobj, (Vector2)verBounds[i], Quaternion.identity);
        }
        for (int i = 0; i < horBounds.Length; i++)
        {
            Instantiate(theobj, (Vector2)horBounds[i], Quaternion.identity);
        }*/
    }
private void Awake()
    {
        cam = Camera.main;
    }
    


    // Update is called once per frame
    void Update()
    {
         CheckBounds();

        transform.Rotate(-Vector3.forward * Input.GetAxis("Horizontal") * rotateSpeed);      
        acceleration = ((transform.up * maxSpeed) - Vector3.zero) / timeToMaxAcceleration;
        var currentSpeed = new Vector3(acceleration.x, acceleration.y, 0) * Time.deltaTime * maxSpeed * Input.GetAxis("Vertical");
        var newSpeed = new Vector3(acceleration.x, acceleration.y, 0) * Time.deltaTime * -2;
        if (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddRelativeForce(Vector3.up * maxSpeed, ForceMode2D.Force);         
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            rb.AddRelativeForce(-Vector3.up * maxSpeed/2, ForceMode2D.Force);
        }

    }

    public void CheckBounds()
    {
        if (transform.position.y >= up.y + 1.3)
        {
            transform.position = new Vector3(transform.position.x, down.y, 0);
        }
        if (transform.position.y <= down.y - 1.3)
        {
            transform.position = new Vector3(transform.position.x, up.y, 0);
        }
        if (transform.position.x >= right.x + 1.3)
        {
            transform.position = new Vector3(left.x, transform.position.y, 0);
        }
        if (transform.position.x <= left.x - 1.3)
        {
            transform.position = new Vector3(right.x, transform.position.y, 0);
        }
    }
}
