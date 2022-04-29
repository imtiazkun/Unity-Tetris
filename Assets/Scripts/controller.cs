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
    public static Transform[,] grid = new Transform[width, height];



    public int points_on_line_completion = 100;

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
                gameObject.tag = "Inactive";
                AddToGrid();
                CheckGameStatus();
                CheckForLines();
                this.enabled = false;
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

            if (grid[roundedX, roundedY] != null) {
                return false;
            }
        }

        return true;
    }


    void AddToGrid () {
        foreach (Transform children in transform) {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundedX, roundedY] = children;
        }
    }

    void CheckGameStatus () {
        if (HasExceededHeight()) {
            FindObjectOfType<BlockSpawner>().StopInvoke();
            FindObjectOfType<BlockSpawner>().SetPraiseBoardMessage("Game Over!!");
        } else {
            FindObjectOfType<BlockSpawner>().Generate();
        }
    }

    /** 
    *   BOOLEAN FUNCTION
    *   check if block has a block below and above is height limit
    */
    bool HasExceededHeight () {
        foreach (Transform children in transform) {
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            if (roundedY >= height - 2) {
                if (gameObject.tag == "Inactive") {
                    return true;
                }
            }
        }
        return false;
    }

    void CheckForLines () {
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i)) {
                DeleteLine(i);
                RowDown(i);
                FindObjectOfType<BlockSpawner>().AddScore(points_on_line_completion);
            }
        }
    }


    // BOOLEAN FUNCTION
    /**
    *   - Checks through array if there is even a single empty cell
    *   - Returns true by default and only returns false if empty cell available
    */
    bool HasLine (int i) {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
            {
                return false;
            }
        }

        return true;
    }

    void DeleteLine (int i) {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j,i].gameObject);
            grid[j, i] = null;
        }
    }


    void RowDown (int i) {
        Debug.Log("Row going Down");
        for (int y = i; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null) {
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;
                    grid[x, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

}
