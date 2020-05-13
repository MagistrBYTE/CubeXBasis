//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема анимации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXTweenColor.cs
*		Компонент для хранения и управления анимацией цветового значения.
*		Реализация компонента для хранения (используется хранилища анимационных кривых) и управления анимацией
*	цветового значения.
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
		/// Компонент для хранения и управления анимацией цветового значения
		/// </summary>
		/// <remarks>
		/// Реализация компонента для хранения (используется хранилища анимационных кривых) и управления анимацией
		/// цветового значения
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("CubeX/Common/Tween/Animation Color")]
		public class CubeXTweenColor : MonoBehaviour
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			[CubeXDisplayName(nameof(Tween))]
			[CubeXSerializeMember(nameof(Tween))]
			internal CTweenColor mTweenColor;
			[SerializeField]
			[CubeXDisplayName(nameof(UseMaterial))]
			[CubeXSerializeMember(nameof(UseMaterial))]
			internal Boolean mUseMaterial;
			[NonSerialized]
			internal Material mTargetMaterial;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Аниматор цветового значения
			/// </summary>
			public CTweenColor Tween
			{
				get { return (mTweenColor); }
			}

			/// <summary>
			/// Текущее значение переменной
			/// </summary>
			public Color Value
			{
				get { return mTweenColor.mValue; }
			}

			/// <summary>
			/// Начальное значение переменной
			/// </summary>
			public Color StartValue
			{
				get { return mTweenColor.mStartValue; }
				set { mTweenColor.mStartValue = value; }
			}

			/// <summary>
			/// Целевое значение переменной
			/// </summary>
			public Color TargetValue
			{
				get { return mTweenColor.mTargetValue; }
				set { mTweenColor.mTargetValue = value; }
			}

			/// <summary>
			/// Режим проигрывания анимации
			/// </summary>
			public TTweenWrapMode WrapMode
			{
				get { return mTweenColor.mWrapMode; }
				set { mTweenColor.mWrapMode = value; }
			}

			/// <summary>
			/// Время в течение которого должно измениться значение переменной от начального к конечной
			/// </summary>
			public Single TimeAnimation
			{
				get { return mTweenColor.mDuration; }
				set { mTweenColor.mDuration = value; }
			}

			/// <summary>
			/// Нормализованное время прохождение анимации в пределах от 0 до 1
			/// </summary>
			public Single NormalizeTime
			{
				get { return mTweenColor.mNormalizeTime; }
			}

			/// <summary>
			/// Статус игнорирования масштабирования времени
			/// </summary>
			public Boolean IgnoreTimeScale
			{
				get { return mTweenColor.IgnoreTimeScale; }
				set { mTweenColor.IgnoreTimeScale = value; }
			}

			/// <summary>
			/// Количество циклов проигрывания циклических анимаций. 0 - бесконечно
			/// </summary>
			public Int32 CountLoop
			{
				get { return mTweenColor.mCountLoop; }
				set { mTweenColor.mCountLoop = value; }
			}

			/// <summary>
			/// Текущие количество проигрывания циклических анимаций
			/// </summary>
			public Int32 CurrentCountLoop
			{
				get { return mTweenColor.mCurrentCountLoop; }
				set { mTweenColor.mCurrentCountLoop = value; }
			}

			/// <summary>
			/// Статус применения изменяемого значения к материалу
			/// </summary>
			public Boolean UseMaterial
			{
				get { return mUseMaterial; }
				set { mUseMaterial = value; }
			}

			/// <summary>
			/// Статус анимации
			/// </summary>
			public Boolean IsPlay
			{
				get { return mTweenColor.mStart; }
			}
			
			/// <summary>
			/// Установка/снятие паузы анимации
			/// </summary>
			public Boolean IsPause
			{
				get { return mTweenColor.mIsPause; }
				set { mTweenColor.mIsPause = value; }
			}

			/// <summary>
			/// Статус проигрывания анимации вперед
			/// </summary>
			public Boolean IsForward
			{
				get { return mTweenColor.mIsForward; }
			}

			/// <summary>
			/// Событие для нотификации о начале анимации. Аргумент - название анимации
			/// </summary>
			public Action<String> OnAnimationStart
			{
				get { return mTweenColor.mOnAnimationStart; }
				set { mTweenColor.mOnAnimationStart = value; }
			}

			/// <summary>
			/// Событие для нотификации об окончании анимации. Аргумент - название анимации
			/// </summary>
			public Action<String> OnAnimationCompleted
			{
				get { return mTweenColor.mOnAnimationCompleted; }
				set { mTweenColor.mOnAnimationCompleted = value; }
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
				Init();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдоконструктор скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Awake()
			{
				Init();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Включение скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnEnable()
			{
				Init();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Start()
			{
				mTargetMaterial = this.GetComponent<Renderer>().sharedMaterial;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление скрипта каждый кадр
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Update()
			{
				if (mTweenColor.IsPlay)
				{
					mTweenColor.UpdateAnimation();
				}

				if (mUseMaterial)
				{
					this.UpdateMaterial();
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
				if (mTweenColor == null)
				{
					mTweenColor = new CTweenColor();
				}
			}
			
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void StartAnimation()
			{
				mTweenColor.StartAnimation();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Остановка анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void StopAnimation()
			{
				mTweenColor.StopAnimation();
			}
			
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление материала
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdateMaterial()
			{
				mTargetMaterial.color = mTweenColor.Value;
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