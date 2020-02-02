using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantBehaviour : MonoBehaviour
{
    public Wall wall;

    public StrikeBehaviour strikePrefab;
    public FistBehaviour fist;
    public Transform giantTransform;

    float timer = 0;
    float maxTime = 4;

    float targetRotate = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Wall.current.wallCollapsed && !Wall.current.won) {
            timer = timer - Time.deltaTime;
        }

        if (Wall.current.won) {
            giantTransform.position = new Vector3(giantTransform.position[0], giantTransform.position[1], giantTransform.position[2] + Time.deltaTime*10);
        }

        if (timer <= 0) {
            timer = maxTime;
            
            spawnStrike();
        }

        float rotateSpeed = 30;
        float parentangle = giantTransform.rotation.eulerAngles[2];

        if (parentangle > 180) {
            parentangle = parentangle - 360;
        }

        if (targetRotate != parentangle) {
            if (parentangle < targetRotate) {
                float newRotate = parentangle + Time.deltaTime*rotateSpeed;

                if (newRotate > targetRotate) {
                    newRotate = targetRotate;
                }
                giantTransform.rotation = Quaternion.Euler(0, 0, newRotate);
            } else {
                float newRotate = parentangle - Time.deltaTime*rotateSpeed;

                //print(newRotate + " : " + targetRotate);
                if (newRotate < targetRotate) {
                    newRotate = targetRotate;
                }
                giantTransform.rotation = Quaternion.Euler(0, 0, newRotate);
            }
        }
    }

    void spawnStrike() {
        bool posFound = false;
        int count = 0;
        while (!posFound) {
            count = count + 1;
            Vector2 pos = new Vector2(wall.transform.position[0] + Random.Range(-wall.nBricksWide*wall.brickWidth/2 + 2, wall.nBricksWide*wall.brickWidth/2 - 2), wall.transform.position[1] + Random.Range(2, wall.nBricksWide*wall.brickHeight));

            targetRotate = -45*(pos[0]/(wall.nBricksWide*wall.brickWidth/2));
            //print(targetRotate);

            if (wall.getBrickAtPos(pos) != null) {
                posFound = true;

                StrikeBehaviour strike = Instantiate(strikePrefab, new Vector3(pos[0], pos[1], -0.5f), Quaternion.identity, transform);
            } else {
                print("No brick found");
            }

            if (count >= 10) {
                break;
            }
        }
    }
}
