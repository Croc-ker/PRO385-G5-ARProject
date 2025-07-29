using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections;

public class ARObjectSpawner : MonoBehaviour
{
   public GameObject beerPrefab;
   public GameObject keyPrefab;
   public GameObject truckPrefab;
   public ARPickupInteraction interactionScript;

   public ARPlaneManager planeManager;
   public float heightOffset = 0.1f; // Slightly above the plane

   private bool beerSpawned = false;
   private bool keySpawned = false;
   private bool truckSpawned = false;

   private float beerTimer = 0;
   private float keyTimer = 0;
   private float truckTimer = 0;

   private GameObject keyObj;
   private GameObject truckObj;

   private int beerLimit = 5;

   void Update()
   {
      if (!beerSpawned && planeManager.trackables.count >= 2)
      {
         if(interactionScript.collectedBeers == interactionScript.spawnedBeers.Count+1)
         {
            if (interactionScript.collectedBeers == beerLimit) beerSpawned = true;
            else SpawnBeers();//StartCoroutine(SpawnTimer("beer"));
         }
         
      }
      else if(beerSpawned && !keySpawned && !interactionScript.drinking)
      {
         interactionScript.goal.text = "You need to experience a DUI, go find your truck keys!";
         StartCoroutine(SpawnTimer("key"));
         keySpawned = true;
      }
      else if(!truckSpawned && interactionScript.hasKey)
      {
         interactionScript.goal.text = "You have your keys, now go find you truck!";
         StartCoroutine(SpawnTimer("truck"));
         truckSpawned = true;
      }

      if (!beerSpawned && interactionScript.drinking)
      {
         beerTimer += Time.deltaTime;
         if(beerTimer >= 30)
         {
            int index = interactionScript.spawnedBeers.Count-1;
            var obj = interactionScript.spawnedBeers[index];
            obj.SetActive(false);
            interactionScript.spawnedBeers.RemoveAt(index);
            Destroy(obj);
            beerTimer = 0;
         }
      }
      else if (keySpawned && !interactionScript.hasKey)
      {
         keyTimer += Time.deltaTime;
         if (keyTimer >= 30)
         {
            Destroy(keyObj);
            StartCoroutine(SpawnTimer("key"));
            keyTimer = 0;
         }
      }
      else if (truckSpawned && !interactionScript.driving)
      {
         truckTimer += Time.deltaTime;
         if (truckTimer >= 30)
         {
            Destroy(truckObj);
            StartCoroutine(SpawnTimer("truck"));
            truckTimer = 0;
         }
      }
   }

   void SpawnBeers()
   {
      List<ARPlane> planes = new List<ARPlane>();
      foreach (var item in planeManager.trackables) planes.Add(item);
      //int beerCount = Random.Range(4, 7);

      //for (int i = 0; i < beerCount; i++)
      //{
      //   var randomPlane = planes[Random.Range(0, planes.Count)];
      //   Vector3 spawnPos = GetRandomLocation(randomPlane);
      //   var obj = Instantiate(beerPrefab, spawnPos, Quaternion.Euler(180f, 0f, 0f));
      //   interactionScript.spawnedBeers.Add(obj);
      //}
      var randomPlane = planes[Random.Range(0, planes.Count)];
      Vector3 spawnPos = GetRandomLocation(randomPlane);
      var obj = Instantiate(beerPrefab, spawnPos, Quaternion.Euler(180f, 0f, 0f));
      interactionScript.spawnedBeers.Add(obj);
      beerTimer = 0;
      StartCoroutine(BeerTimer(obj));
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
         keyObj = Instantiate(keyPrefab, GetRandomLocation(plane), Quaternion.identity);
         interactionScript.pickupObject = keyObj;
      }
      else
      {
         // Spawn truck
         truckObj = Instantiate(truckPrefab, GetRandomLocation(plane), Quaternion.identity);
         interactionScript.carObject = truckObj;
      }
   }

   IEnumerator SpawnTimer(string type)
   {
      yield return new WaitForSeconds(8);
      switch (type)
      {
         case "beer":
            SpawnBeers();  // Old Beer code no longer in use
            break;
         case "key":
            SpawnObjects(true);
            break;
         case "truck":
            SpawnObjects(false);
            break;
      }
   }

   IEnumerator BeerTimer(GameObject obj)
   {
      obj.SetActive(false);
      yield return new WaitForSeconds(1);
      obj.SetActive(true);
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

   public void restart()
   {
      StopAllCoroutines();
      beerSpawned = false;
      keySpawned = false;
      truckSpawned = false;

      Destroy(keyObj);
      Destroy(truckObj);

      keyTimer = 0;
      truckTimer = 0;
      beerTimer = 0;
   }
}