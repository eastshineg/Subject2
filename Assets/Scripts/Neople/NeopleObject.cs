using UnityEngine;

namespace Neople 
{
	public enum EnumObjectType
	{
		None = 0,
		Player = 1,
		Item = 2,
	}
	
	public abstract class BlackBoard
	{
		public virtual EnumObjectType ObjectType => EnumObjectType.None;
		public abstract void Clear();
	}
	
	public class NeopleObject
	{
		int _objectId = 0;
		// gameobject는
		protected BlackBoard _blackBoard = null;
		public virtual EnumObjectType ObjectType => _blackBoard == null ? EnumObjectType.None : _blackBoard.ObjectType;
		public virtual void Release()
		{
			_objectId = 0;
			_blackBoard?.Clear();
			_blackBoard = null;
		}
		

		public bool IsValid
		{
			get
			{
				if (_objectId == 0) return false;
				if (_blackBoard == null) return false;
				return true;
			}
		}
		
		public virtual void Initialize(int objectId)
		{
			_objectId = objectId;
		}

		public virtual void OnCollision(NeopleObject colliderObject)
		{
			
		}
		
		public virtual void Update()
		{
			
		}

		public virtual void ApplyEffect(Effect.IEffectData effectData) { }
	}
}