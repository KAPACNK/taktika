using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerController : MonoBehaviour
{

    public float timeBetweenAttack;

    public LineRenderer laser;

    private LineRenderer towerLaser;

    public EnemyController targetEnemy = null;
    public float attackRadius = 3f;

    public float attackTime;
    public bool canAttack = true;

    public int damage = 1;

    public int lvl = 1;

    public int upgradeGold = 1;

    //public Text levelUI;


    // Start is called before the first frame update
    void Start()
    {
        towerLaser = Instantiate(laser) as LineRenderer;
    }

    

    // Update is called once per frame
    void Update()
    {

        if (!canAttack)
        {
            if (attackTime + timeBetweenAttack  <= Time.time)
            {
                canAttack = true;
            }
        }

        if (targetEnemy == null)
        {
            EnemyController nearstEnemy = GetNearstEnemy();
            if (nearstEnemy != null && Vector2.Distance(transform.localPosition, nearstEnemy.transform.localPosition) <= attackRadius)
            {
                targetEnemy = nearstEnemy;
            }
        }
        else
        {
            if (Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRadius)
            {
                targetEnemy = null;
            }
        }



    }

    void Laser() {
        if (targetEnemy != null) {
            towerLaser.SetPosition(0, transform.localPosition);
            towerLaser.SetPosition(1, targetEnemy.transform.localPosition);
            Invoke("StopLaser", 0.2f);
        }
    }

    void StopLaser() {
            towerLaser.SetPosition(1, transform.localPosition);
    }


    void OnClick()
    {
        Destroy(gameObject);
    }

    public void FixedUpdate()
    {

        if (canAttack && targetEnemy != null)
        {
            Attack();
        }
    }

    public void Attack()
    {
        Laser();
        if (canAttack) {
            attackTime = Time.time;
            targetEnemy.hp -= damage;
            Debug.DrawLine(transform.position, targetEnemy.transform.position, Color.red, 0.1f, false);
            canAttack = false;
        }
    }

    public void Upgrade() {
        if (Manager.Instance.gold >= upgradeGold) {
            Manager.Instance.gold -= upgradeGold;
            damage++;
            lvl++;
            upgradeGold += upgradeGold;
        }
    }


    private float GetTargetDistance(EnemyController thisEnemy)
    {
        if (thisEnemy == null)
        {
            thisEnemy = GetNearstEnemy();
            if (thisEnemy == null)
            {
                return 0f;
            }
        }
        return Mathf.Abs(Vector2.Distance(transform.localPosition, thisEnemy.transform.localPosition));
    }

    private List<EnemyController> GetEnemiesInRange()
    {
        List<EnemyController> enemiesInRange = new List<EnemyController>();

        foreach (EnemyController enemy in Manager.Instance.enemyList)
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRadius)
            {
                enemiesInRange.Add(enemy);
            }
        }

        return enemiesInRange;
    }

    private EnemyController GetNearstEnemy()
    {
        EnemyController nearstEnemy = null;
        float smallestDistance = float.PositiveInfinity;

        foreach (EnemyController enemy in GetEnemiesInRange())
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
                nearstEnemy = enemy;
            }
        }

        return nearstEnemy;
    }


}
