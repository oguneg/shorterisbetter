using System;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform characterBody, characterHead, characterHandLeft, characterHandRight;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 2.5f;

    [SerializeField] private float turnSpeed = 720f;

    [SerializeField] private Animator animator;
    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");
    private bool isRunning = false;

    private Vector3 inputVector;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (!GameManager.isInputEnabled || DialoguePanel.IsDialogueActive)
        {
            inputVector = Vector3.zero;
            SetRunningState(false);
            return;
        }
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        inputVector = new Vector3(horizontal, 0, vertical).normalized;

        if (inputVector.magnitude > 0.1f)
        {
            SetRunningState(true);
        }
        else
        {
            SetRunningState(false);
        }
    }

    private void FixedUpdate()
    {
        if (inputVector.magnitude > 0.1f)
        {
            Vector3 movementVector = (inputVector.x * new Vector3(-1, 0, 1) + inputVector.z * new Vector3(-1, 0, -1))
                .normalized;

            Vector3 targetPosition = rb.position + movementVector * (speed * Time.fixedDeltaTime);
            rb.MovePosition(targetPosition);

            Quaternion targetRotation = Quaternion.LookRotation(movementVector);
            Quaternion smoothRotation =
                Quaternion.RotateTowards(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(smoothRotation);
        }
    }

    private void SetRunningState(bool value)
    {
        if (isRunning == value) return;
        isRunning = value;
        animator.SetBool(IsRunningHash, isRunning);
    }
}