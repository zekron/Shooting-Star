using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] PlayerInputSO input;
    [SerializeField] float moveSpeed = 20;

    Rigidbody2D playerRigidbody;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        input.onMove += Move;
        input.onStopMove += StopMove;
    }
    private void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
    }

    private void Start()
    {
        playerRigidbody.gravityScale = 0;

        input.EnableGameplayInput();
    }

    private void Update()
    {

    }

    private void Move(Vector2 moveInput)
    {
        playerRigidbody.velocity = moveInput * moveSpeed;
    }

    private void StopMove()
    {
        playerRigidbody.velocity = Vector2.zero;
    }
}