using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    public bool canMove;

    private void Awake()
    {
        player = GetComponent<Player>();
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * player.movementSpeed * Time.deltaTime;
            transform.up = Vector3.Lerp(transform.up, GetShipTurnLocation(), player.turnSpeed * Time.deltaTime);
        }
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    public Vector2 GetMovementDirection()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));   
    }

    private Vector2 GetShipTurnLocation()
    {
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 relativeMousePos = new Vector2(worldPos.x - transform.position.x, worldPos.y - transform.position.y);
        Vector2 turnDirection = (relativeMousePos.normalized);
        return turnDirection;
    }
}
