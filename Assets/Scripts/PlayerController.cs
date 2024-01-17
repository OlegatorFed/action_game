using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float MovementSpeed;
    [SerializeField]
    private float JumpSpeed;

    private float JumpHeight;
    private float JumpHeightLimit = 7.5f;
    private bool Jumped = false;
    [SerializeField]
    private byte JumpCounter;

    private Vector3 prevPosition;
    private Vector3 velocityVector = Vector3.zero;
    private Vector3 MoveCommandVector = Vector3.zero;
    private float deltaPosition;

    private float verticalVelocity = 0f;
    [SerializeField]
    private float MaxFallSpeed = 10f;
    [SerializeField]
    private float FallAcceleration = 1f;

    private Vector3 AxisDirection;

    private Animator animator;

    private enum State
    {
        Grounded,
        Jumping,
        Falling
    }

    private State currentState;

    void MovementCalculation()
    {
        verticalVelocity = Mathf.Clamp(verticalVelocity - FallAcceleration, -MaxFallSpeed, JumpSpeed);

        float verticalMovement = verticalVelocity;
        Vector3 movementVector = AxisDirection;

        movementVector = movementVector.normalized * MovementSpeed;

        velocityVector = new Vector3(movementVector.x, verticalMovement, -movementVector.z);
    }

    void Move()
    {
        transform.GetComponent<Rigidbody>().velocity = velocityVector;

        //transform.Translate(vTranslation, fallTranslation * Time.deltaTime, -hTranslation);
    }

    void Jump()
    {
        verticalVelocity = JumpSpeed;
    }

    bool IsCollidingVertically()
    {
        Vector3 currentPosition = transform.position;
        RaycastHit hitInfo;

        Ray ray = new Ray(currentPosition, Vector3.down);
        Debug.DrawRay(currentPosition, Vector3.down, Color.red);
        

        if (Physics.Raycast(ray, out hitInfo, 1)) 
        {
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
            return true;
        }

        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.GetContact(0).point + "; " + (transform.position - GetComponent<Collider>().bounds.min));
        if (collision.GetContact(0).point.y == transform.position.y)
        {
            Jumped = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (GetComponent<Rigidbody>().velocity.y == 0)
        {
            //Jumped = false;
        }
    }

    private void Awake()
    {
        currentState = State.Grounded;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(50f, 0, 400, Screen.height));
        GUILayout.Label("\nDelta position: " + deltaPosition + "\nAxis: " + MoveCommandVector + "\nJumped: " + Jumped + "\nVelocity: " + velocityVector + "\n" + transform.GetComponent<Rigidbody>().velocity);
        GUILayout.EndArea();
    }

    private void FixedUpdate()
    {
        deltaPosition = Vector3.Distance(prevPosition, transform.position);
        //velocityVector = Vector3.Normalize(transform.position - prevPosition);
        //velocityVector = new Vector3(velocityVector.x, 0, velocityVector.z);
        prevPosition = transform.position;
        Move();
    }

    void Update()
    {
        float vAxis = Input.GetAxis("Vertical");
        float hAxis = Input.GetAxis("Horizontal");
        bool jumpInput = Input.GetButtonDown("Jump");

        AxisDirection = Vector3.Normalize(new Vector3(vAxis, 0, hAxis));

        if (jumpInput && !Jumped)
        {
            Jump();
            Jumped = true;
        }

        MovementCalculation();

        if (velocityVector.x != 0 || velocityVector.z != 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        Debug.DrawRay(new Vector3(transform.position.x, gameObject.GetComponent<Collider>().bounds.max.y, transform.position.z), MoveCommandVector, Color.red);
        MoveCommandVector = Vector3.Normalize(new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal")));
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, MoveCommandVector, .05f, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
