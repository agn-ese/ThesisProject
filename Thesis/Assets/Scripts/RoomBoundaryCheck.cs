using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomBoundaryCheck : MonoBehaviour
{
    [SerializeField] private Transform[] walls;
    private Vector3 roomMinBounds;
    private Vector3 roomMaxBounds;
    [SerializeField] private Transform[] respawnPoint;
    [SerializeField] private Transform[] objects;

    // Update is called once per frame
    private void Start()
    {
        roomMaxBounds = new Vector3(walls[0].position.x+1, walls[1].position.y+1, walls[2].position.z+1);
        roomMinBounds = new Vector3(walls[3].position.x-1, walls[4].position.y-1, walls[5].position.z-1);
    }
    void Update()
    {

        for (int i = 0; i < objects.Length; i++)
        {
            if (!IsObjectInsideRoom(objects[i].position))
            {
                RespawnObject(i);
            }
        }

    }

    bool IsObjectInsideRoom(Vector3 objectPosition)
    {
        // Controlla se l'oggetto è all'interno dei limiti definiti per ogni asse
        return (objectPosition.x >= roomMinBounds.x && objectPosition.x <= roomMaxBounds.x &&
                objectPosition.y >= roomMinBounds.y && objectPosition.y <= roomMaxBounds.y &&
                objectPosition.z >= roomMinBounds.z && objectPosition.z <= roomMaxBounds.z);
    }

    void RespawnObject(int index)
    {
        
        Rigidbody rb = objects[index].GetComponent<Rigidbody>();
        //rb.isKinematic = true;
        objects[index].position = respawnPoint[index].position;
        //rb.MovePosition(respawnPoint[index].position);
        //rb.MoveRotation(respawnPoint[index].rotation);
        //rb.isKinematic = false;
        rb.angularVelocity = Vector3.zero;
    }
}
