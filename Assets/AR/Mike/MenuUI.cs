using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARFoundation;

public class MenuUI : MonoBehaviour
{
   public ARObjectSpawner spawner;
   public ARPickupInteraction interactor;

   public void RetartGame()
   {
      spawner.restart();
      interactor.restart();
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
