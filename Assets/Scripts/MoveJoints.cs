using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveJoints : MonoBehaviour
{
    public GameObject[] bodyParts;

    private int[][] angleCalcOrder;
    private Vector3[] parts;
    private Quaternion[] allPartsRotation;
    private Vector3 defaultPosition;

    private int partId;
    private float initAngle;
    private float angle;
    private float angle2;

    void Start()
    {
        parts = new Vector3[25];
        allPartsRotation = new Quaternion[bodyParts.Length];

        for (int i = 0; i < bodyParts.Length; i++)
        {
            allPartsRotation[i] = bodyParts[i].transform.localRotation;
        }

        angleCalcOrder = new int[][]
        {
            new int[] { 1, 2, 3, 4 }, //R.Shoulder
            new int[] { 1, 5, 6, 7 }, //L.Shoulder
            new int[] { 9, 10, 11 },  //R.Leg
            new int[] { 12, 13, 14 }  //L.Leg
        };

        defaultPosition = new Vector3(bodyParts[12].transform.position.x, bodyParts[12].transform.position.y, bodyParts[12].transform.position.z);
    }
    
    void Update()
    {
        for (int i = 0; i < 25; i++)
        {
            parts[i] = new Vector3(JsonTest.pose_joint_x[i] * 0.1f, JsonTest.pose_joint_y[i] * -0.1f, JsonTest.confidence[i]); // Pose Keypoints(X-axis, Y-axis, Confidence)
        }
        
        partId = 0;
        for (int i = 0; i <= 3; i++)
        {
            initAngle = 0;
            if (i == 0 || i == 1)
            {
                for (int j = 0; j <= 2; j++)
                {
                    angle2 = 0;
                    if (i == 0)
                    {
                        angle = 180 - Mathf.Atan2(parts[angleCalcOrder[i][j + 1]].y - parts[angleCalcOrder[i][j]].y,
                                                  parts[angleCalcOrder[i][j + 1]].x - parts[angleCalcOrder[i][j]].x) * Mathf.Rad2Deg - initAngle;
                        if (partId == 1)
                        {
                            if ((parts[3].x > parts[2].x && parts[3].y < parts[2].y) || (parts[4].x > parts[2].x && parts[4].y < parts[2].y))
                            {
                                angle2 = -40;
                            }
                        }
                    }
                    else if (i == 1)
                    {
                        angle = Mathf.Atan2(parts[angleCalcOrder[i][j + 1]].y - parts[angleCalcOrder[i][j]].y,
                                            parts[angleCalcOrder[i][j + 1]].x - parts[angleCalcOrder[i][j]].x) * Mathf.Rad2Deg - initAngle;
                        if (partId == 4)
                        {
                            if ((parts[6].x < parts[5].x && parts[6].y < parts[5].y) || (parts[7].x < parts[5].x && parts[7].y < parts[5].y))
                            {
                                angle2 = 40;
                            }
                        }
                    }
                    initAngle = angle;

                    bodyParts[partId].transform.localRotation = allPartsRotation[partId] * Quaternion.Euler(angle, 0, angle2);
                    partId++;
                }
            }
            else if (i == 2 || i == 3)
            {
                for (int j = 0; j <= 1; j++)
                {
                    angle2 = 0;
                    if (i == 2)
                    {
                        angle = 90 + Mathf.Atan2(parts[angleCalcOrder[i][j + 1]].y - parts[angleCalcOrder[i][j]].y,
                                                 parts[angleCalcOrder[i][j + 1]].x - parts[angleCalcOrder[i][j]].x) * Mathf.Rad2Deg - initAngle; 
                        if (partId == 6)
                        {
                            if ((parts[10].x > parts[8].x && parts[10].y < parts[8].y) || (parts[11].x > parts[8].x && parts[11].y < parts[8].y))
                            {
                                angle2 = 15;
                            }
                        }
                    }
                    if (i == 3)
                    {
                        angle = 90 + Mathf.Atan2(parts[angleCalcOrder[i][j + 1]].y - parts[angleCalcOrder[i][j]].y,
                                                 parts[angleCalcOrder[i][j + 1]].x - parts[angleCalcOrder[i][j]].x) * Mathf.Rad2Deg - initAngle;
                        if (partId == 8)
                        {
                            if ((parts[13].x < parts[8].x && parts[13].y < parts[8].y) || (parts[14].x < parts[8].x && parts[14].y < parts[8].y))
                            {
                                angle2 = -15;
                            }
                        }
                    }
                    initAngle = angle;

                    bodyParts[partId].transform.localRotation = allPartsRotation[partId] * Quaternion.Euler(angle2, 0, angle);
                    partId++;
                }
            }

            if (bodyParts[10].transform.position.y > 0)
            {
                bodyParts[12].transform.position = new Vector3(defaultPosition.x, defaultPosition.y - bodyParts[10].transform.position.y, defaultPosition.z);
            }
            else if (bodyParts[11].transform.position.y > 0)
            {
                bodyParts[12].transform.position = new Vector3(defaultPosition.x, defaultPosition.y - bodyParts[11].transform.position.y, defaultPosition.z);
            }
            else
            {
                bodyParts[12].transform.position = defaultPosition;
            }
        }

        /*
        if (parts[8].z != 0)
        {
            xBias = parts[8].x;
        }
        else
        {
            xBias = parts[1].x;
        }

        yBias = parts[0].y;
        for (int i = 1; i < 25; i++)
        {
            if (parts[i].y < yBias)
            {
                yBias = parts[i].y;
            }
        }

        for (int i = 0; i < partsCount; i++)
        {
            finalPosition = new Vector3(parts[i].x - xBias, parts[i].y - yBias, 0);
            if (i == 0)
            {
                hight = finalPosition.y;
                resizeRatio = PoseCheck.hight / hight;
            }
            finalPosition = new Vector3(finalPosition.x * resizeRatio, finalPosition.y * resizeRatio, 0);
            bodyParts[i].transform.localPosition = finalPosition;
        }

        for (int i = 0; i < linesCount; i++)
        {
            for (int j = 0; j < positionCount[i]; j++)
            {
                if (parts[setPosition[i][j]].z != 0)
                {
                    lineRenderers[i].SetPosition(j, bodyParts[setPosition[i][j]].transform.localPosition);
                    bodyParts[setPosition[i][j]].SetActive(true);
                }
                else
                {
                    lineRenderers[i].positionCount = j;
                    for (int k = j; k < positionCount[i]; k++)
                    {
                        bodyParts[setPosition[i][k]].SetActive(false);
                    }
                    break;
                }
            }
        }
        */
    }
}
