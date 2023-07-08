using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public long initialPopulation;
    public Vector2 boundary;
    public float startWait;
    public float waveWait;
    public int MaxWaves;

    public Text restartText;
    public Text gameOverText;
    public Text gameOverSubText;
    public Text waveText;
    public Text populationText;
    public Text enemiesRemaining;
    public Text warningText;
    public Text[] gameAssistanceTexts;

    public GameObject youWinEnemyExplosion;

    private int waveNumber;
    private bool gameOver;
    private int score;
    private bool restart;
    private long population;

    private WaveSettings currentWave;

    public float laserShotSpeed;
    public float rocketShotSpeed;
    public float lightningShotSpeed;

    private float modifiedLaserShotSpeed;
    private float modifiedRocketShotSpeed;
    private float modifiedLightningShotSpeed;
    private float lastWaveStart;

    static int retries = 0;

    public Dictionary<int, string> previouslyShownHelpTexts = new Dictionary<int, string>();
    
    // Use this for initialization
    void Start () {
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        gameOverSubText.text = "";
        waveText.text = "";
        warningText.text = "";
        enemiesRemaining.text = "";

        foreach (Text textObject in gameAssistanceTexts)
        {
            textObject.text = "";
        }
        
        population = initialPopulation;

        score = 0;
        waveNumber = 0;
        UpdatePopulation();
        
        GameObject waveSettingsObject = GameObject.FindWithTag("Wave0");
        if (waveSettingsObject != null)
        {
            currentWave = waveSettingsObject.GetComponent<WaveSettings>();
        }
        else
        {
            Debug.Log("Cannot find game controller script");
        }

        StartCoroutine(SpawnWave());
        InvokeRepeating("UpdateWeaponRates", 5, 1);

        modifiedLaserShotSpeed = laserShotSpeed;
        modifiedRocketShotSpeed = rocketShotSpeed;
        modifiedLightningShotSpeed = lightningShotSpeed;

        restart = true;
    }
    
    private void Update()
    {
        if (restart || retries == 0)
        {
            if (Input.GetKeyDown(KeyCode.R) || retries == 0)
            {
                retries++;
                Application.LoadLevel(Application.loadedLevel);
            }
        }     
        
        if (gameOverText.text != "")
        {
            gameOverText.transform.RotateAround(Vector3.up, Time.deltaTime);
        }
    }


    public void ShowMotherShipProximityAlert()
    {
        StartCoroutine(MotherShipAlert());
    }

    IEnumerator MotherShipAlert()
    {
        for (int i = 0; i < 10; i++)
        {
            warningText.text = "WARNING!! Mother Ship Detected on long range scanners!";

            yield return new WaitForSeconds(0.5f);

            warningText.text = "";

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ShowGameAssistance(int key, string value)
    {
        if (!previouslyShownHelpTexts.ContainsKey(key))
        {
            previouslyShownHelpTexts.Add(key, value);
            StartCoroutine(ShowAssistanceMessage(key, value));            
        }        
    }

    IEnumerator ShowAssistanceMessage(int key, string value)
    {
        // show the text for N seconds, then hide it
        gameAssistanceTexts[key-1].text = value;
        yield return new WaitForSeconds(10.0f);
        gameAssistanceTexts[key-1].text = "";        
    }

    private void InstantiateHazard()
    {
        GameObject hazard = currentWave.hazards[Random.Range(0, currentWave.hazards.Length)];

        Vector3 spawnPosition = Vector3.up;

        // must be on edge of boundary box (off screen)
        int origin = Random.Range(0, 4); // 4 possible values indicating left, top, right or bottom spawn point

        switch (origin)
        {
            case 0:
                // top
                spawnPosition = new Vector3(Random.Range(-boundary.x, boundary.x), 0, boundary.y);
                break;

            case 1:
                // bottom
                spawnPosition = new Vector3(Random.Range(-boundary.x, boundary.x), 0, -boundary.y);
                break;

            case 2:
                // left
                spawnPosition = new Vector3(-boundary.x, 0, Random.Range(-boundary.y, boundary.y));
                break;

            case 3:
                // right
                spawnPosition = new Vector3(boundary.x, 0, Random.Range(-boundary.y, boundary.y));
                break;
        }

        Quaternion spawnRotation = Quaternion.identity;

        Instantiate(hazard, spawnPosition, spawnRotation);
    }
    
    IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(startWait);
        
        while (currentWave != null)
        {
            waveText.text = "";

            Debug.Log("Initiating hazards for wave # " + waveNumber);

            for (int i = 0; i < (currentWave.numHazards) && !gameOver; i++)
            {
                InstantiateHazard();
                yield return new WaitForSeconds(currentWave.spawnWait);
            }

            if (gameOver)
            {
                break;
            }

            waveNumber++;
            updateWaveCount();

            while (true)
            {
                if (gameOver)
                {
                    currentWave = null;
                    break;
                }
                else
                {
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

                    enemiesRemaining.text = "Enemies Remaining: " + enemies.Length;                    

                    Debug.Log("Active Hazard count: " + enemies.Length);

                    if (enemies.Length == 0 || Time.time > (lastWaveStart + currentWave.waveDurationInSeconds))
                    {
                        lastWaveStart = Time.time;

                        if (waveNumber < MaxWaves)
                        {
                            string waveName = "Wave" + waveNumber;

                            Debug.Log("Starting Wave: " + waveName);

                            GameObject waveSettingsObject = GameObject.FindWithTag(waveName);
                            currentWave = waveSettingsObject.GetComponent<WaveSettings>();
                        }
                        else
                        {
                            YouWin();
                            currentWave = null;
                            break;
                        }
                        
                        break;
                    }
                }

                yield return new WaitForSeconds(1);
            }
        }

        waveText.text = "";
        restartText.text = "Press 'R' to Restart";
        restart = true;        
    }

    public void ReducePopulation(long minAmt, long maxAmt)
    {
        long deaths = (long)Random.Range(minAmt, maxAmt);

        population -= deaths;
        population = (long)Mathf.Max(0, population);

        UpdatePopulation();

        if (population == 0)
        {
            GameOver();
        }
    }

    void updateWaveCount()
    {
        waveText.text = "Wave " + waveNumber;
    }
    
    void UpdatePopulation()
    {
        populationText.text = "Population: " + string.Format("{0:n0}", population);
    }
    
    public void YouWin()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            Instantiate(youWinEnemyExplosion, enemy.transform.position, enemy.transform.rotation);
            Destroy(enemy);
        }

        enemiesRemaining.text = "Enemies Remaining: 0";

        gameOverText.text = "You win!!";
        
        gameOverSubText.text = "You saved " + string.Format("{0:n0}", population) + " souls from extinction.";
        gameOver = true;
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over";
        gameOver = true;
    }

    public int WaveNumber()
    {
        return waveNumber;
    }

    public float GetLaserShotSpeed()
    {
        return modifiedLaserShotSpeed;
    }

    public float GetRocketShotSpeed()
    {
        return modifiedRocketShotSpeed;
    }

    public float GetLightningShotSpeed()
    {
        return modifiedLightningShotSpeed;
    }

    void UpdateWeaponRates()
    {        
        // each second, update the weapon fire rates depending on currently active upgrades (that haven't expired yet)
        GameObject[] laserUpgrades = GameObject.FindGameObjectsWithTag("LaserFireRate");
        int totalLaserUpgrades = laserUpgrades.Length;
        modifiedLaserShotSpeed = laserShotSpeed;
        for (int i = 0; i < totalLaserUpgrades; i++)
        {
            modifiedLaserShotSpeed *= 0.8f;
        }

        GameObject[] rocketUpgrades = GameObject.FindGameObjectsWithTag("RocketFireRate");
        int totalRocketUpgrades = rocketUpgrades.Length;
        modifiedRocketShotSpeed = rocketShotSpeed;        
        for (int i = 0; i < totalRocketUpgrades; i++)
        {
            modifiedRocketShotSpeed *= 0.8f;
        }
        
        GameObject[] lightningUpgrades = GameObject.FindGameObjectsWithTag("LightningFireRate");
        int totalLightningUpgrades = lightningUpgrades.Length;
        modifiedLightningShotSpeed = lightningShotSpeed;
        for (int i = 0; i < totalLightningUpgrades; i++)
        {
            modifiedLightningShotSpeed *= 0.8f;
        }

        Debug.Log("Weapon Rates-> Lasers: " + totalLaserUpgrades);
    }    
}
