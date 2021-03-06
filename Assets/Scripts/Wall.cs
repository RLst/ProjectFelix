﻿ using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Brick brickPrefab;
    public Collider brickColliderPrefab;
    public Mesh[] brickMeshes;

    public Brick[,] brickPositions;
    public Collider[,] brickColliderPositions;
    
    [SerializeField]float nBricks = 0;

    public float brickWidth = 1;
    public float brickHeight = 0.4f;

    public int nBricksTall = 20;
    public int nBricksWide = 18;

    public static Wall current = null;

    public bool wallCollapsed = false;
    public bool won = false;

    private void Awake()
    {
        if (current == null) {
            current= this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        brickPositions = new Brick[nBricksWide, nBricksTall];
        brickColliderPositions = new Collider[nBricksWide, nBricksTall];
        
        generateWall();

        //getBrickAtPos(new Vector2(-1f, 5f)).removeBrick();

        //Vector3 fistPos = new Vector3(-1f, 3f, 1);
        //float fistRad = 1f;
        //foreach(BrickBehaviour brick in getBricksInRadius(fistPos, fistRad)) {
            //brick.removeBrick(fistPos, fistRad);
        //}
    }

    public void generateWall() {
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

                if (i >= 0 && i < nBricksTall && j >= 0 && j < nBricksWide) {
                    spawnBrick(new Vector2(xPos, yPos), new Vector2(j, i));
                } else {
                    //spawnBorder(new Vector2(xPos, yPos));
                }
            }
        }
    }

    public Brick spawnBrick(Vector2 pos, Vector2 arrayCoords) {
        int prefabN = Random.Range(0, 3);
        Brick brick = Instantiate(brickPrefab, new Vector3(transform.position[0] + pos[0], transform.position[1] + pos[1], transform.position[2]), Quaternion.identity, transform);
        
        //Collider brickCollider = Instantiate(brickColliderPrefab, new Vector3(transform.position[0] + pos[0], transform.position[1] + pos[1], transform.position[2]), Quaternion.identity, transform);

        //brickColliderPositions[(int)arrayCoords[0],(int)arrayCoords[1]] = brickCollider;
        //brickCollider.enabled = false;

        brick.wallPos = arrayCoords;
        brick.wall = this;

        brick.GetComponentInChildren<MeshFilter>().mesh = brickMeshes[Random.Range(0, brickMeshes.Length)];

        brickPositions[(int)arrayCoords[0],(int)arrayCoords[1]] = brick;

        if (Random.Range(0, 2) == 0) {
            brick.transform.Rotate(new Vector3(0, 0, 180));
        }
        if (Random.Range(0, 2) == 0) {
            brick.transform.Rotate(new Vector3(0, 180, 0));
        }
        if (Random.Range(0, 2) == 0) {
            brick.transform.Rotate(new Vector3(180, 0, 0));
        }

        nBricks = nBricks + 1;

        return brick;
    }

    public void spawnBorder(Vector2 pos) {
        Collider brickCollider = Instantiate(brickColliderPrefab, new Vector3(transform.position[0] + pos[0], transform.position[1] + pos[1], transform.position[2]), Quaternion.identity, transform);
    }

    public Brick getBrickAtPos(Vector2 pos) {
        pos = pos - (Vector2)transform.position;
        int i = (int)Mathf.Round((pos[1]/brickHeight) - 0.5f);
        int j = (int)(Mathf.Round((pos[0]/brickWidth) - 0.5f) + Mathf.Floor(nBricksWide/2));
        if (i%2 == 1) {
            j = (int)(Mathf.Round((pos[0]/brickWidth - brickWidth/2) - 0.5f) + Mathf.Floor(nBricksWide/2));
        }

        if (i >= 0 && i < nBricksTall && j >= 0 && j < nBricksWide) {
            //print(j + " : " + i);
            Brick brick = brickPositions[j,i];

            //if (brick != null) {
            //    brick.removeBrick();
            //}

            return brick;
        } else {
            return null;
        }
    }

    public Vector2 getPosAtBrickCoords(Vector2 brickCoords) {
        float x = brickWidth*(brickCoords[0] - nBricksWide/2) + brickWidth/2;
        float y = brickHeight*brickCoords[1] + brickHeight/2;
        if (brickCoords[1]%2 == 1) {
            x = x + 0.5f;
        }

        return new Vector2(x, y);
    }
    
    public List<Brick> getBricksInRadius(Vector2 pos, float radius) {
        List<Brick> bricks = new List<Brick>();
        for (int j = 0; j < nBricksWide; j++) {
            for (int i = 0; i < nBricksTall; i++) {
                Brick brick = brickPositions[j,i];
                if (brick != null) {
                    Vector2 diff = (Vector2)brick.transform.position - pos;
                    if (diff.magnitude <= radius) {
                        bricks.Add(brick);
                    }
                }
            }
        }
        return bricks;
    }

    public void removeBrick(Vector2 brickCoords) {
        brickPositions[(int)brickCoords[0], (int)brickCoords[1]] = null;
        nBricks = nBricks - 1;

        checkStability();
        //brickColliderPositions[(int)brickCoords[0], (int)brickCoords[1]].enabled = true;
    }

    public void insertBrick(Vector2 brickCoords, Brick brick) {
        if (brickPositions[(int)brickCoords[0], (int)brickCoords[1]] == null) {
            brickPositions[(int)brickCoords[0], (int)brickCoords[1]] = brick;

            Vector2 worldCoords = getPosAtBrickCoords(brickCoords);

            brick.transform.position = new Vector3(worldCoords[0], worldCoords[1], transform.position[2]);
            brick.transform.rotation = Quaternion.identity;

            brick.transform.SetParent(null);

            brick.reInsertBrick();
            //brickColliderPositions[(int)brickCoords[0], (int)brickCoords[1]].enabled = false;
        }

        nBricks = nBricks + 1;
        checkStability();
    }

    public bool insertBrickAtClosest(Vector2 position, float armLength, Brick insertingBrick) {
        bool brickFound = false;
        Vector2 closestBrick = new Vector2(0, 0);
        float closetBrickDist = -1;
        
        for (int j = 0; j < nBricksWide; j++) {
            for (int i = 0; i < nBricksTall; i++) {
                Brick brick = brickPositions[j,i];
                if (brick == null) {
                    Vector2 brickPos = getPosAtBrickCoords(new Vector2(j, i));

                    Vector2 diff = brickPos - position;
                    float dist = diff.magnitude;
                    if (diff.magnitude <= armLength) {
                        if (closetBrickDist == -1) {
                            brickFound = true;
                            closetBrickDist = dist;
                            closestBrick = new Vector2(j, i);
                        } else {
                            if (closetBrickDist > dist) {
                                brickFound = true;
                                closetBrickDist = dist;
                                closestBrick = new Vector2(j, i);
                            }
                        }
                    }
                }
            }
        }

        if (brickFound) {
            insertBrick(closestBrick, insertingBrick);
        }
        return brickFound;
    }

    int currentRocky = 0;
    void checkStability() {
        int newRocky = currentRocky;
        if (nBricks < 280) {
            for (int j = 0; j < nBricksWide; j++) {
                for (int i = 0; i < nBricksTall; i++) {
                    Brick brick = brickPositions[j,i];
                    if (brick != null) {
                        brick.removeBrick();
                    }
                }
            }
            wallCollapsed = true;
        } else if (nBricks < 290) {
            newRocky = 2;
        } else if (nBricks < 310) {
            newRocky = 1;
        }

        if (newRocky != currentRocky) {
            currentRocky = newRocky;

            foreach (Transform brickT in transform) {
                Brick brick = brickT.GetComponent<Brick>();

                brick.rocky = newRocky;
            }
        }
    }
}