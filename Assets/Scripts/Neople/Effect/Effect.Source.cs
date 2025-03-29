using Unity.VisualScripting.FullSerializer.Internal.Converters;
using UnityEngine;
namespace Neople.Effect.Source
{
	public class IncreseSpeedData : IEffectData
	{
		public IncreseSpeedData(float factor, float duration)
		{
			_factor = factor;
			_duration = duration;
		}
		
		public EnumEffect EffectType => EnumEffect.IncreseSpeed;
		
		float _factor = 1f;
		public float Factor => _factor;
		float _duration = 0f;
		public float Duration => _duration;
		
		public bool IsValid()
		{
			return _duration > 0f;
		}
	}
	
	public class RecoverHPSouce : IEffectData
	{
		public RecoverHPSouce(int hp)
		{
			_hp = hp;
		}
		
		public EnumEffect EffectType => EnumEffect.RecoverHP;
		
		int _hp = 0;
		public int HP => _hp;
		public bool IsValid()
		{
			return _hp > 0;
		}
	}
}