using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tessalate : MonoBehaviour {

  private Material mat;
  private Vector2 bOff;

  [Range(0.1f, 10)]
  public float duration = 1.0f;
  private float current = 0.0f;
  private float target = 1.0f;

  // height map variables
  private bool flipDirection = false;
  private float heightScalar = 1.0f;
  private float heightOffset = 0.0f;

  [Range(0.1f, 1.0f)]
  public float HeightScale = 0.3f;

  // Use this for initialization
  void Start ()
  {
    mat = GetComponent<Renderer>().material;
  }
	
	// Update is called once per frame
	void FixedUpdate ()
  {
    current = Mathf.MoveTowards(current, target, (1 / duration) * Time.deltaTime);

    if (duration <= 0.0f)
    {
      duration = 0.001f;
    }

    if (current >= 1.0f)
    {
      current -= Mathf.Ceil(current);
      flipDirection = !flipDirection;      

      if (flipDirection)
      {
        heightScalar = -1.0f;
        heightOffset = HeightScale;
      }
      else
      {
        heightScalar = 1.0f;
        heightOffset = 0;
      }
    }

    bOff.y = current;
    float height = heightOffset + heightScalar * current * HeightScale;
    mat.SetTextureOffset("_BumpMap", bOff);
    mat.SetTextureOffset("_ParallaxMap", new Vector2(0, height));
    //mat.SetFloat("_Parallax", height);

    //mat.SetFloat("_Parallax", Mathf.Sin(current * direction));
  }
}
