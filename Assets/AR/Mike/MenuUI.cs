using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARFoundation;

public class MenuUI : MonoBehaviour
{
   public ARObjectSpawner spawner;
   public ARPickupInteraction interactor;
   public PlaceObjects placer;
   public ARSession ARSession;

   public Toggle options;

   public void RetartGame()
   {
      SoundManager.StopLoopingSound();
      SoundManager.PlayLoopingSound(SoundType.ASGORE_PASSIVE, 0.05f);
      spawner.restart();
      interactor.restart();
      placer.Restart();
      options.isOn = false;
      //var obj = GameObject.Find("DrunkVolume");
      //obj.GetComponent<Volume>().enabled = false;
      //Destroy(obj);
      ARSession.Reset();
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
