using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverPoint : MonoBehaviour
{
    public bool occupied ; //Check xem cover point da duoc su dung hay chua
    public void SetOccupied(bool value)
    {
        this.occupied = value; // Set the occupied status of the cover point
    }
}
