using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Linq;
using UnityEngine.UI;
using Unity.XR.CoreUtils;
using TMPro;
using System;

public class PanelHandler : MonoBehaviour 
{
	public static List<Gesture> gestures;
	// public static float SMALL = 10f;
	public static float SIZE = 8f;
	// public static float LARGE = 30f;

	public static string SMALL = "SMALL";
    public static string MED = "MEDIUM";
    public static string LARGE = "LARGE";

    public static string SIMPLE = "SIMPLE";
    public static string MEDIUM = "MEDIUM_C";
    public static string COMPLEX = "COMPLEX";

    public static void CreateGestureList(Image[] imgs) {
    	gestures = new List<Gesture>();
    	for (int i = 1; i < 13; i++) {
    		gestures.Add(new Gesture(i, SMALL, imgs[i-1]));
    		gestures.Add(new Gesture(i, MED, imgs[i-1]));
    		gestures.Add(new Gesture(i, LARGE, imgs[i-1]));
    	}
    	//return gestures;
    }

    public static void CreateLimittedGestureList(Image[] imgs)
    {
        gestures = new List<Gesture>();
        for (int i = 1; i < 13; i++)
        {
            gestures.Add(new Gesture(i, SMALL, imgs[i - 1]));
        }
        //return gestures;
    }

    public static void ClearGestures() {
        gestures.Clear();
    }

    public static int SetNewPanel(List<GameObject> panelList) {
        int index;
        //Debug.Log("Gesture size: " + gestures.Count + ", " + PlayerPrefs.GetInt("overallState"));
    	if (gestures.Count > 0) {
    		int r = UnityEngine.Random.Range(0,gestures.Count);
	    	// Debug.Log(r + ", " + gestures.Count);
	    	Gesture g = gestures[r];
	    	gestures.RemoveAt(r);

            index = g.GetNum();
            GameObject _panel = panelList[index];
            string s = g.GetSize();

            panelList[index].gameObject.SetActive(true);

            GameObject textbox = _panel.transform.GetChild(1).gameObject;
            GameObject textItem = textbox.transform.GetChild(1).gameObject;
            TextMeshProUGUI toWrite = textItem.GetComponent<TextMeshProUGUI>();
            toWrite.text = "Take a look at this shape and memorize it. When you are ready press the space bar and draw a " + s + " size version of the shape. When you are done press the space bar again to stop drawing.";

            /*
            switch (s)
            {
                case ("SMALL"):
                    //_panel.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
                    //GameObject image = _panel.transform.GetChild(0).gameObject;
                    //image.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                    GameObject textbox = _panel.transform.GetChild(1).gameObject;
                    GameObject text = textbox.transform.GetChild(1).gameObject;
                    text.GetComponent<TextMeshPro>().text = "Take a look at this shape and memorize it. When you are ready press the space bar and draw a " + s + " size version of the shape. When you are done press the space bar again to stop drawing.";


                    break;

                case ("MEDIUM"):


                    break;

                case ("LARGE"):


                    break;

            }
            */


            SaveData.SetGestureName(g.GetName());
            SaveData.SetGestureSize(s);
            SaveData.SetGestureComplexity(g.GetComplexity());
	    	
	    } else {
            index = 0;

            NextScene();
	    }

        return index;
    }

    public static void SetImgMaterial(GameObject canvas, Material m) {
    	canvas.GetComponent<Renderer>().material = m;
    	// Debug.Log("After set img to blank");
    }

    public static void NextScene()
    {
        int state = PlayerPrefs.GetInt("overallState") + 1;
        PlayerPrefs.SetInt("overallState", state);

        string currentScene = SceneManager.GetActiveScene().name.ToString();
        int order = PlayerPrefs.GetInt("conditionOrdering");

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
