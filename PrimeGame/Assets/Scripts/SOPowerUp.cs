using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create Power Up", menuName = "Create Power Up")]
public class SOPowerUp : ScriptableObject
{
    public string actorName;
    public PowerUpType powerUpType;

    public enum PowerUpType
    {
        plusHealth, magnet, shield, doubleMoney, doubleScore
    }

    public string description;
    public int cost;
    public Material actorMaterial;
}
