using UnityEngine;

public class Powerup : MonoBehaviour
{
    public enum Type
    {
        Dropped,
        NonDropped
    }

    private Type _type = Type.NonDropped;

    public Type PowerupType
    {
        get
        {
            return _type;
        }

        set
        {
            _type = value;
        }
    }
}
