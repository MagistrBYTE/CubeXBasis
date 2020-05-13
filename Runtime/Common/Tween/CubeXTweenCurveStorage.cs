//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема анимации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXTweenCurveStorage.cs
*		Хранилище (ресурс) анимационных кривых.
*		Ресурс представляет методы добавления, удаления анимационных кривых, доступ к кривой по индексу, вычисление 
*	значения кривой по ключу времени, а также методы по сохранению данных всех анимационных кривых в файл и последующей загрузки.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
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
		/// Класс для хранения анимационной кривой
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[CubeXSerializeAliasType("Curve")]
		public class CTweenCurveData : ICubeXSerializeImplementationXML
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			[CubeXColumn(48)]
			[CubeXDisplayName(nameof(Name))]
			internal String mName;

			[SerializeField]
			[CubeXColumn(48)]
			[CubeXDisplayName(nameof(Curve))]
			internal AnimationCurve mCurve;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			[HideInInspector]
			internal Boolean mExpanded;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя анимационной кривой
			/// </summary>
			public String Name
			{
				get { return mName; }
				set { mName = value; }
			}

			/// <summary>
			/// Анимационная кривая содержащая ключи. Время и значения должны быть в пределах от 0 до 1
			/// </summary>
			public AnimationCurve Curve
			{
				get { return mCurve; }
				set { mCurve = value; }
			}

			/// <summary>
			/// Длина времени (по умолчанию равна 1)
			/// </summary>
			public Single TimeLength
			{
				get { return mCurve[mCurve.length - 1].time; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CTweenCurveData()
			{
				mName = "";
				mCurve = AnimationCurve.Linear(0, 0, 1, 1);
			}
			#endregion

			#region ======================================= МЕТОДЫ СЕРИАЛИЗАЦИИ =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных анимационной кривой в формате элемента XML
			/// </summary>
			/// <param name="writer">Средство записи данных формата XML</param>
			//---------------------------------------------------------------------------------------------------------
			public void WriteToXml(XmlWriter writer)
			{
#if UNITY_EDITOR
				writer.WriteStartAttribute("Name");
				writer.WriteValue(mName);
				writer.WriteEndAttribute();

				writer.WriteStartAttribute("PostWrapMode");
				writer.WriteValue(mCurve.postWrapMode.ToString());
				writer.WriteEndAttribute();

				writer.WriteStartAttribute("PreWrapMode");
				writer.WriteValue(mCurve.preWrapMode.ToString());
				writer.WriteEndAttribute();

				writer.WriteStartAttribute("CountKeys");
				writer.WriteValue(mCurve.keys.Length.ToString());
				writer.WriteEndAttribute();

				for (Int32 i = 0; i < mCurve.keys.Length; i++)
				{
					writer.WriteStartElement("Key");

					writer.WriteStartAttribute("InTangent");
					writer.WriteValue(mCurve.keys[i].inTangent);
					writer.WriteEndAttribute();

					writer.WriteStartAttribute("OutTangent");
					writer.WriteValue(mCurve.keys[i].outTangent);
					writer.WriteEndAttribute();

					writer.WriteStartAttribute("Time");
					writer.WriteValue(mCurve.keys[i].time);
					writer.WriteEndAttribute();

					writer.WriteStartAttribute("Value");
					writer.WriteValue(mCurve.keys[i].value);
					writer.WriteEndAttribute();

					writer.WriteEndElement();
				}
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных анимационной кривой в формате элемента XML
			/// </summary>
			/// <param name="reader">Средство чтении данных формата XML</param>
			//---------------------------------------------------------------------------------------------------------
			public void ReadFromXml(XmlReader reader)
			{
				this.Name = reader.ReadStringFromAttribute("Name", "NoName");
				this.mCurve.postWrapMode = reader.ReadEnumFromAttribute<WrapMode>("PostWrapMode");
				this.mCurve.preWrapMode = reader.ReadEnumFromAttribute<WrapMode>("PreWrapMode");
				Int32 count_key = reader.ReadIntegerFromAttribute("CountKeys", 1);

				List<Keyframe> keys = new List<Keyframe>();
				Int32 index_key = 0;
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						if (reader.Name == "Key")
						{
							Keyframe key = new Keyframe();
							key.inTangent = reader.ReadSingleFromAttribute("InTangent");
							key.outTangent = reader.ReadSingleFromAttribute("OutTangent");
							key.time = reader.ReadSingleFromAttribute("Time");
							key.value = reader.ReadSingleFromAttribute("Value");
							keys.Add(key);
							index_key++;

							if (index_key == count_key) break;
						}
						else
						{
							break;
						}
					}
				}

				mCurve.keys = keys.ToArray();
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Хранилище (ресурс) анимационных кривых
		/// </summary>
		/// <remarks>
		/// Ресурс представляет методы добавления, удаления анимационных кривых, доступ к кривой по индексу, вычисление 
		/// значения кривой по ключу времени, а также методы по сохранению данных всех анимационных кривых в файл и 
		/// последующей загрузки
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[CubeXDescriptionType("Хранилище(ресурс) анимационных кривых")]
		public class CubeXTweenCurveStorage : ScriptableObject, ICubeXResourceable
		{
			#region ======================================= ДАННЫЕ ====================================================
			[CubeXHeaderGroupBox("Curve Data")]
			[SerializeField]
			[CubeXDisplayName("Curves")]
			[CubeXReorderable]
			[CubeXInLine]
			internal List<CTweenCurveData> mCurves;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Список анимационных кривых
			/// </summary>
			public List<CTweenCurveData> Curves
			{
				get { return mCurves; }
				set { mCurves = value; }
			}

			/// <summary>
			/// Количество анимационных кривых
			/// </summary>
			public Int32 Count
			{
				get { return mCurves.Count; }
			}

			/// <summary>
			/// Список имен анимационных кривых
			/// </summary>
			public String[] ListNames
			{
				get
				{
					String[] names = new String[mCurves.Count];

					for (Int32 i = 0; i < mCurves.Count; i++)
					{
						names[i] = mCurves[i].Name;
					}

					return names;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXResourceable =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичное создание хранилища анимационных кривых
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Create()
			{
				if (mCurves == null)
				{
					mCurves = new List<CTweenCurveData>();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная безопасная инициализация несериализуемых данных
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Init()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Принудительный сброс записанных данных на диск
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Flush()
			{
#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(this);
#endif
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление новой анимационной кривой для хранения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void AddCurve()
			{
				mCurves.Add(new CTweenCurveData());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаления анимационной кривой
			/// </summary>
			/// <param name="index">Индекс удаляемой кривой</param>
			//---------------------------------------------------------------------------------------------------------
			public void RemoveCurve(Int32 index)
			{
				mCurves.RemoveAt(index);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление значения на кривой
			/// </summary>
			/// <param name="index">Индекс кривой</param>
			/// <param name="time">Время в диапазоне от 0 до 1</param>
			/// <returns>Соответствующие значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single Evaluate(Int32 index, Single time)
			{
				return mCurves[index].mCurve.Evaluate(time);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление значения на кривой
			/// </summary>
			/// <param name="curve_name">Имя анимационной кривой</param>
			/// <param name="time">Время в диапазоне от 0 до 1</param>
			/// <returns>Соответствующие значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single Evaluate(String curve_name, Single time)
			{
				List<CTweenCurveData> curve_storages = Curves;

				for (Int32 i = 0; i < curve_storages.Count; i++)
				{
					if (curve_storages[i].Name == curve_name)
					{
						return curve_storages[i].mCurve.Evaluate(time);
					}
				}

				return 0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение индекса кривой по имени
			/// </summary>
			/// <param name="curve_name">Имя анимационной кривой</param>
			/// <returns>Индекс анимационной кривой</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetIndexCurve(String curve_name)
			{
				Int32 result = 0;
				List<CTweenCurveData> curve_storages = Curves;

				for (Int32 i = 0; i < curve_storages.Count; i++)
				{
					if (curve_storages[i].Name == curve_name)
					{
						result = i;
						break;
					}
				}

				return result;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение времени анимационной кривой
			/// </summary>
			/// <param name="index">Индекс анимационной кривой</param>
			/// <returns>Время</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetTimeCurve(Int32 index)
			{
				return Curves[index].Curve[Curves[index].Curve.length - 1].time;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение времени анимационной кривой
			/// </summary>
			/// <param name="curve_name">Имя анимационной кривой</param>
			/// <returns>Время</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetTimeCurve(String curve_name)
			{
				List<CTweenCurveData> curve_storages = Curves;

				for (Int32 i = 0; i < curve_storages.Count; i++)
				{
					if (curve_storages[i].Name == curve_name)
					{
						return curve_storages[i].Curve[curve_storages[i].Curve.length - 1].time;
					}
				}

				return 1;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранение списка анимационных кривых в отдельный файл
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			[CubeXHeaderGroupBox("Curve Manager")]
			[CubeXMethodArgFile(DisplayName = "Save to file XML", Extension = "XML")]
			public void Save(String file_name)
			{
				if (file_name.IsExists())
				{
					XSerializationDispatcher.SaveToXml(file_name, mCurves);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка новых анимационных кривых из текстового ресурса
			/// </summary>
			/// <param name="file_asset">Текстовый ресурс - данные в формате XML</param>
			//---------------------------------------------------------------------------------------------------------
			[CubeXMethodCall("Load new from file XML")]
			public void LoadNew(TextAsset file_asset)
			{
				if (file_asset != null)
				{
					mCurves = XSerializationDispatcher.LoadListFromStringXml<CTweenCurveData>(file_asset.text);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка(добавление) анимационных кривых из текстового ресурса
			/// </summary>
			/// <param name="file_asset">Текстовый ресурс - данные в формате XML</param>
			//---------------------------------------------------------------------------------------------------------
			[CubeXMethodCall("Load add from file XML")]
			public void LoadAdd(TextAsset file_asset)
			{
				if (file_asset != null)
				{
					mCurves.AddRange(XSerializationDispatcher.LoadListFromStringXml<CTweenCurveData>(file_asset.text));
				}
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================