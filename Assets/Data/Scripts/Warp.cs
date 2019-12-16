using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour {

    public GameObject[] players;
    public GameObject[] portals;
    public float portalRange;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(players != null && portals != null)
        {
            foreach(GameObject player in players)
            {
                if (player == null)
                    continue;

                Vector3 tmpPos = player.transform.position;
                foreach(GameObject portal in portals)
                {
                    if (portal == null)
                        continue;

                    Vector3 tmpWarp = tmpPos - portal.transform.position;
                    if(Mathf.Abs(tmpWarp.magnitude) < portalRange)
                    {
                        player.GetComponent<Respawn>().RespawnJump(transform.position);
                    }
                }
            }
        }
	}
}
