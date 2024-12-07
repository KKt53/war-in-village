using System.Collections.Generic;

[System.Serializable]
public class CharacterData
{
    public int hp;
    public int strengh;
    public float speed;
    public int attack_frequency;
    public int contact_range;
    public int attack_scope;
    public int reaction_rate_max;
    public int reaction_rate_min;
    public int wait;
}

[System.Serializable]
public class CharacterDataList
{
    public List<CharacterData> characters;
}
