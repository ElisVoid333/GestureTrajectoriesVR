using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vkKey : MonoBehaviour
{
	
	public string k = "xyz";
	
    // Start is called before the first frame update
    void Start()
    {
        //KeyClick();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void KeyClick(){
		VirtualKeyboard.instance.KeyPress(k);
	}
}
