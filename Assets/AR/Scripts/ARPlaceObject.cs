using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceObjects : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private Transform playerTransform;
    bool isPlacing = false;

   private List<GameObject> spawnedObjects = new List<GameObject>();

   void Start()
    {
        // Get the ARRaycastManager component if it's not already assigned
        raycastManager ??= GetComponent<ARRaycastManager>();
    }
    void Update()
    {
        // Exit early if ARRaycastManager is not assigned
        if (raycastManager == null) return;
        // Handle touch input (on phones/tablets)
        if (Touchscreen.current != null &&
        Touchscreen.current.touches.Count > 0 &&
        Touchscreen.current.touches[0].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began && !isPlacing)
        {
            // Get touch position on screen
            Vector2 touchPos = Touchscreen.current.touches[0].position.ReadValue();

            if (IsTouchOverUIObject(touchPos))  return;

         isPlacing = true;
            // Place the object at touch position
            PlaceObject(touchPos);
        }
        // Handle mouse input (for desktop testing)
        else if (Mouse.current != null &&
        Mouse.current.leftButton.wasPressedThisFrame && !isPlacing)
        {
            // Get mouse position on screen
            Vector2 mousePos = Mouse.current.position.ReadValue();

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            isPlacing = true;
            // Place the object at mouse click
            PlaceObject(mousePos);
        }
    }
    void PlaceObject(Vector2 position)
    {
        // Create a list to store raycast hit results
        var rayHits = new List<ARRaycastHit>();
        // Raycast from screen position into AR world using trackable type
        raycastManager.Raycast(position, rayHits, TrackableType.PlaneWithinPolygon);
        // If we hit a valid surface
        if (rayHits.Count > 0)
        {
            // Get the position and rotation of the first hit
            Vector3 hitPosePosition = rayHits[0].pose.position + new Vector3(0, 0.1f, 0);
            // Quaternion hitPoseRotation = rayHits[0].pose.rotation;
            Quaternion hitPoseRotation = Quaternion.Euler(0, Quaternion.LookRotation(hitPosePosition-playerTransform.position).eulerAngles.y-90, 0);
            // Instantiate the prefab at the hit location
            var obj = Instantiate(prefabs[Random.Range(0,prefabs.Length)], hitPosePosition, hitPoseRotation);
            spawnedObjects.Add(obj);
        }
        // Wait briefly before allowing another placement
        StartCoroutine(SetPlacingToFalseWithDelay());
    }
    IEnumerator SetPlacingToFalseWithDelay()
    {
        // Wait for a short delay
        yield return new WaitForSeconds(0.25f);
        // Allow placing again
        isPlacing = false;
    }

   private bool IsTouchOverUIObject(Vector2 touchPosition)
   {
      PointerEventData eventData = new PointerEventData(EventSystem.current);
      eventData.position = touchPosition;

      List<RaycastResult> results = new List<RaycastResult>();
      EventSystem.current.RaycastAll(eventData, results);

      return results.Count > 0;
   }

   public void Restart()
   {
      foreach (var item in spawnedObjects) Destroy(item);
      spawnedObjects.Clear();
   }
}
