using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeBehaviour : MonoBehaviour
{
    float timer = 3;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0) {
            timer = timer - Time.deltaTime;

            transform.localScale = new Vector3(timer/3, timer/3, timer/3);
            if (timer <= 0) {
                Vector3 fistPos = new Vector3(transform.position[0], transform.position[1], 1);
                float fistRad = 1f;

                foreach(BrickBehaviour brick in transform.parent.GetComponent<GiantBehaviour>().wall.getBricksInRadius(transform.position, 1)) {
                    brick.removeBrick(fistPos, fistRad);
                };
            }
        }
    }
}
