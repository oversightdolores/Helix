using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goalontroller : MonoBehaviour
{
    void OnCollisionEnter(Collision collisionInfo)
   {
    GameManager.singleton.NextLevel();
   }
}
