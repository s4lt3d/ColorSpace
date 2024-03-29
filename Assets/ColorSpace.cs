﻿using UnityEngine;
using System.Collections;

public class CIELCH
{
	public float l;
	public float c;
	public float h;
	public float alpha;
	public override string ToString ()
	{
		return "[" + l.ToString () + ", " + c.ToString () + ", " + h.ToString () + ", " + alpha.ToString () + "]";
	}
}

public class CIELAB
{
	public float l;
	public float a;
	public float b;
	public float alpha;
	public override string ToString ()
	{
		return "[" + l.ToString () + ", " + a.ToString () + ", " + b.ToString () + ", " + alpha.ToString () + "]";
	}
}

public class XYZ
{
	public float x;
	public float y;
	public float z;
	public float alpha;
	public override string ToString ()
	{
		return "[" + x.ToString () + ", " + y.ToString () + ", " + z.ToString () + ", " + alpha.ToString () + "]";
	}
}

public class ColorSpace {

	const float ref_X = 95.047f;
	const float ref_Y = 100f;
	const float ref_Z = 108.883f;

	public CIELCH RGBtoCIELCH(Color c)
	{
		return CIELabtoCIELCH (XYZtoCIELab (RGBtoXYZ (c)));
	}

	public Color CIELCHtoRGB(CIELCH cie)
	{
		return XYZtoRGB (CIELabtoXYZ (CIELCHtoCIELab (cie)));
	}


	// setup variables that are constantly used;
	float r,g,b;
	float x,y,z;
	float X,Y,Z;
	float x3,y3,z3;
	float CIE_L, CIE_a, CIE_b;

	XYZ RGBtoXYZ(Color c)
	{
		r = c.r;
		g = c.g;
		b = c.b;
		
		if (r > 0.04045f)
			r = Mathf.Pow ((float)((r + 0.055f) / 1.055f), 2.4f);
		else                   
			r=r / 12.92f;

		if (g > 0.04045) 
			g = Mathf.Pow ((float)((g + 0.055f) / 1.055f), 2.4f);
		else                   
			g = g / 12.92f;

		if (b > 0.04045f) 
			b = Mathf.Pow ((float)((b + 0.055f) / 1.055f), 2.4f);
		else
			b = b / 12.92f;
		
		r = r * 100;
		g = g * 100;
		b = b * 100;
		
		//Observer. = 2°, Illuminant = D65
		X = r * 0.4124f + g * 0.3576f + b * 0.1805f;
		Y = r * 0.2126f + g * 0.7152f + b * 0.0722f;
		Z = r * 0.0193f + g * 0.1192f + b * 0.9505f;
		XYZ xyz = new XYZ ();
		xyz.x = (float)X;
		xyz.y = (float)Y;
		xyz.z = (float)Z;
		xyz.alpha = c.a;

		return xyz;
	}

	Color XYZtoRGB(XYZ xyz)
	{
		x = xyz.x / 100;        //X from 0 to  95.047      (Observer = 2°, Illuminant = D65)
		y = xyz.y / 100;        //Y from 0 to 100.000
		z = xyz.z / 100;        //Z from 0 to 108.883
		
		r = x * 3.2406f + y * -1.5372f + z * -0.4986f;
		g = x * -0.9689f + y * 1.8758f + z * 0.0415f;
		b = x * 0.0557f + y * -0.2040f + z * 1.0570f;
		
		if (r > 0.0031308f) 
			r = 1.055f * Mathf.Pow ((float)r, (1 / 2.4f)) - 0.055f;
		else                     
			r = 12.92f * r;
		if (g > 0.0031308f) 
			g = 1.055f * Mathf.Pow ((float)g, (1 / 2.4f)) - 0.055f;
		else
			g = 12.92f * g;
		if (b > 0.0031308f)
			b = 1.055f * Mathf.Pow ((float)b, (1 / 2.4f)) - 0.055f;
		else
			b = 12.92f * b;

		//r = shift((float)r,1);
		//g = shift((float)g,1);
		//b = shift((float)b,1);

		Color c = new Color((float)r,(float)g,(float)b, xyz.alpha);
		return c;
	}

	CIELAB XYZtoCIELab(XYZ xyz)
	{

		x = xyz.x / ref_X;          //ref_X =  95.047   Observer= 2°, Illuminant= D65
		y = xyz.y / ref_Y;         //ref_Y = 100.000
		z = xyz.z / ref_Z;         //ref_Z = 108.883
			
		if (x > 0.008856f) 
			x = Mathf.Pow ((float)x, 1 / 3f);
		else                    
			x = (7.787f * x) + (16 / 116.0f);
		if (y > 0.008856f) 
			y = Mathf.Pow ((float)y, 1 / 3f);
		else
			y = (7.787f * y) + (16 / 116.0f);

		if (z > 0.008856f) 
			z = Mathf.Pow ((float)z, 1 / 3f);
		else
			z = (7.787f * z) + (16 / 116.0f);

			
		CIE_L  = ( 116 * y ) - 16;
		CIE_a  = 500 * ( x - y );
		CIE_b  = 200 * ( y - z );

		CIELAB cie = new CIELAB();
		cie.l = (float)CIE_L;
		cie.a = (float)CIE_a;
		cie.b = (float)CIE_b;
		cie.alpha = xyz.alpha;
		return cie;
	}

	XYZ CIELabtoXYZ(CIELAB cie)
	{
		y = (cie.l + 16) / 116.0f;
		x = cie.a / 500 + y;
		z = y - cie.b / 200.0f;
		x3 = Mathf.Pow((float)x, 3f);
		y3 = Mathf.Pow((float)y, 3f);
		z3 = Mathf.Pow((float)z, 3f);
				
		if (y3 > 0.008856f) 
			y = y3;
		else
			y = (y - 16 / 116.0f) / 7.787f;

		if (x3 > 0.008856f)
			x = x3;
		else
			x = (x - 16 / 116.0f) / 7.787f;

		if (z3 > 0.008856f) 
			z = z3;
		else
			z = (z - 16 / 116.0f) / 7.787f;
				
		X = ref_X * x;     //ref_X =  95.047     Observer= 2°, Illuminant= D65
		Y = ref_Y * y;    //ref_Y = 100.000
		Z = ref_Z * z;     //ref_Z = 108.883
		XYZ xyz = new XYZ ();
		xyz.x = (float)X;
		xyz.y = (float)Y;
		xyz.z = (float)Z;
		xyz.alpha = cie.alpha;
		return xyz;
	}

	CIELCH CIELabtoCIELCH(CIELAB cielab)
	{
		float h = Mathf.Atan2 (cielab.b, cielab.a);
		if (h > 0) 
			h = (h / Mathf.PI) * 180.0f;
		else
			h = 360 - (Mathf.Abs (h) / Mathf.PI) * 180;

		CIELCH cielch = new CIELCH ();
		cielch.l = cielab.l;
		cielch.c = Mathf.Sqrt (cielab.a * cielab.a + cielab.b * cielab.b);
		cielch.h = h;
		cielch.alpha = cielab.alpha;
		return cielch;
	}

	CIELAB CIELCHtoCIELab(CIELCH cielch)
	{
		float ciea = Mathf.Cos (Mathf.Deg2Rad *cielch.h) * cielch.c;
		float cieb = Mathf.Sin(Mathf.Deg2Rad * cielch.h) * cielch.c;
		CIELAB cielab = new CIELAB ();
		cielab.a = ciea;
		cielab.b = cieb;
		cielab.l = cielch.l;
		cielab.alpha = cielch.alpha;
		return cielab;

	}
}

