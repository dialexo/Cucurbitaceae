using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public PickupItem.ItemType toolNeeded;
    
    public Sprite[] sprites;

    //public AudioSource audio;
    public bool tended;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        tended = false;
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Tend(PickupItem.ItemType toolUsed) {
        if(toolNeeded == toolUsed) {
            tended = true;
            spriteRenderer.sprite = sprites[1];
            Debug.Log("Plant tended");
        } else {
            spriteRenderer.sprite = sprites[2];
            Debug.Log("Plant destroyed");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Character>()) {
            other.GetComponent<Character>().candidatePlants.Add(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.GetComponent<Character>()) {
            other.GetComponent<Character>().candidatePlants.Remove(this);
        }
    }
}
