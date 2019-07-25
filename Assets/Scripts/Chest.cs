using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    #region variables
    public GameObject healthPack;
    private Transform chestTransform;
    private bool opened;
    #endregion

    #region functions
    public void Interact() {
        StartCoroutine(DeleteChest()); }

    IEnumerator DeleteChest() {
    yield return new WaitForSeconds(0.5f);
        Instantiate(healthPack, transform.position, transform.rotation);
        Debug.Log("some");
        Destroy(this.gameObject);
        }
    #endregion
}
