using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SaveData 
{

  	public static string filepathData;
  	public static string filepathRaw;


  	public static int pid;

  	// public static List<HandPoint> hps = new List<HandPoint>();
    public static List<(Vector3, long)> path = new List<(Vector3, long)>();
    public static long timeTrialStart=0;
    public static long timeDrawStart=0;
    public static long timeDrawEnd=0;
    public static string gestureName = "G?";
    public static string gestureSize = "";
    public static string gestureComplexity = "";
    public static string condition = "";

    public static DateTime BEGIN = new DateTime(2020, 1, 1);

  	public static void SetFilePath(int pid) {
  		string fp = "participant_data_" + pid;
  		
  		while (File.Exists(fp+".csv")) {
  			fp = fp + "_n";
  		}

  		filepathData = fp + ".csv";

  		string fpRaw = "participant_raw_" + pid;
  		
  		while (File.Exists(fpRaw+".csv")) {
  			fpRaw = fpRaw + "_n";
  		}

  		filepathRaw = fpRaw + ".csv";
  		SetHeaders();
   	}

   	public static void SetHeaders() {
   		try {
    		using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepathData, true)) {

    			file.WriteLine("pid;block;condition;gestureName;gestureSize;gestureComplexity;timeTrialStart;timeDrawStart;timeDrawEnd;pathData;");

    		} 
    	}
    	catch (Exception ex) {
    		throw new ApplicationException("Could not write header: ", ex);
    	}


    	try {
    		using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepathRaw, true)) {

    			file.WriteLine("pid;block;condition;gestureName;gestureSize;gestureComplexity;handPoint");

    		} 
    	}
    	catch (Exception ex) {
    		throw new ApplicationException("Could not write header: ", ex);
    	}
   	}

   	public static void SetTimeTrialStart() {
   		timeTrialStart = DateTime.Now.Ticks-BEGIN.Ticks;
   	}

   	public static void SetTimeDrawStart() {
   		timeDrawStart = DateTime.Now.Ticks-BEGIN.Ticks;

   	}

   	public static void SetTimeDrawEnd() {
   		timeDrawEnd = DateTime.Now.Ticks-BEGIN.Ticks;
   	}

   	public static void SetGestureName(string name) {
   		gestureName = name;
   	}

   	public static void SetGestureSize(string size) {
   		gestureSize = size;
   	}

   	public static void SetGestureComplexity(string complexity) {
   		gestureComplexity = complexity;

   	}

   	public static void SetCondition(string cond) {
   		condition = cond;
   	}

    public static void AddCurrentRecord() {
    	// string handData = CreateJSONFromList("handData",hps);
    	string pathData = CreateJSONFromList("pathData",path);
    	int block = PlayerPrefs.GetInt("overallState");
    	// hps.Clear();
    	path.Clear();
    	try {
    		using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepathData, true)) {

    			file.WriteLine(pid+";"+block+";"+condition+";"+gestureName+";"+gestureSize+";"+gestureComplexity+";"+timeTrialStart+";"+timeDrawStart+";"+timeDrawEnd+";"+pathData);

    		} 
    	}
    	catch (Exception ex) {
    		throw new ApplicationException("Could not save to file: ", ex);
    	}
    
    }

    /*
    public static void writeHandPoint(HandPoint handPoint) {
    	// string handData = CreateJSONFromList("handData",hps);
    	// string pathData = CreateJSONFromList("pathData",path);
    	string hp = JsonUtility.ToJson(handPoint);
    	    	int block = PlayerPrefs.GetInt("overallState");

    	// hps.Clear();
    	// path.Clear();
    	try {
    		using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepathRaw, true)) {

    			file.WriteLine(pid+";"+block+";"+condition+";"+gestureName+";"+gestureSize+";"+gestureComplexity+";"+hp);

    		} 
    	}
    	catch (Exception ex) {
    		throw new ApplicationException("Could not save to file: ", ex);
    	}
    
    }
    */

    // public static void CreateJSONs() {
    // 	StringBuilder handData = new StringBuilder("{");
    //     for (int i = 0; i < hps.Count; i++) {
    //         handData = handData.Append(JsonUtility.ToJson(hps[i]) + ",");
    //     }
    //     handData[handData.Length-1] = '}';

    //     StringBuilder pathData = new StringBuilder("{");
    //     for (int i = 0; i < path.Count; i++) {
    //         pathData = pathData.Append(JsonUtility.ToJson(path[i]) + ",");
    //     }

    //     pathData[pathData.Length-1] = '}';

    //     Debug.Log(handData.ToString());
    //     hps.Clear();
    //     path.Clear();
    // }

    public static string CreateJSONFromList<T>(string name, List<T> x) {
    	StringBuilder xStr = new StringBuilder("\""+name+"\":{");
    	for (int i = 0; i < x.Count; i++) {
    		xStr = xStr.Append(JsonUtility.ToJson(x[i]) + ",");
    	}
    	xStr[xStr.Length-1] = '}';
    	return xStr.ToString();
    }

 
}
