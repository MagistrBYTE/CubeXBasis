//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема анимации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXTweenCommonList.cs
*		Список однотипных аниматоров.
*		Списки однотипных аниматоров применять тогда кода нужно управлять параметрами анимации для списка однотипных аниматоров.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		/// Шаблонный список однотипных аниматоров
		/// </summary>
		/// <typeparam name="TTweenObject">Класс для хранения и управления анимации обобщенным типом</typeparam>
		/// <typeparam name="TAnimatableType">Тип анимационного значения</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class ListTweenGenericObject<TTweenObject, TAnimatableType> where TTweenObject : TweenGenericObject<TAnimatableType>
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal List<TTweenObject> mTweens;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Название анимации
			/// </summary>
			public String Name
			{
				get { return mTweens[0].mName; }
				set
				{
					for (Int32 i = 0; i < mTweens.Count; i++)
					{
						if (mTweens[i] != null)
						{
							mTweens[i].mName = value;
						}
					}
				}
			}

			/// <summary>
			/// Текущее значение переменной
			/// </summary>
			public TAnimatableType Value
			{
				get { return mTweens[0].mValue; }
				set
				{
					for (Int32 i = 0; i < mTweens.Count; i++)
					{
						if (mTweens[i] != null)
						{
							mTweens[i].mValue = value;
						}
					}
				}
			}

			/// <summary>
			/// Начальное значение переменной
			/// </summary>
			public TAnimatableType StartValue
			{
				get { return mTweens[0].mStartValue; }
				set
				{
					for (Int32 i = 0; i < mTweens.Count; i++)
					{
						if (mTweens[i] != null)
						{
							mTweens[i].mStartValue = value;
						}
					}
				}
			}

			/// <summary>
			/// Целевое значение переменной
			/// </summary>
			public TAnimatableType TargetValue
			{
				get { return mTweens[0].mTargetValue; }
				set
				{
					for (Int32 i = 0; i < mTweens.Count; i++)
					{
						if (mTweens[i] != null)
						{
							mTweens[i].mTargetValue = value;
						}
					}
				}
			}

			/// <summary>
			/// Режим проигрывания анимации
			/// </summary>
			public TTweenWrapMode WrapMode
			{
				get { return mTweens[0].mWrapMode; }
				set
				{
					for (Int32 i = 0; i < mTweens.Count; i++)
					{
						if (mTweens[i] != null)
						{
							mTweens[i].mWrapMode = value;
						}
					}
				}
			}

			/// <summary>
			/// Время(по умолчанию) в течение которого должно измениться значение переменной от начального к конечной
			/// </summary>
			public Single Duration
			{
				get { return mTweens[0].mDuration; }
				set
				{
					for (Int32 i = 0; i < mTweens.Count; i++)
					{
						if (mTweens[i] != null)
						{
							mTweens[i].mDuration = value;
						}
					}
				}
			}

			/// <summary>
			/// Статус игнорирования масштабирования времени
			/// </summary>
			public Boolean IgnoreTimeScale
			{
				get { return mTweens[0].mIgnoreTimeScale; }
				set
				{
					for (Int32 i = 0; i < mTweens.Count; i++)
					{
						if (mTweens[i] != null)
						{
							mTweens[i].mIgnoreTimeScale = value;
						}
					}
				}
			}

			/// <summary>
			/// Список аниматоров
			/// </summary>
			public List<TTweenObject> Tweens
			{
				get { return (mTweens); }
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс для хранения и управления списком аниматоров целых значений
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CListTweenInteger : ListTweenGenericObject<CTweenInteger, Int32>
		{

		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================