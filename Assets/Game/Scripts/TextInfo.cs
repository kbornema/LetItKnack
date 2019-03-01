using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TextInfo : ScriptableObject
{   
    [TextArea(2, 25)]
    public string text = default;
}
