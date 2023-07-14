using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;

public class InputManager : MonoBehaviour
{
    public TMP_InputField pidTextField;
    //public Toggle isLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTask()
    {

        int id = int.Parse(pidTextField.text);
        PlayerPrefs.SetInt("taskState", -1);
        PlayerPrefs.SetInt("overallState", 0);
        PlayerPrefs.SetInt("pid", id);

        // PlayerPrefs.SetInt("ordering", ordering.value);

        //PlayerPrefs.SetInt("left", isLeft.isOn ? 1 : 0);
        // Debug.Log("Uh hello");
        int conditionOrderString = GetConditionOrdering(id);
        // Debug.Log("///// " + conditionOrderString);
        PlayerPrefs.SetInt("conditionOrdering", conditionOrderString);

        SaveData.SetFilePath(id);
        SceneManager.LoadScene("IntroScene");
    }

    public static int GetConditionOrdering(int pid)
    {

        int order = pid % 3;
        // Debug.Log(order);
        /*
        string conditions = "";
        switch (order)
        {

            case (0):
                conditions = "Touch,Index,Pinch";
                break;
            case (1):
                conditions = "Index,Pinch,Touch";
                break;
            case (2):
                conditions = "Pinch,Touch,Index";
                break;

        }*/

        return order;
    }
}
