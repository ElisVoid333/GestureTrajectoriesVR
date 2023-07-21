using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gesture 
{
    private string name;
    private int num;
    private string size;
    private Image img;
    private string complexity;

    public Gesture(int n, string s, Image i) {
        name = "G"+n;
        num = n;
        size = s;
        img = i;
        if (n < 5) {
            complexity = PanelHandler.SIMPLE;
        } else if (n > 8) {
            complexity = PanelHandler.COMPLEX;
        } else {
            complexity = PanelHandler.MEDIUM;
        }
    }

    public string GetSize() {
        return size;
    }

    public string GetName() {
        return name;
    }

   public Image GetImage() {
   		return img;
   }

   public string GetComplexity() {
        return complexity;
   }

   public int GetNum()
    {
        return num;
    }
}
