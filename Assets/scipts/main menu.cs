using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void startGame()
   { 
   SceneManager.LoadScene("select scene");
   }
   public void quit()
   { Application.Quit(); }
}