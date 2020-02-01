using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistBehaviour : MonoBehaviour
{
    private bool struck = false;
    private bool withdrawing = false;
    private float withdrawTimer = 0;
    private float stopWithdrawTimer = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (struck) {
            if (withdrawTimer > 0) {
                withdrawTimer = withdrawTimer - Time.deltaTime;

                if (withdrawTimer <= 0) {
                    withdrawing = true;
                    stopWithdrawTimer = 1;
                }
            }
        }
        
        if (withdrawing) {
            transform.position = transform.position + new Vector3(0, 0, 2*Time.deltaTime);

            stopWithdrawTimer = stopWithdrawTimer - Time.deltaTime;
            if (stopWithdrawTimer < 0) {
                withdrawing = false;
            }
        }
    }

    public void strike(Vector3 pos) {
        transform.position = pos;

        struck = true;
        withdrawTimer = 1;
        withdrawing = false;
    }
}
