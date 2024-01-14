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
    private float deltaPosition;
    private Vector3 velocityVector = Vector3.zero;

    private enum State
    {
        Grounded,
        Jumping,
        Falling
    }

    private State currentState;

    void Move()
    {
        float vTranslation = Input.GetAxis("Vertical") * MovementSpeed;
        float hTranslation = Input.GetAxis("Horizontal") * MovementSpeed;
        float fallTranslation = IsColliding() ? 0 : -GravityForce;
        

        vTranslation *= Time.deltaTime;
        hTranslation *= Time.deltaTime;

        transform.Translate(vTranslation, fallTranslation * Time.deltaTime, -hTranslation);
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

    bool IsColliding()
    {
        //gameObject.GetComponent<Collider>().bounds;
        Vector3 currentPosition = transform.position;
        RaycastHit hitInfo;

        Ray ray = new Ray(currentPosition, Vector3.down);
        Debug.DrawRay(currentPosition, Vector3.down, Color.red);
        //Debug.DrawRay(new Vector3(currentPosition.x, gameObject.GetComponent<Collider>().bounds.max.y, currentPosition.z), new Vector3(prevPosition.x, 0, prevPosition.z), Color.red);

        if (Physics.Raycast(ray, out hitInfo, 1)) 
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

        prevPosition = transform.position;
        
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(50f, 0, 400, Screen.height));
        GUILayout.Label("\nDelta position: " + deltaPosition + "\nPrevious position: " + prevPosition);
        GUILayout.EndArea();
    }

    void Update()
    {
        Move();
        Jump();
    }
}
