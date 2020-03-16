using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawn : MonoBehaviour
{
  public GameObject player;
  public readonly int MAX_VEHICLES = 10;
  public GameObject[] vehicles = new GameObject[5];
  public GameObject[] spawnedVehicles;
  public Vector3 offset;

  void Start()
  {
    spawnedVehicles = new GameObject[MAX_VEHICLES];

    if (player == null)
    player = GameObject.Find("Player");
    //SpawnAllVehicles();
  }

  // Update is called once per frame
  void Update()
  {

  }

  public bool SpawnVehicle(int index)
  {
    if (vehicles != null)
      if (index > -1 && index < vehicles.Length)
        if (vehicles[index] != null)
        {
          Vector3 playerPos = player.transform.position;
          spawnedVehicles[index] = Instantiate(vehicles[index], player.transform.position + offset, Quaternion.Euler(0, 0, 0));
          spawnedVehicles[index].GetComponent<Rigidbody>().useGravity = true;
          spawnedVehicles[index].transform.SetParent(this.transform);
          return true;
        }
        else
          return false;
      else
        return false;
    else
      return false;
  }

  public void SpawnAllVehicles()
  {
    Vector3 wOriginaloffset = offset;
    if (player != null)
    {
      for (int i = 0; i < vehicles.Length; i++)
      {
        if(SpawnVehicle(i))
        offset.x += (i+1) * 3.0f;
      }
    }

    offset = wOriginaloffset;
  }
}
