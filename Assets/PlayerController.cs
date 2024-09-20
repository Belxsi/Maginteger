using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public PlayerMoveAction pma;
    public Vector2 vectorMovement;
    void Awake()
    {
        pma = new PlayerMoveAction();
        pma.Enable();
    }
    public void Update()
    {
        vectorMovement = pma.Player.Move.ReadValue<Vector2>();

    }
   
    

}
