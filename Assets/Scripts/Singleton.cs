using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class SingletonAttribute : Attribute
{
	bool _dontDestroy = false;
	public bool DontDestroy { get { return _dontDestroy; } }

	HideFlags _hideFlags = HideFlags.DontSave;
	public HideFlags HideFlag => _hideFlags;

	public SingletonAttribute(bool dontDestroy, HideFlags hideFlags = HideFlags.DontSave)
	{
		_dontDestroy = dontDestroy;
		_hideFlags = hideFlags;
	}
}

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	static T _instance = null;
	public static void WarmUp()
	{
		if (_instance != null) return;
		createInstance();
	}
	// Instance를 접근할때 instance를 생성하는 형태의 로직은 제거하고
	// 생성되었는지를 확인된 경우에 한해서 호출한다.
	// (원하지 않는 상황에서 instance가 강제로 활성화 되는 상황을 막고, 항상 instance가 없을 수 있는 상황을 유도해서 로직을 구성하게끔 하기 위한 방식)
	// singleton의 경우 생명주기를 유의미 하게 강제하지 않으면 무분별한 접근으로 인해 생성타이밍과 파괴 타이밍에서 다양한 crash나 올바르지 않은 로직을 수행하게 된다.
	public static bool TryGet(out T outInstance)
	{
		outInstance = null;
		
		if (_instance == null)
		{
			return false;
		}
		
		outInstance = _instance;
		return true;
	}

	static void createInstance()
	{
		_instance = FindObjectOfType<T>();

		if (_instance == null)
		{
			bool isDefine = Attribute.IsDefined(typeof(T), typeof(SingletonAttribute));
			bool dontDestroy = false;
			if (isDefine)
			{
				SingletonAttribute attr = (SingletonAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(SingletonAttribute));
				dontDestroy = attr.DontDestroy;
			}

			var name = typeof(T).ToString();
			var go = GameObject.Find(name);

			if (go == null)
			{
				go = new GameObject("[s]" + name);
				_instance = go.AddComponent<T>();
			}
			else
			{
				_instance = go.GetComponent<T>();
			}

#if UNITY_EDITOR
			if(_instance.tag.Contains("singleton") == false)
				_instance.tag = "singleton";
#endif

			if (_instance != null && dontDestroy == true)
			{
				if (Application.isPlaying == true)
				{
					DontDestroyOnLoad(_instance.gameObject);
				}
			}
		}
	}

	void Awake()
	{
		Initialize();
	}

	//Initializes the game for each level.
	protected virtual void Initialize()
	{
		
	}
}