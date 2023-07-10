using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellixController : MonoBehaviour
{
    private Vector2 lastTapPosition;
    private Vector3 startRotation;

    public Transform topTransform;
    public Transform goalTransform;

    public GameObject helixLevelPrefab;

    public List<Stage> allStages = new List<Stage>();

    public float helixDistance;

    private List<GameObject> spawnedLevels = new List<GameObject>();

    private void Awake()
    {
        startRotation = transform.localEulerAngles;

        helixDistance = topTransform.localPosition.y - (goalTransform.localPosition.y + .1f);
        LoadStage(0);
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 currentTapPosition = touch.position;

                if (lastTapPosition == Vector2.zero)
                {
                    lastTapPosition = currentTapPosition;
                }

                float distance = lastTapPosition.x - currentTapPosition.x;
                lastTapPosition = currentTapPosition;
                transform.Rotate(Vector3.up * distance);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                lastTapPosition = Vector2.zero;
            }
        }
    }

    public void LoadStage(int stageNumber)
    {
        Stage stage = allStages[Mathf.Clamp(stageNumber, 0, allStages.Count - 1)];

        if (stage == null)
        {
            Debug.LogError("Stage is null");
            return;
        }

        Camera.main.backgroundColor = allStages[stageNumber].stageBackgroundColor;

        Object.FindAnyObjectByType<BallController>().GetComponent<Renderer>().material.color = allStages[stageNumber].StageBallColor;

        transform.localEulerAngles = startRotation;

        foreach (GameObject go in spawnedLevels)
        {
            Destroy(go);
        }

        float levelDistance = helixDistance / stage.levels.Count;
        float spawnPosY = topTransform.localPosition.y;

        for (int i = 0; i < stage.levels.Count; i++)
        {
            spawnPosY -= levelDistance;
            GameObject level = Instantiate(helixLevelPrefab, transform);
            level.transform.localPosition = new Vector3(0, spawnPosY, 0);
            spawnedLevels.Add(level);

            int partToDisabled = 12 - stage.levels[i].partCount;
            List<GameObject> disabledParts = new List<GameObject>();

            while (disabledParts.Count < partToDisabled)
            {
                GameObject randomPart = level.transform.GetChild(Random.Range(0, level.transform.childCount)).gameObject;
                if (!disabledParts.Contains(randomPart))
                {
                    randomPart.SetActive(false);
                    disabledParts.Add(randomPart);
                }
            }

            List<GameObject> leftParts = new List<GameObject>();

            foreach (Transform t in level.transform)
            {
                t.GetComponent<Renderer>().material.color = allStages[stageNumber].stageLevelPartColor;
                if (t.gameObject.activeInHierarchy)
                {
                    leftParts.Add(t.gameObject);
                }
            }

            List<GameObject> deathParts = new List<GameObject>();

            while (deathParts.Count < stage.levels[i].deathPartCount)
            {
                GameObject randomPart = leftParts[Random.Range(0, leftParts.Count)];
                if (!deathParts.Contains(randomPart))
                {
                    randomPart.gameObject.AddComponent<DeathPart>();
                    deathParts.Add(randomPart);
                }
            }
        }
    }
}
