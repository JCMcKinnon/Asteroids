using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayScore : MonoBehaviour
{
    // Start is called before the first frame update
    public TMPro.TextMeshProUGUI tmp;
    public ScoreManager sm;
    void Start()
    {
        tmp = GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        tmp.text = sm.points.ToString();
    }
}
