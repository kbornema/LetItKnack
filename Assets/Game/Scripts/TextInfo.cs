using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TextInfo : ScriptableObject
{   
    [TextArea()]
    public string text = default;
}
