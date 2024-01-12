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
    private byte JumpsLimit;


    void Move()
    {
        float vTranslation = Input.GetAxis("Vertical") * MovementSpeed;
        float hTranslation = Input.GetAxis("Horizontal") * MovementSpeed;
        

        vTranslation *= Time.deltaTime;
        hTranslation *= Time.deltaTime;

        transform.Translate(vTranslation, -1 * Time.deltaTime, -hTranslation);
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
            //fall
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*Vector3 normal = collision.contacts[0].normal;

        if (normal.y > 0)
        {
            Debug.Log("Fell of OMEGALUL");
        }*/

        Debug.Log("Collided gachiGASM");
    }

    void Start()
    {
        
    }


    void Update()
    {
        Move();
        Jump();
    }
}
