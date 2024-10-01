using UnityEngine;
using UnityEngine.UI;

public class SpeedToggle : Toggle
{
    // Custom property not show in inspector for almost UI component
    // I am lazy to rewirte editor panel, use debug model to change this value
    public int moveSpeed;
}
