using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSuperviser : MonoBehaviour
{

    public GameObject[] plants;
    public GameObject[] tools;
    public GameObject herbarium;
    public Text timerDisplay;
    public GameObject arrosoirUI;
    public GameObject sprayUI;
    public GameObject shovelUI;

    public GameObject belladonnaUI;
    public GameObject cannabisUI;
    public GameObject cathaUI;
    public GameObject colchicumUI;
    public GameObject leucanthemumUI;
    public GameObject ricinusUI;
    public GameObject veratrumUI;

    public float timer = 0f;

    public Character[] characters;

    public (int, 
        int, 
        PickupItem.ItemType,
        int,
        PickupItem.ItemType,
        int,
        PickupItem.ItemType,
        int,
        PickupItem.ItemType,
        int,
        PickupItem.ItemType,
        int,
        PickupItem.ItemType,
        int,
        PickupItem.ItemType)[] config;

    private int currentLevel;
    public bool gameover = false;
    private bool timerRunning = false;
    private bool gamePaused = false;
    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 1;
        config = new (int,
        int,
        PickupItem.ItemType,
        int,
        PickupItem.ItemType,
        int,
        PickupItem.ItemType,
        int,
        PickupItem.ItemType,
        int,
        PickupItem.ItemType,
        int,
        PickupItem.ItemType,
        int,
        PickupItem.ItemType)[3];
        config[0] = (0, 0, PickupItem.ItemType.Shovel, 1, PickupItem.ItemType.Shovel, 1, PickupItem.ItemType.Shovel, 1, PickupItem.ItemType.Shovel, 1, PickupItem.ItemType.Shovel, 1, PickupItem.ItemType.Shovel, 1, PickupItem.ItemType.Shovel);
        config[1] = (0, 1, PickupItem.ItemType.Arrosoir, 1, PickupItem.ItemType.Shovel, 1, PickupItem.ItemType.Shovel, 1, PickupItem.ItemType.Shovel, 1, PickupItem.ItemType.Shovel, 1, PickupItem.ItemType.Shovel, 1, PickupItem.ItemType.Shovel);
        config[2] = (0, 3, PickupItem.ItemType.Spray, 1, PickupItem.ItemType.Shovel, 1, PickupItem.ItemType.Shovel, 1, PickupItem.ItemType.Shovel, 1, PickupItem.ItemType.Shovel, 1, PickupItem.ItemType.Shovel, 1, PickupItem.ItemType.Shovel);

        StartCoroutine(Game());
    }

    // Update is called once per frame
    void Update()
    {
        if(timerRunning) {
            if (timer > 0f) {
                timerDisplay.text = timer.ToString("00") + ":" + ((timer * 100f) % 100f).ToString("00");
                timer -= Time.unscaledDeltaTime;
            } else {
                timerRunning = false;
                timer = 0f;
                timerDisplay.text = "00:00";
            }
        }
        
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(gamePaused) {
                UnpauseGame();
            } else {
                PauseGame();
            }
        }
    }

    private IEnumerator Game() {
        currentLevel = 1;
        while(!gameover) {

            //Setup Herbarium
            int catNum;
            int belladonnaNum, cannabisNum, cathaNum, colchicumNum, leucanthemumNum, ricinusNum, veratrumNum;
            PickupItem.ItemType belladonnaTool, cannabisTool, cathaTool, colchicumTool, leucanthemumTool, ricinusTool, veratrumTool;
            (catNum,
                belladonnaNum,
                belladonnaTool,
                cannabisNum,
                cannabisTool,
                cathaNum,
                cathaTool,
                colchicumNum,
                colchicumTool,
                leucanthemumNum,
                leucanthemumTool,
                ricinusNum,
                ricinusTool,
                veratrumNum, 
                veratrumTool) = config[currentLevel - 1];


            //Setup Herbarium
            if(belladonnaNum > 0) {
                belladonnaUI.SetActive(true);
                if (belladonnaTool == PickupItem.ItemType.Arrosoir) {
                    belladonnaUI.transform.position = new Vector3(arrosoirUI.transform.position.x, belladonnaUI.transform.position.y, belladonnaUI.transform.position.z);
                } else if (belladonnaTool == PickupItem.ItemType.Shovel) {
                    belladonnaUI.transform.position = new Vector3(shovelUI.transform.position.x, belladonnaUI.transform.position.y, belladonnaUI.transform.position.z);
                } else {
                    belladonnaUI.transform.position = new Vector3(sprayUI.transform.position.x, belladonnaUI.transform.position.y, belladonnaUI.transform.position.z);
                }
            } else {
                belladonnaUI.SetActive(false);
            }

            //First Instantiate Flowers
            //Move tools not held around
            //Setup Herborium
            //Display Herborium/pause
            PauseGame();
            yield return new WaitWhile(() => gamePaused);
            timer = 5f;
            timerRunning = true;
            yield return new WaitWhile(() => timerRunning);
            yield return new WaitForSecondsRealtime(0.2f);
            currentLevel++;
            if (currentLevel > config.Length) {
                gameover = true;
                break;
            }
        }
        yield return new WaitForSecondsRealtime(0.2f);

        


    }

    private void PauseGame() {
        herbarium.SetActive(true);
        gamePaused = true;
        Time.timeScale = 0f;
    }

    private void UnpauseGame() {
        herbarium.SetActive(false);
        gamePaused = false;
        Time.timeScale = 1f;
    }
}
