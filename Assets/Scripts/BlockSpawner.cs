using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockSpawner : MonoBehaviour
{
    public GameObject[] blocks;

    public int Score = 0;
    public GameObject ScoreDisplay;
    public GameObject PraiseBoard;
    // Start is called before the first frame update
    void Start()
    {
        Generate();
        InvokeRepeating("TimeScore", 2.0f, 0.3f);
    }

    public void StopInvoke () {
        CancelInvoke();
    }

    public void Generate () {
        Instantiate(blocks[Random.Range(0, blocks.Length)], transform.position, Quaternion.identity);
    }

    void TimeScore () {
        AddScore(5);
    }

    public void AddScore (int amount) {
        int hundredth = 0;
        int thousandth = 0;

        Score += amount;
        
        hundredth = Score;
        thousandth = Score;

        ScoreDisplay.GetComponent<TextMeshPro>().text = Score.ToString();
        
        if (hundredth == 100) {
            PraiseBoard.GetComponent<TextMeshPro>().text = "Going Smooth!!";
            hundredth = 0;
        } else if (thousandth == 1000) {
            PraiseBoard.GetComponent<TextMeshPro>().text = "Going Big!!";
            thousandth = 0;
        } else {
            PraiseBoard.GetComponent<TextMeshPro>().text = "Great Going!!";
        }
    }


    public void SetPraiseBoardMessage (string message) {
        PraiseBoard.GetComponent<TextMeshPro>().text = message;
    }
}
