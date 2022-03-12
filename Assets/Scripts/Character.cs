using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public Vector3 movement;
    public Vector3 previousMovement;
    public float speed = 30f;
    public SpriteRenderer sprite;

    public Sprite[] anim;
    private bool animLoop = false;
    private float animPeriod = 0.2f;
    private float animTime = 0f;

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

        //movement/collision
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
        animTime = (animTime + Time.deltaTime) % (2 * animPeriod);
        animLoop = animTime < animPeriod;
        if(movement.x > 0) {
            if(animLoop) {
                sprite.sprite = anim[13];
            }
            else {
                sprite.sprite = anim[14];
            }
            previousMovement = movement;
        } else if (movement.x < 0) {
            if (animLoop) {
                sprite.sprite = anim[9];
            } else {
                sprite.sprite = anim[10];
            }
            previousMovement = movement;
        } else {
            if (movement.y > 0) {
                if (animLoop) {
                    sprite.sprite = anim[5];
                } else {
                    sprite.sprite = anim[6];
                }
                previousMovement = movement;
            }
            else if (movement.y < 0) {
                if (animLoop) {
                    sprite.sprite = anim[1];
                } else {
                    sprite.sprite = anim[2];
                }
                previousMovement = movement;
            } 
            else {
                if(previousMovement.x > 0) {
                    sprite.sprite = anim[12];
                } else if (previousMovement.x < 0) {
                    sprite.sprite = anim[8];
                } else if (previousMovement.y > 0) {
                    sprite.sprite = anim[4];
                } else if (previousMovement.y < 0) {
                    sprite.sprite = anim[0];
                } else {
                    sprite.sprite = anim[0];
                }
            }
        }

        //action
        if(Input.GetKey(KeyCode.F)) {

        }
        
    }

    private bool CheckCollision(Vector3 movement) {
        return 0 < rb.Cast(new Vector2(movement.x, movement.y), results, Time.deltaTime * speed);
    }


}
