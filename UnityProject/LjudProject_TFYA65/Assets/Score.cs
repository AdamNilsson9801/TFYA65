using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI text;

    private float score;

    void Update()
    {
        score += Time.deltaTime;
        text.text = Mathf.FloorToInt(score).ToString();
    }
}
