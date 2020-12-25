using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle
{

    public Vector3 a;
    public Vector3 b;
    public Vector3 c;

	private float elevation = 0.0f;

	public Triangle(Vector3 a, Vector3 b, Vector3 c)
    {
		this.a = a;
		this.b = b;
		this.c = c;
    }

	public float GetArea() { 
		float s = (a.magnitude+b.magnitude+c.magnitude)/2; 
		return Mathf.Sqrt(s*(s-a.magnitude)*(s-b.magnitude)*(s-c.magnitude));
	}

	public Vector3 GetCenterPoint() {
		float x = (a.x + b.x + c.x)/3;
		float y = (a.y + b.y + c.y)/3;
		float z = (a.z + b.z + c.z)/3;
		return new Vector3(x,y,z);
	}

	public bool IsPointInside(Vector3 point, float tolerance)
	{
		float minX = Mathf.Min (Mathf.Min (a.x, b.x), Mathf.Min (b.x, c.x));
		float maxX = Mathf.Max (Mathf.Max (a.x, b.x), Mathf.Max (b.x, c.x));
		float minY = Mathf.Min (Mathf.Min (a.y, b.y), Mathf.Min (b.y, c.y));
		float maxY = Mathf.Max (Mathf.Max (a.y, b.y), Mathf.Max (b.y, c.y));
		float minZ = Mathf.Min (Mathf.Min (a.z, b.z), Mathf.Min (b.z, c.z));
		float maxZ = Mathf.Max (Mathf.Max (a.z, b.z), Mathf.Max (b.z, c.z));

		return minX <= point.x + tolerance && point.x <= maxX + tolerance &&
			minY <= point.y + tolerance && point.y <= maxY + tolerance &&
			minZ <= point.z + tolerance && point.z <= maxZ + tolerance
		;
	}

	public void Extrude(float elevation)
	{
		if (elevation != this.elevation) {
			a = a.normalized * (a.magnitude - this.elevation);
			b = b.normalized * (b.magnitude - this.elevation);
			c = c.normalized * (c.magnitude - this.elevation);

			a = a.normalized * (a.magnitude + elevation);
			b = b.normalized * (b.magnitude + elevation);
			c = c.normalized * (c.magnitude + elevation);
		
			this.elevation = elevation;
		}
	}
}
