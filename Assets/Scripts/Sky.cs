using UnityEngine;
using System.Collections;

public class Sky : MonoBehaviour {
	public Mesh mesh;
	public Vector3[] vertices;
	public Color[] colors;

	private Color[] sunsetColors;

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

		sunsetColors = new Color[6];

		// Top
		sunsetColors[1] = new Color (57/255.0f,33/255.0f,31/255.0f,1);//red
		sunsetColors[2] = new Color (57/255.0f,33/255.0f,31/255.0f,1);//red

		// Middle
		sunsetColors[0] = new Color (250/255.0f,97/255.0f,86/255.0f,1);//green
		sunsetColors[3] = new Color (250/255.0f,97/255.0f,86/255.0f,1);//green

		// Bottom
		sunsetColors[4] = new Color (253/255.0f,137/255.0f,107/255.0f,1);//new Color (0/255.0f,0/255.0f,255/255.0f,1);//blue
		sunsetColors[5] = new Color (253/255.0f,137/255.0f,107/255.0f,1);//blue
 
        StartCoroutine(Sunset());
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (sunset) {
			mesh.colors = sunsetColors;
		}
		if (night) {
			for (int i = 0; i < vertices.Length; i++) {
				colors [i] = Color.Lerp (sunsetColors [i], Color.blue, 1.0f);
			}
			mesh.colors = colors;
		}
		if (sunrise) {
			mesh.colors = sunsetColors;
		}

	}

	IEnumerator Sunset() {

//		sunsetColors = new Color[6];
////		sunsetColors[0] = new Color (57/255.0f,33/255.0f,31/255.0f,1);
////		sunsetColors[1] = new Color (57/255.0f,33/255.0f,31/255.0f,1);
////		sunsetColors[2] = new Color (250/255.0f,97/255.0f,86/255.0f,1);
////		sunsetColors[3] = new Color (250/255.0f,97/255.0f,86/255.0f,1);
////		sunsetColors[4] = new Color (253/255.0f,137/255.0f,107/255.0f,1);
////		sunsetColors[5] = new Color (253/255.0f,137/255.0f,107/255.0f,1);
//
//		// Vertex indices
//		//1 ---- 2
//		//0 ---- 3
//		//4 ---- 5
//
//		// Top
//		sunsetColors[1] = new Color (57/255.0f,33/255.0f,31/255.0f,1);//red
//		sunsetColors[2] = new Color (57/255.0f,33/255.0f,31/255.0f,1);//red
//
//		// Middle
//		sunsetColors[0] = new Color (250/255.0f,97/255.0f,86/255.0f,1);//green
//		sunsetColors[3] = new Color (250/255.0f,97/255.0f,86/255.0f,1);//green
//
//		// Bottom
//		sunsetColors[4] = new Color (253/255.0f,137/255.0f,107/255.0f,1);//new Color (0/255.0f,0/255.0f,255/255.0f,1);//blue
//		sunsetColors[5] = new Color (253/255.0f,137/255.0f,107/255.0f,1);//blue

        yield return new WaitForSeconds(5);
		sunset = true;
//		mesh.colors = sunsetColors;
		StartCoroutine(Night());
    }

	IEnumerator Night() {
        yield return new WaitForSeconds(3);
        night = true;
//		for (int i = 0; i < vertices.Length; i++)
//			colors[i] = Color.Lerp(sunsetColors[i], Color.blue, 0.1f);
//        
//        // assign the array of colors to the Mesh.
//        mesh.colors = colors;
//		StartCoroutine(Sunrise());
    }
	IEnumerator Sunrise() {
        yield return new WaitForSeconds(3);
		for (int i = 0; i < vertices.Length; i++)
			colors[i] = Color.Lerp(Color.yellow, Color.yellow, vertices[i].y);
        
        // assign the array of colors to the Mesh.
        mesh.colors = colors;
    }
}
