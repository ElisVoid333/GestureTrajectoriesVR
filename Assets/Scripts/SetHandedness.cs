using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SetHandedness : MonoBehaviour
{
    private bool left;
    public Image background;
    // Start is called before the first frame update
    void Start()
    {
        SetRight();
        left = false;
        background.color = Color.gray;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleLeft()
    {
        if (!left)
        {
            SetLeft();
            left = true;
        }
        else
        {
            SetRight();
            left = false;
        }
    }

    private void SetLeft()
    {
        PlayerPrefs.SetInt("left", 1);
        background.color = Color.blue;
    }

    private void SetRight()
    {
        PlayerPrefs.SetInt("left", 0);
        background.color = Color.gray;
    }
}
