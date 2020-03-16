using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Build : MonoBehaviour
{
  public Transform buildsContainer;
  public GameObject[] Models;
  public List<GameObject> builds;
  //private Camera cam;

  // Use this for initialization
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
    // read building input
    if (Input.GetMouseButtonDown(3))
    {
      BuildRamp();
      Debug.Log("Side mouse button 1 pressed");
    }
    if (Input.GetMouseButtonDown(4))
    {
      Debug.Log("Side mouse button 2 pressed");
      BuildWall();
    }
    if (Input.GetMouseButtonDown(2))
    {
      BuildFloor();
      Debug.Log("Scroll wheel pressed");
    }

  }

  void BuildRamp()
  {
    GameObject ramp = Instantiate(Models[0], Snap(transform), Quaternion.identity, buildsContainer);
    builds.Add(ramp);
  }

  void BuildWall()
  {
    GameObject wall = Instantiate(Models[1], Snap(transform), Quaternion.identity, buildsContainer);
    builds.Add(wall);
  }

  void BuildFloor()
  {
    //Ray ray = cam.ScreenPointToRay(new Vector3(cam.pixelWidth/2, cam.pixelHeight/2, 0));
    //RaycastHit hit;
    //if (Physics.Raycast(ray, out hit, ))

    GameObject floor = Instantiate(Models[2], Snap(transform), Models[2].transform.rotation, buildsContainer);
    builds.Add(floor);
  }

  private Vector3 Snap(Transform player)
  {
    int[] gridPos = new int[3]{0,0,0};
    gridPos[0] = ((int)player.position.x / 5) * 5;
    gridPos[1] = ((int)player.position.y / 5) * 5;
    gridPos[2] = ((int)player.position.z / 5) * 5;
    
    return new Vector3(gridPos[0], gridPos[1], gridPos[2]);
  }
}
