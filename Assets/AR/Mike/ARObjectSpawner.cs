using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARObjectSpawner : MonoBehaviour
{
   public GameObject beerPrefab;
   public GameObject keyPrefab;
   public GameObject truckPrefab;

   public ARPlaneManager planeManager;
   public float heightOffset = 0.05f; // Slightly above the plane

   private bool hasSpawned = false;

   void Update()
   {
      // Wait until at least one plane is detected and we haven't spawned yet
      if (!hasSpawned && planeManager.trackables.count > 0)
      {
         SpawnObjects();
         hasSpawned = true;
      }
   }

   void SpawnObjects()
   {
      List<ARPlane> planes = new List<ARPlane>();//planeManager.trackables

      if (planes.Count == 0) return;

      // Pick a random plane (or use largest one)
      ARPlane plane = planes[Random.Range(0, planes.Count)];

      // Spawn beers
      int beerNum = Random.Range(5, 21);
      for (int i = 0; i < beerNum; i++)
      {
         Vector3 position = GetRandomPointOnPlane(plane);
         Instantiate(beerPrefab, position, Quaternion.identity);
      }

      // Spawn key
      Instantiate(keyPrefab, GetRandomPointOnPlane(plane), Quaternion.identity);

      // Spawn truck
      Instantiate(truckPrefab, GetRandomPointOnPlane(plane), Quaternion.identity);
   }

   Vector3 GetRandomPointOnPlane(ARPlane plane)
   {
      Vector3 center = plane.transform.position;
      Vector3 normal = plane.transform.up;

      // Plane's boundary polygon points (for random position)
      var boundary = new List<Vector2>();
      //plane.boundary.CopyTo(boundary);

      if (boundary.Count < 3)
      {
         // Fallback if boundary is too small
         return center + normal * heightOffset;
      }

      // Pick a random point in plane bounds
      Vector2 randomPoint2D = boundary[Random.Range(0, boundary.Count)];
      Vector3 worldPoint = plane.transform.TransformPoint(new Vector3(randomPoint2D.x, 0, randomPoint2D.y));

      // Add height offset
      return worldPoint + normal * heightOffset;
   }
}