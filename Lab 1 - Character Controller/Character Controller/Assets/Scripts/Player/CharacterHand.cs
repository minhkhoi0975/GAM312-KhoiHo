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

    private void FixedUpdate()
    {
        UpdatePushedObjectPosition();
    }

    public void PickUpObject(GameObject gameObject)
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
        transform.position = rigidBody.position;
        rigidBody.useGravity = false;
        rigidBody.freezeRotation = true;
        rigidBody.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    public void UpdatePushedObjectPosition()
    {
        if (!pushedGameObject)
            return;

        Rigidbody rigidBody = pushedGameObject.GetComponent<Rigidbody>();
        rigidBody.position = transform.position;
        rigidBody.rotation = transform.rotation;
    }

    public void DropObject()
    {
        if (!pushedGameObject)
            return;

        Rigidbody rigidBody = pushedGameObject.GetComponent<Rigidbody>();
        rigidBody.useGravity = true;
        rigidBody.velocity = new Vector3(0.0f, 0.0f, 0.0f);

        pushedGameObject = null;
    }
}
