using Oculus.Interaction.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;


public enum Status { ShowGesture, BlankBeforeDraw, Drawing, BlankAfterDraw }


public class GestureManager : MonoBehaviour
{
    public VideoPlayer _videoPlayer;
    public List<GameObject> _panel = new List<GameObject>();
    public List<GameObject> _buttons = new List<GameObject>();
    //private GameObject[] panelArr;

    int pid;
    int order;
    string currentScene;
    private int handedness;
    private bool isDrawing;
    private bool isPinching;
    private bool isPointing;

    public GameObject leftTip; 
    public GameObject rightTip; 
    public GameObject leftPen; 
    public GameObject rightPen;

    public OVRSkeleton _rightSkeleton;
    public Hand _rightHand;
    public OVRSkeleton _leftSkeleton;
    public Hand _leftHand;

    public bool testing;

    private OVRSkeleton _Skeleton;
    private Hand _Hand;
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

        SaveData.SetPID(pid);
        SaveData.SetCondition(currentScene);

        if (testing)
        {
            PanelHandler.CreateLimittedGestureList(LoadImages());
        }
        else
        {
            PanelHandler.CreateGestureList(LoadImages());
        }
    }

    // Update is called once per frame
    void Update()
    {
        isDrawing = PenControl.isDrawing;

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
                        SaveData.AddCurrentRecord();
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
                    ShowPanel();

                    prevStatus = status;
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        status = Status.BlankBeforeDraw;
                        break;
                    }
                }

                break;

            case (Status.BlankBeforeDraw):

                Debug.Log("**********************BLANK BEFORE DRAW****************************");

                if (prevStatus != status)
                {
                    HidePanel();

                    SaveData.SetTimeTrialStart();
                    
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

                    _buttons[1].gameObject.SetActive(false);
                }

                break;


            case (Status.Drawing):

                Debug.Log("**********************DRAWING****************************");

                if (prevStatus != status)
                {
                    if (Input.GetKeyDown(KeyCode.Space) && prevStatus != Status.ShowGesture)
                    {
                        PenControl.EndUse();
                        _buttons[1].gameObject.SetActive(true);
                        SaveData.SetTimeDrawEnd();
                        prevStatus = status;
                        break;
                    }

                    if (prevStatus != Status.ShowGesture)
                    {
                        HandPoint handPoint = new HandPoint(_Skeleton, _Hand, isPinching, isPointing, isDrawing);
                        SaveData.writeHandPoint(handPoint);
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
        string line = PenControl.RecordLine();
        //Debug.Log("Line Vector List : " + line);
        SaveData.SetLine(line);
        //string gesture = SaveData.CreateJSONstring(line);
    }

    public void TogglePinching(bool pinching)
    {
        isPinching = pinching;
    }

    public void TogglePointing(bool pointing)
    {
        isPointing = pointing;
    }

    public void HidePanel()
    {
        _panel[indexP].gameObject.SetActive(false);
    }

    public void ShowPanel()
    {
        //_panel[indexP].gameObject.SetActive(true);
        indexP = PanelHandler.SetNewPanel(_panel);
        
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

            _Skeleton = _rightSkeleton;
            _Hand = _rightHand;
        }
        else
        {
            rightPen.SetActive(false);
            rightTip.SetActive(false);
            leftPen.SetActive(true);
            leftTip.SetActive(true);

            PenControl = leftPen.GetComponent<Pen>();

            _Skeleton = _leftSkeleton;
            _Hand = _leftHand;
        }
    }

    public Image[] LoadImages()
    {
        Image[] items = new Image[12];

        for (int i = 0; i < items.Length; i++)
        {
            items[i] = Resources.Load("Gestures/Image/G" + (i + 1), typeof(Image)) as Image;
            // items[i] = Resources.Load("Default/Materials/untitled",typeof(Material)) as Material;
        }
        return items;
    }

    public void NextScene()
    {
        int state = PlayerPrefs.GetInt("overallState") + 1;
        PlayerPrefs.SetInt("overallState", state);

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
