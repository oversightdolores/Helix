
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassScorePoint : MonoBehaviour
{

     private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

   void OnTriggerEnter(Collider other)
   {
    GameManager.singleton.AddScore(1);
     Object.FindAnyObjectByType<BallController>().perfectPass ++;
   }

    public void ResetEllix()
    {
        transform.position = startPosition;
        
    }
}
