using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHand : MonoBehaviour
{
    // Reference to the character.
    [SerializeField] Character character;
    public Character Character
    {
        get
        {
            return character;
        }
    }

    // The game object being pushed.
    GameObject pushedGameObject;  
    public GameObject PushedGameObject
    {
        get
        {
            return pushedGameObject;
        }
    }

    // The initial drag of the pushed object.
    // When an object is pushed, its drag is set to 0.
    // Use this variable to reset drag when the object stops being pushed by the player.
    float pushedGameObjectInitialDrag;
    public float PushedGameObjectInitialDrag
    {
        get
        {
            return pushedGameObjectInitialDrag;
        }
    }

    private void Awake()
    {
        if(!character)
        {
            character = FindObjectOfType<Character>();
        }
    }

    // Start pushing an object.
    // gameObject is the object to be pushed.
    // initialPushingDirection is the initial direction in world space from the player to the pushed object.
    public void StartPushingObject(GameObject gameObject, Vector3 initialPushingDirection)
    {
        // I'm current pushing an object. I cannot push another one.
        if (pushedGameObject)
            return;

        // I cannot push this object if it has no physics.
        Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();
        if (!rigidBody)
            return;

        // I cannot push this object if it is not pushable.
        PushableObject pushable = gameObject.GetComponent<PushableObject>();
        if (!pushable)
            return;

        // The hand needs to know what game object is being pushed, and the pushed object needs to know the character that is pushing it.
        pushedGameObject = gameObject;
        pushable.pusher = character;

        // Set the pushed object's drag to 0 to make it easy to move.
        pushedGameObjectInitialDrag = rigidBody.drag;
        rigidBody.drag = 0;
   
        // Set the pushing direction relative to the pushed object.
        pushable.relativePushingDirection = pushable.transform.InverseTransformDirection(initialPushingDirection.normalized);

        // Set the initial position of the character relative to the pushed object.
        pushable.relativeAttachmentPosition = pushable.transform.InverseTransformPoint(character.transform.position);
    }

    public void StopPushingObject()
    {
        if (!pushedGameObject)
            return;

        // Reset the drag of the pushed object.
        Rigidbody rigidBody = pushedGameObject.GetComponent<Rigidbody>();
        rigidBody.drag = pushedGameObjectInitialDrag;

        // Detach the pushed object from the character.
        PushableObject pushable = pushedGameObject.GetComponent<PushableObject>();
        pushable.pusher = null;
        pushedGameObject = null;
    }
}