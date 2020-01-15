﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class CustomNetworkManager : NetworkManager
{
   public void StartHosting()
   {
      SetPort();
      NetworkManager.singleton.StartHost();

   }

   public void JoinGame()
   {
      SetIPAddress();
      SetPort();
      NetworkManager.singleton.StartClient();
   }
   
   void SetPort()
   {
      NetworkManager.singleton.networkPort=7777;
   } 

   void SetIPAddress()
   {
      NetworkManager.singleton.networkAddress = "localhost";
   } 

   void OnLevelWasLoaded(int level)
   {
      if(level == 0)
      {
         
      }
      else
      {

      }
   }

   public void DisconnectFromHost()
   {
      NetworkManager.singleton.StopHost();
        SceneManager.LoadScene(0);
   }
}
