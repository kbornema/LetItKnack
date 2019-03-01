using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HierarchySeparator_Editor : MonoBehaviour
{
    private const string PREFIX = "----------";
    private const string EDITOR_TAG = "EditorOnly";

    [SerializeField]
    private string _name = "";
    [SerializeField, HideInInspector]
    private string _lastName = "";

    private void Reset()
    {
        gameObject.tag = EDITOR_TAG;
        gameObject.isStatic = true;
        transform.localPosition = Vector3.zero;
    }

    private void OnValidate()
    {
        if (_name != _lastName)
        {
            _lastName = _name;
            gameObject.name = string.Concat(PREFIX, _name, PREFIX);
        }
    }

    private void Start()
    {
        if (transform.childCount > 0)
        {
            Debug.LogWarning("EditorGameObject has children object. This might cause bugs in build. Intended?", this);
        }

        if (tag != EDITOR_TAG)
        {
            Debug.Log("EditorGameObject has not EditorOnly tag set!", this);
        }
    }
}
