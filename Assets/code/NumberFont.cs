using System.Collections.Generic;
using UnityEngine;

public class NumberFont : MonoBehaviour
{
    public Sprite[] numberSprites; // 0?9のスプライトを割り当て
    private Dictionary<char, Sprite> spriteDictionary;

    private void Awake()
    {
        spriteDictionary = new Dictionary<char, Sprite>();

        for (int i = 0; i < numberSprites.Length; i++)
        {
            char digit = (char)('0' + i);
            spriteDictionary[digit] = numberSprites[i];
        }
    }

    public Sprite GetSprite(char digit)
    {
        if (spriteDictionary.TryGetValue(digit, out var sprite))
        {
            return sprite;
        }
        return null;
    }
}
