using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHand : MonoBehaviour
{
    GameObject pushedGameObject;  // The game object being pushed.
    public GameObject PushedGameObject
    {
        get
        {
            return pushedGameObject;
        }
    }

    // The initial mass of the pushed object.
    // Use to reset the mass when the pushed object is dropped.
    float pushedGameObjectMash;

    private void FixedUpdate()
    {
        UpdatePushedObjectPosition();
    }

    public void StartPushingObject(GameObject gameObject)
    {
        // Check if gameObject is null.
        if (!gameObject)
            return;

        // Check if gameObject has Rigidbody component.
        Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();
        if (!rigidBody)
            return;

        // 
        pushedGameObject = gameObject;
        pushedGameObject.transform.parent = transform;
        transform.position = rigidBody.position;
        rigidBody.useGravity = false;
        pushedGameObjectMash = rigidBody.mass;
        rigidBody.mass = 0.0f;
        rigidBody.freezeRotation = true;
        rigidBody.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    public void UpdatePushedObjectPosition()
    {
        if (!pushedGameObject)
            return;

        // If pushedGameObject is too far from the hand, stop pushing it.
        if(Vector3.Distance(pushedGameObject.transform.position, transform.position) > 5.0f)
        {
            StopPushingObject();
            return;
        }

        Rigidbody rigidBody = pushedGameObject.GetComponent<Rigidbody>();
        rigidBody.position = transform.position;      
        //rigidBody.rotation = transform.rotation;
    }

    public void StopPushingObject()
    {
        if (!pushedGameObject)
            return;

        Rigidbody rigidBody = pushedGameObject.GetComponent<Rigidbody>();
        rigidBody.mass = pushedGameObjectMash;
        rigidBody.useGravity = true;
        rigidBody.velocity = new Vector3(0.0f, 0.0f, 0.0f);

        pushedGameObject.transform.parent = null;
        pushedGameObject = null;
    }
}
