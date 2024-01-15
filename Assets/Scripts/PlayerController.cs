using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float MovementSpeed;
    [SerializeField]
    private float JumpSpeed;

    [SerializeField]
    private float GravityForce = 1;

    private float JumpHeight;
    private float JumpHeightLimit = 7.5f;
    private bool Jumped = false;
    [SerializeField]
    private byte JumpsLimit;

    private Vector3 prevPosition;
    private Vector3 velocityVector = Vector3.zero;
    private Vector3 MoveCommandVector = Vector3.zero;
    private float deltaPosition;

    private Vector3 AxisDirection;

    private enum State
    {
        Grounded,
        Jumping,
        Falling
    }

    private State currentState;

    void Move()
    {
        float fallTranslation = -GravityForce;

        Vector3 movementVector = AxisDirection;

        movementVector = movementVector.normalized * Time.deltaTime * MovementSpeed;

        transform.GetComponent<Rigidbody>().velocity = new Vector3(movementVector.x, transform.GetComponent<Rigidbody>().velocity.y, -movementVector.z);

        //transform.Translate(vTranslation, fallTranslation * Time.deltaTime, -hTranslation);
    }

    void Jump()
    {
        if (Input.GetButton("Jump"))
        {
            Jumped = true;
        }
        if (Jumped && JumpHeight < JumpHeightLimit)
        {
            float jumpTranslation = JumpSpeed * Time.deltaTime;
            transform.Translate(0, jumpTranslation, 0);
            JumpHeight += jumpTranslation;
        }
        if (JumpHeight >= JumpHeightLimit)
        {
            //JumpHeight = 0;
            currentState = State.Falling;
        }
    }

    bool IsCollidingVertically()
    {
        //gameObject.GetComponent<Collider>().bounds;
        Vector3 currentPosition = transform.position;
        RaycastHit hitInfo;

        Ray ray = new Ray(currentPosition, Vector3.down);
        Debug.DrawRay(currentPosition, Vector3.down, Color.red);
        

        if (Physics.Raycast(ray, out hitInfo, 1)) 
        {
            //Debug.Log("collided Okage");
            return true;
        }

        return false;
    }

    bool IsColludingHorizontally()
    {
        Vector3 currentPosition = transform.position;
        RaycastHit hitInfo;

        Ray ray = new Ray(new Vector3(currentPosition.x, gameObject.GetComponent<Collider>().bounds.max.y, currentPosition.z), MoveCommandVector);

        if (Physics.Raycast(ray , out hitInfo, 0.5f))
        {
            Debug.Log("collided Okage");
            return true;
        }

        return false;
    }

    private void Awake()
    {
        currentState = State.Grounded;
    }

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        deltaPosition = Vector3.Distance(prevPosition, transform.position);
        velocityVector = Vector3.Normalize(transform.position - prevPosition);
        prevPosition = transform.position;
        Move();
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(50f, 0, 400, Screen.height));
        GUILayout.Label("\nDelta position: " + deltaPosition + "\nAxis: " + MoveCommandVector);
        GUILayout.EndArea();
    }

    void Update()
    {
        float vAxis = Input.GetAxis("Vertical");
        float hAxis = Input.GetAxis("Horizontal");

        AxisDirection = Vector3.Normalize(new Vector3(vAxis, 0, hAxis));

        Debug.DrawRay(new Vector3(transform.position.x, gameObject.GetComponent<Collider>().bounds.max.y, transform.position.z), MoveCommandVector, Color.red);
        MoveCommandVector = Vector3.Normalize(new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal")));
    }
}
