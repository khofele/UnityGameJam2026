using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityProperties", menuName = "Scriptable Entity Objects/EntityProperties")]
public class EntityProperties : ScriptableObject
{
    [Header("Entity Stats")]
    [SerializeField] private int m_minActionPoints = 0;
    [SerializeField] private int m_maxActionPoints = 0;
    [SerializeField] private float m_maxHealth = 100.0f;
    //Hier auf max damage und min damage --- ist noch im levelManager


    [Header("Entity Actions")]
    [SerializeField] private List<EntityAction> m_entityActions = null;

    public int MinActionPoints
    {
        get { return m_minActionPoints; }
    }

    public int MaxActionPoints
    {
        get { return m_maxActionPoints; }
    }

    public float MaxHealth 
    {
        get { return m_maxHealth; }
    }

    public List<EntityAction> EntityActions
    {
        get { return m_entityActions; }
    }
}
