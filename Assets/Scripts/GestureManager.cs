using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


public enum Status { ShowGesture, BlankBeforeDraw, Drawing, BlankAfterDraw }


public class GestureManager : MonoBehaviour
{
    public VideoPlayer _videoPlayer;
    public List<GameObject> _panel = new List<GameObject>();
    public List<GameObject> _buttons = new List<GameObject>();

    int pid;
    int order;
    string currentScene;
    private int handedness;
    private bool isDrawing;
    public GameObject leftTip; 
    public GameObject rightTip; 
    public GameObject leftPen; 
    public GameObject rightPen;
    private Pen PenControl;

    private int indexP;
    //private int indexB;

    public static Status status, prevStatus;

    // Start is called before the first frame update
    void Start()
    {
        indexP = 0;
        //indexB = 0;

        _videoPlayer.loopPointReached += EndReached;

        pid = PlayerPrefs.GetInt("pid");
        order = PlayerPrefs.GetInt("conditionOrdering");
        currentScene = SceneManager.GetActiveScene().name.ToString();

        handedness = PlayerPrefs.GetInt("left");
        ActivateHand(handedness);

        prevStatus = Status.ShowGesture;
        status = Status.Drawing;

        SaveData.SetCondition(currentScene);
    }

    // Update is called once per frame
    void Update()
    {
        //isDrawing = PenControl.isDrawing;

        switch (status)
        {
            case (Status.BlankAfterDraw):

                Debug.Log("**********************BLANK AFTER DRAW****************************");

                if (prevStatus != status)
                {
                    if (indexP != 0)
                    {
                        RecordGesture();
                        Debug.Log("************ERASE***************");

                        Erase();
                        prevStatus = status;
                        break;
                    }
                    else
                    {
                        HidePanel();
                        prevStatus = status;
                    }
                }
                else
                {
                    status = Status.ShowGesture;
                }

                break;


            case (Status.ShowGesture):

                Debug.Log("**********************SHOW GESTURE****************************");

                if (prevStatus != status)
                {
                    if (indexP < _panel.Count - 1)
                    {
                        indexP++;
                        ShowPanel();
                    }
                    else
                    {
                        NextScene();
                    }

                    prevStatus = status;
                }

                break;

            case (Status.BlankBeforeDraw):

                Debug.Log("**********************BLANK BEFORE DRAW****************************");

                if (prevStatus != status)
                {
                    HidePanel();

                    prevStatus = status;
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        PenControl.BeginUse();
                        SaveData.SetTimeDrawStart();
                        status = Status.Drawing;
                        break;
                    }
                }

                break;


            case (Status.Drawing):

                Debug.Log("**********************DRAWING****************************");

                if (prevStatus != status)
                {
                    if (Input.GetKeyDown(KeyCode.Space) && prevStatus != Status.ShowGesture)
                    {
                        PenControl.EndUse();
                        SaveData.SetTimeDrawEnd();
                        prevStatus = status;
                        break;
                    }

                }
                else
                {
                    status = Status.BlankAfterDraw;
                }

                break;
        }
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        HidePanel();

        _buttons[0].gameObject.SetActive(false);
        _buttons[1].gameObject.SetActive(true);

    }

    public void ShowNext()
    {

        switch (status) //Cycles through the trial states
        {
            case (Status.BlankAfterDraw):

                status = Status.ShowGesture;

                break;

            case (Status.ShowGesture):

                status = Status.BlankBeforeDraw;

                break;

            case (Status.BlankBeforeDraw):

                status = Status.Drawing;

                break;

            case (Status.Drawing):

                status = Status.BlankAfterDraw;

                break;
        }
    }

    private void RecordGesture()
    {

    }

    public void HidePanel()
    {
        _panel[indexP].gameObject.SetActive(false);
    }

    public void ShowPanel()
    {
        _panel[indexP].gameObject.SetActive(true);
    }

    public void Erase()
    {
        PenControl.ClearDrawing();
    }

    private void ActivateHand(int hand)
    {
        if (hand == 0)
        {
            rightPen.SetActive(true);
            rightTip.SetActive(true);
            leftPen.SetActive(false);
            leftTip.SetActive(false);

            PenControl = rightPen.GetComponent<Pen>();
        }
        else
        {
            rightPen.SetActive(false);
            rightTip.SetActive(false);
            leftPen.SetActive(true);
            leftTip.SetActive(true);

            PenControl = leftPen.GetComponent<Pen>();
        }
    }


    public void NextScene()
    {
        switch (order)
        {

            case (0):
                if (currentScene == "IntroScene")
                {
                    SceneManager.LoadScene("Touch");
                }
                else if (currentScene == "Touch")
                {
                    SceneManager.LoadScene("Index");
                }
                else if (currentScene == "Index")
                {
                    SceneManager.LoadScene("Pinch");
                }
                else
                {
                    SceneManager.LoadScene("EndScene");
                }

                break;


            case (1):
                if (currentScene == "IntroScene")
                {
                    SceneManager.LoadScene("Index");
                }
                else if (currentScene == "Index")
                {
                    SceneManager.LoadScene("Pinch");
                }
                else if (currentScene == "Pinch")
                {
                    SceneManager.LoadScene("Touch");
                }
                else
                {
                    SceneManager.LoadScene("EndScene");
                }

                break;


            case (2):
                if (currentScene == "IntroScene")
                {
                    SceneManager.LoadScene("Pinch");
                }
                else if (currentScene == "Pinch")
                {
                    SceneManager.LoadScene("Touch");
                }
                else if (currentScene == "Touch")
                {
                    SceneManager.LoadScene("Index");
                }
                else
                {
                    SceneManager.LoadScene("EndScene");
                }

                break;

        }
    }
}
