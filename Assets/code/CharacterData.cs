using System.Collections.Generic;

[System.Serializable]
public class CharacterData
{
    public int hp;
    public int strengh;
    public float speed;
    public int attack_frequency;
    public float contact_range;
    public float attack_scope;
    public float reaction_rate_max;
    public float reaction_rate_min;
    public int wait;
}

[System.Serializable]
public class CharacterDataList
{
    public List<CharacterData> characters;
}
