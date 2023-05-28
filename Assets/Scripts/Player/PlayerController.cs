using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    private Vector2 _moveInput;
    private Rigidbody _rb;
    private Vector3 rotate;
    private Coroutine lookCoroutine;
    public ParticleSystem leftDustPS;
    public ParticleSystem rightDustPS;
    public bool dustActive;
    private AudioSource audioSource;

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void Awake()
    {
        _rb = GetComponentInChildren<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Vector3 vec3 = new Vector3(-1, 1, -1);
        // Debug.Log(vec3.normalized);
        // Debug.Log(vec3.magnitude * vec3.normalized);
        dustActive = false;
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = audioSource.volume / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (LevelManager.Instance.currentState == State.Building)
            return;
        Friction(playerData.frictionAmount);
        // MovePlayer(1);
        NewMovePlayer(1);
        
        if (_moveInput.magnitude > 0.8f)
        {
            rotate = new Vector3(_moveInput.x, 0, _moveInput.y);
            Quaternion lookRotate = Quaternion.LookRotation(rotate, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotate, playerData.rotateSpeed * 180 * Time.deltaTime);

            if (!dustActive)
            {
                dustActive = true;
                leftDustPS.Play();
                rightDustPS.Play();
                //audioSource.Play();
                audioSource.volume = audioSource.volume * 2f;
            }
        }
        else
        {
            if (dustActive)
            {
                dustActive = false;
                leftDustPS.Stop();
                rightDustPS.Stop();
                //audioSource.Pause();
                audioSource.volume = audioSource.volume / 2f;
            }
        }
    }
    public void StartRotating()
    {
        if(lookCoroutine!= null)
        {
            StopCoroutine(lookCoroutine);
        }
        lookCoroutine = StartCoroutine(LookAt());
    }

    private IEnumerator LookAt()
    {
        Quaternion lookRotation = Quaternion.LookRotation(rotate + transform.position);
        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
            time += Time.deltaTime * playerData.rotateSpeed;
            yield return null;
        }
    }
    public void MovePlayer(float lerpAmount)
    {
        Vector3 movementVector3 = new Vector3(_moveInput.x, 0f, _moveInput.y);
        float targetSpeed = movementVector3.magnitude * playerData.maxSpeed;

        float speedDif = targetSpeed - _rb.velocity.magnitude;

        //Gets accel value based on if we are accelerating or trying to decelerate.
        float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? playerData.moveAccel : playerData.moveDecel;
        float velPower;
        if (playerData.allowStopPower && Mathf.Abs(targetSpeed) < 0.01f)
            velPower = playerData.stopPower;
        else
            velPower = playerData.accelPower;

        //Applies acceleration to speed difference, raise to set power so acceleration increases, multiply by sign to preserve direction
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        movement = Mathf.Lerp(_rb.velocity.magnitude, movement, lerpAmount); //Lerp to prevent Walk from immediately slowing down playing in situations.

        _rb.AddForce(movementVector3 * movement);
    }

    public void NewMovePlayer(float lerpAmount)
    {
        Vector3 movementVector3 = new Vector3(_moveInput.x, 0f, _moveInput.y);
        Vector3 targetSpeed = movementVector3 * playerData.maxSpeed;

        Vector3 speedDif = targetSpeed - _rb.velocity;
        //Gets accel value based on if we are accelerating or trying to decelerate.
        float accelRate = Mathf.Abs(targetSpeed.magnitude) > 0.01f ? playerData.moveAccel : playerData.moveDecel;
        float velPower;
        if (playerData.allowStopPower && Mathf.Abs(targetSpeed.magnitude) < 0.01f)
            velPower = playerData.stopPower;
        else
            velPower = playerData.accelPower;

        //Applies acceleration to speed difference, raise to set power so acceleration increases, multiply by sign to preserve direction
        Vector3 movement = Mathf.Pow(speedDif.magnitude * accelRate, velPower) * speedDif.normalized;
        movement = Vector3.Lerp(_rb.velocity, movement, lerpAmount); //Lerp to prevent Walk from immediately slowing down playing in situations.

        _rb.AddForce(movement);
    }

    private void Friction(float amount)
    {
        Vector3 velocity = _rb.velocity;
        Vector3 force = amount * velocity.normalized;
        force.x = Mathf.Min(Mathf.Abs(velocity.x), Mathf.Abs(force.x)); //Ensures we only slow player down
        force.z = Mathf.Min(Mathf.Abs(velocity.z), Mathf.Abs(force.z)); //and apply a small force if they are moving slowly.
        force.x *= Mathf.Sign(velocity.x); //Find direction to apply force.
        force.z *= Mathf.Sign(velocity.z);

        _rb.AddForce(-force, ForceMode.Impulse);
    }
}