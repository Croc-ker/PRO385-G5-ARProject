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

   public Toggle options;

   public void RetartGame()
   {
      SoundManager.StopLoopingSound();
      SoundManager.PlayLoopingSound(SoundType.ASGORE_PASSIVE, 0.75f);
      spawner.restart();
      interactor.restart();
      placer.Restart();
      options.isOn = false;
      //StartCoroutine(RestartAR());
   }

   private IEnumerator RestartAR()
   {

      yield return null;
      //ARSession arSession = FindFirstObjectByType<ARSession>();
      //if (arSession != null)
      //{
      //   arSession.Reset();
      //   yield return null; // Let AR session process the reset
      //}

      //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
