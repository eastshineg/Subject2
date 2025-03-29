using Neople.Effect;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
namespace Neople
{
	public partial class NeoplePlayObject
	{
		public class EffectService
		{
			PlayerBlackBoard _blackboard = null;
			
			IncreseSpeedeEffect _increseSpeedeEffect = new();
			RecoverHPEffect _recoverHpEffect = new();
			
			internal void ChangeBlackBoard(PlayerBlackBoard blackboard)
			{
				_blackboard = blackboard;
			}

			internal void ApplyEffect(IEffectData effectData)
			{
				if (_blackboard == null)
				{
					return;
				}
				
				switch (effectData.EffectType)
				{
					case EnumEffect.IncreseSpeed:
						{
							_increseSpeedeEffect.Apply(effectData);
						}
						break;
					case EnumEffect.RecoverHP:
						{
							_recoverHpEffect.Apply(effectData);
						}
						break;
				}

				apply();
			}

			void apply()
			{
				if (_blackboard == null)
				{
					return;
				}
				
				if (_increseSpeedeEffect.IsDirty == true)
				{
					_blackboard.Stat.Speed.Reset();
					_blackboard.Stat.Speed.Change(_blackboard.Stat.Speed.Speed * _increseSpeedeEffect.SpeedFactor);
					
					_increseSpeedeEffect.Clean();
				}

				if (_recoverHpEffect.IsDirty == true)
				{
					_blackboard.Stat.HP.Change(_blackboard.Stat.HP.Curr + _recoverHpEffect.HP);
					_recoverHpEffect.Clean();
				}
			}
			
			internal void Update()
			{
				_increseSpeedeEffect.Update();
				
				if (_increseSpeedeEffect.IsDirty == true)
				{
					apply();
				}
			}

			internal void Release()
			{
				_blackboard = null;
			}
		}
	}
}