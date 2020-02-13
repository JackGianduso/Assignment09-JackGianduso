using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayMov : MonoBehaviour
{
    public Text healthText;
    public float health = 200f;
    public float speed = 8.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    public GameObject winText;
    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        if (health <= 0)
            Destroy(gameObject);
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);            
            moveDirection *= speed;            
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }      
        moveDirection.y -= gravity * Time.deltaTime;     
        controller.Move(moveDirection * Time.deltaTime);
        healthText.text = "Health: " + health;
    }
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "win")
        {
            winText.SetActive(true);
        }
    }
}
