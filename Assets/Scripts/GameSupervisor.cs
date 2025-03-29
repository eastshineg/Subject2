using Neople;
using Neople.Effect.Source;
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

          {
               if (_objectManager.TryClaim(EnumObjectType.Item, out var item1) == false)
               {
                    Debug.LogError("item is not valid");
                    return;
               }

               if (item1 is NeopleItemObject neopleItemObject)
               {
                    neopleItemObject.SetEffect(new RecoverHPSouce(20));
                    player.ApplyEffect(neopleItemObject.CachedBlackBoard.EffectData);
               }
          }

          {
               if (_objectManager.TryClaim(EnumObjectType.Item, out var item2) == false)
               {
                    Debug.LogError("item is not valid");
                    return;
               }
          
               if (item2 is NeopleItemObject neopleItemObject)
               {
                    neopleItemObject.SetEffect(new IncreseSpeedData(2f, 10f));
                    player.ApplyEffect(neopleItemObject.CachedBlackBoard.EffectData);
               }
          }
     }
     
     public void Update()
     {
          _objectManager.Update();
     }
}
