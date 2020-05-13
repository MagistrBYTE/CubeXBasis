//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема анимации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXTweenSpriteRender.cs
*		Компонент для хранения и управления анимацией спрайтов для рендера спрайтов.
*		Реализация компонента для хранения (используется хранилища анимационных спрайтов) и управления анимацией 
*	спрайтов для стандартного компонента рендера спрайтов.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
//=====================================================================================================================
namespace CubeX
{
	namespace Common
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup UnityCommonTween
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Компонент для хранения и управления анимацией спрайтов для рендера спрайтов
		/// </summary>
		/// <remarks>
		/// Реализация компонента для хранения (используется хранилища анимационных спрайтов) и управления анимацией 
		/// спрайтов для стандартного компонента рендера спрайтов
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("CubeX/Common/Tween/Animation SpriteRender")]
		public class CubeXTweenSpriteRender : MonoBehaviour
		{
			#region ======================================= ДАННЫЕ ====================================================
			[SerializeField]
			[CubeXDisplayName(nameof(Tween))]
			[CubeXSerializeMember(nameof(Tween))]
			internal CTweenSprite mTweenSprite;

			[SerializeField]
			[CubeXDisplayName(nameof(IsUpdate))]
			[CubeXSerializeMember(nameof(IsUpdate))]
			internal Boolean mIsUpdate;

			[NonSerialized]
			internal SpriteRenderer mSpriteRender;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Статус обновления анимации в методе <see cref="Update"/>
			/// </summary>
			public Boolean IsUpdate
			{
				get { return mIsUpdate; }
				set { mIsUpdate = value; }
			}

			//
			// ПАРАМЕТРЫ АНИМАЦИИ
			//
			/// <summary>
			/// Аниматор спрайтов
			/// </summary>
			public CTweenSprite Tween
			{
				get { return (mTweenSprite); }
			}

			/// <summary>
			/// Имя группы анимационных цепочек
			/// </summary>
			public String GroupName
			{
				get
				{
					return (mTweenSprite.GroupName);
				}
			}

			/// <summary>
			/// Индекс анимационной цепочки в группе
			/// </summary>
			public Int32 GroupSpriteIndex
			{
				get { return (mTweenSprite.GroupSpriteIndex); }
				set
				{
					mTweenSprite.GroupSpriteIndex = value;
				}
			}

			/// <summary>
			/// Имя анимационной цепочки из группы
			/// </summary>
			public String GroupSpriteName
			{
				get
				{
					return (mTweenSprite.GroupSpriteName);
				}
			}

			/// <summary>
			/// Индекс анимационной цепочки из хранилища анимаций спрайтов
			/// </summary>
			public Int32 StorageSpriteIndex
			{
				get { return mTweenSprite.StorageSpriteIndex; }
				set { mTweenSprite.StorageSpriteIndex = value; }
			}

			/// <summary>
			/// Режим проигрывания анимации
			/// </summary>
			public TTweenWrapMode WrapMode
			{
				get { return mTweenSprite.WrapMode; }
				set { mTweenSprite.WrapMode = value; }
			}

			/// <summary>
			/// Начальный кадр
			/// </summary>
			public Int32 StartFrame
			{
				get { return mTweenSprite.StartFrame; }
				set { mTweenSprite.StartFrame = value; }
			}

			/// <summary>
			/// Целевое кадр
			/// </summary>
			public Int32 TargetFrame
			{
				get { return mTweenSprite.TargetFrame; }
				set { mTweenSprite.TargetFrame = value; }
			}

			/// <summary>
			/// Время в течение которого должно измениться значение переменной от начального к конечной
			/// </summary>
			public Single Duration
			{
				get { return mTweenSprite.Duration; }
				set { mTweenSprite.Duration = value; }
			}

			/// <summary>
			/// Нормализованное время прохождение анимации в пределах от 0 до 1
			/// </summary>
			public Single NormalizeTime
			{
				get { return mTweenSprite.NormalizeTime; }
			}

			/// <summary>
			/// Номер текущего кадра
			/// </summary>
			public Int32 FrameIndex
			{
				get { return mTweenSprite.FrameIndex; }
			}

			/// <summary>
			/// Текущий проигрываемый спрайт
			/// </summary>
			public Sprite FrameSprite
			{
				get
				{
					if (mTweenSprite.StorageSpriteIndex == -1)
					{
						return null;
					}
					else
					{
						return CubeXTweenDispatcher.SpriteStorage.GetFrameSprite(mTweenSprite.GroupIndex, mTweenSprite.GroupSpriteIndex, mTweenSprite.FrameIndex);
					}
				}
			}

			/// <summary>
			/// Статус анимации
			/// </summary>
			public Boolean IsPlay
			{
				get { return mTweenSprite.IsPlay; }
			}

			/// <summary>
			/// Установка/снятие паузы анимации
			/// </summary>
			public Boolean IsPause
			{
				get { return mTweenSprite.IsPause; }
				set { mTweenSprite.IsPause = value; }
			}

			/// <summary>
			/// Статус проигрывания анимации вперед
			/// </summary>
			public Boolean IsForward
			{
				get { return mTweenSprite.IsForward; }
			}

			/// <summary>
			/// Событие для нотификации о начале анимации. Аргумент - название анимации
			/// </summary>
			public Action<String> OnAnimationStart
			{
				get { return mTweenSprite.OnAnimationStart; }
				set { mTweenSprite.OnAnimationStart = value; }
			}

			/// <summary>
			/// Событие для нотификации об окончании анимации. Аргумент - название анимации
			/// </summary>
			public Action<String> OnAnimationCompleted
			{
				get { return mTweenSprite.OnAnimationCompleted; }
				set { mTweenSprite.OnAnimationCompleted = value; }
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация скрипта при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Reset()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдоконструктор скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Awake()
			{
				
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Включение скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnEnable()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Start()
			{
				mSpriteRender = this.GetComponent<SpriteRenderer>();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление скрипта каждый кадр
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Update()
			{
				if(mIsUpdate) UpdateAnimation();
			}
			#endregion

			#region ======================================= ИНДЕКСАТОР ================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Индексация анимационных цепочек
			/// </summary>
			/// <param name="index">Индекс</param>
			/// <returns>Анимационная цепочка</returns>
			//---------------------------------------------------------------------------------------------------------
			public CTweenSpriteData this[Int32 index]
			{
				get
				{
					return CubeXTweenDispatcher.SpriteStorage[mTweenSprite.GroupIndex, index];
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных и параметров компонента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Init()
			{
				if (mTweenSprite == null)
				{
					mTweenSprite = new CTweenSprite();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт анимации
			/// </summary>
			/// <param name="animation_name">Имя проигрываемой анимации</param>
			//---------------------------------------------------------------------------------------------------------
			public void StartAnimation(String animation_name)
			{
				mTweenSprite.StartAnimation(animation_name);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт анимации
			/// </summary>
			/// <param name="index">Индекс проигрываемой анимации</param>
			//---------------------------------------------------------------------------------------------------------
			public void StartAnimation(Int32 index)
			{
				mTweenSprite.StartAnimation(index);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void StartAnimation()
			{
				mTweenSprite.StartAnimation();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdateAnimation()
			{
				mTweenSprite.UpdateAnimation();
				mSpriteRender.sprite = CubeXTweenDispatcher.SpriteStorage.GetFrameSprite(mTweenSprite.GroupIndex,
					mTweenSprite.GroupSpriteIndex, mTweenSprite.FrameIndex);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Остановка анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void StopAnimation()
			{
				mTweenSprite.StopAnimation();
				mSpriteRender.sprite = CubeXTweenDispatcher.SpriteStorage.GetFrameSprite(mTweenSprite.GroupIndex,
					mTweenSprite.GroupSpriteIndex, mTweenSprite.FrameIndex);
			}

#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			[ContextMenu("Play")]
			[CubeXMethodCall("Play", 0, TMethodCallMode.OnlyPlay)]
			public void PlayInEditor()
			{
				StartAnimation();
			}
#endif
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================