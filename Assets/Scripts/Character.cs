using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public Vector3 movement;
    public Vector3 previousMovement;
    public float speed = 30f;
    public SpriteRenderer sprite;

    public Sprite[] animation;

    private Rigidbody2D rb;
    private RaycastHit2D[] results = new RaycastHit2D[3];
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement = Vector3.zero;
        if(Input.GetKey(KeyCode.W) ^ Input.GetKey(KeyCode.S)) {
            if (Input.GetKey(KeyCode.W)) {
                if(!CheckCollision(Vector3.up)) {
                    movement += Vector3.up;
                }
            }
            if (Input.GetKey(KeyCode.S)) {
                if (!CheckCollision(Vector3.down)) {
                    movement += Vector3.down;
                }
            }
        }
        if(Input.GetKey(KeyCode.A) ^ Input.GetKey(KeyCode.D)) {
            if (Input.GetKey(KeyCode.A)) {
                if (!CheckCollision(Vector3.left)) {
                    movement += Vector3.left;
                }
            }
            if (Input.GetKey(KeyCode.D)) {
                if (!CheckCollision(Vector3.right)) {
                    movement += Vector3.right;
                }
            }
        }
        movement.Normalize();
        transform.position += speed * Time.deltaTime * movement;

        //animation
        if(movement.x > 0) {
            sprite.sprite = animation[1];
        } else if (movement.x < 0) {
            sprite.sprite = animation[0];
        } else {
            sprite.sprite = animation[0];
        }
        previousMovement = movement;
    }

    private bool CheckCollision(Vector3 movement) {
        int count = rb.Cast(new Vector2(movement.x, movement.y), results, Time.deltaTime * speed);
        return count > 0;
    }


}
