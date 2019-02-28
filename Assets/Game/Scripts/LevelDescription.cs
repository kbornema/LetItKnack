using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelDescription : ScriptableObject
{
    [SerializeField]
    public TextInfo TextToDisplay = default;

    public List<PinConfig> Pins = default;

    private void OnValidate()
    {   
        const int CONFIGS_NEEDED = 7;

        if(Pins.Count != CONFIGS_NEEDED)
        {
            Pins.Clear();

            for (int i = 0; i < CONFIGS_NEEDED; i++)
                Pins.Add(null);
        }
    }
}
