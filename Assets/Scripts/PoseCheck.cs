using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseCheck : MonoBehaviour
{
    public int poseId;
    public static float hight;
    private int poseCount;
    private int partsCount;
    private int linesCount;
    private int[] positionCount;
    private int[][] setPosition;

    public GameObject[] bodyParts;
    public GameObject[] lines;

    private GameObject poseTable;
    private GameObject poseSelect;
    private GameObject renderers;
    private GameObject answer;
    private LineRenderer[] lineRenderers;   

    void Start()
    {
        poseTable = GameObject.Find("Pose Table");
        poseCount = poseTable.transform.childCount;
        poseSelect = poseTable.transform.GetChild(0).gameObject;
        partsCount = poseSelect.transform.childCount;

        for (int i = 0; i < poseCount; i++)
        {
            poseSelect = poseTable.transform.GetChild(i).gameObject;
            for (int j = 0; j < partsCount; j++)
            {
                bodyParts[j] = poseSelect.transform.GetChild(j).gameObject;
                bodyParts[j].GetComponent<MeshRenderer>().enabled = false;
            }
        }

        renderers = GameObject.Find("Renderers");
        answer = renderers.transform.GetChild(0).gameObject;
        linesCount = answer.transform.childCount;

        lineRenderers = new LineRenderer[linesCount];
        for (int i = 0; i < linesCount; i++)
        {
            lines[i] = answer.transform.GetChild(i).gameObject;
            lineRenderers[i] = lines[i].GetComponent<LineRenderer>();
        }
        
        positionCount = new int[] { 2, 2, 7, 2, 7 };
        setPosition = new int[][]
        {
            new int[] { 0, 0 },
            new int[] { 0, 1 },
            new int[] { 4, 3, 2, 1, 5, 6, 7 },
            new int[] { 1, 8 },
            new int[] { 11, 10, 9, 8, 12, 13, 14 }
        };

        for (int i = 0; i < linesCount; i++)
        {
            lineRenderers[i].positionCount = positionCount[i];
        }
    }

    void Update()
    {
        int realPoseId = poseId - 1;
        //float passRange = 1.5f;
        //Vector2 check;

        if (realPoseId < 0)
        {
            realPoseId = 0;
        }
        else if (realPoseId >= poseCount)
        {
            realPoseId = poseCount - 1;
        }

        for (int i = 0; i < poseCount; i++)
        {
            poseSelect = poseTable.transform.GetChild(i).gameObject;
            if (i == realPoseId)
            {
                poseSelect.SetActive(true);
            }
            else
            {
                poseSelect.SetActive(false);
            }
        }

        poseSelect = poseTable.transform.GetChild(realPoseId).gameObject;
        for (int i = 0; i < partsCount; i++)
        {
            bodyParts[i] = poseSelect.transform.GetChild(i).gameObject;
        }
        hight = bodyParts[0].transform.localPosition.y;

        for (int i = 0; i < linesCount; i++)
        {
            for (int j = 0; j < positionCount[i]; j++)
            {
                lineRenderers[i].SetPosition(j, bodyParts[setPosition[i][j]].transform.localPosition);
            }
        }
        /*
        for (int i = 0; i < partsCount; i++)
        {
            check = new Vector2(MoveJoints.toPoseCheck[i].x, MoveJoints.toPoseCheck[i].y);
            if ((check.x >= bodyParts[i].transform.localPosition.x - passRange) && (check.x <= bodyParts[i].transform.localPosition.x + passRange))
            {
                if ((check.y >= bodyParts[i].transform.localPosition.y - passRange) && (check.y <= bodyParts[i].transform.localPosition.y + passRange))
                {
                    
                }
            }
        }
        */
    }
}
