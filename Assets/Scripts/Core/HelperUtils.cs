using UnityEngine;

public class HelperUtils : MonoBehaviour
{
    public static void ClearChildren(GameObject parent)
    {
        if (parent == null || parent.transform.childCount == 0)
        {
            return;
        }

        for (int i = parent.transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(parent.transform.GetChild(i).gameObject);
        }
    }
}
