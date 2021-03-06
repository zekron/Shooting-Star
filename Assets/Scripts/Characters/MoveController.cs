using System;
using System.Collections;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] private PlayerInputSO inputEvent;
    [SerializeField] private Vector3EventChannelSO playerMoveInputEventSO;

    [Header("Move")]
    private float moveSpeed = 20f;
    private float moveRotationAngle = 50f;
    /// <summary>
    /// 变速因子
    /// </summary>
    [SerializeField, Range(0, 5f)] private float shiftFactor = 3f;

    private Vector2 finalMoveDirection;
    private Quaternion finalMoveRotation;
    private Vector2 tempPlayerVelocity;
    private Quaternion tempPlayerRotation;

    private Character character;

    private bool canMove = false;

    private void OnEnable()
    {
        inputEvent.onMove += Move;
        inputEvent.eventOnStopMove += StopMove;
    }
    private void OnDisable()
    {
        inputEvent.onMove -= Move;
        inputEvent.eventOnStopMove -= StopMove;
    }

    private void Awake()
    {
        character = GetComponent<Character>();

        inputEvent.EnableGameplayInput();
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Playing) return;
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
        playerMoveInputEventSO.RaiseEvent(moveInput);
        playerMoveInputEventSO.RaiseEvent(moveInput);
        finalMoveDirection = moveInput * moveSpeed;
        finalMoveRotation = Quaternion.AngleAxis(-moveRotationAngle * moveInput.x, Vector3.up);
    }

    private void StopMove()
    {
        canMove = false;
        finalMoveDirection = Vector2.zero;
        playerMoveInputEventSO.RaiseEvent(Vector2.zero);
        finalMoveRotation = Quaternion.identity;
    }

    public void SetMoveProfile(float moveSpeed, float moveRotationAngle)
    {
        this.moveSpeed = moveSpeed;
        this.moveRotationAngle = moveRotationAngle;
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