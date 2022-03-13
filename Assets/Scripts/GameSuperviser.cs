using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSuperviser : MonoBehaviour
{

    public GameObject[] plants;
    public GameObject[] enemies;
    public PickupItem[] tools;
    public GameObject herbarium;
    public Text timerDisplay;
    public Text levelDisplay;
    public GameObject gameoverDisplay;
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

    public List<GameObject> plantsPlanted;
    public List<GameObject> enemiesPlanted;

    public float timer = 0f;

    public Character[] characters;

    public (float,
        int, 
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
    private bool allPlantsTended = false;
    private GameObject lastCreated;
    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 1;
        config = new (float,
        int,
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
        PickupItem.ItemType)[5];
        config[0] = (60f, 0, 3, PickupItem.ItemType.Arrosoir, 4, PickupItem.ItemType.Spray, 0, PickupItem.ItemType.Shovel, 0, PickupItem.ItemType.Shovel, 0, PickupItem.ItemType.Shovel, 0, PickupItem.ItemType.Shovel, 0, PickupItem.ItemType.Shovel);
        config[1] = (60f, 0, 3, PickupItem.ItemType.Arrosoir, 4, PickupItem.ItemType.Spray, 3, PickupItem.ItemType.Spray, 2, PickupItem.ItemType.Arrosoir, 0, PickupItem.ItemType.Shovel, 0, PickupItem.ItemType.Shovel, 0, PickupItem.ItemType.Shovel);
        config[2] = (45f, 0, 1, PickupItem.ItemType.Spray, 1, PickupItem.ItemType.Shovel, 1, PickupItem.ItemType.Arrosoir, 1, PickupItem.ItemType.Spray, 1, PickupItem.ItemType.Shovel, 0, PickupItem.ItemType.Shovel, 0, PickupItem.ItemType.Shovel);
        config[3] = (45f, 0, 3, PickupItem.ItemType.Spray, 2, PickupItem.ItemType.Shovel, 3, PickupItem.ItemType.Shovel, 4, PickupItem.ItemType.Shovel,3, PickupItem.ItemType.Arrosoir, 1, PickupItem.ItemType.Shovel, 1, PickupItem.ItemType.Shovel);
        config[4] = (30f, 0, 3, PickupItem.ItemType.Arrosoir, 2, PickupItem.ItemType.Shovel, 2, PickupItem.ItemType.Arrosoir, 1, PickupItem.ItemType.Shovel, 3, PickupItem.ItemType.Arrosoir, 2, PickupItem.ItemType.Spray, 4, PickupItem.ItemType.Spray);




        StartCoroutine(Game());
    }

    // Update is called once per frame
    void Update()
    {
        if(timerRunning) {
            bool accTended = true;
            foreach(GameObject plant in plantsPlanted) {
                accTended = accTended && plant.GetComponent<Plant>().tended;
            }
            allPlantsTended = accTended;
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

            //Setup Variables
            timer = 0f;
            float levelTimer;
            int catNum;
            int belladonnaNum, cannabisNum, cathaNum, colchicumNum, leucanthemumNum, ricinusNum, veratrumNum;
            PickupItem.ItemType belladonnaTool, cannabisTool, cathaTool, colchicumTool, leucanthemumTool, ricinusTool, veratrumTool;
            (levelTimer,
                catNum,
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

            //Display level
            levelDisplay.text = "Level " + currentLevel;

            //Setup Herbarium
            if (belladonnaNum > 0) {
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

            if (cannabisNum > 0) {
                cannabisUI.SetActive(true);
                if (cannabisTool == PickupItem.ItemType.Arrosoir) {
                    cannabisUI.transform.position = new Vector3(arrosoirUI.transform.position.x, cannabisUI.transform.position.y, cannabisUI.transform.position.z);
                } else if (cannabisTool == PickupItem.ItemType.Shovel) {
                    cannabisUI.transform.position = new Vector3(shovelUI.transform.position.x, cannabisUI.transform.position.y, cannabisUI.transform.position.z);
                } else {
                    cannabisUI.transform.position = new Vector3(sprayUI.transform.position.x, cannabisUI.transform.position.y, cannabisUI.transform.position.z);
                }
            } else {
                cannabisUI.SetActive(false);
            }

            if (cathaNum > 0) {
                cathaUI.SetActive(true);
                if (cathaTool == PickupItem.ItemType.Arrosoir) {
                    cathaUI.transform.position = new Vector3(arrosoirUI.transform.position.x, cathaUI.transform.position.y, cathaUI.transform.position.z);
                } else if (cathaTool == PickupItem.ItemType.Shovel) {
                    cathaUI.transform.position = new Vector3(shovelUI.transform.position.x, cathaUI.transform.position.y, cathaUI.transform.position.z);
                } else {
                    cathaUI.transform.position = new Vector3(sprayUI.transform.position.x, cathaUI.transform.position.y, cathaUI.transform.position.z);
                }
            } else {
                cathaUI.SetActive(false);
            }

            if (colchicumNum > 0) {
                colchicumUI.SetActive(true);
                if (colchicumTool == PickupItem.ItemType.Arrosoir) {
                    colchicumUI.transform.position = new Vector3(arrosoirUI.transform.position.x, colchicumUI.transform.position.y, colchicumUI.transform.position.z);
                } else if (colchicumTool == PickupItem.ItemType.Shovel) {
                    colchicumUI.transform.position = new Vector3(shovelUI.transform.position.x, colchicumUI.transform.position.y, colchicumUI.transform.position.z);
                } else {
                    colchicumUI.transform.position = new Vector3(sprayUI.transform.position.x, colchicumUI.transform.position.y, colchicumUI.transform.position.z);
                }
            } else {
                colchicumUI.SetActive(false);
            }

            if (leucanthemumNum > 0) {
                leucanthemumUI.SetActive(true);
                if (leucanthemumTool == PickupItem.ItemType.Arrosoir) {
                    leucanthemumUI.transform.position = new Vector3(arrosoirUI.transform.position.x, leucanthemumUI.transform.position.y, leucanthemumUI.transform.position.z);
                } else if (leucanthemumTool == PickupItem.ItemType.Shovel) {
                    leucanthemumUI.transform.position = new Vector3(shovelUI.transform.position.x, leucanthemumUI.transform.position.y, leucanthemumUI.transform.position.z);
                } else {
                    leucanthemumUI.transform.position = new Vector3(sprayUI.transform.position.x, leucanthemumUI.transform.position.y, leucanthemumUI.transform.position.z);
                }
            } else {
                leucanthemumUI.SetActive(false);
            }

            if (ricinusNum > 0) {
                ricinusUI.SetActive(true);
                if (ricinusTool == PickupItem.ItemType.Arrosoir) {
                    ricinusUI.transform.position = new Vector3(arrosoirUI.transform.position.x, ricinusUI.transform.position.y, ricinusUI.transform.position.z);
                } else if (ricinusTool == PickupItem.ItemType.Shovel) {
                    ricinusUI.transform.position = new Vector3(shovelUI.transform.position.x, ricinusUI.transform.position.y, ricinusUI.transform.position.z);
                } else {
                    ricinusUI.transform.position = new Vector3(sprayUI.transform.position.x, ricinusUI.transform.position.y, ricinusUI.transform.position.z);
                }
            } else {
               ricinusUI.SetActive(false);
            }

            if (veratrumNum > 0) {
                veratrumUI.SetActive(true);
                if (veratrumTool == PickupItem.ItemType.Arrosoir) {
                    veratrumUI.transform.position = new Vector3(arrosoirUI.transform.position.x, veratrumUI.transform.position.y, veratrumUI.transform.position.z);
                } else if (veratrumTool == PickupItem.ItemType.Shovel) {
                    veratrumUI.transform.position = new Vector3(shovelUI.transform.position.x, veratrumUI.transform.position.y, veratrumUI.transform.position.z);
                } else {
                    veratrumUI.transform.position = new Vector3(sprayUI.transform.position.x, veratrumUI.transform.position.y, veratrumUI.transform.position.z);
                }
            } else {
                veratrumUI.SetActive(false);
            }

            //Move tools not held around
            foreach(PickupItem tool in tools) {
                if(!tool.held) {
                    tool.transform.position = FindPos(38f, 20f);
                }
            }

            // Instantiate Plants
            for (int i = 0; i < belladonnaNum; i++) {
                lastCreated = Instantiate(plants[0], FindPos(38f, 20f), Quaternion.identity);
                lastCreated.GetComponent<Plant>().toolNeeded = belladonnaTool;
                plantsPlanted.Add(lastCreated);
            }

            for (int i = 0; i < cannabisNum; i++) {
                lastCreated = Instantiate(plants[1], FindPos(38f, 20f), Quaternion.identity);
                lastCreated.GetComponent<Plant>().toolNeeded = cannabisTool;
                plantsPlanted.Add(lastCreated);
            }

            for (int i = 0; i < cathaNum; i++) {
                lastCreated = Instantiate(plants[2], FindPos(38f, 20f), Quaternion.identity);
                lastCreated.GetComponent<Plant>().toolNeeded = cathaTool;
                plantsPlanted.Add(lastCreated);
            }

            for (int i = 0; i < colchicumNum; i++) {
                lastCreated = Instantiate(plants[3], FindPos(38f, 20f), Quaternion.identity);
                lastCreated.GetComponent<Plant>().toolNeeded = colchicumTool;
                plantsPlanted.Add(lastCreated);
            }

            for (int i = 0; i < leucanthemumNum; i++) {
                lastCreated = Instantiate(plants[4], FindPos(38f, 20f), Quaternion.identity);
                lastCreated.GetComponent<Plant>().toolNeeded = leucanthemumTool;
                plantsPlanted.Add(lastCreated);
            }

            for (int i = 0; i < ricinusNum; i++) {
                lastCreated = Instantiate(plants[5], FindPos(38f, 20f), Quaternion.identity);
                lastCreated.GetComponent<Plant>().toolNeeded = ricinusTool;
                plantsPlanted.Add(lastCreated);
            }

            for (int i = 0; i < veratrumNum; i++) {
                lastCreated = Instantiate(plants[6], FindPos(38f, 20f), Quaternion.identity);
                lastCreated.GetComponent<Plant>().toolNeeded = veratrumTool;
                plantsPlanted.Add(lastCreated);
            }

            //Instantiate Enemies
            for (int i = 0; i < catNum; i++) {
                enemiesPlanted.Add(Instantiate(enemies[0], FindPos(38f, 20f), Quaternion.identity));
            }

            //Pause ready to start
            PauseGame();
            yield return new WaitWhile(() => gamePaused);
            timer = levelTimer;
            timerRunning = true;


            //Wait for end of level
            yield return new WaitUntil(() => !timerRunning || gameover || allPlantsTended);
            yield return new WaitForSecondsRealtime(0.2f);
            
            
            currentLevel++;
            if (!allPlantsTended) {
                gameover = true;
                gameoverDisplay.SetActive(true);
                break;
            }
            if (currentLevel > config.Length) {
                gameover = true;
                gameoverDisplay.SetActive(true);
                break;
            }

            //Clear the scene
            foreach (GameObject plant in plantsPlanted) {
                Destroy(plant);
            }
            foreach (GameObject enemy in enemiesPlanted) {
                Destroy(enemy);
            }
            plantsPlanted.Clear();
        }


        PauseGame();
        herbarium.SetActive(false);
        yield return new WaitWhile(() => gamePaused);
        Application.Quit();
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

    public Vector3 FindPos(float boundsX, float boundsY) {
        Vector2 probbedPos = Vector2.zero;
        for (int i = 0; i < 8; i++) {
            probbedPos = new Vector2((-boundsX / 2) + (Random.value * boundsX), (-boundsY / 2) + (Random.value * boundsY));
            RaycastHit2D hit = Physics2D.CircleCast(probbedPos, 1.5f, Vector2.up, 0f);
            if (!hit) {
                return probbedPos;
            }
        }
        return probbedPos;   
    }


}
