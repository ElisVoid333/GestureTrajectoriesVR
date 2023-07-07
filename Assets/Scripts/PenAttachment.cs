using System.Collections.Generic;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Pen : MonoBehaviour {
    /// <summary>
    /// Transform used to target the tip oh the pen.
    /// </summary>
    [Tooltip("The tip of the head.")]
    [SerializeField] private Transform tip;
    
    /// <summary>
    /// Material used to represent the tip.
    /// </summary>
    [Tooltip("Material used to represent the tip.")]
    [SerializeField] private Material tipMaterial;
    
    
    /// <summary>
    /// Material used to represent the line to draw.
    /// </summary>
    [Tooltip("Material used to represent the line to draw.")]
    [SerializeField] private Material drawingMaterial;
    
    /// <summary>
    /// The with of the line to draw.
    /// </summary>
    [Tooltip("The with of the line to draw.")]
    [SerializeField] [Range(0.01f, .1f)] 
    private float penWidth = 0.01f;
    
    /// <summary>
    /// The distance that separate 2 key points.
    /// </summary>
    [Tooltip("The width of the line to draw.")]
    [SerializeField] [Range(0.001f, 0.01f)] 
    private float accuracy = 0.01f; 
    
    /// <summary>
    /// The pen's colour.
    /// </summary>
    [Tooltip("The pen's colour.")]
    [SerializeField] private Color penColour;
    
    /// <summary>
    /// Used to save the lines key points to draw lines. 
    /// </summary>
    private Stack<LineRenderer> lineRenderers;
    
    /// <summary>
    /// Counter used to save pen's key points with a unique index.
    /// </summary>
    private int index;

    /// <summary>
    /// Whether or not the user is drawing.
    /// </summary>
    private bool isDrawing;
    
    
    private void Start() {
        tipMaterial.color = penColour;
        lineRenderers = new Stack<LineRenderer>();
    }

    private void Update() {
        if(isDrawing)
            Draw();


        //Activate Draw with space button *************************************************
        if (Input.GetKeyDown(KeyCode.Space) && !isDrawing)
        {
            BeginUse();
        }else if (Input.GetKeyDown(KeyCode.Space) && isDrawing)
        {
            EndUse();
        }
    }

    /// <summary>
    /// Clear the current drawing.
    /// </summary>
    public void ClearDrawing() {
        // Destroying all associated game object and removing lines from the stack.
        while (lineRenderers.Count > 0)
            Destroy(lineRenderers.Pop().gameObject);
    }

    private void CreateNewLine() {
        index = 0;
        
        LineRenderer line = new GameObject().AddComponent<LineRenderer>();
        line.material = drawingMaterial;
        
        line.positionCount = 1;
        line.SetPosition(0, tip.position);
        
        line.startWidth = line.endWidth = penWidth;
        line.startColor = line.endColor = penColour;
        
        lineRenderers.Push(line);
    }

    /// <summary>
    /// If possible, add a new kay point to the LineRenderer.
    /// </summary>
    private void Draw() {
        LineRenderer currentLine = lineRenderers.Peek(); // Last line used in the drawing
        
        // Check if the position is far enough to be considered as a keypoint.
        Vector3 lastRecordedPosition = currentLine.GetPosition(index);
        if (Vector3.Distance(lastRecordedPosition, tip.position) < accuracy)
            return;
        
        // Adding a new key point.
        index++;
        currentLine.positionCount = index + 1;
        currentLine.SetPosition(index, tip.position);
    }

    public void BeginUse() {
        isDrawing = true;
        CreateNewLine();
    }

    public void EndUse() {
        isDrawing = false;
    }

    public float ComputeUseStrength(float strength) {
        return strength;
    }
}