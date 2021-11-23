using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private PlayerInputSO input;

    [SerializeField] private Canvas canvasHUD;

    [SerializeField] private AudioDataSO confirmGameOverSound;

    private int exitStateID = Animator.StringToHash("GameOverScreenExit");

    private Canvas canvasGameOver;

    private Animator animator;

    void Awake()
    {
        canvasGameOver = GetComponent<Canvas>();
        animator = GetComponent<Animator>();

        canvasGameOver.enabled = false;
        animator.enabled = false;
    }

    void OnEnable()
    {
        GameManager.onGameOver += OnGameOver;

        input.onConfirmGameOver += OnConfirmGameOver;
    }

    void OnDisable()
    {
        GameManager.onGameOver -= OnGameOver;

        input.onConfirmGameOver -= OnConfirmGameOver;
    }

    void OnGameOver()
    {
        canvasHUD.enabled = false;
        canvasGameOver.enabled = true;
        animator.enabled = true;
        input.DisableAllInputs();
    }

    void OnConfirmGameOver()
    {
        AudioManager.Instance.PlaySFX(confirmGameOverSound);
        input.DisableAllInputs();
        animator.Play(exitStateID);
        SceneLoader.Instance.LoadScoringScene();
    }

    // Animation Event
    void EnableGameOverScreenInput()
    {
        input.EnableGameOverScreenInput();
    }
}