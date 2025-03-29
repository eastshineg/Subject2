using Neople.Effect;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
namespace Neople
{
	public interface IStat { }
	public interface IResetableStat : IStat
	{
		void Reset();
	}
	
	public class SpeedStat : IResetableStat
	{
		float _speed = 1f;
		float _currSpeed = 1f;
		
		public float Speed => _currSpeed;
		public void Change(float speed)
		{
			_currSpeed = speed;
		}
		public void Reset()
		{
			_currSpeed = _speed;
		}
	}

	public class HPStat : IStat
	{
		int _maxHP = 0;
		int _curr = 0;
		public int Curr => _curr;
		public void ChangeMax(int maxHp)
		{
			_maxHP = maxHp;
		}
		
		public void Change(int hp)
		{
			_curr = math.min(hp, _maxHP);
		}
	}

	public class StatInfo
	{
		SpeedStat _speedStat = new();
		public SpeedStat Speed => _speedStat;

		HPStat _hpStat = new();
		public HPStat HP => _hpStat;
	}
	
	public class PlayerBlackBoard : BlackBoard
	{
		public override EnumObjectType ObjectType => EnumObjectType.Player;
		
		public override void Clear()
		{
			_statInfo.Speed.Reset();
			_statInfo.HP.ChangeMax(0);
			_statInfo.HP.Change(0);
		}

		StatInfo _statInfo = new();
		public StatInfo Stat => _statInfo;
	}
	
	public partial class NeoplePlayObject : NeopleObject
	{
		PlayerBlackBoard _cachedBlackBoard = null;
		PlayerBlackBoard PlayerBoard
		{
			get
			{
				if (_cachedBlackBoard == null) _cachedBlackBoard = _blackBoard as PlayerBlackBoard;
				return _cachedBlackBoard;
			}
		}
		
		EffectService _effService = new();
		
		public override void Initialize(int objectId)
		{
			base.Initialize(objectId);
			
			_blackBoard ??= new PlayerBlackBoard();
			_blackBoard.Clear();

			var playerBlackBoard = PlayerBoard;
			playerBlackBoard.Clear();
			playerBlackBoard.Stat.Speed.Reset();
			playerBlackBoard.Stat.HP.ChangeMax(200);
			playerBlackBoard.Stat.HP.Change(100);
			
			_effService.ChangeBlackBoard(PlayerBoard);
		}

		public override void Release()
		{
			base.Release();
			_effService.Release();
			PlayerBoard?.Clear();
		}
		
		public override void ApplyEffect(IEffectData effectData)
		{
			_effService.ApplyEffect(effectData);
		}

		public override void Update()
		{
			base.Update();
			
			_effService.Update();
		}
	}
}