using Neople;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Singleton(true)]
public class GameSupervisor : Singleton<GameSupervisor>
{
     ObjectManager _objectManager = new();

     public void Play()
     {
          if (_objectManager.TryClaim(EnumObjectType.Player, out var player) == false)
          {
               Debug.LogError("player is not valid");
               return;
          }
          
          if (_objectManager.TryClaim(EnumObjectType.Item, out var item1) == false)
          {
               Debug.LogError("item is not valid");
               return;
          }
          
          if (_objectManager.TryClaim(EnumObjectType.Item, out var item2) == false)
          {
               Debug.LogError("item is not valid");
               return;
          }
     }
     
     public void Update()
     {
          _objectManager.Update();
     }
}
