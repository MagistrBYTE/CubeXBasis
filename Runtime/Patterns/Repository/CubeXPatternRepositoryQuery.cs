//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Репозиторий данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternRepositoryQuery.cs
*		Определение основных типов и структур данных для формирования универсальных запросов к репозиторию.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Schema;
using System.Linq;
using System.Text;
//---------------------------------------------------------------------------------------------------------------------
#if USE_WINDOWS
using CubeX.Windows;
#endif
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CorePatternRepository
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Оператор сравнения применяемый в запросе данных
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public enum TComparisonQueryOperator
		{
			/// <summary>
			/// Равно
			/// </summary>
			[Description("=")]
			Equality,

			/// <summary>
			/// Не равно
			/// </summary>
			[Description("!=")]
			Inequality,

			/// <summary>
			/// Меньше
			/// </summary>
			[Description("<")]
			LessThan,

			/// <summary>
			/// Меньше или равно
			/// </summary>
			[Description("<=")]
			LessThanOrEqual,

			/// <summary>
			/// Больше
			/// </summary>
			[Description(">")]
			GreaterThan,

			/// <summary>
			/// Больше или равно
			/// </summary>
			[Description(">=")]
			GreaterThanOrEqual,

			/// <summary>
			/// Между
			/// </summary>
			[Description("><")]
			Between
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширений для перечисления <see cref="TComparisonQueryOperator"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionComparisonQueryOperator
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение строкового представления оператора сравнения
			/// </summary>
			/// <param name="comparison_operator">Оператор сравнения</param>
			/// <returns>Строковое представление</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String GetOperatorOfString(this TComparisonQueryOperator comparison_operator)
			{
				String result = "";
				switch (comparison_operator)
				{
					case TComparisonQueryOperator.Equality:
						result = " = ";
						break;
					case TComparisonQueryOperator.Inequality:
						result = " != ";
						break;
					case TComparisonQueryOperator.LessThan:
						result = " < ";
						break;
					case TComparisonQueryOperator.LessThanOrEqual:
						result = " <= ";
						break;
					case TComparisonQueryOperator.GreaterThan:
						result = " > ";
						break;
					case TComparisonQueryOperator.GreaterThanOrEqual:
						result = " >= ";
						break;
					default:
						break;
				}

				return (result);
			}
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс представляющий запрос
		/// </summary>
		/// <remarks>
		/// <para>
		/// Запрос представляет собой набор определённых условий по которым извлекаются данные или фильтруются для отображения
		/// </para>
		/// <para>
		/// Он может применяться как к списку с объектам конкретных типов так для работы с сырыми данным
		/// </para>
		/// <para>
		/// Запрос при этом представлен в виде стандартного SQL Запрос и предиката
		/// </para>
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public class CQuery : PropertyChangedBase, ICubeXNotify
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			private static readonly PropertyChangedEventArgs PropertyArgsSQLQuery = new PropertyChangedEventArgs(nameof(SQLQuery));
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			protected internal ListArray<CQueryItem> mItems;
			protected internal String mSQLQuery;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Элементы запроса
			/// </summary>
			public ListArray<CQueryItem> Items
			{
				get
				{
					return (mItems);
				}
			}

			/// <summary>
			/// Стандартный SQL запрос
			/// </summary>
			public String SQLQuery
			{
				get
				{
					ComputeSQLQuery();
					return (mSQLQuery);
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CQuery()
			{
				mItems = new ListArray<CQueryItem>();
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXNotify =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Информирование данного объекта о начале изменения данных указанного объекта
			/// </summary>
			/// <param name="source">Объект данные которого будут меняться</param>
			/// <param name="data_name">Имя данных</param>
			/// <returns>Статус разрешения/согласования изменения данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean OnNotifyUpdating(System.Object source, String data_name)
			{
				return (true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Информирование данного объекта об окончании изменении данных указанного объекта
			/// </summary>
			/// <param name="source">Объект данные которого изменились</param>
			/// <param name="data_name">Имя данных</param>
			//---------------------------------------------------------------------------------------------------------
			public void OnNotifyUpdated(System.Object source, String data_name)
			{
				NotifyPropertyChanged(PropertyArgsSQLQuery);
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление SQL запроса на основе элементов запроса
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ComputeSQLQuery()
			{
				String sql_query = "";

				for (Int32 i = 0; i < mItems.Count; i++)
				{
					if (mItems[i].ComputeSQLQuery(ref sql_query))
					{
						if (i < mItems.Count - 1)
						{
							sql_query += " AND";
						}
					}
				}

				mSQLQuery = sql_query;
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс представляющий элемент запроса
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CQueryItem : PropertyChangedBase, ICubeXNotCalculation
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			public static readonly PropertyChangedEventArgs PropertyArgsSQLQueryItem = new PropertyChangedEventArgs(nameof(SQLQueryItem));
			public static readonly PropertyChangedEventArgs PropertyArgsNotCalculation = new PropertyChangedEventArgs(nameof(NotCalculation));
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			protected internal String mPropertyName;
			protected internal CQuery mQueryOwned;
			protected internal Boolean mNotCalculation;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя свойства/столбца
			/// </summary>
			public String PropertyName
			{
				get
				{
					return (mPropertyName);
				}
				set
				{
					mPropertyName = value;
				}
			}

			/// <summary>
			/// Запрос
			/// </summary>
			public CQuery QueryOwned
			{
				get
				{
					return (mQueryOwned);
				}
				set
				{
					mQueryOwned = value;
				}
			}

			/// <summary>
			/// Элемент стандартного SQL запроса
			/// </summary>
			public String SQLQueryItem
			{
				get
				{
					return (ToString());
				}
			}

			/// <summary>
			/// Элемент запроса не участвует в запросе
			/// </summary>
			public Boolean NotCalculation
			{
				get { return (mNotCalculation); }
				set
				{
					mNotCalculation = value;
					NotifyPropertyChanged(PropertyArgsNotCalculation);
					if (QueryOwned != null) QueryOwned.OnNotifyUpdated(this, nameof(NotCalculation));
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CQueryItem()
			{
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Формирование SQL запроса
			/// </summary>
			/// <param name="sql_query">SQL запрос</param>
			/// <returns>Статус формирования элемента запроса</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Boolean ComputeSQLQuery(ref String sql_query)
			{
				return (false);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс представляющий элемент запроса для числовых значений
		/// </summary>
		/// <remarks>
		/// Поддерживаются стандартные операторы сравнения и стандартная операция BETWEEN
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public class CQueryItemNumber : CQueryItem
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			private static readonly PropertyChangedEventArgs PropertyArgsComparisonOperator = new PropertyChangedEventArgs(nameof(ComparisonOperator));
			private static readonly PropertyChangedEventArgs PropertyArgsComparisonValueLeft = new PropertyChangedEventArgs(nameof(ComparisonValueLeft));
			private static readonly PropertyChangedEventArgs PropertyArgsComparisonValueRight = new PropertyChangedEventArgs(nameof(ComparisonValueRight));
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			protected TComparisonQueryOperator mComparisonOperator;
			protected Double mComparisonValueLeft;
			protected Double mComparisonValueRight;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Оператор сравнения
			/// </summary>
			public TComparisonQueryOperator ComparisonOperator
			{
				get
				{
					return (mComparisonOperator);
				}
				set
				{
					if (mComparisonOperator != value)
					{
						mComparisonOperator = value;
						NotifyPropertyChanged(PropertyArgsComparisonOperator);
						NotifyPropertyChanged(PropertyArgsSQLQueryItem);
						if (QueryOwned != null) QueryOwned.OnNotifyUpdated(this, nameof(ComparisonOperator));
					}
				}
			}

			/// <summary>
			/// Значение для сравнения слева
			/// </summary>
			public Double ComparisonValueLeft
			{
				get
				{
					return (mComparisonValueLeft);
				}
				set
				{
					if (Math.Abs(mComparisonValueLeft - value) > 0.000001)
					{
						mComparisonValueLeft = value;
						NotifyPropertyChanged(PropertyArgsComparisonValueLeft);
						NotifyPropertyChanged(PropertyArgsSQLQueryItem);
						if (QueryOwned != null) QueryOwned.OnNotifyUpdated(this, nameof(ComparisonValueLeft));
					}
				}
			}

			/// <summary>
			/// Значение для сравнения справа
			/// </summary>
			public Double ComparisonValueRight
			{
				get
				{
					return (mComparisonValueRight);
				}
				set
				{
					if (Math.Abs(mComparisonValueRight - value) > 0.000001)
					{
						mComparisonValueRight = value;
						NotifyPropertyChanged(PropertyArgsComparisonValueRight);
						NotifyPropertyChanged(PropertyArgsSQLQueryItem);
						if (QueryOwned != null) QueryOwned.OnNotifyUpdated(this, nameof(mComparisonValueRight));
					}
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CQueryItemNumber()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="comparison_operator">Оператор сравнения</param>
			/// <param name="comparison_value">Значение для сравнения</param>
			//---------------------------------------------------------------------------------------------------------
			public CQueryItemNumber(TComparisonQueryOperator comparison_operator, Double comparison_value)
			{
				mComparisonOperator = comparison_operator;
				mComparisonValueLeft = comparison_value;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="comparison_value_left">Значение для сравнения слева</param>
			/// <param name="comparison_value_right">Значение для сравнения справа</param>
			//---------------------------------------------------------------------------------------------------------
			public CQueryItemNumber(Double comparison_value_left, Double comparison_value_right)
			{
				mComparisonOperator = TComparisonQueryOperator.Between;
				mComparisonValueLeft = comparison_value_left;
				mComparisonValueRight = comparison_value_right;
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Наименование объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				String name = "";
				ComputeSQLQuery(ref name);
				return (name);
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Формирование SQL запроса
			/// </summary>
			/// <param name="sql_query">SQL запрос</param>
			/// <returns>Статус формирования элемента запроса</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Boolean ComputeSQLQuery(ref String sql_query)
			{
				if(mNotCalculation == false && Double.IsInfinity(mComparisonValueLeft) == false &&
					Double.IsNaN(mComparisonValueLeft) == false)
				{
					if(mComparisonOperator == TComparisonQueryOperator.Between)
					{
						if(mComparisonValueRight > mComparisonValueLeft)
						{
							sql_query += " " + mPropertyName + " BETWEEN " + mComparisonValueLeft.ToString()
								+ " AND " + mComparisonValueRight.ToString();
							return (true);
						}
					}
					else
					{
						sql_query += " " + mPropertyName + mComparisonOperator.GetOperatorOfString() + mComparisonValueLeft.ToString();
						return (true);
					}
				}

				return (false);
			}
			#endregion

			#region ======================================= МЕТОДЫ ПРИВЯЗКИ ===========================================
#if USE_WINDOWS
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Привязка выпадающего списка к оператору сравнения
			/// </summary>
			/// <param name="combo_box">Выпадающий список</param>
			//---------------------------------------------------------------------------------------------------------
			public void BindingComboBoxToComparisonOperator(in System.Windows.Controls.ComboBox combo_box)
			{
				if (combo_box != null)
				{
					System.Windows.Data.Binding binding = new System.Windows.Data.Binding();
					binding.Source = this;
					binding.Path = new System.Windows.PropertyPath(nameof(ComparisonOperator));
					binding.Converter = EnumToStringConverter.Instance;

					combo_box.ItemsSource = XEnum.GetDescriptions(typeof(TComparisonQueryOperator));
					System.Windows.Data.BindingOperations.SetBinding(combo_box,
						System.Windows.Controls.ComboBox.SelectedValueProperty, binding);
				}
			}
#endif
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс представляющий элемент запроса для строковых значений
		/// </summary>
		/// <remarks>
		/// Поддерживаются опции поиска представление типом <see cref="TStringSearchOption"/> и стандартная операция LIKE
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public class CQueryItemString : CQueryItem
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			private static readonly PropertyChangedEventArgs PropertyArgsSearchOption = new PropertyChangedEventArgs(nameof(SearchOption));
			private static readonly PropertyChangedEventArgs PropertyArgsSearchValue = new PropertyChangedEventArgs(nameof(SearchValue));
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			protected TStringSearchOption mSearchOption;
			protected String mSearchValue;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Опции поиска в строке
			/// </summary>
			public TStringSearchOption SearchOption
			{
				get
				{
					return (mSearchOption);
				}
				set
				{
					if (mSearchOption != value)
					{
						mSearchOption = value;
						NotifyPropertyChanged(PropertyArgsSearchOption);
						NotifyPropertyChanged(PropertyArgsSQLQueryItem);
						if (QueryOwned != null) QueryOwned.OnNotifyUpdated(this, nameof(SearchOption));
					}
				}
			}

			/// <summary>
			/// Значение для сравнения
			/// </summary>
			public String SearchValue
			{
				get
				{
					return (mSearchValue);
				}
				set
				{
					if (mSearchValue != value)
					{
						mSearchValue = value;
						NotifyPropertyChanged(PropertyArgsSearchValue);
						NotifyPropertyChanged(PropertyArgsSQLQueryItem);
						if (QueryOwned != null) QueryOwned.OnNotifyUpdated(this, nameof(SearchValue));
					}
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CQueryItemString()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="search_option">Опции поиска в строке</param>
			/// <param name="search_value">Значение для сравнения</param>
			//---------------------------------------------------------------------------------------------------------
			public CQueryItemString(TStringSearchOption search_option, String search_value)
			{
				mSearchOption = search_option;
				mSearchValue = search_value;
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Наименование объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				String name = "";
				ComputeSQLQuery(ref name);
				return (name);
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Формирование SQL запроса
			/// </summary>
			/// <param name="sql_query">SQL запрос</param>
			/// <returns>Статус формирования элемента запроса</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Boolean ComputeSQLQuery(ref String sql_query)
			{
				if ((mNotCalculation == false) && (String.IsNullOrEmpty(mSearchValue) == false))
				{
					switch (mSearchOption)
					{
						case TStringSearchOption.Start:
							{
								sql_query += " " + mPropertyName + " LIKE '" + mSearchValue + "%'";
							}
							break;
						case TStringSearchOption.End:
							{
								sql_query += " " + mPropertyName + " LIKE '%" + mSearchValue + "'";
							}
							break;
						case TStringSearchOption.Contains:
							{
								sql_query += " " + mPropertyName + " LIKE '%" + mSearchValue + "%'";
							}
							break;
						case TStringSearchOption.Equal:
							break;
						default:
							break;
					}

					return (true);
				}

				return (false);
			}
			#endregion

			#region ======================================= МЕТОДЫ ПРИВЯЗКИ ===========================================
#if USE_WINDOWS
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Привязка текстового поля к строке поиска
			/// </summary>
			/// <param name="text_box">Текстовое поле</param>
			//---------------------------------------------------------------------------------------------------------
			public void BindingTextBoxToSearchValue(in System.Windows.Controls.TextBox text_box)
			{
				if(text_box != null)
				{
					System.Windows.Data.Binding binding = new System.Windows.Data.Binding();
					binding.Source = this;
					binding.Path = new System.Windows.PropertyPath(nameof(SearchValue));

					System.Windows.Data.BindingOperations.SetBinding(text_box,
						System.Windows.Controls.TextBox.TextProperty, binding);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Привязка выпадающего списка к опциям поиска
			/// </summary>
			/// <param name="combo_box">Выпадающий список</param>
			//---------------------------------------------------------------------------------------------------------
			public void BindingComboBoxToSearchOption(in System.Windows.Controls.ComboBox combo_box)
			{
				if (combo_box != null)
				{
					System.Windows.Data.Binding binding = new System.Windows.Data.Binding();
					binding.Source = this;
					binding.Path = new System.Windows.PropertyPath(nameof(SearchOption));
					binding.Converter = EnumToStringConverter.Instance;

					combo_box.ItemsSource = XEnum.GetDescriptions(typeof(TStringSearchOption));
					System.Windows.Data.BindingOperations.SetBinding(combo_box,
						System.Windows.Controls.ComboBox.SelectedValueProperty, binding);
				}
			}
#endif
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс представляющий элемент запроса для значений даты-времени
		/// </summary>
		/// <remarks>
		/// Поддерживаются стандартные операторы сравнения и стандартная операция BETWEEN
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public class CQueryItemDateTime : CQueryItem
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			private static readonly PropertyChangedEventArgs PropertyArgsComparisonOperator = new PropertyChangedEventArgs(nameof(ComparisonOperator));
			private static readonly PropertyChangedEventArgs PropertyArgsComparisonValueLeft = new PropertyChangedEventArgs(nameof(ComparisonValueLeft));
			private static readonly PropertyChangedEventArgs PropertyArgsComparisonValueRight = new PropertyChangedEventArgs(nameof(ComparisonValueRight));
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			protected TComparisonQueryOperator mComparisonOperator;
			protected DateTime mComparisonValueLeft;
			protected DateTime mComparisonValueRight;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Оператор сравнения
			/// </summary>
			public TComparisonQueryOperator ComparisonOperator
			{
				get
				{
					return (mComparisonOperator);
				}
				set
				{
					if (mComparisonOperator != value)
					{
						mComparisonOperator = value;
						NotifyPropertyChanged(PropertyArgsComparisonOperator);
						NotifyPropertyChanged(PropertyArgsSQLQueryItem);
						if (QueryOwned != null) QueryOwned.OnNotifyUpdated(this, nameof(ComparisonOperator));
					}
				}
			}

			/// <summary>
			/// Значение для сравнения слева
			/// </summary>
			public DateTime ComparisonValueLeft
			{
				get
				{
					return (mComparisonValueLeft);
				}
				set
				{
					if (ComparisonValueLeft != value)
					{
						mComparisonValueLeft = value;
						NotifyPropertyChanged(PropertyArgsComparisonValueLeft);
						NotifyPropertyChanged(PropertyArgsSQLQueryItem);
						if (QueryOwned != null) QueryOwned.OnNotifyUpdated(this, nameof(ComparisonValueLeft));
					}
				}
			}

			/// <summary>
			/// Значение для сравнения справа
			/// </summary>
			public DateTime ComparisonValueRight
			{
				get
				{
					return (mComparisonValueRight);
				}
				set
				{
					if (mComparisonValueRight != value)
					{
						mComparisonValueRight = value;
						NotifyPropertyChanged(PropertyArgsComparisonValueRight);
						NotifyPropertyChanged(PropertyArgsSQLQueryItem);
						if (QueryOwned != null) QueryOwned.OnNotifyUpdated(this, nameof(mComparisonValueRight));
					}
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CQueryItemDateTime()
			{
				mComparisonValueLeft = DateTime.Now;
				mComparisonValueRight = DateTime.Now;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="comparison_operator">Оператор сравнения</param>
			/// <param name="comparison_value">Значение для сравнения</param>
			//---------------------------------------------------------------------------------------------------------
			public CQueryItemDateTime(TComparisonQueryOperator comparison_operator, DateTime comparison_value)
			{
				mComparisonOperator = comparison_operator;
				mComparisonValueLeft = comparison_value;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="comparison_value_left">Значение для сравнения слева</param>
			/// <param name="comparison_value_right">Значение для сравнения справа</param>
			//---------------------------------------------------------------------------------------------------------
			public CQueryItemDateTime(DateTime comparison_value_left, DateTime comparison_value_right)
			{
				mComparisonOperator = TComparisonQueryOperator.Between;
				mComparisonValueLeft = comparison_value_left;
				mComparisonValueRight = comparison_value_right;
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Наименование объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				String name = "";
				ComputeSQLQuery(ref name);
				return (name);
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Формирование SQL запроса
			/// </summary>
			/// <param name="sql_query">SQL запрос</param>
			/// <returns>Статус формирования элемента запроса</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Boolean ComputeSQLQuery(ref String sql_query)
			{
				if (mNotCalculation == false)
				{
					if (mComparisonOperator == TComparisonQueryOperator.Between)
					{
						if (mComparisonValueRight > mComparisonValueLeft)
						{
							sql_query += " " + mPropertyName + " BETWEEN " + mComparisonValueLeft.ToString()
								+ " AND " + mComparisonValueRight.ToString();
							return (true);
						}
					}
					else
					{
						sql_query += " " + mPropertyName + mComparisonOperator.GetOperatorOfString() + mComparisonValueLeft.ToString();
						return (true);
					}
				}

				return (false);
			}
			#endregion

			#region ======================================= МЕТОДЫ ПРИВЯЗКИ ===========================================
#if USE_WINDOWS
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Привязка выпадающего списка к оператору сравнения
			/// </summary>
			/// <param name="combo_box">Выпадающий список</param>
			//---------------------------------------------------------------------------------------------------------
			public void BindingComboBoxToComparisonOperator(in System.Windows.Controls.ComboBox combo_box)
			{
				if (combo_box != null)
				{
					System.Windows.Data.Binding binding = new System.Windows.Data.Binding();
					binding.Source = this;
					binding.Path = new System.Windows.PropertyPath(nameof(ComparisonOperator));
					binding.Converter = EnumToStringConverter.Instance;

					combo_box.ItemsSource = XEnum.GetDescriptions(typeof(TComparisonQueryOperator));
					System.Windows.Data.BindingOperations.SetBinding(combo_box,
						System.Windows.Controls.ComboBox.SelectedValueProperty, binding);
				}
			}
#endif
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс представляющий элемент запроса для перечисляемых значений
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CQueryItemEnum : CQueryItem
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			private static readonly PropertyChangedEventArgs PropertyArgsSourceItems = new PropertyChangedEventArgs(nameof(SourceItems));
			private static readonly PropertyChangedEventArgs PropertyArgsFiltredItems = new PropertyChangedEventArgs(nameof(FiltredItems));
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			protected List<System.Object> mSourceItems;
			protected List<System.Object> mFiltredItems;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Коллекция которая является источником данных
			/// </summary>
			public List<System.Object> SourceItems
			{
				get
				{
					return (mSourceItems);
				}
				set
				{
					mSourceItems = value;
					NotifyPropertyChanged(PropertyArgsSourceItems);
				}
			}

			/// <summary>
			/// Список элементов которые выбраны
			/// </summary>
			public List<System.Object> FiltredItems
			{
				get
				{
					return (mFiltredItems);
				}
				set
				{
					mFiltredItems = value;
					NotifyPropertyChanged(PropertyArgsFiltredItems);
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CQueryItemEnum()
			{
				mFiltredItems = new List<System.Object>();
				mSourceItems = new List<System.Object>();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="enum_type">Тип перечисления</param>
			//---------------------------------------------------------------------------------------------------------
			public CQueryItemEnum(Type enum_type)
			{
				mFiltredItems = new List<System.Object>();
				mSourceItems = new List<System.Object>();
				mSourceItems.AddRange(XEnum.GetDescriptions(enum_type));
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Наименование объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				return (JoinFiltredItems());
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Формирование SQL запроса
			/// </summary>
			/// <param name="sql_query">SQL запрос</param>
			/// <returns>Статус формирования элемента запроса</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Boolean ComputeSQLQuery(ref String sql_query)
			{
				if (mNotCalculation == false)
				{
					if (mFiltredItems.Count > 0)
					{
						StringBuilder included = new StringBuilder(mFiltredItems.Count * 10);
;						for (Int32 i = 0; i < mFiltredItems.Count; i++)
						{
							if(i != 0)
							{
								included.Append(", ");
							}

							included.Append("'" + mFiltredItems[i].ToString() + "'");
						}

						sql_query += " " + mPropertyName + " IN (" + included.ToString() + ")";
						return (true);
					}
				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Соединение выбранных элементов одну строку
			/// </summary>
			/// <returns>Строка с выбранными элементами</returns>
			//---------------------------------------------------------------------------------------------------------
			public String JoinFiltredItems()
			{
				if (mFiltredItems.Count > 0)
				{
					StringBuilder included = new StringBuilder(mFiltredItems.Count * 10);
					for (Int32 i = 0; i < mFiltredItems.Count; i++)
					{
						if (i != 0)
						{
							included.Append(", ");
						}

						included.Append(mFiltredItems[i].ToString());
					}

					return (included.ToString());
				}

				return (String.Empty);
			}
			#endregion

			#region ======================================= МЕТОДЫ ПРИВЯЗКИ ===========================================
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================