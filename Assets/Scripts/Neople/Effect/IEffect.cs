namespace Neople.Effect
{
	
	public interface IEffect
	{
		public EnumEffect EffectType { get; }
		public void Apply(IEffectData data);
		bool IsDirty { get; }
		void Clean();
	}

	// 자체 update가 필요한 경우만 사용
	public interface IUpdateEffect
	{
		void Update();
	}
	
	public interface IEffectData
	{
		public EnumEffect EffectType { get; }
		bool IsValid();
	}
}