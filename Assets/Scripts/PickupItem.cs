using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    //public Character[] characters;
    public enum ItemType {
        Shovel,
        Spray,
        Arrosoir
    }
    public bool held = false;
    public ItemType type;
    public Vector3 offset;

    private Transform originalParent;
    // Start is called before the first frame update
    void Start()
    {
        originalParent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Grab(Transform parent) {
        held = true;
        transform.position = parent.position + offset;
        transform.parent = parent;
        GetComponent<Rigidbody2D>().isKinematic = true;
    }

    public void Release() {
        held = false;
        transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
        transform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);
        transform.parent = originalParent;
        GetComponent<Rigidbody2D>().isKinematic = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Character>()) {
            other.GetComponent<Character>().pickupItems.Add(this); 
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.GetComponent<Character>()) {
            other.GetComponent<Character>().pickupItems.Remove(this);
        }
    }

}
