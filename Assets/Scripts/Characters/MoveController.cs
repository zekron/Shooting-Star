using System;
using System.Collections;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] private PlayerInputSO inputEvent;

    [Header("Move")]
    [SerializeField] private float moveSpeed = 20f;
    /// <summary>
    /// 变速因子
    /// </summary>
    [SerializeField, Range(0, 5f)] private float shiftFactor = 3f;
    [SerializeField] private float moveRotationAngle = 50f;

    private Vector2 finalMoveDirection;
    private Quaternion finalMoveRotation;
    private Vector2 tempPlayerVelocity;
    private Quaternion tempPlayerRotation;

    private Character character;

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
        finalMoveDirection = moveInput * moveSpeed;
        finalMoveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.x, Vector3.up);
    }

    private void StopMove()
    {
        canMove = false;
        finalMoveDirection = Vector2.zero;
        finalMoveRotation = Quaternion.identity;
    }

    public void SetMoveSpeedByValue(int value)
    {
        moveSpeed += value;
    }
    public void SetMoveSpeedByFactor(float factor)
    {
        moveSpeed *= factor;
    }
    #endregion
}