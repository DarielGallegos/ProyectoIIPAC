using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithFloor : MonoBehaviour
{
    CharacterController player;
    Vector3 groundPosition;
    Vector3 lastGroundPosition;
    string groundName;
    string lastGroundName;

    Quaternion actualRot;
    Quaternion lastRot;

    public Vector3 originOffset;
    public float factorDivision = 4.7f;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isGrounded)
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position + originOffset, player.radius / factorDivision, -transform.up, out hit))
            {   
                GameObject groundedIn = hit.collider.gameObject;
                groundName = groundedIn.name;
                groundPosition = groundedIn.transform.position;
                actualRot = groundedIn.transform.rotation;
                Debug.Log(groundName);
                // Mover al jugador si la posición del suelo cambia
                if (groundPosition != lastGroundPosition && groundName == lastGroundName)
                {
                    Vector3 movement = groundPosition - lastGroundPosition;
                    transform.position += movement;
                    Debug.Log("aqui calcula nueva posicion");
                }
                Debug.Log(lastGroundName);
                // Rotar al jugador si la rotación del suelo cambia
                if (actualRot != lastRot && groundName == lastGroundName)
                {
                    Quaternion rotationDelta = actualRot * Quaternion.Inverse(lastRot);
                    Vector3 pivotPoint = groundedIn.transform.position;
                    transform.RotateAround(pivotPoint, Vector3.up, rotationDelta.eulerAngles.y);
                }

                // Actualizar las últimas posiciones y rotaciones del suelo
                lastGroundPosition = groundPosition;
                lastGroundName = groundName;
                lastRot = actualRot;
            }
        }
        else if (!player.isGrounded)
        {
            // Resetear los valores cuando el jugador no está en el suelo
            lastGroundName = null;
            lastGroundPosition = Vector3.zero;
            lastRot = Quaternion.identity;
        }
    }

    private void OnDrawGizmos()
    {
        if (player == null)
        {
            player = GetComponent<CharacterController>();
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + originOffset, player.radius / factorDivision);
    }
}
