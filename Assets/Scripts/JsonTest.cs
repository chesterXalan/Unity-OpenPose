using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModelTest
{
    public List<People_sets> people;
}

[Serializable]
public class People_sets
{
    public float[] pose_keypoints;
}

public class JsonTest : MonoBehaviour
{
    private string jsonPath;
    private float[] temp_x;
    private float[] temp_y;
    private float[] temp_c;

    public static float[] pose_joint_x;
    public static float[] pose_joint_y;
    public static float[] confidence;
    
    void Start()
    {
        jsonPath = Application.dataPath + "/Resources/keypoints.json";

        temp_x = new float[25];
        temp_y = new float[25];
        temp_c = new float[25];
        pose_joint_x = new float[25];
        pose_joint_y = new float[25];
        confidence = new float[25];
    }
    
    void Update()
    {
        int data;
        int personId = 0;
        float conf;
        float maxValue = 0;

        try
        {
            string jsonData = File.ReadAllText(jsonPath, Encoding.UTF8);
            ModelTest obj = JsonUtility.FromJson<ModelTest>(jsonData);
            int cnt = obj.people.Count;

            if (cnt > 0)
            {
                for (int c = 0; c < cnt; c++)
                {
                    conf = 0.0f;
                    for (int i = 0; i < 25; i++)
                    {
                        data = i * 3;
                        conf += obj.people[c].pose_keypoints[data + 2];
                    }

                    if (c == 0)
                    {
                        maxValue = conf;
                    }
                    else
                    {
                        if (conf > maxValue)
                        {
                            maxValue = conf;
                            personId = c;
                        }
                    }
                }
            }
            for (int i = 0; i < 25; i++)
            {
                data = i * 3;
                pose_joint_x[i] = obj.people[personId].pose_keypoints[data];
                pose_joint_y[i] = obj.people[personId].pose_keypoints[data + 1];
                confidence[i] = obj.people[personId].pose_keypoints[data + 2];

                temp_x[i] = obj.people[personId].pose_keypoints[data];
                temp_y[i] = obj.people[personId].pose_keypoints[data + 1];
                temp_c[i] = obj.people[personId].pose_keypoints[data + 2];
            }
        }
        catch (IOException)
        {
            for(int i = 0; i < 25; i++)
            {
                pose_joint_x[i] = temp_x[i];
                pose_joint_y[i] = temp_y[i];
                confidence[i] = temp_c[i];
            }
        }
    }
}
