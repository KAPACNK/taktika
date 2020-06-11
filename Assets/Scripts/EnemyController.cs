using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    public int target = 0;
    public Transform exit;
    public Transform[] wayPoints;
    public float navigation;
    public int hp = 1;
    public bool isDead = false;
    public int goldPerDeath = 1;
    public int damage = 1;

    public Text hpUI;

    private Transform enemy;
    private float navigationTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        damage = goldPerDeath = hp = Manager.Instance.waveNuber;
        enemy = GetComponent<Transform>();
        Manager.Instance.RegisterEnemy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            Death();
        }

        if (wayPoints != null)
        {
            navigationTime += Time.deltaTime;            
            if (navigationTime > navigation)
            {
                if (target < wayPoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, wayPoints[target].position, navigationTime);
                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exit.position, navigationTime);
                }
                navigationTime = 0;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyMovePoint")
        {
            target++;
        }
        else if (collision.tag == "Finish")
        {
            Damage();
            Manager.Instance.UnregisterEnemy(this);
            Destroy(gameObject);
        }
    }

    void Death() {
        Manager.Instance.gold += goldPerDeath;
        Manager.Instance.points++;
        Manager.Instance.UnregisterEnemy(this);
        Destroy(gameObject);
    }

    void Damage() {
        Manager.Instance.hp -= damage;
    }
}
