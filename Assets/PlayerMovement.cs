using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float horizontal;
    private float vertical;

    private float velocity = 2f;
    private float verticalSpeed = 0.05f;
    private float jumpForce = 16f;
    private bool isFacingRight = true;

    private float maxSpeed = 350f;
    private float accelerationRate = 0.5f;

    public bool withFlippers = false;

    [SerializeField] private InputActionReference movement, attack, mousePosition, rightStickInput;

    private Vector2 pointerInput;
    private Vector2 movementInput;

    private Vector3 aimlocalScale = Vector3.one;

    //private PlayerAim playerAim;

    public Transform playerAim;

    private void Awake()
    {
        ///playerAim = GetComponentInChildren<PlayerAim>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //rb.velocity = new Vector2(horizontal * horizontalSpeed, rb.velocity.y + vertical * verticalSpeed);
        //if (movementInput != Vector2.zero && velocity < maxSpeed)
        //{
        //    velocity += accelerationRate * Time.deltaTime;
        //}
        //else if (movementInput == Vector2.zero)
        //{
        //    velocity -= accelerationRate * Time.deltaTime;
        //}
        //rb.velocity = movementInput * velocity;
        if (withFlippers)
        {
            rb.AddForce(movementInput * new Vector2(10, 10));
        }
        else
        {
            rb.AddForce(movementInput * new Vector2(5, 5));
        }

        if (!isFacingRight && movementInput.x > 0f)
        {
            Flip();
        }
        else if (isFacingRight && movementInput.x < 0f)
        {
            Flip();
        }
        movementInput = movement.action.ReadValue<Vector2>();

        float angle = Mathf.Atan2(playerAim.transform.right.y, playerAim.transform.right.x) * Mathf.Rad2Deg;
        if (rightStickInput.action.ReadValue<Vector2>() != Vector2.zero)
        {
            angle = Mathf.Atan2(GetRightStickInput().y, GetRightStickInput().x) * Mathf.Rad2Deg;
            playerAim.eulerAngles = new Vector3(0, 0, angle);
        }
        pointerInput = GetPointerInput();
        playerAim.GetComponent<PlayerAim>().MousePosition = pointerInput;
        //playerAim.MousePosition = pointerInput;
        //print(playerAim.MousePosition);

        if (angle > 90 || angle < -90)
        {
            aimlocalScale.y = -1f;
        }
        else
        {
            aimlocalScale.y = +1f;
        }
    
        playerAim.transform.localScale = aimlocalScale;
    }

    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = mousePosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private Vector2 GetRightStickInput()
    {
        return rightStickInput.action.ReadValue<Vector2>();
    }

    private void Flip()
    {
        if (isFacingRight)
        {
            isFacingRight = false;
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            isFacingRight = true;
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        
        //Vector3 localScale = transform.localScale;
        //localScale.x *= -1f;
        //transform.localScale = localScale;
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        vertical = context.ReadValue<Vector2>().y;
    }
}
