using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Neople
{
	public class ObjectManager
	{
		int _keyId = 0;
		Dictionary<EnumObjectType, List<NeopleObject>> _dictObjects = new();
		HashSet<NeopleObject> _claimedDictObjects = new();
		
		public void Release(NeopleObject NeopleObject)
		{
			if (NeopleObject == null) return;
			// 중복 해제 방지
			if (NeopleObject.IsValid == false) return;
			if (_dictObjects.TryGetValue(NeopleObject.ObjectType, out var objList) == false)
			{
				Debug.LogError("Not found object list");
				return;
			}
			
			NeopleObject.Release();
			objList.Add(NeopleObject);

			_claimedDictObjects.Remove(NeopleObject);
		}
		
		public bool TryClaim(EnumObjectType objectType, out NeopleObject outNeopleObject)
		{
			outNeopleObject = null;
			
			if (_dictObjects.TryGetValue(objectType, out var objList) == false)
			{
				objList = new();
				_dictObjects.Add(objectType, objList);
			}
			
			for(int i = 0; i < objList.Count; i++)
			{
				var obj = objList[i];
				if (obj.IsValid == false)
				{
					outNeopleObject = obj;
					objList.RemoveAt(i);
					break;
				}
			}

			if (outNeopleObject == null)
			{
				switch (objectType)
				{
					case EnumObjectType.Player:
						outNeopleObject = new NeoplePlayObject();
						break;
					case EnumObjectType.Item:
						outNeopleObject = new NeopleItemObject();
						break;
					default:
						{
							Debug.LogError("Not implemented object type");
							return false;
						}
				}	
			}
			
			_keyId++;
			outNeopleObject.Initialize(_keyId);
			if (outNeopleObject.IsValid == false)
			{
				Release(outNeopleObject);
				outNeopleObject = null;
				return false;
			}
			
			_claimedDictObjects.Add(outNeopleObject);
			return true;
		}

		public void Update()
		{
			foreach (var obj in _claimedDictObjects)
			{
				if (obj.IsValid == false)
				{
					continue;
				}
				
				obj.Update();
			}
		}
	}
}