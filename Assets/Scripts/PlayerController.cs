using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalControl;
    public float verticalControl;

    public CharacterController player;

    public float speedPlayer;

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
    }

    private void FixedUpdate() 
    {
        //Tema de llamadas de movimiento
        player.Move(new Vector3(horizontalControl, 0, verticalControl) * speedPlayer * Time.deltaTime);
    }
}
