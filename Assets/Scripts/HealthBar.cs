using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform healthBar;

    public void SetPercentFill(float percentFill)
    {
        healthBar.localScale = new Vector2(percentFill, 1);
    }
}
