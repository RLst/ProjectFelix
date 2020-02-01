using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : MonoBehaviour
{
    public BrickBehaviour brickPrefab;
    public Collider brickColliderPrefab;
    public Mesh[] brickMeshes;

    public BrickBehaviour[,] brickPositions;
    public Collider[,] brickColliderPositions;
    
    [SerializeField]float nBricks = 0;

    public float brickWidth = 1;
    public float brickHeight = 0.4f;

    public int nBricksTall = 20;
    public int nBricksWide = 18;
    // Start is called before the first frame update
    void Start()
    {
        brickPositions = new BrickBehaviour[nBricksWide, nBricksTall];
        brickColliderPositions = new Collider[nBricksWide, nBricksTall];
        
        generateWall();

        //getBrickAtPos(new Vector2(-1f, 5f)).removeBrick();

        //Vector3 fistPos = new Vector3(-1f, 3f, 1);
        //float fistRad = 1f;
        //foreach(BrickBehaviour brick in getBricksInRadius(fistPos, fistRad)) {
            //brick.removeBrick(fistPos, fistRad);
        //}
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
                spawnBrick(new Vector2(xPos, yPos), new Vector2(j, i));
            }
        }
    }

    public BrickBehaviour spawnBrick(Vector2 pos, Vector2 arrayCoords) {
        int prefabN = Random.Range(0, 3);
        BrickBehaviour brick = Instantiate(brickPrefab, new Vector3(transform.position[0] + pos[0], transform.position[1] + pos[1], transform.position[2]), Quaternion.identity, transform);
        
        Collider brickCollider = Instantiate(brickColliderPrefab, new Vector3(transform.position[0] + pos[0], transform.position[1] + pos[1], transform.position[2]), Quaternion.identity, transform);

        brickColliderPositions[(int)arrayCoords[0],(int)arrayCoords[1]] = brickCollider;
        brickCollider.enabled = false;

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

    public BrickBehaviour getBrickAtPos(Vector2 pos) {
        pos = pos - (Vector2)transform.position;
        int i = (int)Mathf.Round((pos[1]/brickHeight) - 0.5f);
        int j = (int)(Mathf.Round((pos[0]/brickWidth) - 0.5f) + Mathf.Floor(nBricksWide/2));
        if (i%2 == 1) {
            j = (int)(Mathf.Round((pos[0]/brickWidth - brickWidth/2) - 0.5f) + Mathf.Floor(nBricksWide/2));
        }

        if (i >= 0 && i < nBricksTall && j >= 0 && j < nBricksWide) {
            //print(j + " : " + i);
            BrickBehaviour brick = brickPositions[j,i];

            //if (brick != null) {
            //    brick.removeBrick();
            //}

            return brick;
        } else {
            return null;
        }
    }
    
    public List<BrickBehaviour> getBricksInRadius(Vector2 pos, float radius) {
        List<BrickBehaviour> bricks = new List<BrickBehaviour>();
        for (int j = 0; j < nBricksWide; j++) {
            for (int i = 0; i < nBricksTall; i++) {
                BrickBehaviour brick = brickPositions[j,i];
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

        brickColliderPositions[(int)brickCoords[0], (int)brickCoords[1]].enabled = true;
    }

    public void insertBrick(Vector2 brickCoords, BrickBehaviour brick) {
        if (brickPositions[(int)brickCoords[0], (int)brickCoords[1]] != null) {
            brickPositions[(int)brickCoords[0], (int)brickCoords[1]] = brick;
            brickColliderPositions[(int)brickCoords[0], (int)brickCoords[1]].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}