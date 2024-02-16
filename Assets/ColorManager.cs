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



	void Start () {
		original_colors = new Color[colors.Length];
		new_colors = new Color[colors.Length];
		colors.CopyTo(original_colors, 0);
		colors.CopyTo(new_colors, 0);
        
        StartCoroutine (ChangeColors());
	}
	
	void Update () {
		current_hue_offset = Mathf.Lerp (current_hue_offset, hue_offset, Time.deltaTime);

		if (Mathf.Abs( current_hue_offset - hue_offset) > 0.01f) {
			for (int i = 0; i < colors.Length; i++) {
				
				// The color changing code
				CIELCH cie = cs.RGBtoCIELCH (original_colors [i]);
				cie.h += current_hue_offset;
				colors [i] = cs.CIELCHtoRGB (cie);

			}

			for (int i = 0; i < colors.Length - 1; i++)
				materials [i].color = colors [i];

            Camera.main.backgroundColor = colors[colors.Length - 1];

        }
    }

	IEnumerator ChangeColors()
	{
		
		hue_offset += Random.Range(60, 120);

        yield return new WaitForSeconds(3);
        StartCoroutine (ChangeColors());
		yield return null;
	}
}
