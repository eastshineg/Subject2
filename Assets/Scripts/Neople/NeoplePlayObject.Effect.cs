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
			
			Dictionary<Effect.EnumEffect, IEffect> _dictEffects = new();
			List<IUpdateEffect> _updateEffects = new();
			
			internal void ChangeBlackBoard(PlayerBlackBoard blackboard)
			{
				_blackboard = blackboard;

				_dictEffects.Clear();
				_updateEffects.Clear();

				{
					var eff = new IncreseSpeedeEffect();
					_dictEffects.Add(eff.EffectType, eff);
				}

				{
					var eff = new RecoverHPEffect();
					_dictEffects.Add(eff.EffectType, eff);	
				}

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

				foreach (var eff in _dictEffects.Values)
				{
					if (eff.IsDirty == false)
					{
						continue;
					}

					switch (eff.EffectType)
					{
						case EnumEffect.IncreseSpeed:
							{
								if (eff is IncreseSpeedeEffect castEff)
								{
									_blackboard.Stat.Speed.Reset();
									if (castEff.Duration > 0f)
									{
										Debug.Log($"effect, _increseSpeedeEffect prev speed : {_blackboard.Stat.Speed.Speed}, duration : {castEff.Duration} ");
									}
									else
									{
										Debug.Log("effect, _increseSpeedeEffect prev speed : " + _blackboard.Stat.Speed.Speed);
									}
					
									_blackboard.Stat.Speed.Change(_blackboard.Stat.Speed.Speed * castEff.SpeedFactor);

									Debug.Log("effect, _increseSpeedeEffect curr speed : " + _blackboard.Stat.Speed.Speed);
									castEff.Clean();				
								}
							}
							break;
						case EnumEffect.RecoverHP:
							{
								if (eff is RecoverHPEffect castEff)
								{
									Debug.Log("effect, _recoverHpEffect prev HP : " + _blackboard.Stat.HP.Curr);
									_blackboard.Stat.HP.Change(_blackboard.Stat.HP.Curr + castEff.HP);
					
									Debug.Log("effect, _recoverHpEffect HP : " + _blackboard.Stat.HP.Curr);
									castEff.Clean();
								}
							}
							break;
					}
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