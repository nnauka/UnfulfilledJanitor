using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InspectorExtensions
{
    private const float ZFightPreventionOffset = 0.01f;

    [MenuItem("BolekTools/Align With Surface Below %g")]
    public static void AlignWithSurfaceBelow()
    {
        var selectedItems = Selection.transforms;
        Undo.RecordObjects(selectedItems, "AligningWithSurface");
        foreach (var selectedItem in selectedItems)
        {
            if (Physics.Raycast(selectedItem.position, Vector3.down, out RaycastHit hit, 2))
            {
                selectedItem.rotation = Quaternion.FromToRotation(selectedItem.up, hit.normal) * selectedItem.rotation;
                selectedItem.position = hit.point;
                selectedItem.position += new Vector3(0, ZFightPreventionOffset, 0);
            } 
        }
    }

    [MenuItem("BolekTools/Align With Surface Below %g", true)]
    public static bool ValidateAlignWithSurfaceBelow()
    {
        return Selection.transforms != null && Selection.transforms.Length > 0;
    }
}
