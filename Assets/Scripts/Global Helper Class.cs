using UnityEngine;
 
public static class GlobalHelperClass
{
    // This method generates a unique ID for a GameObject based on its scene name and position.
    // It concatenates the scene name with the x and y coordinates of the GameObject's position.
    // This is useful for identifying GameObjects uniquely in a scene.

     public static string GenerateUniqueID(GameObject obj)// This method generates a unique ID for a GameObject based on its scene name and position.
    {
        return $"{obj.scene.name}_{obj.transform.position.x}_{obj.transform.position.y}"; // This is useful for identifying GameObjects uniquely in a scene.
    }
}
