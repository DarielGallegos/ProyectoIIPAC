using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalControl;
    public float verticalControl;
    private Vector3 playerInput;

    public CharacterController player;

    public float speedPlayer;
    private Vector3 movePlayer;
    
    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Calculos y logica
        horizontalControl = Input.GetAxis("Horizontal");
        verticalControl = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalControl, 0, verticalControl);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);
        
        camDirection();
   
        movePlayer = playerInput.x * camRight + playerInput.z * camForward;

        //El jugador se movera en direccion a la camara
        player.transform.LookAt(player.transform.position + movePlayer);

        //Tema de llamadas de movimiento
        player.Move(movePlayer * speedPlayer * Time.deltaTime);

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
}
