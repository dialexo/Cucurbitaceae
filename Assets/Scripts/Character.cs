using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public KeyCode[] keybinding;
    public Vector3 movement;
    public Vector3 previousMovement;
    public float speed = 30f;
    public SpriteRenderer sprite;
    private Rigidbody2D rb;
    private RaycastHit2D[] results = new RaycastHit2D[5];

    public Sprite[] anim;
    private bool animLoop = false;
    private float animPeriod = 0.2f;
    private float animTime = 0f;

    public List<PickupItem> pickupItems;
    public PickupItem item;

    public List<Plant> candidatePlants;

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
        if(Input.GetKey(keybinding[0]) ^ Input.GetKey(keybinding[1])) {
            if (Input.GetKey(keybinding[0])) {
                if(!CheckCollision(Vector3.up)) {
                    movement += Vector3.up;
                }
            }
            if (Input.GetKey(keybinding[1])) {
                if (!CheckCollision(Vector3.down)) {
                    movement += Vector3.down;
                }
            }
        }
        if(Input.GetKey(keybinding[2]) ^ Input.GetKey(keybinding[3])) {
            if (Input.GetKey(keybinding[2])) {
                if (!CheckCollision(Vector3.left)) {
                    movement += Vector3.left;
                }
            }
            if (Input.GetKey(keybinding[3])) {
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
            if (item) {
                item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, 1f);
                item.transform.localScale = new Vector3(-1f, 1f, 1f);
                item.transform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);

            }
            previousMovement = movement;
        } else if (movement.x < 0) {
            if (animLoop) {
                sprite.sprite = anim[9];
            } else {
                sprite.sprite = anim[10];
            }
            if (item) {
                item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, -1f);
                item.transform.localScale = new Vector3(1f, 1f, 1f);
                item.transform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);

            }
            previousMovement = movement;
        } else {
            if (movement.y > 0) {
                if (animLoop) {
                    sprite.sprite = anim[5];
                } else {
                    sprite.sprite = anim[6];
                }
                if (item) {
                    item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, 1f);
                    item.transform.localScale = new Vector3(-1f, 1f, 1f);
                    item.transform.GetChild(0).localPosition = new Vector3(0.45f, 0f, 0f);
                }
                previousMovement = movement;
            }
            else if (movement.y < 0) {
                if (animLoop) {
                    sprite.sprite = anim[1];
                } else {
                    sprite.sprite = anim[2];
                }
                if (item) {
                    item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, -1f);
                    item.transform.localScale = new Vector3(1f, 1f, 1f);
                    item.transform.GetChild(0).localPosition = new Vector3(0.45f, 0f, 0f);
                }
                previousMovement = movement;
            } 
            else {
                if(previousMovement.x > 0) {
                    sprite.sprite = anim[12];
                    if (item) {
                        item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, 1f);
                        item.transform.localScale = new Vector3(-1f, 1f, 1f);
                        item.transform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);
                    }
                } else if (previousMovement.x < 0) {
                    sprite.sprite = anim[8];
                    if (item) {
                        item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, -1f);
                        item.transform.localScale = new Vector3(1f, 1f, 1f);
                        item.transform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);
                    }
                } else if (previousMovement.y > 0) {
                    sprite.sprite = anim[4];
                    if (item) {
                        item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, 1f);
                        item.transform.localScale = new Vector3(-1f, 1f, 1f);
                        item.transform.GetChild(0).localPosition = new Vector3(0.45f, 0f, 0f);
                    }
                } else if (previousMovement.y < 0) {
                    sprite.sprite = anim[0];
                    if (item) {
                        item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, -1f);
                        item.transform.localScale = new Vector3(1f, 1f, 1f);
                        item.transform.GetChild(0).localPosition = new Vector3(0.45f, 0f, 0f);
                    }
                } else {
                    sprite.sprite = anim[0];
                    if (item) {
                        item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, -1f);
                        item.transform.localScale = new Vector3(1f, 1f, 1f);
                        item.transform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);
                    }
                }
            }
        }

        //pickup
        if(Input.GetKeyDown(keybinding[4])) {
            if(item) {
                item.Release();
                item = null;
            } else {
                foreach(PickupItem pickable in pickupItems) {
                    if(!pickable.held) {
                        pickable.Grab(transform);
                        item = pickable;
                        break;
                    }
                }
            }
        }

        if(Input.GetKeyDown(keybinding[5])) {
            foreach (Plant plant in candidatePlants) {
                if (!plant.tended) {
                    plant.Tend(item.type);
                    break;
                }
            }
        }
        
    }

    private bool CheckCollision(Vector3 movement) {
        int count = rb.Cast(new Vector2(movement.x, movement.y), results, Time.deltaTime * speed);
        int triggers = 0;
        for(int i = 0; i < count; i++) {
            if(results[i].collider.isTrigger) {
                triggers++;
            }
        }
        return 0 < count - triggers;
    }

}
