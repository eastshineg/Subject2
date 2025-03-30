using Neople.Effect;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

namespace Neople
{
	public partial class NeoplePlayObject
	{
		public class EffectService
		{
			PlayerBlackBoard _blackboard = null;
			
			IncreseSpeedeEffect _increseSpeedeEffect = new();
			RecoverHPEffect _recoverHpEffect = new();
			
			Dictionary<Effect.EnumEffect, IEffect> _dictEffects = new();
			List<IUpdateEffect> _updateEffects = new();
			
			internal void ChangeBlackBoard(PlayerBlackBoard blackboard)
			{
				_blackboard = blackboard;

				_dictEffects.Clear();
				_updateEffects.Clear();

				_dictEffects.Add(_increseSpeedeEffect.EffectType, _increseSpeedeEffect);
				_dictEffects.Add(_recoverHpEffect.EffectType, _recoverHpEffect);

				foreach (var eff in _dictEffects.Values)
				{
					if (eff is not IUpdateEffect updateEffect)
					{
						continue;
					}
					
					_updateEffects.Add(updateEffect);
				}
			}

			internal void ApplyEffect(IEffectData effectData)
			{
				if (_blackboard == null)
				{
					return;
				}

				if (_dictEffects.TryGetValue(effectData.EffectType, out var effect) == false)
				{
					Debug.LogError($"{effectData.EffectType} not found");
					return;
				}

				effect.Apply(effectData);
				if (effect.IsDirty == true)
				{
					apply();	
				}
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
					Debug.Log("effect, _increseSpeedeEffect prev speed : " + _blackboard.Stat.Speed.Speed);
					_blackboard.Stat.Speed.Change(_blackboard.Stat.Speed.Speed * _increseSpeedeEffect.SpeedFactor);

					Debug.Log("effect, _increseSpeedeEffect curr speed : " + _blackboard.Stat.Speed.Speed);
					_increseSpeedeEffect.Clean();
				}

				if (_recoverHpEffect.IsDirty == true)
				{
					Debug.Log("effect, _recoverHpEffect prev HP : " + _blackboard.Stat.HP.Curr);
					_blackboard.Stat.HP.Change(_blackboard.Stat.HP.Curr + _recoverHpEffect.HP);
					
					Debug.Log("effect, _recoverHpEffect HP : " + _blackboard.Stat.HP.Curr);
					_recoverHpEffect.Clean();
				}
			}
			
			internal void Update()
			{
				var isDirty = false;
				foreach (var eff in _updateEffects)
				{
					eff.Update();
				}

				foreach (var eff in _dictEffects.Values)
				{
					if (isDirty == false)
					{
						isDirty = eff.IsDirty;
						if (isDirty == true)
						{
							break;
						}
					}
				}
				
				if (isDirty == true)
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