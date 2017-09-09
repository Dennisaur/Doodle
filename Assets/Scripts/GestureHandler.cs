using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using PDollarGestureRecognizer;

public class GestureHandler : MonoBehaviour {

	public Transform gestureOnScreenPrefab;
	public GameObject player;
	public GameObject bullet;
	public float gestureThreshold = 0.4f;
	public GestureSpawner gestureSpawner;

	private List<Gesture> trainingSet = new List<Gesture>();

	private List<Point> points = new List<Point>();
	private int strokeId = -1;

	private Vector3 virtualKeyPosition = Vector2.zero;
	private bool isDrawing = false;
	private Rect drawArea;

	private RuntimePlatform platform;
	private int vertexCount = 0;

	private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
	private LineRenderer currentGestureLineRenderer;

	//GUI
	private string message;
	private bool recognized;
	private string newGestureName = "";

	void Start () {

		platform = Application.platform;
		drawArea = new Rect(0, 50, Screen.width, Screen.height - 50);

		//Load pre-made gestures
//		TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
//		foreach (TextAsset gestureXml in gesturesXml)
//			trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
		
		//Load custom pre-made gestures
		TextAsset[] customGesturesXml = Resources.LoadAll<TextAsset>("CustomGestures/");
		foreach (TextAsset gestureXml in customGesturesXml)
			trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
	}

	void Update () {
		if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer) {
			if (Input.touchCount > 0) {
				virtualKeyPosition = new Vector3 (Input.GetTouch (0).position.x, Input.GetTouch (0).position.y);
			} else if (isDrawing) {
				isDrawing = false;
				CompleteGesture ();
			}
		} else {
			if (Input.GetMouseButton(0)) {
				virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
			} else if (isDrawing) {
				isDrawing = false;
				CompleteGesture ();
			}
		}

		if (drawArea.Contains(virtualKeyPosition)) {
			if (Input.GetMouseButtonDown(0)) {

				isDrawing = true;

				++strokeId;
				
				Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
				currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();
				
				gestureLinesRenderer.Add(currentGestureLineRenderer);
				
				vertexCount = 0;
			}
			
			if (Input.GetMouseButton(0)) {
				points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

				currentGestureLineRenderer.SetVertexCount(++vertexCount);
				currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
			}
		}
	}

	void OnGUI() {

		GUI.Box(drawArea, "Draw Area");
		GUI.Label(new Rect(10, Screen.height - 40, 500, 50), message);

	}

	// Called when input released (gesture completed)
	void CompleteGesture() {
		// Get gesture result
		Gesture candidate = new Gesture(points.ToArray());
		Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

		message = gestureResult.GestureClass + " " + gestureResult.Score;

		CheckGesture(gestureResult);

		ClearDrawing ();
	}

	// Compare gesture with recognizer
	void CheckGesture(Result gestureResult) {

		if (gestureResult.Score > gestureThreshold) {
			switch (gestureResult.GestureClass) {
			case "Left Arrow":
				gestureSpawner.Spawn (0);
				break;
			case "Up Arrow":
				gestureSpawner.Spawn (1);
				break;
			case "Right Arrow":
				gestureSpawner.Spawn (2);
				print ("Right Arrow");
				break;
			}
		}
	}

	// Clear line renderer
	void ClearDrawing() {
		strokeId = -1;

		points.Clear();

		foreach (LineRenderer lineRenderer in gestureLinesRenderer) {

			lineRenderer.SetVertexCount(0);
			Destroy(lineRenderer.gameObject);
		}

		gestureLinesRenderer.Clear();
	}
}
