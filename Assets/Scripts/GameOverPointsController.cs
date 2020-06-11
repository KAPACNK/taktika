using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPointsController : MonoBehaviour
{

    public Text points;
    // Start is called before the first frame update
    void Start()
    {
        points.text = "points: " + Convert.ToString(PlayerPrefs.GetInt("points"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
