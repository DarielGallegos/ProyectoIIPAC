using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    public float horizontalControl;
    public float verticalControl;
    private Vector3 playerInput;
    public CharacterController player;
    public float speedPlayer;
    private Vector3 movePlayer;
    public float gravity = 9.8f;
    public float fallVelocity;
    public float jumpForce;
    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;

    public bool isOnSlope = false;
    private Vector3 hitNormal;

    public float slideSpeed = 6.0f;
    public float slopeForceDown = 6.0f;
    
    private Transform platformTransform; // Para rastrear la plataforma en la que está el jugador
    private Vector3 previousPlatformPosition;

    
    // Variables de animaciones
    public Animator playerAnimatorController;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
        playerAnimatorController  = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Calculos y logica
        horizontalControl = Input.GetAxis("Horizontal");
        verticalControl = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalControl, 0, verticalControl);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        playerAnimatorController.SetFloat("PlayerWalkVelocity", playerInput.magnitude * speedPlayer) ;

        camDirection();

        movePlayer = playerInput.x * camRight + playerInput.z * camForward;

        movePlayer = movePlayer * speedPlayer;

        //El jugador se movera en direccion a la camara
        player.transform.LookAt(player.transform.position + movePlayer);

        SetGravity();
        PlayerSkills();

        if (platformTransform != null)
        {
            Vector3 platformMovement = platformTransform.position - previousPlatformPosition;
            movePlayer += platformMovement / Time.deltaTime;
            previousPlatformPosition = platformTransform.position;
        }

        //Tema de llamadas de movimiento
        player.Move(movePlayer * Time.deltaTime);

        //Velocidad que tiene el jugador en cada momento (se le puede quitar)
        Debug.Log(player.velocity.magnitude);
    }

    void camDirection()
    {
        //Direccion hacia adelante y hacia la derecha de la camara 
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        //Dejamos en 0 los valores de y que serian abajo y arriba 
        camForward.y = 0;
        camRight.y = 0;

        //valor normalizado de las direccciones
        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    // Movimientos del jugador
    public void PlayerSkills()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallVelocity = jumpForce;
            movePlayer.y = fallVelocity;

            playerAnimatorController.SetTrigger("PlayerJump");
        }
    }

    //Función para la gravedad
    void SetGravity()
    {
        if (player.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;

            playerAnimatorController.SetFloat("PlayerVerticalVelocity", player.velocity.y);
        }
        playerAnimatorController.SetBool("IsGrounded", player.isGrounded);
        slideDown();
    }

    public void slideDown()
    {
        isOnSlope = Vector3.Angle(Vector3.up, hitNormal) >= player.slopeLimit;
        if (isOnSlope)
        {
            movePlayer.x += (hitNormal.x * (1f - hitNormal.y)) * slideSpeed;
            movePlayer.z += (hitNormal.z * (1f - hitNormal.y)) * slideSpeed;
            movePlayer.y += slopeForceDown;
        }
    }

    //Funcion para dectetar si el character controller colisiona con con colisionador
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Detectar si estamos sobre una plataforma móvil
        if (hit.collider.CompareTag("MovablePlatform"))
        {
            platformTransform = hit.collider.transform;
            previousPlatformPosition = platformTransform.position;
        }
        else
        {
            platformTransform = null;
        }

        hitNormal = hit.normal;
    }

    private void OnAnimatorMove(){
        
    }
}
