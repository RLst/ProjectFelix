using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : MonoBehaviour
{
    public BrickBehaviour brickPrefab;

    private BrickBehaviour[,] brickPositions;
    
    float brickWidth = 1;
    float brickHeight = 0.4f;

    int nBricksTall = 15;
    int nBricksWide = 10;
    // Start is called before the first frame update
    void Start()
    {
        brickPositions = new BrickBehaviour[nBricksWide, nBricksTall];
        
        generateWall();

        getBrickAtPos(new Vector2(-1f, 0.6f));
    }

    void generateWall() {
        for (int i = 0; i<nBricksTall; i++) {
            int nBricksInLayer = nBricksWide;
            float startX = -brickWidth*(nBricksInLayer/2) + brickWidth/2;
            float yPos = brickHeight*i + brickHeight/2;
            bool rotateEdges = true;
            if (i%2 == 1) {
                nBricksInLayer = nBricksInLayer - 1;
                startX = startX + brickWidth/2;
                rotateEdges = false;
            }
            for (int j = 0; j<nBricksInLayer; j++) {
                float xPos = startX + brickWidth*j;

                Quaternion angle = Quaternion.identity;

                if (rotateEdges && (j == 0 || j == nBricksInLayer - 1)) {
                    //angle = Quaternion.Euler(0, 90, 0);
                }
                BrickBehaviour brick = Instantiate(brickPrefab, new Vector3(transform.position[0] + xPos, transform.position[1] + yPos, transform.position[2]), angle, transform);

                brickPositions[j,i] = brick;
            }
        }
    }

    public BrickBehaviour getBrickAtPos(Vector2 pos) {
        pos = pos - (Vector2)transform.position;
        int i = (int)Mathf.Round((pos[1]/brickHeight) - 0.5f);
        int j = (int)(Mathf.Round((pos[0]/brickWidth) - 0.5f) + Mathf.Floor(nBricksWide/2));
        if (i%2 == 1) {
            j = (int)(Mathf.Round((pos[0]/brickWidth + brickWidth/2) - 0.5f) + Mathf.Floor(nBricksWide/2));
        }

        print(j + " : " + i);
        if (i > 0 && i < nBricksTall && j > 0 && j < nBricksTall) {
            return brickPositions[j,i];
        } else {
            return null;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}