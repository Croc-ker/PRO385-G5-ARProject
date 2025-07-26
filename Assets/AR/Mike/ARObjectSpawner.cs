using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
//using System.Linq;
using System.Collections;

public class ARObjectSpawner : MonoBehaviour
{
   public GameObject beerPrefab;
   public GameObject keyPrefab;
   public GameObject truckPrefab;
   public ARPickupInteraction interactionScript;

   public ARPlaneManager planeManager;
   public float heightOffset = 0.05f; // Slightly above the plane

   private bool beerSpawned = false;
   private bool keySpawned = false;
   private bool truckSpawned = false;

   public int beersCollected = 0;

   void Update()
   {
      if (!beerSpawned && planeManager.trackables.count >= 2)
      {
         StartCoroutine(SpawnTimer("beer"));
         beerSpawned = true;
      }
      else if(beerSpawned & !keySpawned & beersCollected >= 5)
      {
         StartCoroutine(SpawnTimer("key"));
         keySpawned = true;
      }
      else if(!truckSpawned & interactionScript.hasKey)
      {
         StartCoroutine(SpawnTimer("truck"));
         truckSpawned = true;
      }
   }

   void SpawnBeers()
   {
      List<ARPlane> planes = new List<ARPlane>();
      foreach (var item in planeManager.trackables) planes.Add(item);
      int beerCount = Random.Range(5, 10);

      for (int i = 0; i < beerCount; i++)
      {
         var randomPlane = planes[Random.Range(0, planes.Count)];
         Vector3 spawnPos = GetRandomLocation(randomPlane);
         var obj = Instantiate(beerPrefab, spawnPos, Quaternion.identity);
         interactionScript.spawnedBeers.Add(obj);
      }
   }

   public void SpawnObjects(bool key)
   {
      List<ARPlane> planes = new List<ARPlane>();
      foreach (var item in planeManager.trackables) planes.Add(item);

      if (planes.Count == 0) return;

      // Pick a random plane (or use largest one)
      ARPlane plane = planes[Random.Range(0, planes.Count)];

      if (key)
      {
         // Spawn key
         var obj = Instantiate(keyPrefab, GetRandomLocation(plane), Quaternion.identity);
         interactionScript.pickupObject = obj;
      }
      else
      {
         // Spawn truck
         var obj = Instantiate(truckPrefab, GetRandomLocation(plane), Quaternion.identity);
         interactionScript.carObject = obj;
      }
   }

   IEnumerator SpawnTimer(string type)
   {
      yield return new WaitForSeconds(6);
      switch (type)
      {
         case "beer":
            SpawnBeers();
            break;
         case "key":
            SpawnObjects(true);
            break;
         case "truck":
            SpawnObjects(false);
            break;
      }
   }

   Vector3 GetRandomLocation(ARPlane plane)
   {
      Vector3 center = plane.center;
      Vector2 extents = plane.extents; // half-size of the bounding box

      float randomX = Random.Range(-extents.x, extents.x);
      float randomY = Random.Range(-extents.y, extents.y);

      Vector3 localPoint = new Vector3(randomX, 0, randomY);
      Vector3 worldPoint = plane.transform.TransformPoint(localPoint);

      return worldPoint + plane.transform.up * heightOffset;
   }
}