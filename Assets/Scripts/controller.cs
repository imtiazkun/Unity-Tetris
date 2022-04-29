using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{

    public Vector3 rotationPoint;
    private float previous_time;
    private float fall_time = 0.8f;



    public static int width = 15;
    public static int height = 20;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W)) {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), 90);
            if (!IsMoveValid()) 
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), -90);

        }

        // Horizontal Controls
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!IsMoveValid()) 
                transform.position -= new Vector3(-1, 0, 0);
            
        } else if (Input.GetKeyDown(KeyCode.D)) {
            transform.position += new Vector3(1, 0, 0);
            if (!IsMoveValid()) 
                transform.position -= new Vector3(1, 0, 0);
        }

        // Falling
        if (Time.time - previous_time > (Input.GetKeyDown(KeyCode.S) ?  fall_time / 10 : fall_time)) {
            transform.position += new Vector3(0, -1, 0);
            if (!IsMoveValid()) {
                transform.position -= new Vector3(0, -1, 0);
                this.enabled = false;
                FindObjectOfType<BlockSpawner>().Generate();
            }
            previous_time = Time.time;
        }
    }


    bool IsMoveValid () {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
            {
                return false;
            }


            Debug.Log(roundedX +" "+ roundedY);
        }

        return true;
    }
}
