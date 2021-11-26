using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class MoveController : MonoBehaviour
{
    [SerializeField] private PlayerInputSO inputEvent;
    [Header("Move")]
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float accelerationTime = 3f;
    [SerializeField, Range(0, 5f)] private float shiftFactor = 3f;
    [SerializeField] private float moveRotationAngle = 50f;

    private Vector2 finalMoveDirection;
    private Quaternion finalMoveRotation;
    private Vector2 tempPlayerVelocity;
    private Quaternion tempPlayerRotation;

    private Character character;

    private float shiftTimer;
    private bool canMove = false;

    private void OnEnable()
    {
        inputEvent.onMove += Move;
        inputEvent.onStopMove += StopMove;
    }
    private void OnDisable()
    {
        inputEvent.onMove -= Move;
        inputEvent.onStopMove -= StopMove;
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        character = GetComponent<Character>();

        inputEvent.EnableGameplayInput();
    }

    private void Update()
    {
        if (GameManager.CurrentGameState != GameState.Playing) return;
        if (!canMove && tempPlayerVelocity == Vector2.zero) return;

        tempPlayerVelocity = Vector2.Lerp(tempPlayerVelocity, finalMoveDirection, Time.deltaTime * moveSpeed * shiftFactor);
        tempPlayerRotation = Quaternion.Lerp(tempPlayerRotation, finalMoveRotation, Time.deltaTime * moveSpeed * shiftFactor);

        if (finalMoveDirection == Vector2.zero)
        {
            if (Mathf.Abs(tempPlayerVelocity.x) <= 1e-2) tempPlayerVelocity.x = 0;
            if (Mathf.Abs(tempPlayerVelocity.y) <= 1e-2) tempPlayerVelocity.y = 0;
            if (Mathf.Abs(tempPlayerRotation.x) <= 1e-2) tempPlayerRotation = finalMoveRotation;
        }

        character.Move(tempPlayerVelocity * Time.deltaTime);
        character.Rotate(tempPlayerRotation);
    }

    #region Move
    private void Move(Vector2 moveInput)
    {
        canMove = true;
        shiftTimer = 0;
        finalMoveDirection = moveInput * moveSpeed;
        finalMoveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);
    }

    private void StopMove()
    {
        canMove = false;
        shiftTimer = 0;
        finalMoveDirection = Vector2.zero;
        finalMoveRotation = Quaternion.identity;
    }
    #endregion
}