using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {

  protected Animator animator;
  public GameObject[] inventory;
  public GameObject[] PrefabModels;
  public int currentItem;

	void Start ()
  {
    currentItem = 0;
    animator = GetComponent<Animator>();
    inventory = new GameObject[6];

    for (int i = 0; i < PrefabModels.Length; i++)
    {
      inventory[i+1] = Instantiate(PrefabModels[i]);
      inventory[i+1].transform.SetParent(animator.GetBoneTransform(HumanBodyBones.RightHand));
      inventory[i+1].transform.localPosition = PrefabModels[i].transform.position;
      inventory[i+1].transform.localRotation = PrefabModels[i].transform.rotation;
      inventory[i+1].SetActive(false);
    }
	}
	
	void Update ()
  {
    var inventoryButtons = GetComponent<InputController>().InventorySelection;

    for(int i = 0; i <inventoryButtons.Length; i++)
    {
      if (inventoryButtons[i])
      {
        if (inventory[currentItem] != null) { inventory[currentItem].SetActive(false); } 
        currentItem = i;
        if (inventory[currentItem] != null) { inventory[currentItem].SetActive(true); }
        break;
      }
    }
	}
}
