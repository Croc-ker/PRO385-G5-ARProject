using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARFoundation;

public class MenuUI : MonoBehaviour
{
   public ARObjectSpawner spawner;
   public ARPickupInteraction interactor;
   public PlaceObjects placer;
   public ARPlaneManager planeManager;

   public Toggle options;

   public void RetartGame()
   {
      SoundManager.StopLoopingSound();
      SoundManager.PlayLoopingSound(SoundType.ASGORE_PASSIVE, 0.75f);
      spawner.restart();
      interactor.restart();
      placer.Restart();
      options.isOn = false;
      //StartCoroutine(DestroyPlanesAfterFrame());


      //foreach (var plane in planeManager.trackables)
      //{
      //   plane.gameObject.SetActive(false);
      //}

      //planeManager.enabled = false;
      //planeManager.enabled = true;
   }

   private IEnumerator DestroyPlanesAfterFrame()
   {
      yield return new WaitForEndOfFrame();

      foreach (var plane in planeManager.trackables)
      {
         Destroy(plane.gameObject);
      }

      planeManager.enabled = false;
      planeManager.enabled = true;


      options.isOn = false;
   }

   public void ExitGame()
   {
      #if UNITY_EDITOR
         EditorApplication.isPlaying = false;
      #else 
         Application.Quit();
      #endif
   }
}
