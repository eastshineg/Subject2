using Unity.VisualScripting.FullSerializer.Internal.Converters;
using UnityEngine;
namespace Neople.Effect
{
	public enum EnumEffect
	{
		None = 0,
		IncreseSpeed = 1,
		RecoverHP = 2,
	}

	public class IncreseSpeedeEffect : IUpdateEffect, IEffect
	{
		public EnumEffect EffectType => EnumEffect.IncreseSpeed;

		bool _isDirty = false;
		public bool IsDirty => _isDirty;
		float _originalDuration = 0f;
		float _originalFactor = 1f;
		float _currDuration = 0f;
		float _currFactor = 1f;
		public float SpeedFactor => _currFactor;
		
		public void Apply(IEffectData data)
		{
			Debug.Log("effect data, " + data.EffectType);
			
			if (data.IsValid() == false)
			{
				Debug.LogError("[IncreseSpeedeEffect] invalid effect data");
				return;
			}
			
			if (data is not Data.IncreseSpeedData incSpeedData)
			{
				Debug.LogError("[IncreseSpeedeEffect] invalid type");
				return;
			}

			_currFactor = incSpeedData.Factor;
			_currDuration = incSpeedData.Duration;
			_isDirty = true;
		}

		public void Clean() { _isDirty = false; }

		public void Update()
		{
			if (_currDuration <= 0f)
			{
				return;
			}
			
			_currDuration -= Time.deltaTime;

			if (_currDuration <= 0f)
			{
				_currDuration = 0f;
				_currFactor = 0f;
				_isDirty = true;
			}
		}
	}
	
	public class RecoverHPEffect : IEffect
	{
		public EnumEffect EffectType => EnumEffect.RecoverHP;
		bool _isDirty = false;
		public bool IsDirty => _isDirty;
		public void Clean() { _isDirty = false; }
		
		public int HP { get; private set; } = 20;
		public void Apply(IEffectData data)
		{
			Debug.Log("effect data, " + data.EffectType);
			
			if(data is not Data.RecoverHPData recoverHPSouce)
			{
				Debug.LogError("[RecoverHPEffect] invalid type");
				return;
			}
			
			HP = recoverHPSouce.HP;
			_isDirty = true;
		}
	}
}