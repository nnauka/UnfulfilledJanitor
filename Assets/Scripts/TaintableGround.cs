using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaintableGround : MonoBehaviour, ITaintable
{
    private float totalTaint;

    public void Taint(float intensity)
    {
        totalTaint -= intensity;
    }
}
