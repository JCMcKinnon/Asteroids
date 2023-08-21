using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer sr1;
    public SpriteRenderer sr2;
    public SpriteRenderer sr3;
    public SpriteRenderer sr4;

    public SpriteRenderer[] srs;

    void Start()
    {
        srs = new SpriteRenderer[3];
        srs[0] = sr1;
        srs[1] = sr2;
        srs[2] = sr4;


        InvokeRepeating("TurnSROff", 0.2f,0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void TurnSROff()
    {
       var index = Random.Range(0, 2);
        var otherIndex = Random.Range(0, 2);

        srs[index].enabled = false;

        if(otherIndex!= index)
        {
            srs[otherIndex].enabled = true;
        }
        else
        {
             otherIndex = Random.Range(0, 2);
            
        }


    }
    void TurnSROn()
    {
        var index = Random.Range(0, 3);
        srs[index].enabled = true;

    }
}
