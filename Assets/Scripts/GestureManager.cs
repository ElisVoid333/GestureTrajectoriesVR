using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


public class GestureManager : MonoBehaviour
{
    public VideoPlayer _videoPlayer;
    public List<GameObject> _panel = new List<GameObject>();
    public List<GameObject> _buttons = new List<GameObject>();

    int pid;
    string order;
    string currentScene;
    private int handedness;
    public GameObject leftTip; 
    public GameObject rightTip; 
    public GameObject leftPen; 
    public GameObject rightPen;

    private int indexP;
    private int indexB;

    // Start is called before the first frame update
    void Start()
    {
        indexP = 0;
        indexB = 0;

        _videoPlayer.loopPointReached += EndReached;

        pid = PlayerPrefs.GetInt("pid");
        order = PlayerPrefs.GetString("conditionOrdering");
        currentScene = SceneManager.GetActiveScene().name.ToString();

        if (currentScene != "IntroScene")
        {
            handedness = PlayerPrefs.GetInt("left");
            ActivateHand(handedness);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        HidePanel();

        _buttons[0].gameObject.SetActive(false);
        _buttons[1].gameObject.SetActive(true);

    }

    public void ShowNext()
    {
        //Debug.Log("Index : " + indexP + "************************");
        //Debug.Log("Panel Count : " + _panel.Count + "************************");

        if (indexP < _panel.Count - 1)
        {
            HidePanel();
            indexP++;
            ShowPanel();
        }
        else
        {
            HidePanel();
            NextScene();
        }
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
        if(handedness == 0)
        {
            rightPen.GetComponent<Pen>().ClearDrawing();
        }
        else
        {
            leftPen.GetComponent<Pen>().ClearDrawing();
        }
    }

    private void ActivateHand(int hand)
    {
        if (hand == 0)
        {
            rightPen.SetActive(true);
            rightTip.SetActive(true);
            leftPen.SetActive(false);
            leftTip.SetActive(false);
        }
        else
        {
            rightPen.SetActive(false);
            rightTip.SetActive(false);
            leftPen.SetActive(true);
            leftTip.SetActive(true);
        }
    }

    public void RecordGesture()
    {

    }

    public void NextScene()
    {
        //Debug.Log("Order: " + order);
        //Debug.Log("Current Scene: " + currentScene);

        if (order == "Touch,Index,Pinch")
        {
            if (currentScene == "IntroScene")
            {
                SceneManager.LoadScene("TouchScene");
            }
            else if (currentScene == "TouchScene")
            {
                SceneManager.LoadScene("IndexScene");
            }
            else if (currentScene == "IndexScene")
            {
                SceneManager.LoadScene("PinchScene");
            }
            else
            {
                SceneManager.LoadScene("EndScene");
            }

        }else if (order == "Index,Pinch,Touch")
        {
            if (currentScene == "IntroScene")
            {
                SceneManager.LoadScene("IndexScene");
            }
            else if (currentScene == "IndexScene")
            {
                SceneManager.LoadScene("PinchScene");
            }
            else if (currentScene == "PinchScene")
            {
                SceneManager.LoadScene("TouchScene");
            }
            else
            {
                SceneManager.LoadScene("EndScene");
            }

        }else if (order == "Pinch,Touch,Index")
        {
            if (currentScene == "IntroScene")
            {
                SceneManager.LoadScene("PinchScene");
            }
            else if (currentScene == "PinchScene")
            {
                SceneManager.LoadScene("TouchScene");
            }
            else if (currentScene == "TouchScene")
            {
                SceneManager.LoadScene("IndexScene");
            }
            else
            {
                SceneManager.LoadScene("EndScene");
            }

        }
    }
}
