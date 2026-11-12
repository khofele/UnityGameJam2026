using UnityEngine;

public class Kinematics
{
    public Vector3 Position = new(0f, 0f, 0f);
    public float Rotation; //

    public Vector3 MoveVelo;
    public float RotVelo;

    public float MaxMoveVelo;
    public float MaxRotVelo;

    public Kinematics(float maxMoveVelo = 1f, float maxRotVelo = 2f)
    {
        MaxMoveVelo = maxMoveVelo;
        MaxRotVelo = maxRotVelo;
    }
}
