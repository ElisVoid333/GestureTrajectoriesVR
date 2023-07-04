using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class VirtualKeyboard : MonoBehaviour
{
	
	public static VirtualKeyboard instance;
	
	public string words = "";
	
	public GameObject vkCanvas;
	
	public TMP_InputField targetText;
	
	
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
		//HideVirtualKeyboard();
		ShowVirtualKeyboard();
		
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void KeyPress(string k){
		words += k;
		targetText.text = words;	
	}
	
	public void Del(){
		if (words != "")
		{
            words = words.Remove(words.Length - 1, 1);
            targetText.text = words;
        }
		
	}

	public void ShowVirtualKeyboard(){
		vkCanvas.SetActive(true);
	}
	
	public void HideVirtualKeyboard(){
		vkCanvas.SetActive(false);
	}
}
