using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    [SerializeField]
    float speed = 3;
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector2 velo = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
            {
                velo = new Vector2(0, speed);

            }
            else if (Input.GetKey(KeyCode.S))
            {
                velo = new Vector2(0, -speed);
            }
            else
            {
                velo = new Vector2(0, 0);
            }
            if (Input.GetKey(KeyCode.A)){
                velo = velo - Vector2.right * speed;
            }
            else if (Input.GetKey(KeyCode.D)){
                velo = velo + Vector2.right * speed;
            }
            other.GetComponent<Rigidbody2D>().velocity = velo;
            other.GetComponent<PlayerMovement>().velocity = new Vector2(velo.x, other.GetComponent<PlayerMovement>().velocity.y);
        }
    }


}
