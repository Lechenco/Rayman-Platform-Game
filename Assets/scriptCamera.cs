using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptCamera : MonoBehaviour
{
    public GameObject player;
    private float xGap;
    private float yGap;
    private float yOffset;
    private float edgeLeft, edgeDown, edgeUp, edgeRight;
    // Start is called before the first frame update
    private void Start() {
        edgeDown = 2f; edgeLeft = -1.5f; 
        edgeRight = 125f; edgeUp = 25f;
        xGap = 3f; yGap = 2.5f;
        yOffset = 1f;
    }

    // Update is called once per frame
    private void Update() {
        FollowPlayer();
        CheckEdges();
    }

    private void CheckEdges() {
        if (this.transform.position.y < edgeDown) {
            this.transform.position = new Vector3(this.transform.position.x,
                            edgeDown, this.transform.position.z);
        }

        if (this.transform.position.x < edgeLeft) {
            this.transform.position = new Vector3(edgeLeft, 
                    this.transform.position.y, this.transform.position.z);
        }

        if (this.transform.position.y > edgeUp) {
            this.transform.position = new Vector3(this.transform.position.x,
                            edgeUp, this.transform.position.z);
        }

        if (this.transform.position.x > edgeRight) {
            this.transform.position = new Vector3(edgeRight, 
                    this.transform.position.y, this.transform.position.z);
        }
    }
    private void FollowPlayer() {
        Vector3 playerPos = player.transform.position;

        if (playerPos.x <= this.transform.position.x - xGap) {
            this.transform.position = new Vector3(playerPos.x + xGap, 
                this.transform.position.y, this.transform.position.z);
        }
        if (playerPos.x >= this.transform.position.x + xGap) {
            this.transform.position = new Vector3(playerPos.x - xGap, 
                this.transform.position.y, this.transform.position.z);
        }
        if (playerPos.y >= this.transform.position.y + yGap) {
            this.transform.position = new Vector3(this.transform.position.x, 
                playerPos.y - yGap , this.transform.position.z);
        }
        if (playerPos.y <= this.transform.position.y - yOffset) {
            this.transform.position = new Vector3(this.transform.position.x, 
                playerPos.y +yOffset, this.transform.position.z);
        }
    }
}
