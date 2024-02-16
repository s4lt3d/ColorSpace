using UnityEngine;
using System.Collections;

public class ColorManager : MonoBehaviour {
	ColorSpace cs = new ColorSpace();

	public Color[] colors;
	Color[] new_colors;
	Color[] original_colors;
	float hue_offset = 0; 
	float current_hue_offset;

	public Material[] materials;

	// Use this for initialization
	void Start () {
		original_colors = new Color[colors.Length];
		new_colors = new Color[colors.Length];
		colors.CopyTo(original_colors, 0);
		colors.CopyTo(new_colors, 0);
		StartCoroutine (ChangeColors());
	}
	
	// Update is called once per frame
	void Update () {
		current_hue_offset = Mathf.Lerp (current_hue_offset, hue_offset, Time.deltaTime);

		if (Mathf.Abs( current_hue_offset - hue_offset) > 0.01f) {
			for (int i = 0; i < colors.Length; i++) {
				CIELCH cie = cs.RGBtoCIELCH (original_colors [i]);
				cie.h += current_hue_offset;
				colors [i] = cs.CIELCHtoRGB (cie);
			}

			for (int i = 0; i < colors.Length; i++)
				materials [i].color = colors [i];
		}
	}

	IEnumerator ChangeColors()
	{
		yield return new WaitForSeconds(5);
		hue_offset += Random.Range(60, 120);

		StartCoroutine (ChangeColors());
		yield return null;
	}
}
