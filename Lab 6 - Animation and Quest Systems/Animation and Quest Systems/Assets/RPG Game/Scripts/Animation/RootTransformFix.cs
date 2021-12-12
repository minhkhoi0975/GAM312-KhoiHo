/**
 * AnimationFix.cs
 * Description: This script adjusts the transform of a character's mesh. Some characters have root that is not at the bottom of the mesh, so they may have awkward transfrom when the play an animation.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootTransformFix : MonoBehaviour
{
    // The pivot of the character mesh.
    [SerializeField] Transform pivotTransform;

    // The root of the character mesh.
    [SerializeField] Transform rootTransform;

    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector3(0.0f, rootTransform.localPosition.y, 0.0f);
    }
}
