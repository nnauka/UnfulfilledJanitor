using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITaintable
{
    public void Taint(float intensity);
}
