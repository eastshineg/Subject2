using Neople.Effect;
namespace Neople
{
	public class ItemBlackBoard : BlackBoard
	{
		public IEffectData EffectData { get; private set; }
		internal void SetEffect(IEffectData effectData)
		{
			if (effectData.IsValid() == false)
			{
				return;
			}
			
			EffectData = effectData;
		}
		
		public override EnumObjectType ObjectType => EnumObjectType.Item;
		public override void Clear()
		{
			
		}
	}
	
	public class NeopleItemObject : NeopleObject
	{
		ItemBlackBoard _cachedBlackBoard = null;
		public ItemBlackBoard CachedBlackBoard => _cachedBlackBoard;
		
		public override void Initialize(int objectId)
		{
			base.Initialize(objectId);
			_blackBoard ??= new ItemBlackBoard();
			_cachedBlackBoard = _blackBoard as ItemBlackBoard;
		}

		public void SetEffect(Effect.IEffectData effectData)
		{
			CachedBlackBoard.SetEffect(effectData);
		}
	}
}