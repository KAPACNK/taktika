using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : Loader<Manager>
{
    public GameObject spawnPoint;
    public GameObject[] enemies;
    public Text UIgold;
    public Text UIhp;
    public Text UIpoints;
    public int gold = 0;
    public int hp = 5;
    public int points = 0;

    public TowerController selectedTower;
    public bool isSelectTower = false;

    public GameObject upgradeButton;
    public Text upgradeButtonText;

    public int waveNuber = 1;


    /* spawn rand enemies between waveNumber and waveNumber + X */
    private int X = 3;
    private float waveSpawnTime;
    private int timeBetweenWaves;

    private bool canSpawnWave = true;

    public List<EnemyController> enemyList = new List<EnemyController>();

    // Start is called before the first frame update
    void Start()
    {
        string json = System.IO.File.ReadAllText(Application.dataPath + "/level_config.json");
        Settings settings = JsonUtility.FromJson<Settings>(json);
        timeBetweenWaves = settings.timeBetweenWaves;

        int enemiesInWave = UnityEngine.Random.Range(waveNuber, waveNuber + X);
        StartCoroutine(SpawnWave(enemiesInWave));
    }

    private void Update()
    {
        if (selectedTower != null)
        {
            upgradeButtonText.text = "lvl: " + Convert.ToString(selectedTower.lvl) + "\nUpgrade: " + Convert.ToString(selectedTower.upgradeGold) + "  gold";
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.transform != null)
            {
                if (hit.transform.gameObject.tag == "Tower")
                {
                    selectedTower = hit.transform.gameObject.GetComponent<TowerController>();
                    isSelectTower = true;
                }
                else if (hit.transform.gameObject.tag == "UpgradeTowerButton")
                {
                    if (selectedTower != null)
                    {
                        isSelectTower = true;
                    }
                }
                else
                {
                    isSelectTower = false;
                }
            }
            else
            {
                isSelectTower = false;
            }
        }

        if (isSelectTower)
        {
            upgradeButton.SetActive(true);
        }
        else
        {
            upgradeButton.SetActive(false);
        }


        UIgold.text = Convert.ToString(gold);
        UIhp.text = Convert.ToString(hp);
        UIpoints.text = Convert.ToString(points);
        if (hp <= 0)
        {
            GameOver();
        }

        if (waveSpawnTime + timeBetweenWaves <= Time.time)
        {
            int enemiesInWave = UnityEngine.Random.Range(waveNuber, waveNuber + X);
            StartCoroutine(SpawnWave(enemiesInWave));
        }

    }

    IEnumerator SpawnWave(int enemiesInWave)
    {
        if (canSpawnWave)
        {
            waveNuber++;
            for (int i = 0; i < enemiesInWave; i++)
            {
                canSpawnWave = false;

                GameObject newEnemy = Instantiate(enemies[0]) as GameObject;
                newEnemy.transform.position = spawnPoint.transform.position;
                yield return new WaitForSeconds(1);
            }
        }
        waveSpawnTime = Time.time;
        canSpawnWave = true;
    }

    public void RegisterEnemy(EnemyController enemy)
    {
        enemyList.Add(enemy);
    }

    public void UnregisterEnemy(EnemyController enemy)
    {
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void DestroyEnemies()
    {
        foreach (EnemyController enemy in enemyList)
        {
            Destroy(enemy.gameObject);
        }

        enemyList.Clear();
    }

    public void GameOver()
    {
        PlayerPrefs.SetInt("points", points);
        SceneManager.LoadScene("GameOverScene");
    }
    public void UpgradeTower()
    {
        selectedTower.Upgrade();
    }

    private class Settings
    {
        public int timeBetweenWaves;
    }
}
