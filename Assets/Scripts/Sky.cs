using UnityEngine;
using System.Collections;

public class Sky : MonoBehaviour {
	public Mesh mesh;
	public Vector3[] vertices;
	public Color[] colors;

	private Color[] afternoonColors;
	private Color[] sunsetColors;
	private Color[] nightColors;
	private Color[] sunriseColors;

	// color lerping
	private float duration = 10;
	private float t = 0;

	// Sky states
	private bool sunset;
	private bool night;
	private bool sunrise;

	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        
        // create new colors array where the colors will be created.
       colors = new Color[vertices.Length];

		afternoonColors = new Color[6];
		sunsetColors = new Color[6];
		nightColors = new Color[6];
		sunriseColors = new Color[6];

		// Afternoon
		// Top
		afternoonColors[1] = new Color (0/255.0f,125/255.0f,255/255.0f,1);//red
		afternoonColors[2] = new Color (0/255.0f,125/255.0f,255/255.0f,1);//red

		// Middle
		afternoonColors[0] = new Color (0/255.0f,170/255.0f,255/255.0f,1);//green
		afternoonColors[3] = new Color (0/255.0f,170/255.0f,255/255.0f,1);//green

		// Bottom
		afternoonColors[4] = new Color (90/255.0f,217/255.0f,249/255.0f,1);//new Color (0/255.0f,0/255.0f,255/255.0f,1);//blue
		afternoonColors[5] = new Color (90/255.0f,217/255.0f,249/255.0f,1);//blue

		// Sunset
		// Top
		sunsetColors[1] = new Color (57/255.0f,33/255.0f,31/255.0f,1);//red
		sunsetColors[2] = new Color (57/255.0f,33/255.0f,31/255.0f,1);//red

		// Middle
		sunsetColors[0] = new Color (250/255.0f,97/255.0f,86/255.0f,1);//green
		sunsetColors[3] = new Color (250/255.0f,97/255.0f,86/255.0f,1);//green

		// Bottom
		sunsetColors[4] = new Color (253/255.0f,137/255.0f,107/255.0f,1);//new Color (0/255.0f,0/255.0f,255/255.0f,1);//blue
		sunsetColors[5] = new Color (253/255.0f,137/255.0f,107/255.0f,1);//blue

		// Night
		// Top
		nightColors[1] = new Color (57/255.0f,38/255.0f,77/255.0f,1);
		nightColors[2] = new Color (57/255.0f,38/255.0f,77/255.0f,1);

		// Middle
		nightColors[0] = new Color (49/255.0f,43/255.0f,88/255.0f,1);
		nightColors[3] = new Color (49/255.0f,43/255.0f,88/255.0f,1);

		// Bottom
		nightColors[4] = new Color (37/255.0f,50/255.0f,88/255.0f,1);
		nightColors[5] = new Color (37/255.0f,50/255.0f,88/255.0f,1);

		// Sunrise
		// Top
		sunriseColors[1] = new Color (193/255.0f,200/255.0f,184/255.0f,1);
		sunriseColors[2] = new Color (193/255.0f,200/255.0f,184/255.0f,1);

		// Middle
		sunriseColors[0] = new Color (243/255.0f,206/255.0f,208/255.0f,1);
		sunriseColors[3] = new Color (243/255.0f,206/255.0f,208/255.0f,1);

		// Bottom
		sunriseColors[4] = new Color (254/255.0f,224/255.0f,151/255.0f,1);
		sunriseColors[5] = new Color (254/255.0f,224/255.0f,151/255.0f,1);


		// set initial colors
		mesh.colors = afternoonColors;
 
        StartCoroutine(Sunset());
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (sunset) {
			for (int i = 0; i < vertices.Length; i++) {
				colors [i] = Color.Lerp (afternoonColors [i], sunsetColors[i], t);
			}
			if (t < 1){ // while t below the end limit...
				// increment it at the desired rate every update:
				t += Time.deltaTime/duration;
			}
			mesh.colors = colors;
		}
		if (night) {
			for (int i = 0; i < vertices.Length; i++) {
				colors [i] = Color.Lerp (sunsetColors [i], nightColors[i], t);
			}
			if (t < 1){ // while t below the end limit...
				// increment it at the desired rate every update:
				t += Time.deltaTime/duration;
			}
			mesh.colors = colors;
		}
		if (sunrise) {
			for (int i = 0; i < vertices.Length; i++) {
				colors [i] = Color.Lerp (nightColors [i], sunriseColors[i], t);
			}
			if (t < 1){ // while t below the end limit...
				// increment it at the desired rate every update:
				t += Time.deltaTime/duration;
			}
			mesh.colors = colors;
		}

	}

	IEnumerator Sunset() {
//		// Vertex indices
//		//1 ---- 2
//		//0 ---- 3
//		//4 ---- 5

		yield return new WaitForSeconds(15);
		t = 0;
		sunset = true;
//		mesh.colors = sunsetColors;
		StartCoroutine(Night());
    }

	IEnumerator Night() {
        yield return new WaitForSeconds(15);
		t = 0;
        night = true;
		StartCoroutine(Sunrise());
//		for (int i = 0; i < vertices.Length; i++)
//			colors[i] = Color.Lerp(sunsetColors[i], Color.blue, 0.1f);
//        
//        // assign the array of colors to the Mesh.
//        mesh.colors = colors;
//		StartCoroutine(Sunrise());
    }
	IEnumerator Sunrise() {
		yield return new WaitForSeconds(15);
		t = 0;
		sunrise = true;
//		StartCoroutine(Night());
//        yield return new WaitForSeconds(3);
//		for (int i = 0; i < vertices.Length; i++)
//			colors[i] = Color.Lerp(Color.yellow, Color.yellow, vertices[i].y);
//        
//        // assign the array of colors to the Mesh.
//        mesh.colors = colors;
    }
}
