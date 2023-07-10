using System;
using System.Transactions;
using UnityEngine;
using UnityEngine.UIElements;


public class UIManager : MonoBehaviour
{
    UIDocument ui;
    Label score;
    Label highScore;
    Label actual;
    Label next;
    ProgressBar slider;

    public Transform topTransform;
    public Transform goalTransform;

    public Transform ball;
   
    void OnEnable()
   {
      ui = GetComponent<UIDocument>();
      VisualElement root = ui.rootVisualElement;
      score = root.Q<Label>("score");
      highScore = root.Q<Label>("highScore");
      actual = root.Q<Label>("Actual");
      next = root.Q<Label>("Next");
      slider = root.Q<ProgressBar>("Progres");


   }
    void Update()
   {
      score.text =  GameManager.singleton.currentScore.ToString();

      highScore.text = GameManager.singleton.bestScore.ToString();

      ChangeSliderLevelAndProgress();
   
   }

   public void ChangeSliderLevelAndProgress()
   {
      actual.text = (GameManager.singleton.currentLevel+ 1).ToString();
      next.text = (GameManager.singleton.currentLevel + 2).ToString();

      float totalDistance = (topTransform.position.y - goalTransform.position.y);

      float distanceLeft = totalDistance - (ball.position.y - goalTransform.position.y);

      float value = distanceLeft / totalDistance;

      slider.value = Mathf.Lerp(slider.value, value,5);

   }
   


    
     
}