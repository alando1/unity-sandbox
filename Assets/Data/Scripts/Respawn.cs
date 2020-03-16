using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
  public Terrain map;
  private Bounds bounds;
  private Rigidbody rb;
  private Vector2 mapSize;
  private Vector3 testSpawn;


  void Start()
  {
    Movement.Respawn += RespawnJump;
    bounds = map.terrainData.bounds;
    rb = GetComponent<Rigidbody>();
    mapSize = new Vector2(bounds.extents.x, bounds.extents.z);
  }

  public void RespawnJump(Vector3 pos)
  {
    rb.velocity = Vector3.zero;
    //Vector3 spawnPos = new Vector3(
    //  Random.Range(bounds.center.x - mapSize.x, bounds.center.x + mapSize.x), 
    //  1000.0f, 
    //  Random.Range(bounds.center.z - mapSize.y, bounds.center.z + mapSize.y));
    //transform.position = spawnPos;
    transform.position = transform.parent.transform.position;
    Debug.Log("Player respawned");
  }

  public bool OutOfBounds()
  {
    string s;
    if (transform.position.x > (bounds.center.x + mapSize.x) ||
        transform.position.x < (bounds.center.x - mapSize.x))
    {
      s = "Out Of Bounds: x = " + transform.position.x;
      Debug.Log(s);
      return true;
    }

    if (transform.position.y < -1000.0f)
    {
      s = "Out Of Bounds: y = " + transform.position.y;
      Debug.Log(s);
      return true;
    }

    if (transform.position.z > (bounds.center.z + mapSize.y) ||
        transform.position.z < (bounds.center.z - mapSize.y))
    {
      s = "Out Of Bounds: z = " + transform.position.z;
      Debug.Log(s);
      return true;
    }

    return false;
  }
}
