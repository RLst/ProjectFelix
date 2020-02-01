using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantBehaviour : MonoBehaviour
{
    public Wall wall;

    public StrikeBehaviour strikePrefab;
    public FistBehaviour fist;

    float timer = 0;
    float maxTime = 4;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer = timer - Time.deltaTime;
        if (timer <= 0) {
            timer = maxTime;
            
            spawnStrike();
        }
    }

    void spawnStrike() {
        bool posFound = false;
        while (!posFound) {
            Vector2 pos = new Vector2(wall.transform.position[0] + Random.Range(-wall.nBricksWide*wall.brickWidth/2 + 2, wall.nBricksWide*wall.brickWidth/2 - 2), wall.transform.position[1] + Random.Range(2, wall.nBricksWide*wall.brickHeight));

            if (wall.getBrickAtPos(pos) != null) {
                posFound = true;

                Instantiate(strikePrefab, new Vector3(pos[0], pos[1], -0.5f), Quaternion.identity, transform);
            } else {
                print("No brick found");
            }
        }
    }
}
