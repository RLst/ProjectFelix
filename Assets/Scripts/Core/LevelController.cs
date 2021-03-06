﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    float levelMaxTime = 60;
    float levelTime;
    
    public Transform winBar;
    void Start()
    {
        levelTime = levelMaxTime;
        resizeWinBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Wall.current.wallCollapsed) {
            levelTime = levelTime - Time.deltaTime;
        }

        if (levelTime <= 0) {
            Wall.current.won = true;
        }
        
        resizeWinBar();
    }

    void resizeWinBar() {
        winBar.localScale = new Vector3(16*(levelTime/levelMaxTime), 0.2f, 0.2f);
    }
}
