using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    [SerializeField] private float movingSpeed = 10f;

    private Rigidbody2D rb;

    private float minMovingSpeed = 0.1f;
    private bool isRun = false;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {
        HandleMovement();
        Vector3 diference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotateZ = Mathf.Atan2(diference.y, diference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotateZ);
    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        inputVector = inputVector.normalized;
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        if (Mathf.Abs(inputVector.x) > minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed)
        {
            isRun = true;
        }
        else
        {
            isRun = false;
        }
    }

    public bool IsRun()
    {
        return isRun;
    }
}