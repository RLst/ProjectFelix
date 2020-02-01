using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeBehaviour : MonoBehaviour
{
    float timer = 3;

    GiantBehaviour parentBehaviour;

    private void Awake()
    {
        parentBehaviour = transform.parent.GetComponent<GiantBehaviour>();
    }

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
                strike();
            }
        }
    }

    void strike() {
        Vector3 fistPos = new Vector3(transform.position[0], transform.position[1], 1);
        float strikeRad = 1f;

        foreach(Brick brick in parentBehaviour.wall.getBricksInRadius(transform.position, 1)) {
            brick.removeBrick(fistPos, strikeRad);
        };

        parentBehaviour.fist.strike(fistPos + new Vector3(0, 0, 1f));
    }
}
