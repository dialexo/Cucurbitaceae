using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public PickupItem.ItemType toolNeeded;
    //public AudioSource audio;
    public bool tended;

    // Start is called before the first frame update
    void Start()
    {
        tended = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Tend(PickupItem.ItemType toolUsed) {
        tended = true;
        if(toolNeeded == toolUsed) {
            Debug.Log("Plant tended");
        } else {
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
