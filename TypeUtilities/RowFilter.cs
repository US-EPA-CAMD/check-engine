using System;
using System.Data;

using ECMPS.Definitions.Enumerations;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.TypeUtilities
{

	#region Public Enumerations

	/// <summary>
	/// Enumeration of filter pair comparison methods.
	/// </summary>
	public enum eFilterConditionCompare
	{
		/// <summary>Return true if field equals the value</summary>
		Equals,
		/// <summary>Return true if field begins with the value</summary>
		BeginsWith,
		/// <summary>Return true if field contains the value</summary>
		Contains,
		/// <summary>Return true if field ends with the value</summary>
		EndsWith,
		/// <summary>Return true if field in the list represented in the value</summary>
		InList,
		/// <summary>Return true if value in the list represented in the field</summary>
		ListHas,
		/// <summary>Return true if field is less than the value</summary>
		LessThan,
		/// <summary>Return true if field is less than or equal to the value</summary>
		LessThanOrEqual,
		/// <summary>Return true if field is greater than or equal to the value</summary>
		GreaterThanOrEqual,
		/// <summary>Return true if field is greater than the value</summary>
		GreaterThan
	}

	/// <summary>
	/// Enumeration of filter pair Relative comparison methods.
	/// </summary>
	public enum eFilterConditionRelativeCompare
	{
		/// <summary>Return true if field equals the value</summary>
		Equals,
		/// <summary>Return true if field is less than the value</summary>
		LessThan,
		/// <summary>Return true if field is less than or equal to the value</summary>
		LessThanOrEqual,
		/// <summary>Return true if field is greater than or equal to the value</summary>
		GreaterThanOrEqual,
		/// <summary>Return true if field is greater than the value</summary>
		GreaterThan
	}

	/// <summary>
	/// Enumeration of filter pair string comparison methods.
	/// </summary>
	public enum eFilterConditionStringCompare
	{
		/// <summary>Return true if field equals the value</summary>
		Equals,
		/// <summary>Return true if field begins with the value</summary>
		BeginsWith,
		/// <summary>Return true if field contains the value</summary>
		Contains,
		/// <summary>Return true if field ends with the value</summary>
		EndsWith,
		/// <summary>Return true if field in the list represented in the value</summary>
		InList,
		/// <summary>Return true if value in the list represented in the field</summary>
		ListHas
	}

	/// <summary>
	/// Enumeration of the data type of the filtered data.
	/// </summary>
	public enum eFilterDataType
	{
		/// <summary>Using string methods for filtering</summary>
		String,
		/// <summary>Using began date methods for filtering</summary>
		DateBegan,
		/// <summary>Using ended date methods for filtering</summary>
		DateEnded,
		/// <summary>Using decimal methods for filtering</summary>
		Decimal,
		/// <summary>Using integer methods for filtering</summary>
		Integer,
		/// <summary>Using long methods for filtering</summary>
		Long
	}

	#endregion


	/// <summary>
	/// Contains filter condition information including the field to compare, the value to compare,
	/// and the type of comparison to perform.
	/// </summary>
	public class cFilterCondition
	{

		#region Constructors

		/// <summary>
		/// Creates a filter condtion class with default settings
		/// </summary>
		public cFilterCondition()
			: base()
		{
		}

		/// <summary>
		/// Creates a string equal filter condition class
		/// </summary>
		public cFilterCondition(string AField, string AValue)
			: base()
		{
			Field = AField;
			DataType = eFilterDataType.String;
			Value = AValue;
			Compare = eFilterConditionCompare.Equals;
			Negate = false;
			SubstrPos = int.MinValue;
			SubstrLen = int.MinValue;
		}

		/// <summary>
		/// Creates a string equal filter condition class with negation flag 
		/// </summary>
		public cFilterCondition(string AField, string AValue, bool ANegate)
			: base()
		{
			Field = AField;
			DataType = eFilterDataType.String;
			Value = AValue;
			Compare = eFilterConditionCompare.Equals;
			Negate = ANegate;
			SubstrPos = int.MinValue;
			SubstrLen = int.MinValue;
		}

		/// <summary>
		/// Creates a string equal filter condition class using a substring of the field
		/// </summary>
		public cFilterCondition(string AField, string AValue, int ASubstrPos, int ASubstrLen)
			: base()
		{
			Field = AField;
			DataType = eFilterDataType.String;
			Value = AValue;
			Compare = eFilterConditionCompare.Equals;
			Negate = false;
			SubstrPos = ASubstrPos;
			SubstrLen = ASubstrLen;
		}

		/// <summary>
		/// Creates a string equal filter condition class with a negation flag and using a substring of the field
		/// </summary>
		public cFilterCondition(string AField, string AValue, bool ANegate, int ASubstrPos, int ASubstrLen)
			: base()
		{
			Field = AField;
			DataType = eFilterDataType.String;
			Value = AValue;
			Compare = eFilterConditionCompare.Equals;
			Negate = ANegate;
			SubstrPos = ASubstrPos;
			SubstrLen = ASubstrLen;
		}

		/// <summary>
		/// Creates a string filter condition class allowing specification of the comparison type
		/// </summary>
		public cFilterCondition(string AField, string AValue, eFilterConditionStringCompare ACompare)
			: base()
		{
			Field = AField;
			DataType = eFilterDataType.String;
			Value = AValue;
			Compare = Convert(ACompare);
			Negate = false;
			SubstrPos = int.MinValue;
			SubstrLen = int.MinValue;
		}

		/// <summary>
		/// Creates a string filter condition class with a negation flag and allowing specification of the comparison type
		/// </summary>
		public cFilterCondition(string AField, string AValue, eFilterConditionStringCompare ACompare, bool ANegate)
			: base()
		{
			Field = AField;
			DataType = eFilterDataType.String;
			Value = AValue;
			Compare = Convert(ACompare);
			Negate = ANegate;
			SubstrPos = int.MinValue;
			SubstrLen = int.MinValue;
		}

		/// <summary>
		/// Creates a string filter condition class using a substring of the field and allowing specification of the comparison type
		/// </summary>
		public cFilterCondition(string AField, string AValue, eFilterConditionStringCompare ACompare, int ASubstrPos, int ASubstrLen)
			: base()
		{
			Field = AField;
			DataType = eFilterDataType.String;
			Value = AValue;
			Compare = Convert(ACompare);
			Negate = false;
			SubstrPos = ASubstrPos;
			SubstrLen = ASubstrLen;
		}

		/// <summary>
		/// Creates a string filter condition class with a negation flag, using a substring of the field and allowing specification of the comparison type
		/// </summary>
		public cFilterCondition(string AField, string AValue, eFilterConditionStringCompare ACompare, bool ANegate, int ASubstrPos, int ASubstrLen)
			: base()
		{
			Field = AField;
			DataType = eFilterDataType.String;
			Value = AValue;
			Compare = Convert(ACompare);
			Negate = ANegate;
			SubstrPos = ASubstrPos;
			SubstrLen = ASubstrLen;
		}

        /// <summary>
        /// Creates a filter condition class for a date/time and specifying a comparison method.
        /// </summary>
        /// <param name="field">The database column involved in the filter.</param>
        /// <param name="compare">The comparison to perform.</param>
        /// <param name="value">The value by which to filter.</param>
        /// <param name="nullDateDefault">Indicates whether to default null database values to the min or max datetime.  Defaults to min.</param>
        public cFilterCondition(string field, eFilterConditionRelativeCompare compare, DateTime value, eNullDateDefault nullDateDefault = eNullDateDefault.Min)
        {
            Field = field;
            DataType = (nullDateDefault == eNullDateDefault.Min) ? eFilterDataType.DateBegan : eFilterDataType.DateEnded;
            Value = value;
            Compare = Convert(compare);
            Negate = false;
            SubstrPos = int.MinValue;
            SubstrLen = int.MinValue;
        }

        /// <summary>
        /// Creates a filter condition class for a date/time and specifying a comparison method.
        /// </summary>
        /// <param name="field">The database column involved in the filter.</param>
        /// <param name="compare">The comparison to perform.</param>
        /// <param name="value">The value by which to filter.</param>
        public cFilterCondition(string field, eFilterConditionRelativeCompare compare, int value)
        {
            Field = field;
            DataType = eFilterDataType.Integer;
            Value = value;
            Compare = Convert(compare);
            Negate = false;
            SubstrPos = int.MinValue;
            SubstrLen = int.MinValue;
        }

        /// <summary>
        /// Creates a filter condition class for a date/time and specifying a comparison method.
        /// </summary>
        /// <param name="field">The database column involved in the filter.</param>
        /// <param name="value">The value by which to filter.</param>
        public cFilterCondition(string field, int value)
        {
            Field = field;
            DataType = eFilterDataType.Integer;
            Value = value;
            Compare = eFilterConditionCompare.Equals;
            Negate = false;
            SubstrPos = int.MinValue;
            SubstrLen = int.MinValue;
        }

        /// <summary>
        /// Creates a equality filter condition class allowing specification of the data type
        /// </summary>
        public cFilterCondition(string AField, object AValue, eFilterDataType ADataType)
			: base()
		{
			Field = AField;
			DataType = ADataType;
			Value = AValue;
			Compare = eFilterConditionCompare.Equals;
			Negate = false;
			SubstrPos = int.MinValue;
			SubstrLen = int.MinValue;
		}

		/// <summary>
		/// Creates a filter condition class allowing specification of the data type and comparison method
		/// </summary>
		public cFilterCondition(string AField, object AValue, eFilterDataType ADataType, eFilterConditionRelativeCompare ACompare)
			: base()
		{
			Field = AField;
			DataType = ADataType;
			Value = AValue;
			Compare = Convert(ACompare);
			Negate = false;
			SubstrPos = int.MinValue;
			SubstrLen = int.MinValue;
		}

		/// <summary>
		/// Creates a filter condition class allowing specification of the data type, comparison method and negation
		/// </summary>
		public cFilterCondition(string AField, object AValue, eFilterDataType ADataType, eFilterConditionRelativeCompare ACompare, bool ANegate)
			: base()
		{
			Field = AField;
			DataType = ADataType;
			Value = AValue;
			Compare = Convert(ACompare);
			Negate = ANegate;
			SubstrPos = int.MinValue;
			SubstrLen = int.MinValue;
		}

		/// <summary>
		/// Creates a equality filter condition class allowing specification of the data type and negation
		/// </summary>
		public cFilterCondition(string AField, object AValue, eFilterDataType ADataType, bool ANegate)
			: base()
		{
			Field = AField;
			DataType = ADataType;
			Value = AValue;
			Compare = eFilterConditionCompare.Equals;
			Negate = ANegate;
			SubstrPos = int.MinValue;
			SubstrLen = int.MinValue;
		}

		#endregion

		#region Public Fields

		/// <summary>The field to compare</summary>
		public string Field;
		/// <summary>The data type of the field to compare</summary>
		public eFilterDataType DataType; // This should default to String
		/// <summary>The value to compare against the field</summary>
		public object Value;
		/// <summary>The comparison method to use</summary>
		public eFilterConditionCompare Compare; // This should default to Equals
		/// <summary>Whether the comparison should return the negation of the condition result</summary>
		public bool Negate; // This should default to false
		/// <summary>The begin position of the substring in the field to use in the discription</summary>
		public int SubstrPos;
		/// <summary>The length of the substring in the field to use in the discription</summary>
		public int SubstrLen;

		#endregion

		#region Private Methods

		private eFilterConditionCompare Convert(eFilterConditionStringCompare ACompare)
		{
			if (ACompare == eFilterConditionStringCompare.InList)
				return eFilterConditionCompare.InList;
			else if (ACompare == eFilterConditionStringCompare.ListHas)
				return eFilterConditionCompare.ListHas;
			else if (ACompare == eFilterConditionStringCompare.Contains)
				return eFilterConditionCompare.Contains;
			else if (ACompare == eFilterConditionStringCompare.BeginsWith)
				return eFilterConditionCompare.BeginsWith;
			else if (ACompare == eFilterConditionStringCompare.EndsWith)
				return eFilterConditionCompare.EndsWith;
			else
				return eFilterConditionCompare.Equals;
		}

		private eFilterConditionCompare Convert(eFilterConditionRelativeCompare ACompare)
		{
			if (ACompare == eFilterConditionRelativeCompare.LessThan)
				return eFilterConditionCompare.LessThan;
			else if (ACompare == eFilterConditionRelativeCompare.LessThanOrEqual)
				return eFilterConditionCompare.LessThanOrEqual;
			else if (ACompare == eFilterConditionRelativeCompare.GreaterThanOrEqual)
				return eFilterConditionCompare.GreaterThanOrEqual;
			else if (ACompare == eFilterConditionRelativeCompare.GreaterThan)
				return eFilterConditionCompare.GreaterThan;
			else
				return eFilterConditionCompare.Equals;
		}

		#endregion

		#region Public Methods: Sets

		/// <summary>
		/// Sets the condtion as a string equal filter condition class
		/// </summary>
		public void Set(string AField, string AValue)
		{
			Field = AField;
			DataType = eFilterDataType.String;
			Value = AValue;
			Compare = eFilterConditionCompare.Equals;
			Negate = false;
			SubstrPos = int.MinValue;
			SubstrLen = int.MinValue;
		}

		/// <summary>
		/// Sets the condtion as a string equal filter condition class with negation flag 
		/// </summary>
		public void Set(string AField, string AValue, bool ANegate)
		{
			Field = AField;
			DataType = eFilterDataType.String;
			Value = AValue;
			Compare = eFilterConditionCompare.Equals;
			Negate = ANegate;
			SubstrPos = int.MinValue;
			SubstrLen = int.MinValue;
		}

		/// <summary>
		/// Sets the condtion as a string equal filter condition class using a substring of the field
		/// </summary>
		public void Set(string AField, string AValue, int ASubstrPos, int ASubstrLen)
		{
			Field = AField;
			DataType = eFilterDataType.String;
			Value = AValue;
			Compare = eFilterConditionCompare.Equals;
			Negate = false;
			SubstrPos = ASubstrPos;
			SubstrLen = ASubstrLen;
		}

		/// <summary>
		/// Sets the condtion as a string equal filter condition class with a negation flag and using a substring of the field
		/// </summary>
		public void Set(string AField, string AValue, bool ANegate, int ASubstrPos, int ASubstrLen)
		{
			Field = AField;
			DataType = eFilterDataType.String;
			Value = AValue;
			Compare = eFilterConditionCompare.Equals;
			Negate = ANegate;
			SubstrPos = ASubstrPos;
			SubstrLen = ASubstrLen;
		}

		/// <summary>
		/// Sets the condtion as a string filter condition class allowing specification of the comparison type
		/// </summary>
		public void Set(string AField, string AValue, eFilterConditionStringCompare ACompare)
		{
			Field = AField;
			DataType = eFilterDataType.String;
			Value = AValue;
			Compare = Convert(ACompare);
			Negate = false;
			SubstrPos = int.MinValue;
			SubstrLen = int.MinValue;
		}

		/// <summary>
		/// Sets the condtion as a string filter condition class with a negation flag and allowing specification of the comparison type
		/// </summary>
		public void Set(string AField, string AValue, eFilterConditionStringCompare ACompare, bool ANegate)
		{
			Field = AField;
			DataType = eFilterDataType.String;
			Value = AValue;
			Compare = Convert(ACompare);
			Negate = ANegate;
			SubstrPos = int.MinValue;
			SubstrLen = int.MinValue;
		}

		/// <summary>
		/// Sets the condtion as a string filter condition class using a substring of the field and allowing specification of the comparison type
		/// </summary>
		public void Set(string AField, string AValue, eFilterConditionStringCompare ACompare, int ASubstrPos, int ASubstrLen)
		{
			Field = AField;
			DataType = eFilterDataType.String;
			Value = AValue;
			Compare = Convert(ACompare);
			Negate = false;
			SubstrPos = ASubstrPos;
			SubstrLen = ASubstrLen;
		}

		/// <summary>
		/// Sets the condtion as a string filter condition class with a negation flag, using a substring of the field and allowing specification of the comparison type
		/// </summary>
		public void Set(string AField, string AValue, eFilterConditionStringCompare ACompare, bool ANegate, int ASubstrPos, int ASubstrLen)
		{
			Field = AField;
			DataType = eFilterDataType.String;
			Value = AValue;
			Compare = Convert(ACompare);
			Negate = ANegate;
			SubstrPos = ASubstrPos;
			SubstrLen = ASubstrLen;
		}

		/// <summary>
		/// Sets the condtion as a equality filter condition class allowing specification of the data type
		/// </summary>
		public void Set(string AField, object AValue, eFilterDataType ADataType)
		{
			Field = AField;
			DataType = ADataType;
			Value = AValue;
			Compare = eFilterConditionCompare.Equals;
			Negate = false;
			SubstrPos = int.MinValue;
			SubstrLen = int.MinValue;
		}

		/// <summary>
		/// Sets the condtion as a filter condition class allowing specification of the data type and comparison method
		/// </summary>
		public void Set(string AField, object AValue, eFilterDataType ADataType, eFilterConditionRelativeCompare ACompare)
		{
			Field = AField;
			DataType = ADataType;
			Value = AValue;
			Compare = Convert(ACompare);
			Negate = false;
			SubstrPos = int.MinValue;
			SubstrLen = int.MinValue;
		}

		/// <summary>
		/// Sets the condtion as a filter condition class allowing specification of the data type, comparison method and negation
		/// </summary>
		public void Set(string AField, object AValue, eFilterDataType ADataType, eFilterConditionRelativeCompare ACompare, bool ANegate)
		{
			Field = AField;
			DataType = ADataType;
			Value = AValue;
			Compare = Convert(ACompare);
			Negate = ANegate;
			SubstrPos = int.MinValue;
			SubstrLen = int.MinValue;
		}

		/// <summary>
		/// Sets the condtion as a equality filter condition class allowing specification of the data type and negation
		/// </summary>
		public void Set(string AField, object AValue, eFilterDataType ADataType, bool ANegate)
		{
			Field = AField;
			DataType = ADataType;
			Value = AValue;
			Compare = eFilterConditionCompare.Equals;
			Negate = ANegate;
			SubstrPos = int.MinValue;
			SubstrLen = int.MinValue;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Displays the condition settings
		/// </summary>
		public override string ToString()
		{
			string sToString;

			if ((SubstrPos >= 0) && (SubstrLen > 0))
				sToString = string.Format("Field='{0}', Value='{1}', Compare={2}, Not={3}, Substr({4},{5})", Field, Value, Compare.ToString(), Negate, SubstrPos.ToString(), SubstrLen.ToString());
			else
				sToString = string.Format("Field='{0}', Value='{1}', Compare={2}, Not={3}", Field, Value, Compare.ToString(), Negate);

			return sToString;
		}

		#endregion

	}

	/// <summary>
	/// Contains functions and types that support the filtering of data views.
	/// </summary>
	public class cRowFilter
	{

		#region Filter Pair with Enum and Methods

		private static bool RowMatches(DataRowView ACheckRow, cFilterCondition AFilterPair)
		{
			if (AFilterPair.DataType == eFilterDataType.String)
				return RowMatches_String(ACheckRow, AFilterPair);
			else if (AFilterPair.DataType == eFilterDataType.DateBegan)
				return RowMatches_Date(ACheckRow, AFilterPair, DateTypes.START);
			else if (AFilterPair.DataType == eFilterDataType.DateEnded)
				return RowMatches_Date(ACheckRow, AFilterPair, DateTypes.END);
			else if (AFilterPair.DataType == eFilterDataType.Decimal)
				return RowMatches_Decimal(ACheckRow, AFilterPair);
			else if (AFilterPair.DataType == eFilterDataType.Integer)
				return RowMatches_Integer(ACheckRow, AFilterPair);
			else if (AFilterPair.DataType == eFilterDataType.Long)
				return RowMatches_Long(ACheckRow, AFilterPair);
			else
				return false;
		}

		private static bool RowMatches_Decimal(DataRowView ACheckRow, cFilterCondition AFilterPair)
		{
			decimal DataValue = cDBConvert.ToDecimal(ACheckRow[AFilterPair.Field]);
			decimal PairValue = cDBConvert.ToDecimal(AFilterPair.Value);

			if (AFilterPair.Compare == eFilterConditionCompare.Equals)
				return ((DataValue == PairValue) != AFilterPair.Negate);
			else if (AFilterPair.Compare == eFilterConditionCompare.LessThan)
				return ((DataValue < PairValue) != AFilterPair.Negate);
			else if (AFilterPair.Compare == eFilterConditionCompare.LessThanOrEqual)
				return ((DataValue <= PairValue) != AFilterPair.Negate);
			else if (AFilterPair.Compare == eFilterConditionCompare.GreaterThanOrEqual)
				return ((DataValue >= PairValue) != AFilterPair.Negate);
			else if (AFilterPair.Compare == eFilterConditionCompare.GreaterThan)
				return ((DataValue > PairValue) != AFilterPair.Negate);
			else
				return false;
		}

		private static bool RowMatches_Integer(DataRowView ACheckRow, cFilterCondition AFilterPair)
		{
			int DataValue = cDBConvert.ToInteger(ACheckRow[AFilterPair.Field]);
			int PairValue = cDBConvert.ToInteger(AFilterPair.Value);

			if (AFilterPair.Compare == eFilterConditionCompare.Equals)
				return ((DataValue == PairValue) != AFilterPair.Negate);
			else if (AFilterPair.Compare == eFilterConditionCompare.LessThan)
				return ((DataValue < PairValue) != AFilterPair.Negate);
			else if (AFilterPair.Compare == eFilterConditionCompare.LessThanOrEqual)
				return ((DataValue <= PairValue) != AFilterPair.Negate);
			else if (AFilterPair.Compare == eFilterConditionCompare.GreaterThanOrEqual)
				return ((DataValue >= PairValue) != AFilterPair.Negate);
			else if (AFilterPair.Compare == eFilterConditionCompare.GreaterThan)
				return ((DataValue > PairValue) != AFilterPair.Negate);
			else
				return false;
		}

		private static bool RowMatches_Long(DataRowView ACheckRow, cFilterCondition AFilterPair)
		{
			long DataValue = cDBConvert.ToLong(ACheckRow[AFilterPair.Field]);
			long PairValue = cDBConvert.ToLong(AFilterPair.Value);

			if (AFilterPair.Compare == eFilterConditionCompare.Equals)
				return ((DataValue == PairValue) != AFilterPair.Negate);
			else if (AFilterPair.Compare == eFilterConditionCompare.LessThan)
				return ((DataValue < PairValue) != AFilterPair.Negate);
			else if (AFilterPair.Compare == eFilterConditionCompare.LessThanOrEqual)
				return ((DataValue <= PairValue) != AFilterPair.Negate);
			else if (AFilterPair.Compare == eFilterConditionCompare.GreaterThanOrEqual)
				return ((DataValue >= PairValue) != AFilterPair.Negate);
			else if (AFilterPair.Compare == eFilterConditionCompare.GreaterThan)
				return ((DataValue > PairValue) != AFilterPair.Negate);
			else
				return false;
		}

		private static bool RowMatches_Date(DataRowView ACheckRow, cFilterCondition AFilterPair, DateTypes ADateType)
		{
			DateTime DataValue = cDBConvert.ToDate(ACheckRow[AFilterPair.Field], ADateType);
			DateTime PairValue = cDBConvert.ToDate(AFilterPair.Value, ADateType);

			if (AFilterPair.Compare == eFilterConditionCompare.Equals)
				return ((DataValue == PairValue) != AFilterPair.Negate);
			else if (AFilterPair.Compare == eFilterConditionCompare.LessThan)
				return ((DataValue < PairValue) != AFilterPair.Negate);
			else if (AFilterPair.Compare == eFilterConditionCompare.LessThanOrEqual)
				return ((DataValue <= PairValue) != AFilterPair.Negate);
			else if (AFilterPair.Compare == eFilterConditionCompare.GreaterThanOrEqual)
				return ((DataValue >= PairValue) != AFilterPair.Negate);
			else if (AFilterPair.Compare == eFilterConditionCompare.GreaterThan)
				return ((DataValue > PairValue) != AFilterPair.Negate);
			else
				return false;
		}

		private static bool RowMatches_String(DataRowView ACheckRow, cFilterCondition AFilterPair)
		{
			string DataValue = cDBConvert.ToString(ACheckRow[AFilterPair.Field]);
			string PairValue = cDBConvert.ToString(AFilterPair.Value);

			if ((AFilterPair.SubstrPos >= 0) && (AFilterPair.SubstrLen > 0))
			{
				if (AFilterPair.SubstrPos >= DataValue.Length)
					DataValue = "";
				else if ((AFilterPair.SubstrPos + AFilterPair.SubstrLen) > DataValue.Length)
					DataValue = DataValue.Substring(AFilterPair.SubstrPos);
				else
					DataValue = DataValue.Substring(AFilterPair.SubstrPos, AFilterPair.SubstrLen);
			}

			if (AFilterPair.Compare == eFilterConditionCompare.BeginsWith)
			{
				DataValue = DataValue.PadRight(PairValue.Length);
				DataValue = DataValue.Substring(0, PairValue.Length);

				return ((DataValue == PairValue) != AFilterPair.Negate);
			}
			else if (AFilterPair.Compare == eFilterConditionCompare.EndsWith)
			{
				DataValue = DataValue.PadLeft(PairValue.Length);
				DataValue = DataValue.Substring(DataValue.Length - PairValue.Length, PairValue.Length);

				return ((DataValue == PairValue) != AFilterPair.Negate);
			}
			else if (AFilterPair.Compare == eFilterConditionCompare.Contains)
			{
				return (DataValue.Contains(PairValue) != AFilterPair.Negate);
			}
			else if (AFilterPair.Compare == eFilterConditionCompare.InList)
			{
				return (DataValue.InList(PairValue) != AFilterPair.Negate);
			}
			else if (AFilterPair.Compare == eFilterConditionCompare.ListHas)
			{
				return (PairValue.InList(DataValue) != AFilterPair.Negate);
			}
			else
			{
				return ((DataValue == PairValue) != AFilterPair.Negate);
			}
		}

		#endregion

		#region Count and Find Methods

		#region CountRows

		/// <summary>
		/// Returns a count of the rows in the view where the boolean value of the row filter
		/// evaluation does not equal the ANotFilter flag.
		/// </summary>
		/// <param name="ASourceView">The data view to check</param>
		/// <param name="ARowFilter">The row filter to apply</param>
		/// <param name="ANotFilter">Whether to negate the value returned by the filter</param>
		/// <returns>The number of rows matching the row filter and not filter.</returns>
		public static int CountRows(DataView ASourceView, cFilterCondition[] ARowFilter, bool ANotFilter)
		{
			int Count = 0;
			bool Match;

			if ((ASourceView != null) && (ASourceView.Table != null) && (ASourceView.Count > 0))
			{
				foreach (DataRowView SourceRow in ASourceView)
				{
					Match = true;

					foreach (cFilterCondition FilterPair in ARowFilter)
					{
						if (!RowMatches(SourceRow, FilterPair))
							Match = false;
					}

					if (Match == !ANotFilter)
						Count += 1;
				}
			}

			return Count;
		}

		/// <summary>
		/// Returns a count of the rows in the view where the boolean value of the row filter
		/// evaluation is true.
		/// </summary>
		/// <param name="ASourceView">The data view to check</param>
		/// <param name="ARowFilter">The row filter to apply</param>
		/// <returns>The number of rows matching the row filter and not filter.</returns>
		public static int CountRows(DataView ASourceView, cFilterCondition[] ARowFilter)
		{
			return CountRows(ASourceView, ARowFilter, false);
		}

		#endregion

		#region CountActiveRows for Active Hour Range

		/// <summary>
		/// Returns a count of the rows whose date and hour range fields overlap the passed
		/// date and hour range values.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="ABeganHour">The began hour of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="AEndedHour">The ended hour of the date and hour range to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="ABeganHourField">The began hour field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="AEndedHourField">The ended hour field name of the rows</param>
		/// <returns>The count of active rows</returns>
		public static int CountActiveRows(DataView ASourceView,
										  DateTime ABeganDate, int ABeganHour,
										  DateTime AEndedDate, int AEndedHour,
										  string ABeganDateField, string ABeganHourField,
										  string AEndedDateField, string AEndedHourField)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, ABeganHour,
										   AEndedDate, AEndedHour,
										   ABeganDateField, ABeganHourField,
										   AEndedDateField, AEndedHourField);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// Returns a count of the rows whose date and hour range fields overlap the passed
		/// date and hour range values.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="ABeganHour">The began hour of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="AEndedHour">The ended hour of the date and hour range to test rows against</param>
		/// <returns>The count of active rows</returns>
		public static int CountActiveRows(DataView ASourceView,
										  DateTime ABeganDate, int ABeganHour,
										  DateTime AEndedDate, int AEndedHour)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, ABeganHour,
										   AEndedDate, AEndedHour,
										   "Begin_Date", "Begin_Hour",
										   "End_Date", "End_Hour");

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// Returns a count of the rows whose date and hour range fields overlap the passed
		/// date and hour range values and for which the row filter evaluation is not equal
		/// to the not filter flag.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="ABeganHour">The began hour of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="AEndedHour">The ended hour of the date and hour range to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="ABeganHourField">The began hour field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="AEndedHourField">The ended hour field name of the rows</param>
		/// <param name="ANotFilter">Whether to negate the value returned by the filter</param>
		/// <param name="ARowFilter">The row filter to apply against each row.</param>
		/// <returns>The count of active rows that match the filter specifications</returns>
		public static int CountActiveRows(DataView ASourceView,
										  DateTime ABeganDate, int ABeganHour,
										  DateTime AEndedDate, int AEndedHour,
										  string ABeganDateField, string ABeganHourField,
										  string AEndedDateField, string AEndedHourField,
										  bool ANotFilter, cFilterCondition[] ARowFilter)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, ABeganHour,
										   AEndedDate, AEndedHour,
										   ABeganDateField, ABeganHourField,
										   AEndedDateField, AEndedHourField,
										   ANotFilter, ARowFilter);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// Returns a count of the rows whose date and hour range fields overlap the passed
		/// date and hour range values and for which the row filter evaluation is true.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="ABeganHour">The began hour of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="AEndedHour">The ended hour of the date and hour range to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="ABeganHourField">The began hour field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="AEndedHourField">The ended hour field name of the rows</param>
		/// <param name="ARowFilter">The row filter to apply against each row.</param>
		/// <returns>The count of active rows that match the filter specifications</returns>
		public static int CountActiveRows(DataView ASourceView,
										  DateTime ABeganDate, int ABeganHour,
										  DateTime AEndedDate, int AEndedHour,
										  string ABeganDateField, string ABeganHourField,
										  string AEndedDateField, string AEndedHourField,
										  cFilterCondition[] ARowFilter)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, ABeganHour,
										   AEndedDate, AEndedHour,
										   ABeganDateField, ABeganHourField,
										   AEndedDateField, AEndedHourField,
										   ARowFilter);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// Returns a count of the rows whose date and hour range fields overlap the passed
		/// date and hour range values and for which the row filter evaluation is not equal
		/// to the not filter flag.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="ABeganHour">The began hour of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="AEndedHour">The ended hour of the date and hour range to test rows against</param>
		/// <param name="ANotFilter">Whether to negate the value returned by the filter</param>
		/// <param name="ARowFilter">The row filter to apply against each row.</param>
		/// <returns>The count of active rows that match the filter specifications</returns>
		public static int CountActiveRows(DataView ASourceView,
										  DateTime ABeganDate, int ABeganHour,
										  DateTime AEndedDate, int AEndedHour,
										  bool ANotFilter, cFilterCondition[] ARowFilter)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, ABeganHour,
										   AEndedDate, AEndedHour,
										   ANotFilter, ARowFilter);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// Returns a count of the rows whose date and hour range fields overlap the passed
		/// date and hour range values and for which the row filter evaluation is true.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="ABeganHour">The began hour of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="AEndedHour">The ended hour of the date and hour range to test rows against</param>
		/// <param name="ARowFilter">The row filter to apply against each row.</param>
		/// <returns>The count of active rows that match the filter specifications</returns>
		public static int CountActiveRows(DataView ASourceView,
										  DateTime ABeganDate, int ABeganHour,
										  DateTime AEndedDate, int AEndedHour,
										  cFilterCondition[] ARowFilter)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, ABeganHour,
										   AEndedDate, AEndedHour,
										   ARowFilter);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		#endregion

		#region CountActiveRows for Active Date Range

		/// <summary>
		/// Returns a count of the rows whose date range fields overlap the passed date range values
		/// including the end points depending on the began and ended date inclusive flags.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="ABeganDateInclusive">Indicates whether began date is included in the range comparison</param>
		/// <param name="AEndedDateInclusive">Indicates whether ended date is included in the range comparison</param>
		/// <returns>The count of active rows</returns>
		public static int CountActiveRows(DataView ASourceView,
										  DateTime ABeganDate, DateTime AEndedDate,
										  string ABeganDateField, string AEndedDateField,
										  bool ABeganDateInclusive, bool AEndedDateInclusive)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ABeganDateField, AEndedDateField,
										   ABeganDateInclusive, AEndedDateInclusive);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// Returns a count of the rows whose date range fields overlap the passed date range values.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <returns>The count of active rows</returns>
		public static int CountActiveRows(DataView ASourceView,
										  DateTime ABeganDate, DateTime AEndedDate,
										  string ABeganDateField, string AEndedDateField)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ABeganDateField, AEndedDateField);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// Returns a count of the rows whose date range fields overlap the passed date range values
		/// including the end points depending on the began and ended date inclusive flags.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ABeganDateInclusive">Indicates whether began date is included in the range comparison</param>
		/// <param name="AEndedDateInclusive">Indicates whether ended date is included in the range comparison</param>
		/// <returns>The count of active rows</returns>
		public static int CountActiveRows(DataView ASourceView,
										  DateTime ABeganDate, DateTime AEndedDate,
										  bool ABeganDateInclusive, bool AEndedDateInclusive)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ABeganDateInclusive, AEndedDateInclusive);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// Returns a count of the rows whose date range fields overlap the passed date range values.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <returns>The count of active rows</returns>
		public static int CountActiveRows(DataView ASourceView,
										  DateTime ABeganDate, DateTime AEndedDate)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// Returns a count of the rows whose date range fields overlap the passed date range values
		/// including the end points depending on the began and ended date inclusive flags, 
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="ABeganDateInclusive">Indicates whether began date is included in the range comparison</param>
		/// <param name="AEndedDateInclusive">Indicates whether ended date is included in the range comparison</param>
		/// <param name="ANotFilter">Indicates whether to negate the result of evaluating the row filter.</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns>The count of active rows that match the filter specifications</returns>
		public static int CountActiveRows(DataView ASourceView,
										  DateTime ABeganDate, DateTime AEndedDate,
										  string ABeganDateField, string AEndedDateField,
										  bool ABeganDateInclusive, bool AEndedDateInclusive,
										  bool ANotFilter, cFilterCondition[] ARowFilter)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ABeganDateField, AEndedDateField,
										   ABeganDateInclusive, AEndedDateInclusive,
										   ANotFilter, ARowFilter);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// Returns a count of the rows whose date range fields overlap the passed date range values
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="ANotFilter">Indicates whether to negate the result of evaluating the row filter.</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns>The count of active rows that match the filter specifications</returns>
		public static int CountActiveRows(DataView ASourceView,
										  DateTime ABeganDate, DateTime AEndedDate,
										  string ABeganDateField, string AEndedDateField,
										  bool ANotFilter, cFilterCondition[] ARowFilter)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ABeganDateField, AEndedDateField,
										   ANotFilter, ARowFilter);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// Returns a count of the rows whose date range fields overlap the passed date range values
		/// including the end points depending on the began and ended date inclusive flags, 
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="ABeganDateInclusive">Indicates whether began date is included in the range comparison</param>
		/// <param name="AEndedDateInclusive">Indicates whether ended date is included in the range comparison</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns></returns>
		public static int CountActiveRows(DataView ASourceView,
										  DateTime ABeganDate, DateTime AEndedDate,
										  string ABeganDateField, string AEndedDateField,
										  bool ABeganDateInclusive, bool AEndedDateInclusive,
										  cFilterCondition[] ARowFilter)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ABeganDateField, AEndedDateField,
										   ABeganDateInclusive, AEndedDateInclusive,
										   ARowFilter);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// Returns a count of the rows whose date range fields overlap the passed date range values
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns>The count of active rows that match the filter specifications</returns>
		public static int CountActiveRows(DataView ASourceView,
										  DateTime ABeganDate, DateTime AEndedDate,
										  string ABeganDateField, string AEndedDateField,
										  cFilterCondition[] ARowFilter)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ABeganDateField, AEndedDateField,
										   ARowFilter);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// Returns a count of the rows whose date range fields overlap the passed date range values
		/// including the end points depending on the began and ended date inclusive flags, 
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ABeganDateInclusive">Indicates whether began date is included in the range comparison</param>
		/// <param name="AEndedDateInclusive">Indicates whether ended date is included in the range comparison</param>
		/// <param name="ANotFilter">Indicates whether to negate the result of evaluating the row filter.</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns>The count of active rows that match the filter specifications</returns>
		public static int CountActiveRows(DataView ASourceView,
										  DateTime ABeganDate, DateTime AEndedDate,
										  bool ABeganDateInclusive, bool AEndedDateInclusive,
										  bool ANotFilter, cFilterCondition[] ARowFilter)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ABeganDateInclusive, AEndedDateInclusive,
										   ANotFilter, ARowFilter);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// Returns a count of the rows whose date range fields overlap the passed date range values
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ANotFilter">Indicates whether to negate the result of evaluating the row filter.</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns>The count of active rows that match the filter specifications</returns>
		public static int CountActiveRows(DataView ASourceView,
										  DateTime ABeganDate, DateTime AEndedDate,
										  bool ANotFilter, cFilterCondition[] ARowFilter)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ANotFilter, ARowFilter);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// Returns a count of the rows whose date range fields overlap the passed date range values
		/// including the end points depending on the began and ended date inclusive flags, 
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ABeganDateInclusive">Indicates whether began date is included in the range comparison</param>
		/// <param name="AEndedDateInclusive">Indicates whether ended date is included in the range comparison</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns>The count of active rows that match the filter specifications</returns>
		public static int CountActiveRows(DataView ASourceView,
										  DateTime ABeganDate, DateTime AEndedDate,
										  bool ABeganDateInclusive, bool AEndedDateInclusive,
										  cFilterCondition[] ARowFilter)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ABeganDateInclusive, AEndedDateInclusive,
										   ARowFilter);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// Returns a count of the rows whose date range fields overlap the passed date range values
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns>The count of active rows that match the filter specifications</returns>
		public static int CountActiveRows(DataView ASourceView,
										  DateTime ABeganDate, DateTime AEndedDate,
										  cFilterCondition[] ARowFilter)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ARowFilter);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// Returns a count of the rows whose date range fields overlap the passed date range values
		/// including the end points depending on the began and ended date inclusive flags, 
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="ABeganDateInclusive">Indicates whether began date is included in the range comparison</param>
		/// <param name="AEndedDateInclusive">Indicates whether ended date is included in the range comparison</param>
		/// <param name="ANotFilter">Indicates whether to negate the result of evaluating the row filter.</param>
		/// <param name="ARowFilterList">A list of row filters that are or'd together to produce na evaluation</param>
		/// <returns>The count of active rows that match the filter specifications</returns>
		public static int CountActiveRows(DataView ASourceView,
										  DateTime ABeganDate, DateTime AEndedDate,
										  string ABeganDateField, string AEndedDateField,
										  bool ABeganDateInclusive, bool AEndedDateInclusive,
										  bool ANotFilter, cFilterCondition[][] ARowFilterList)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ABeganDateField, AEndedDateField,
										   ABeganDateInclusive, AEndedDateInclusive,
										   ANotFilter, ARowFilterList);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		/// <summary>
		/// Returns a count of the rows whose date range fields overlap the passed date range values
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ARowFilterList">A list of row filters that are or'd together to produce na evaluation</param>
		/// <returns>The count of active rows that match the filter specifications</returns>
		public static int CountActiveRows(DataView ASourceView,
										  DateTime ABeganDate, DateTime AEndedDate,
										  params cFilterCondition[][] ARowFilterList)
		{
			DataView View = FindActiveRows(ASourceView,
										   ABeganDate, AEndedDate,
										   ARowFilterList);

			if (View != null)
				return View.Count;
			else
				return 0;
		}

		#endregion

		#region FindRow

		/// <summary>
		/// Finds the first row that matches the filter specification and return true if the rwo is found.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <param name="AFilteredRow">Out parameter of the first row found through filtering</param>
		/// <returns>True if a row was found otherwise false.</returns>
		public static bool FindRow(DataView ASourceView, cFilterCondition[] ARowFilter, out DataRowView AFilteredRow)
		{
			bool Result;
			bool Match;

			if ((ASourceView != null) && (ASourceView.Table != null) && (ASourceView.Count > 0))
			{
				AFilteredRow = null;
				Result = false;

				foreach (DataRowView SourceRow in ASourceView)
				{
					Match = true;

					foreach (cFilterCondition FilterPair in ARowFilter)
					{
						if (!RowMatches(SourceRow, FilterPair))
							Match = false;
					}

					if (Match)
					{
						AFilteredRow = SourceRow;
						Result = true;
						break;
					}
				}
			}
			else
			{
				AFilteredRow = null;
				Result = false;
			}

			return Result;
		}

		/// <summary>
		/// Returns the first row that matches the filter specification.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns>The first row that matches the filter specification, or null if no row matches</returns>
		public static DataRowView FindRow(DataView ASourceView, cFilterCondition[] ARowFilter)
		{
			DataRowView Result;

			if (!FindRow(ASourceView, ARowFilter, out Result))
				Result = null;

			return Result;
		}

		/// <summary>
		/// Returns the first row that matches the filter specification.
		/// </summary>
		/// <param name="sourceView">The data view object in which to count rows</param>
		/// <param name="rowFilterList">A list of row filters that are or'd together to produce na evaluation</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataRowView FindRow(DataView sourceView, params cFilterCondition[][] rowFilterList)
		{
			DataRowView result = null;

			if ((sourceView != null) && (sourceView.Table != null))
			{
				if (sourceView.Count > 0)
				{
					foreach (DataRowView SourceRow in sourceView)
					{
						bool match = false;

						foreach (cFilterCondition[] FilterPairs in rowFilterList)
						{
							bool filterMatch = true;

							foreach (cFilterCondition FilterPair in FilterPairs)
							{
								if (!RowMatches(SourceRow, FilterPair))
									filterMatch = false;
							}

							if (filterMatch) match = true;
						}

						if (match)
						{
							result = SourceRow; ;
							break;
						}
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Returns the first row after applying the row filter and sort order.
		/// </summary>
		/// <param name="sourceView">The dataview on which to apply the filter and sort.</param>
		/// <param name="sortOrder">The sort to apply.</param>
		/// <param name="rowFilter">The filter to apply.</param>
		/// <returns>The first row after applying the filter and sort.</returns>
		public static DataRowView FindFirstRow(DataView sourceView, string sortOrder, cFilterCondition[] rowFilter)
		{
			DataRowView result;

			DataView foundRows = FindRows(sourceView, rowFilter);

			if ((foundRows != null) && (foundRows.Count > 0))
			{
				try
				{
					foundRows.Sort = sortOrder;
					result = foundRows[0];
				}
				catch
				{
					result = null;
				}
			}
			else
				result = null;

			return result;
		}

		/// <summary>
		/// Returns the last row after applying the row filter and sort order.
		/// </summary>
		/// <param name="sourceView">The dataview on which to apply the filter and sort.</param>
		/// <param name="sortOrder">The sort to apply.</param>
		/// <param name="rowFilter">The filter to apply.</param>
		/// <returns>The last row after applying the filter and sort.</returns>
		public static DataRowView FindLastRow(DataView sourceView, string sortOrder, cFilterCondition[] rowFilter)
		{
			DataRowView result;

			DataView foundRows = FindRows(sourceView, rowFilter);

			if ((foundRows != null) && (foundRows.Count > 0))
			{
				try
				{
					foundRows.Sort = sortOrder;
					result = foundRows[foundRows.Count - 1];
				}
				catch
				{
					result = null;
				}
			}
			else
				result = null;

			return result;
		}

		#endregion

		#region  FindRows

		/// <summary>
		/// Returns the rows that match the filter specification.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <param name="ANotFilter">Indicates whether to negate the result of evaluating the row filter.</param>
		/// <returns>Data view of the rows that match the filter specification</returns>
		public static DataView FindRows(DataView ASourceView, cFilterCondition[] ARowFilter, bool ANotFilter)
		{
			if ((ASourceView != null) && (ASourceView.Table != null))
			{
				DataTable FilterTable = CloneTable(ASourceView.Table);

				if (ASourceView.Count > 0)
				{
					DataRow FilterRow;
					bool Match;

					foreach (DataRowView SourceRow in ASourceView)
					{
						Match = true;

						foreach (cFilterCondition FilterPair in ARowFilter)
						{
							if (!RowMatches(SourceRow, FilterPair))
								Match = false;
						}

						if (Match == !ANotFilter)
						{
							FilterRow = FilterTable.NewRow();

							foreach (DataColumn Column in FilterTable.Columns)
								FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

							FilterTable.Rows.Add(FilterRow);
						}
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return null;
		}

		/// <summary>
		/// Returns the rows that match the filter specification.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns>Data view of the rows that match the filter specification</returns>
		public static DataView FindRows(DataView ASourceView, cFilterCondition[] ARowFilter)
		{
			return FindRows(ASourceView, ARowFilter, false);
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ANotFilter">Indicates whether to negate the result of evaluating the row filter.</param>
		/// <param name="ARowFilterList">A list of row filters that are or'd together to produce na evaluation</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindRows(DataView ASourceView,
										bool ANotFilter,
										params cFilterCondition[][] ARowFilterList)
		{

			if ((ASourceView != null) && (ASourceView.Table != null))
			{
				DataTable FilterTable = CloneTable(ASourceView.Table);

				if (ASourceView.Count > 0)
				{
					DataRow FilterRow;

					foreach (DataRowView SourceRow in ASourceView)
					{
						bool Match = false;

						foreach (cFilterCondition[] FilterPairs in ARowFilterList)
						{
							bool FilterMatch = true;

							foreach (cFilterCondition FilterPair in FilterPairs)
							{
								if (!RowMatches(SourceRow, FilterPair))
									FilterMatch = false;
							}

							if (FilterMatch) Match = true;
						}

						if (Match == !ANotFilter)
						{
							FilterRow = FilterTable.NewRow();

							foreach (DataColumn Column in FilterTable.Columns)
								FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

							FilterTable.Rows.Add(FilterRow);
						}
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return null;
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ARowFilterList">A list of row filters that are or'd together to produce na evaluation</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindRows(DataView ASourceView,
										params cFilterCondition[][] ARowFilterList)
		{
			DataView result;

			result = FindRows(ASourceView, false, ARowFilterList);

			return result;
		}

		#endregion

		#region FindActiveRows for Active Hour Range

		/// <summary>
		/// Returns a data view of the rows whose date and hour range fields overlap the passed
		/// date and hour range values.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="ABeganHour">The began hour of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="AEndedHour">The ended hour of the date and hour range to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="ABeganHourField">The began hour field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="AEndedHourField">The ended hour field name of the rows</param>
		/// <returns>The data view of active rows</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime ABeganDate, int ABeganHour,
											  DateTime AEndedDate, int AEndedHour,
											  string ABeganDateField, string ABeganHourField,
											  string AEndedDateField, string AEndedHourField)
		{
			DateTime BeganDate; int BeganHour;
			DateTime EndedDate; int EndedHour;

			if ((ASourceView != null) && (ASourceView.Table != null))
			{
				DataTable FilterTable = CloneTable(ASourceView.Table);

				if (ASourceView.Count > 0)
				{
					DataRow FilterRow;

					foreach (DataRowView SourceRow in ASourceView)
					{
						BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateField], DateTypes.START);
						BeganHour = cDBConvert.ToHour(SourceRow[ABeganHourField], DateTypes.START);
						EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);
						EndedHour = cDBConvert.ToHour(SourceRow[AEndedHourField], DateTypes.END);

						if (((BeganDate < AEndedDate) || ((BeganDate == AEndedDate) && (BeganHour <= AEndedHour))) &&
							((EndedDate > ABeganDate) || ((EndedDate == ABeganDate) && (EndedHour >= ABeganHour))))
						{
							FilterRow = FilterTable.NewRow();

							foreach (DataColumn Column in FilterTable.Columns)
								FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

							FilterTable.Rows.Add(FilterRow);
						}
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return null;
		}

		/// <summary>
		/// Returns a data view of the rows whose date and hour range fields overlap the passed
		/// date and hour range values.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="ABeganHour">The began hour of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="AEndedHour">The ended hour of the date and hour range to test rows against</param>
		/// <returns>The data view of active rows</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime ABeganDate, int ABeganHour,
											  DateTime AEndedDate, int AEndedHour)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, ABeganHour, AEndedDate, AEndedHour,
								  "Begin_Date", "Begin_Hour", "End_Date", "End_Hour");
		}

		/// <summary>
		/// Returns a data view of the rows whose date and hour range fields overlap the passed
		/// date and hour range values and for which the row filter evaluation is not equal
		/// to the not filter flag.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="ABeganHour">The began hour of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="AEndedHour">The ended hour of the date and hour range to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="ABeganHourField">The began hour field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="AEndedHourField">The ended hour field name of the rows</param>
		/// <param name="ANotFilter">Whether to negate the value returned by the filter</param>
		/// <param name="ARowFilter">The row filter to apply against each row.</param>
		/// <returns>The count of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime ABeganDate, int ABeganHour,
											  DateTime AEndedDate, int AEndedHour,
											  string ABeganDateField, string ABeganHourField,
											  string AEndedDateField, string AEndedHourField,
											  bool ANotFilter, cFilterCondition[] ARowFilter)
		{
			DateTime BeganDate; int BeganHour;
			DateTime EndedDate; int EndedHour;

			if ((ASourceView != null) && (ASourceView.Table != null))
			{
				DataTable FilterTable = CloneTable(ASourceView.Table);

				if (ASourceView.Count > 0)
				{
					DataRow FilterRow;
					bool Match;

					foreach (DataRowView SourceRow in ASourceView)
					{
						BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateField], DateTypes.START);
						BeganHour = cDBConvert.ToHour(SourceRow[ABeganHourField], DateTypes.START);
						EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);
						EndedHour = cDBConvert.ToHour(SourceRow[AEndedHourField], DateTypes.END);

						if (((BeganDate < AEndedDate) || ((BeganDate == AEndedDate) && (BeganHour <= AEndedHour))) &&
							((EndedDate > ABeganDate) || ((EndedDate == ABeganDate) && (EndedHour >= ABeganHour))))
						{
							Match = true;

							foreach (cFilterCondition FilterPair in ARowFilter)
							{
								if (!RowMatches(SourceRow, FilterPair))
									Match = false;
							}

							if (Match == !ANotFilter)
							{
								FilterRow = FilterTable.NewRow();

								foreach (DataColumn Column in FilterTable.Columns)
									FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

								FilterTable.Rows.Add(FilterRow);
							}
						}
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return null;
		}

		/// <summary>
		/// Returns a data view of the rows whose date and hour range fields overlap the passed
		/// date and hour range values and for which the row filter evaluation is true.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="ABeganHour">The began hour of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="AEndedHour">The ended hour of the date and hour range to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="ABeganHourField">The began hour field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="AEndedHourField">The ended hour field name of the rows</param>
		/// <param name="ARowFilter">The row filter to apply against each row.</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime ABeganDate, int ABeganHour,
											  DateTime AEndedDate, int AEndedHour,
											  string ABeganDateField, string ABeganHourField,
											  string AEndedDateField, string AEndedHourField,
											  cFilterCondition[] ARowFilter)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, ABeganHour, AEndedDate, AEndedHour,
								  ABeganDateField, ABeganHourField, AEndedDateField, AEndedHourField,
								  false, ARowFilter);
		}

		/// <summary>
		/// Returns a data view of the rows whose date and hour range fields overlap the passed
		/// date and hour range values and for which the row filter evaluation is not equal
		/// to the not filter flag.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="ABeganHour">The began hour of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="AEndedHour">The ended hour of the date and hour range to test rows against</param>
		/// <param name="ANotFilter">Whether to negate the value returned by the filter</param>
		/// <param name="ARowFilter">The row filter to apply against each row.</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime ABeganDate, int ABeganHour,
											  DateTime AEndedDate, int AEndedHour,
											  bool ANotFilter, cFilterCondition[] ARowFilter)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, ABeganHour, AEndedDate, AEndedHour,
								  "Begin_Date", "Begin_Hour", "End_Date", "End_Hour",
								  ANotFilter, ARowFilter);
		}

		/// <summary>
		/// Returns a data view of the rows whose date and hour range fields overlap the passed
		/// date and hour range values and for which the row filter evaluation is true.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="ABeganHour">The began hour of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="AEndedHour">The ended hour of the date and hour range to test rows against</param>
		/// <param name="ARowFilter">The row filter to apply against each row.</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime ABeganDate, int ABeganHour,
											  DateTime AEndedDate, int AEndedHour,
											  cFilterCondition[] ARowFilter)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, ABeganHour, AEndedDate, AEndedHour,
								  false, ARowFilter);
		}

		#endregion

		#region FindActiveRows for Active Date Range

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values
		/// including the end points depending on the began and ended date inclusive flags.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="ABeganDateInclusive">Indicates whether began date is included in the range comparison</param>
		/// <param name="AEndedDateInclusive">Indicates whether ended date is included in the range comparison</param>
		/// <returns>The data view of active rows</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime ABeganDate, DateTime AEndedDate,
											  string ABeganDateField, string AEndedDateField,
											  bool ABeganDateInclusive, bool AEndedDateInclusive)
		{
			DateTime BeganDate; DateTime EndedDate;

			if ((ASourceView != null) && (ASourceView.Table != null))
			{
				DataTable FilterTable = CloneTable(ASourceView.Table);

				if (ASourceView.Count > 0)
				{
					DataRow FilterRow;

					foreach (DataRowView SourceRow in ASourceView)
					{
						BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateField], DateTypes.START);
						EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);

						if (((BeganDate < AEndedDate) || (ABeganDateInclusive && (BeganDate == AEndedDate))) &&
							((EndedDate > ABeganDate) || (AEndedDateInclusive && (EndedDate == ABeganDate))))
						{
							FilterRow = FilterTable.NewRow();

							foreach (DataColumn Column in FilterTable.Columns)
								FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

							FilterTable.Rows.Add(FilterRow);
						}
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return null;
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <returns>The data view of active rows</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime ABeganDate, DateTime AEndedDate,
											  string ABeganDateField, string AEndedDateField)
		{
			return FindActiveRows(ASourceView, ABeganDate, AEndedDate, ABeganDateField, AEndedDateField, true, true);
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values
		/// including the end points depending on the began and ended date inclusive flags.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ABeganDateInclusive">Indicates whether began date is included in the range comparison</param>
		/// <param name="AEndedDateInclusive">Indicates whether ended date is included in the range comparison</param>
		/// <returns>The data view of active rows</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime ABeganDate, DateTime AEndedDate,
											  bool ABeganDateInclusive, bool AEndedDateInclusive)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, AEndedDate,
								  "Begin_Date", "End_Date",
								  ABeganDateInclusive, AEndedDateInclusive);
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <returns>The data view of active rows</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime ABeganDate, DateTime AEndedDate)
		{
			return FindActiveRows(ASourceView, ABeganDate, AEndedDate, "Begin_Date", "End_Date", true, true);
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values
		/// including the end points depending on the began and ended date inclusive flags, 
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="ABeganDateInclusive">Indicates whether began date is included in the range comparison</param>
		/// <param name="AEndedDateInclusive">Indicates whether ended date is included in the range comparison</param>
		/// <param name="ANotFilter">Indicates whether to negate the result of evaluating the row filter.</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime ABeganDate, DateTime AEndedDate,
											  string ABeganDateField, string AEndedDateField,
											  bool ABeganDateInclusive, bool AEndedDateInclusive,
											  bool ANotFilter, cFilterCondition[] ARowFilter)
		{
			DateTime BeganDate; DateTime EndedDate;

			if ((ASourceView != null) && (ASourceView.Table != null))
			{
				DataTable FilterTable = CloneTable(ASourceView.Table);

				if (ASourceView.Count > 0)
				{
					DataRow FilterRow;
					bool Match;

					foreach (DataRowView SourceRow in ASourceView)
					{
						BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateField], DateTypes.START);
						EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);

						if (((BeganDate < AEndedDate) || (ABeganDateInclusive && (BeganDate == AEndedDate))) &&
							((EndedDate > ABeganDate) || (AEndedDateInclusive && (EndedDate == ABeganDate))))
						{
							Match = true;

							foreach (cFilterCondition FilterPair in ARowFilter)
							{
								if (!RowMatches(SourceRow, FilterPair))
									Match = false;
							}

							if (Match == !ANotFilter)
							{
								FilterRow = FilterTable.NewRow();

								foreach (DataColumn Column in FilterTable.Columns)
									FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

								FilterTable.Rows.Add(FilterRow);
							}
						}
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return null;
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="ANotFilter">Indicates whether to negate the result of evaluating the row filter.</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime ABeganDate, DateTime AEndedDate,
											  string ABeganDateField, string AEndedDateField,
											  bool ANotFilter, cFilterCondition[] ARowFilter)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, AEndedDate,
								  ABeganDateField, AEndedDateField,
								  true, true,
								  ANotFilter, ARowFilter);
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values
		/// including the end points depending on the began and ended date inclusive flags, 
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="ABeganDateInclusive">Indicates whether began date is included in the range comparison</param>
		/// <param name="AEndedDateInclusive">Indicates whether ended date is included in the range comparison</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime ABeganDate, DateTime AEndedDate,
											  string ABeganDateField, string AEndedDateField,
											  bool ABeganDateInclusive, bool AEndedDateInclusive,
											  cFilterCondition[] ARowFilter)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, AEndedDate,
								  ABeganDateField, AEndedDateField,
								  ABeganDateInclusive, AEndedDateInclusive,
								  false, ARowFilter);
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime ABeganDate, DateTime AEndedDate,
											  string ABeganDateField, string AEndedDateField,
											  cFilterCondition[] ARowFilter)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, AEndedDate,
								  ABeganDateField, AEndedDateField,
								  true, true,
								  false, ARowFilter);
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values
		/// including the end points depending on the began and ended date inclusive flags, 
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ABeganDateInclusive">Indicates whether began date is included in the range comparison</param>
		/// <param name="AEndedDateInclusive">Indicates whether ended date is included in the range comparison</param>
		/// <param name="ANotFilter">Indicates whether to negate the result of evaluating the row filter.</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime ABeganDate, DateTime AEndedDate,
											  bool ABeganDateInclusive, bool AEndedDateInclusive,
											  bool ANotFilter, cFilterCondition[] ARowFilter)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, AEndedDate,
								  "Begin_Date", "End_Date",
								  ABeganDateInclusive, AEndedDateInclusive,
								  ANotFilter, ARowFilter);
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ANotFilter">Indicates whether to negate the result of evaluating the row filter.</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime ABeganDate, DateTime AEndedDate,
											  bool ANotFilter, cFilterCondition[] ARowFilter)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, AEndedDate,
								  "Begin_Date", "End_Date",
								  true, true,
								  ANotFilter, ARowFilter);
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values
		/// including the end points depending on the began and ended date inclusive flags, 
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ABeganDateInclusive">Indicates whether began date is included in the range comparison</param>
		/// <param name="AEndedDateInclusive">Indicates whether ended date is included in the range comparison</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime ABeganDate, DateTime AEndedDate,
											  bool ABeganDateInclusive, bool AEndedDateInclusive,
											  cFilterCondition[] ARowFilter)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, AEndedDate,
								  ABeganDateInclusive, AEndedDateInclusive,
								  false, ARowFilter);
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime ABeganDate, DateTime AEndedDate,
											  cFilterCondition[] ARowFilter)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, AEndedDate,
								  true, true,
								  false, ARowFilter);
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values
		/// including the end points depending on the began and ended date inclusive flags, 
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="ABeganDateInclusive">Indicates whether began date is included in the range comparison</param>
		/// <param name="AEndedDateInclusive">Indicates whether ended date is included in the range comparison</param>
		/// <param name="ANotFilter">Indicates whether to negate the result of evaluating the row filter.</param>
		/// <param name="ARowFilterList">A list of row filters that are or'd together to produce na evaluation</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime ABeganDate, DateTime AEndedDate,
											  string ABeganDateField, string AEndedDateField,
											  bool ABeganDateInclusive, bool AEndedDateInclusive,
											  bool ANotFilter, cFilterCondition[][] ARowFilterList)
		{
			DateTime BeganDate; DateTime EndedDate;

			if ((ASourceView != null) && (ASourceView.Table != null))
			{
				DataTable FilterTable = CloneTable(ASourceView.Table);

				if (ASourceView.Count > 0)
				{
					DataRow FilterRow;

					foreach (DataRowView SourceRow in ASourceView)
					{
						BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateField], DateTypes.START);
						EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);

						if (((BeganDate < AEndedDate) || (ABeganDateInclusive && (BeganDate == AEndedDate))) &&
							((EndedDate > ABeganDate) || (AEndedDateInclusive && (EndedDate == ABeganDate))))
						{
							bool Match = false;

							foreach (cFilterCondition[] FilterPairs in ARowFilterList)
							{
								bool FilterMatch = true;

								foreach (cFilterCondition FilterPair in FilterPairs)
								{
									if (!RowMatches(SourceRow, FilterPair))
										FilterMatch = false;
								}

								if (FilterMatch) Match = true;
							}

							if (Match == !ANotFilter)
							{
								FilterRow = FilterTable.NewRow();

								foreach (DataColumn Column in FilterTable.Columns)
									FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

								FilterTable.Rows.Add(FilterRow);
							}
						}
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return null;
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDate">The ended date of the date and hour range to test rows against</param>
		/// <param name="ARowFilterList">A list of row filters that are or'd together to produce na evaluation</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime ABeganDate, DateTime AEndedDate,
											  params cFilterCondition[][] ARowFilterList)
		{
			return FindActiveRows(ASourceView,
								  ABeganDate, AEndedDate,
								  "Begin_Date", "End_Date",
								  true, true,
								  false, ARowFilterList);
		}

		#endregion

		#region FindActiveRows for Operating Hour

		/// <summary>
		/// Returns a data view of the rows whose date and hour range fields overlap the passed
		/// date and hour range values.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="AOpDate">The operating date to test rows against</param>
		/// <param name="AOpHour">The operating hour to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="ABeganHourField">The began hour field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="AEndedHourField">The ended hour field name of the rows</param>
		/// <returns>The data view of active rows</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime AOpDate, int AOpHour,
											  string ABeganDateField, string ABeganHourField,
											  string AEndedDateField, string AEndedHourField)
		{
			DateTime BeganDate; int BeganHour;
			DateTime EndedDate; int EndedHour;

			if ((ASourceView != null) && (ASourceView.Table != null))
			{
				DataTable FilterTable = CloneTable(ASourceView.Table);

				if (ASourceView.Count > 0)
				{
					DataRow FilterRow;

					foreach (DataRowView SourceRow in ASourceView)
					{
						BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateField], DateTypes.START);
						BeganHour = cDBConvert.ToHour(SourceRow[ABeganHourField], DateTypes.START);
						EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);
						EndedHour = cDBConvert.ToHour(SourceRow[AEndedHourField], DateTypes.END);

						if (((BeganDate < AOpDate) || ((BeganDate == AOpDate) && (BeganHour <= AOpHour))) &&
							((EndedDate > AOpDate) || ((EndedDate == AOpDate) && (EndedHour >= AOpHour))))
						{
							FilterRow = FilterTable.NewRow();

							foreach (DataColumn Column in FilterTable.Columns)
								FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

							FilterTable.Rows.Add(FilterRow);
						}
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return null;
		}

		/// <summary>
		/// Returns a data view of the rows whose date and hour range fields overlap the passed
		/// date and hour range values.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="AOpDate">The operating date to test rows against</param>
		/// <param name="AOpHour">The operating hour to test rows against</param>
		/// <returns>The data view of active rows</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime AOpDate, int AOpHour)
		{
			return FindActiveRows(ASourceView,
								  AOpDate, AOpHour,
								  "Begin_Date", "Begin_Hour", "End_Date", "End_Hour");
		}

		/// <summary>
		/// Returns a data view of the rows whose date and hour range fields overlap the passed
		/// date and hour range values and for which the row filter evaluation is not equal
		/// to the not filter flag.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="AOpDate">The operating date to test rows against</param>
		/// <param name="AOpHour">The operating hour to test rows against</param>
		/// <param name="ANotFilter">Whether to negate the value returned by the filter</param>
		/// <param name="ARowFilterList">The row filter to apply against each row.</param>
		/// <returns>The count of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime AOpDate, int AOpHour,
											  bool ANotFilter, params cFilterCondition[][] ARowFilterList)
		{
			return FindActiveRows(ASourceView,
								  AOpDate, AOpHour,
								  "Begin_Date", "Begin_Hour", "End_Date", "End_Hour",
								  ANotFilter, ARowFilterList);
		}

		/// <summary>
		/// Returns a data view of the rows whose date and hour range fields overlap the passed
		/// date and hour range values and for which the row filter evaluation is not equal
		/// to the not filter flag.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="AOpDate">The operating date to test rows against</param>
		/// <param name="AOpHour">The operating hour to test rows against</param>
		/// <param name="ARowFilterList">The row filter to apply against each row.</param>
		/// <returns>The count of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime AOpDate, int AOpHour,
											  params cFilterCondition[][] ARowFilterList)
		{
			return FindActiveRows(ASourceView,
								  AOpDate, AOpHour,
								  "Begin_Date", "Begin_Hour", "End_Date", "End_Hour",
								  false, ARowFilterList);
		}

		/// <summary>
		/// Returns a data view of the rows whose date and hour range fields overlap the passed
		/// date and hour range values and for which the row filter evaluation is not equal
		/// to the not filter flag.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="AOpDate">The operating date to test rows against</param>
		/// <param name="AOpHour">The operating hour to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="ABeganHourField">The began hour field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="AEndedHourField">The ended hour field name of the rows</param>
		/// <param name="ANotFilter">Whether to negate the value returned by the filter</param>
		/// <param name="ARowFilterList">The row filter to apply against each row.</param>
		/// <returns>The count of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime AOpDate, int AOpHour,
											  string ABeganDateField, string ABeganHourField,
											  string AEndedDateField, string AEndedHourField,
											  bool ANotFilter, params cFilterCondition[][] ARowFilterList)
		{
			DateTime BeganDate; int BeganHour;
			DateTime EndedDate; int EndedHour;

			if ((ASourceView != null) && (ASourceView.Table != null))
			{
				DataTable FilterTable = CloneTable(ASourceView.Table);

				if (ASourceView.Count > 0)
				{
					DataRow FilterRow;
					bool Match;

					foreach (DataRowView SourceRow in ASourceView)
					{
						BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateField], DateTypes.START);
						BeganHour = cDBConvert.ToHour(SourceRow[ABeganHourField], DateTypes.START);
						EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);
						EndedHour = cDBConvert.ToHour(SourceRow[AEndedHourField], DateTypes.END);

						if (((BeganDate < AOpDate) || ((BeganDate == AOpDate) && (BeganHour <= AOpHour))) &&
							((EndedDate > AOpDate) || ((EndedDate == AOpDate) && (EndedHour >= AOpHour))))
						{
							Match = false;

							foreach (cFilterCondition[] FilterPairs in ARowFilterList)
							{
								bool FilterMatch = true;

								foreach (cFilterCondition FilterPair in FilterPairs)
								{
									if (!RowMatches(SourceRow, FilterPair))
										FilterMatch = false;
								}

								if (FilterMatch) Match = true;
							}

							if (Match == !ANotFilter)
							{
								FilterRow = FilterTable.NewRow();

								foreach (DataColumn Column in FilterTable.Columns)
									FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

								FilterTable.Rows.Add(FilterRow);
							}
						}
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return null;
		}

		// TODO: Remove this because above should handle call
		/// <summary>
		/// Returns a data view of the rows whose date and hour range fields overlap the passed
		/// date and hour range values and for which the row filter evaluation is not equal
		/// to the not filter flag.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="AOpDate">The operating date to test rows against</param>
		/// <param name="AOpHour">The operating hour to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="ABeganHourField">The began hour field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="AEndedHourField">The ended hour field name of the rows</param>
		/// <param name="ANotFilter">Whether to negate the value returned by the filter</param>
		/// <param name="ARowFilter">The row filter to apply against each row.</param>
		/// <returns>The count of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime AOpDate, int AOpHour,
											  string ABeganDateField, string ABeganHourField,
											  string AEndedDateField, string AEndedHourField,
											  bool ANotFilter, cFilterCondition[] ARowFilter)
		{
			DateTime BeganDate; int BeganHour;
			DateTime EndedDate; int EndedHour;

			if ((ASourceView != null) && (ASourceView.Table != null))
			{
				DataTable FilterTable = CloneTable(ASourceView.Table);

				if (ASourceView.Count > 0)
				{
					DataRow FilterRow;
					bool Match;

					foreach (DataRowView SourceRow in ASourceView)
					{
						BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateField], DateTypes.START);
						BeganHour = cDBConvert.ToHour(SourceRow[ABeganHourField], DateTypes.START);
						EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);
						EndedHour = cDBConvert.ToHour(SourceRow[AEndedHourField], DateTypes.END);

						if (((BeganDate < AOpDate) || ((BeganDate == AOpDate) && (BeganHour <= AOpHour))) &&
							((EndedDate > AOpDate) || ((EndedDate == AOpDate) && (EndedHour >= AOpHour))))
						{
							Match = true;

							foreach (cFilterCondition FilterPair in ARowFilter)
							{
								if (!RowMatches(SourceRow, FilterPair))
									Match = false;
							}

							if (Match == !ANotFilter)
							{
								FilterRow = FilterTable.NewRow();

								foreach (DataColumn Column in FilterTable.Columns)
									FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

								FilterTable.Rows.Add(FilterRow);
							}
						}
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return null;
		}

		/// <summary>
		/// Returns a data view of the rows whose date and hour range fields overlap the passed
		/// date and hour range values and for which the row filter evaluation is true.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="AOpDate">The operating date to test rows against</param>
		/// <param name="AOpHour">The operating hour to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="ABeganHourField">The began hour field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="AEndedHourField">The ended hour field name of the rows</param>
		/// <param name="ARowFilter">The row filter to apply against each row.</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime AOpDate, int AOpHour,
											  string ABeganDateField, string ABeganHourField,
											  string AEndedDateField, string AEndedHourField,
											  cFilterCondition[] ARowFilter)
		{
			return FindActiveRows(ASourceView,
								  AOpDate, AOpHour,
								  ABeganDateField, ABeganHourField, AEndedDateField, AEndedHourField,
								  false, ARowFilter);
		}

		/// <summary>
		/// Returns a data view of the rows whose date and hour range fields overlap the passed
		/// date and hour range values and for which the row filter evaluation is not equal
		/// to the not filter flag.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="AOpDate">The operating date to test rows against</param>
		/// <param name="AOpHour">The operating hour to test rows against</param>
		/// <param name="ANotFilter">Whether to negate the value returned by the filter</param>
		/// <param name="ARowFilter">The row filter to apply against each row.</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime AOpDate, int AOpHour,
											  bool ANotFilter, cFilterCondition[] ARowFilter)
		{
			return FindActiveRows(ASourceView,
								  AOpDate, AOpHour,
								  "Begin_Date", "Begin_Hour", "End_Date", "End_Hour",
								  ANotFilter, ARowFilter);
		}

		/// <summary>
		/// Returns a data view of the rows whose date and hour range fields overlap the passed
		/// date and hour range values and for which the row filter evaluation is true.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="AOpDate">The operating date to test rows against</param>
		/// <param name="AOpHour">The operating hour to test rows against</param>
		/// <param name="ARowFilter">The row filter to apply against each row.</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime AOpDate, int AOpHour,
											  cFilterCondition[] ARowFilter)
		{
			return FindActiveRows(ASourceView,
								  AOpDate, AOpHour,
								  false, ARowFilter);
		}

		#endregion

		#region FindActiveRows for Operating Date

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values
		/// including the end points depending on the began and ended date inclusive flags.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="AOpDate">The operating date to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <returns>The data view of active rows</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime AOpDate,
											  string ABeganDateField, string AEndedDateField)
		{
			DateTime BeganDate; DateTime EndedDate;

			if ((ASourceView != null) && (ASourceView.Table != null))
			{
				DataTable FilterTable = CloneTable(ASourceView.Table);

				if (ASourceView.Count > 0)
				{
					DataRow FilterRow;

					foreach (DataRowView SourceRow in ASourceView)
					{
						BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateField], DateTypes.START);
						EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);

						if ((BeganDate <= AOpDate) && (EndedDate >= AOpDate))
						{
							FilterRow = FilterTable.NewRow();

							foreach (DataColumn Column in FilterTable.Columns)
								FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

							FilterTable.Rows.Add(FilterRow);
						}
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return null;
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="AOpDate">The operating date to test rows against</param>
		/// <returns>The data view of active rows</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime AOpDate)
		{
			return FindActiveRows(ASourceView, AOpDate, "Begin_Date", "End_Date");
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values
		/// including the end points depending on the began and ended date inclusive flags, 
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="AOpDate">The operating date to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="ANotFilter">Indicates whether to negate the result of evaluating the row filter.</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime AOpDate,
											  string ABeganDateField, string AEndedDateField,
											  bool ANotFilter, cFilterCondition[] ARowFilter)
		{
			DateTime BeganDate; DateTime EndedDate;

			if ((ASourceView != null) && (ASourceView.Table != null))
			{
				DataTable FilterTable = CloneTable(ASourceView.Table);

				if (ASourceView.Count > 0)
				{
					DataRow FilterRow;
					bool Match;

					foreach (DataRowView SourceRow in ASourceView)
					{
						BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateField], DateTypes.START);
						EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);

						if ((BeganDate <= AOpDate) && (EndedDate >= AOpDate))
						{
							Match = true;

							foreach (cFilterCondition FilterPair in ARowFilter)
							{
								if (!RowMatches(SourceRow, FilterPair))
									Match = false;
							}

							if (Match == !ANotFilter)
							{
								FilterRow = FilterTable.NewRow();

								foreach (DataColumn Column in FilterTable.Columns)
									FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

								FilterTable.Rows.Add(FilterRow);
							}
						}
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return null;
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values
		/// including the end points depending on the began and ended date inclusive flags, 
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="AOpDate">The operating date to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime AOpDate,
											  string ABeganDateField, string AEndedDateField,
											  cFilterCondition[] ARowFilter)
		{
			return FindActiveRows(ASourceView,
								  AOpDate,
								  ABeganDateField, AEndedDateField,
								  false, ARowFilter);
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="AOpDate">The operating date to test rows against</param>
		/// <param name="ANotFilter">Indicates whether to negate the result of evaluating the row filter.</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime AOpDate,
											  bool ANotFilter, cFilterCondition[] ARowFilter)
		{
			return FindActiveRows(ASourceView,
								  AOpDate,
								  "Begin_Date", "End_Date",
								  ANotFilter, ARowFilter);
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="AOpDate">The operating date to test rows against</param>
		/// <param name="ARowFilter">The row filter to apply against the rows being tested.</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime AOpDate,
											  cFilterCondition[] ARowFilter)
		{
			return FindActiveRows(ASourceView,
								  AOpDate,
								  false, ARowFilter);
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values
		/// including the end points depending on the began and ended date inclusive flags, 
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="AOpDate">The operating date to test rows against</param>
		/// <param name="ABeganDateField">The began date field name of the rows</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="ANotFilter">Indicates whether to negate the result of evaluating the row filter.</param>
		/// <param name="ARowFilterList">A list of row filters that are or'd together to produce na evaluation</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime AOpDate,
											  string ABeganDateField, string AEndedDateField,
											  bool ANotFilter, cFilterCondition[][] ARowFilterList)
		{
			DateTime BeganDate; DateTime EndedDate;

			if ((ASourceView != null) && (ASourceView.Table != null))
			{
				DataTable FilterTable = CloneTable(ASourceView.Table);

				if (ASourceView.Count > 0)
				{
					DataRow FilterRow;

					foreach (DataRowView SourceRow in ASourceView)
					{
						BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateField], DateTypes.START);
						EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);

						if ((BeganDate <= AOpDate) && (EndedDate >= AOpDate))
						{
							bool Match = false;

							foreach (cFilterCondition[] FilterPairs in ARowFilterList)
							{
								bool FilterMatch = true;

								foreach (cFilterCondition FilterPair in FilterPairs)
								{
									if (!RowMatches(SourceRow, FilterPair))
										FilterMatch = false;
								}

								if (FilterMatch) Match = true;
							}

							if (Match == !ANotFilter)
							{
								FilterRow = FilterTable.NewRow();

								foreach (DataColumn Column in FilterTable.Columns)
									FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

								FilterTable.Rows.Add(FilterRow);
							}
						}
					}
				}

				return FilterTable.DefaultView;
			}
			else
				return null;
		}

		/// <summary>
		/// Returns a data view of the rows whose date range fields overlap the passed date range values
		/// and matching the filter conditions.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="AOpDate">The operating date to test rows against</param>
		/// <param name="ARowFilterList">A list of row filters that are or'd together to produce na evaluation</param>
		/// <returns>The data view of active rows that match the filter specifications</returns>
		public static DataView FindActiveRows(DataView ASourceView,
											  DateTime AOpDate,
											  params cFilterCondition[][] ARowFilterList)
		{
			return FindActiveRows(ASourceView,
								  AOpDate,
								  "Begin_Date", "End_Date",
								  false, ARowFilterList);
		}

		#endregion

		#region FindMostRecentRow for Active Hour

    /// <summary>
    /// Finds the row with an ended date and hour closest but not after to the specified 
    /// began date and hour and matching the filter specifications.
    /// 
    /// Note unlike some of its siblings, the most recent cannot be on or after referenceDatetime
    /// </summary>
    /// <param name="sourceView"></param>
    /// <param name="referenceDatetime"></param>
    /// <param name="dataDateTimeField"></param>
    /// <param name="notFilter"></param>
    /// <param name="rowFilter"></param>
    /// <returns></returns>
    public static DataRowView FindMostRecentRow(DataView sourceView,
                                                DateTime referenceDatetime,
                                                string dataDateTimeField,
                                                bool notFilter,
                                                cFilterCondition[] rowFilter)
    {
      if ((sourceView != null) && (sourceView.Table != null))
      {
        if (sourceView.Count > 0)
        {
          bool match;

          DateTime dataDateTime;

          DataTable filterTable = CloneTable(sourceView.Table);
          DateTime foundDateTime = DateTime.MinValue;
          DataRowView foundRow = null;

          foreach (DataRowView sourceRow in sourceView)
          {
            dataDateTime = sourceRow[dataDateTimeField].AsEndDateTime();

            if (dataDateTime < referenceDatetime)
            {
              match = true;

              foreach (cFilterCondition filterPair in rowFilter)
              {
                if (!RowMatches(sourceRow, filterPair))
                  match = false;
              }

              if ((match == !notFilter) && (dataDateTime > foundDateTime))
              {
                foundDateTime = dataDateTime;
                foundRow = sourceRow;
              }
            }
          }

          return foundRow;
        }
        else
          return null;
      }
      else
        return null;
    }

		/// <summary>
		/// Finds the row with an ended date and hour closest but not after to the specified 
		/// began date and hour and matching the filter specifications.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="ABeganHour">The began hour of the date and hour range to test rows against</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="AEndedHourField">The ended hour field name of the rows</param>
		/// <param name="ANotFilter">Whether to negate the value returned by the filter</param>
		/// <param name="ARowFilter">The row filter to apply against each row.</param>
		/// <returns>The most recent row based on its end date and hour</returns>
		public static DataRowView FindMostRecentRow(DataView ASourceView,
													   DateTime ABeganDate, int ABeganHour,
													   string AEndedDateField, string AEndedHourField,
													   bool ANotFilter, cFilterCondition[] ARowFilter)
		{
			DateTime EndedDate; int EndedHour;

			DateTime FoundDate = DateTime.MinValue; int FoundHour = int.MinValue;
			DataRowView FoundRow = null;

			if ((ASourceView != null) && (ASourceView.Table != null))
			{
				DataTable FilterTable = CloneTable(ASourceView.Table);

				if (ASourceView.Count > 0)
				{
					bool Match;

					foreach (DataRowView SourceRow in ASourceView)
					{
						EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);
						EndedHour = cDBConvert.ToHour(SourceRow[AEndedHourField], DateTypes.END);

						if ((EndedDate < ABeganDate) || ((EndedDate == ABeganDate) && (EndedHour <= ABeganHour)))
						{
							Match = true;

							foreach (cFilterCondition FilterPair in ARowFilter)
							{
								if (!RowMatches(SourceRow, FilterPair))
									Match = false;
							}

							if ((Match == !ANotFilter) &&
								((EndedDate > FoundDate) || ((EndedDate == FoundDate) && (EndedHour > FoundHour))))
							{
								FoundDate = EndedDate;
								FoundHour = EndedHour;
								FoundRow = SourceRow;
							}
						}
					}
				}

				return FoundRow;
			}
			else
				return null;
		}

		/// <summary>
		/// Finds the row with an ended date and hour closest but not after to the specified 
		/// began date and hour and matching the filter specifications.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="ABeganHour">The began hour of the date and hour range to test rows against</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="AEndedHourField">The ended hour field name of the rows</param>
		/// <param name="ARowFilter">The row filter to apply against each row.</param>
		/// <returns>The most recent row based on its end date and hour</returns>
		public static DataRowView FindMostRecentRow(DataView ASourceView,
													   DateTime ABeganDate, int ABeganHour,
													   string AEndedDateField, string AEndedHourField,
													   cFilterCondition[] ARowFilter)
		{
			return FindMostRecentRow(ASourceView,
									 ABeganDate, ABeganHour,
									 AEndedDateField, AEndedHourField,
									 false, ARowFilter);
		}

		/// <summary>
		/// Finds the row with an ended date and hour closest but not after to the specified 
		/// began date and hour and matching the filter specifications.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="ABeganHour">The began hour of the date and hour range to test rows against</param>
		/// <param name="ANotFilter">Whether to negate the value returned by the filter</param>
		/// <param name="ARowFilter">The row filter to apply against each row.</param>
		/// <returns>The most recent row based on its end date and hour</returns>
		public static DataRowView FindMostRecentRow(DataView ASourceView,
													   DateTime ABeganDate, int ABeganHour,
													   bool ANotFilter, cFilterCondition[] ARowFilter)
		{
			return FindMostRecentRow(ASourceView,
									 ABeganDate, ABeganHour,
									 "End_Date", "End_Hour",
									 ANotFilter, ARowFilter);
		}

		/// <summary>
		/// Finds the row with an ended date and hour closest but not after to the specified 
		/// began date and hour and matching the filter specifications.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="ABeganHour">The began hour of the date and hour range to test rows against</param>
		/// <param name="ARowFilter">The row filter to apply against each row.</param>
		/// <returns>The most recent row based on its end date and hour</returns>
		public static DataRowView FindMostRecentRow(DataView ASourceView,
													   DateTime ABeganDate, int ABeganHour,
													   cFilterCondition[] ARowFilter)
		{
			return FindMostRecentRow(ASourceView,
									 ABeganDate, ABeganHour,
									 "End_Date", "End_Hour",
									 false, ARowFilter);
		}

		#endregion

		#region FindMostRecentRow for Active DateTime

		/// <summary>
		/// Finds the row with an ended date closest but not after the specified began date 
		/// and matching the filter specifications.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="ANotFilter">Whether to negate the value returned by the filter</param>
		/// <param name="ARowFilter">The row filter to apply against each row.</param>
		/// <param name="ACompare">The compare operator for filtering the recordset.</param>
		/// <returns>The most recent row based on its end date and hour</returns>
		public static DataRowView FindMostRecentRow(DataView ASourceView,
													   DateTime ABeganDate,
													   string AEndedDateField,
													   bool ANotFilter, cFilterCondition[] ARowFilter,
														eFilterConditionRelativeCompare ACompare)
		{
			DateTime EndedDate;

			DateTime FoundDate = DateTime.MinValue;
			DataRowView FoundRow = null;

			if ((ASourceView != null) && (ASourceView.Table != null))
			{
				DataTable FilterTable = CloneTable(ASourceView.Table);

				if (ASourceView.Count > 0)
				{
					bool Match;

					foreach (DataRowView SourceRow in ASourceView)
					{
						EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateField], DateTypes.END);

						//use compare parameter for match criteria
						cFilterCondition rowFilterCondition = new cFilterCondition(AEndedDateField, ABeganDate, eFilterDataType.DateEnded, ACompare);

						if (RowMatches_Date(SourceRow, rowFilterCondition, DateTypes.END)) // (EndedDate <= ABeganDate)
						{
							Match = true;

							foreach (cFilterCondition FilterPair in ARowFilter)
							{
								if (!RowMatches(SourceRow, FilterPair))
									Match = false;
							}

							if ((Match == !ANotFilter) && (EndedDate > FoundDate))
							{
								FoundDate = EndedDate;
								FoundRow = SourceRow;
							}
						}
					}
				}

				return FoundRow;
			}
			else
				return null;
		}

		/// <summary>
		/// Finds the row with an ended date and hour closest but not after to the specified 
		/// began date and hour and matching the filter specifications.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="AEndedDateField">The ended date field name of the rows</param>
		/// <param name="ARowFilter">The row filter to apply against each row.</param>
		/// <param name="ACompare">The compare operator for filtering the recordset.</param>
		/// <returns>The most recent row based on its end date and hour</returns>
		public static DataRowView FindMostRecentRow(DataView ASourceView,
													   DateTime ABeganDate,
													   string AEndedDateField,
													   cFilterCondition[] ARowFilter,
														eFilterConditionRelativeCompare ACompare)
		{
			return FindMostRecentRow(ASourceView,
									 ABeganDate,
									 AEndedDateField,
									 false, ARowFilter, ACompare);
		}

		/// <summary>
		/// Finds the row with an ended date and hour closest but not after to the specified 
		/// began date and hour and matching the filter specifications.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="ANotFilter">Whether to negate the value returned by the filter</param>
		/// <param name="ARowFilter">The row filter to apply against each row.</param>
		/// <param name="ACompare">The compare operator for filtering the recordset.</param>
		/// <returns>The most recent row based on its end date and hour</returns>
		public static DataRowView FindMostRecentRow(DataView ASourceView,
													   DateTime ABeganDate,
													   bool ANotFilter, cFilterCondition[] ARowFilter,
														eFilterConditionRelativeCompare ACompare)
		{
			return FindMostRecentRow(ASourceView,
									 ABeganDate,
									 "End_Date",
									 ANotFilter, ARowFilter, ACompare);
		}

		/// <summary>
		/// Finds the row with an ended date and hour closest but not after to the specified 
		/// began date and hour and matching the filter specifications.
		/// </summary>
		/// <param name="ASourceView">The data view object in which to count rows</param>
		/// <param name="ABeganDate">The began date of the date and hour range to test rows against</param>
		/// <param name="ARowFilter">The row filter to apply against each row.</param>
		/// <param name="ACompare">The compare operator for filtering the recordset.</param>
		/// <returns>The most recent row based on its end date and hour</returns>
		public static DataRowView FindMostRecentRow(DataView ASourceView,
														DateTime ABeganDate,
														cFilterCondition[] ARowFilter,
														eFilterConditionRelativeCompare ACompare)
		{
			return FindMostRecentRow(ASourceView,
									 ABeganDate,
									 "End_Date",
									 false, ARowFilter, ACompare);
		}

		#endregion

		#region FindEarliestRow

		/// <summary>
		/// Finds the row that contains the current date/hour and with the earliest began date/hour.
		/// If the began date/hour are the same for mote than one row, then uses the row with the
		/// latest end date/hour, picking first record if more than one record has the latest end 
		/// date/hour.
		/// </summary>
		/// <param name="sourceView">The data view object in which to count rows</param>
		/// <param name="currentDate">The began date of the date and hour range to test rows against</param>
		/// <param name="currentHour">The began hour of the date and hour range to test rows against</param>
		/// <param name="beganDateColumnName">The began date field name of the rows</param>
		/// <param name="beganHourColumnName">The began hour field name of the rows</param>
		/// <param name="endedDateColumnName">The ended date field name of the rows</param>
		/// <param name="endedHourColumnName">The ended hour field name of the rows</param>
		/// <param name="notFilter">Whether to negate the value returned by the filter</param>
		/// <param name="rowFilter">The row filter to apply against each row.</param>
		/// <returns>The earliest row based on its begin date and hour</returns>
		public static DataRowView FindEarliestRow(DataView sourceView,
												  DateTime currentDate, int currentHour,
												  string beganDateColumnName, string beganHourColumnName,
												  string endedDateColumnName, string endedHourColumnName,
												  bool notFilter, cFilterCondition[] rowFilter)
		{
			DataRowView result = null;

			if ((sourceView != null) && (sourceView.Table != null))
			{
				DataTable FilterTable = CloneTable(sourceView.Table);

				if (sourceView.Count > 0)
				{
					bool match;

					DateTime resultBeganDate = currentDate;
					int resultBeganHour = currentHour;
					DateTime resultEndedDate = currentDate;
					int resultEndedHour = currentHour;

					foreach (DataRowView sourceRow in sourceView)
					{
						DateTime sourceBeganDate = cDBConvert.ToDate(sourceRow[beganDateColumnName], DateTypes.START);
						int sourceBeganHour = cDBConvert.ToHour(sourceRow[beganHourColumnName], DateTypes.START);
						DateTime sourceEndedDate = cDBConvert.ToDate(sourceRow[endedDateColumnName], DateTypes.END);
						int sourceEndedHour = cDBConvert.ToHour(sourceRow[endedHourColumnName], DateTypes.END);

						if ((((sourceBeganDate < resultBeganDate) ||
							  ((sourceBeganDate == resultBeganDate) && (sourceBeganHour < resultBeganHour))) &&
							 ((sourceEndedDate > currentDate) ||
							  ((sourceEndedDate == currentDate) && (sourceEndedHour >= currentHour)))) ||
							((sourceBeganDate == resultBeganDate) && (sourceBeganHour == resultBeganHour) &&
							 ((sourceEndedDate > resultEndedDate) ||
							  ((sourceEndedDate == resultEndedDate) && (sourceEndedHour > resultEndedHour)))))
						{
							match = true;

							if (rowFilter != null)
							{
								foreach (cFilterCondition filterPair in rowFilter)
								{
									if (!RowMatches(sourceRow, filterPair))
										match = false;
								}
							}

							if ((rowFilter == null) || (match == !notFilter))
							{
								result = sourceRow;
								resultBeganDate = sourceBeganDate;
								resultBeganHour = sourceBeganHour;
								resultEndedDate = sourceEndedDate;
								resultEndedHour = sourceEndedHour;
							}
						}
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Finds the row that contains the current date/hour and with the earliest began date/hour.
		/// If the began date/hour are the same for mote than one row, then uses the row with the
		/// latest end date/hour, picking first record if more than one record has the latest end 
		/// date/hour.
		/// </summary>
		/// <param name="sourceView">The data view object in which to count rows</param>
		/// <param name="currentDate">The began date of the date and hour range to test rows against</param>
		/// <param name="currentHour">The began hour of the date and hour range to test rows against</param>
		/// <param name="notFilter">Whether to negate the value returned by the filter</param>
		/// <param name="rowFilter">The row filter to apply against each row.</param>
		/// <returns>The earliest row based on its begin date and hour</returns>
		public static DataRowView FindEarliestRow(DataView sourceView,
												  DateTime currentDate, int currentHour,
												  bool notFilter, cFilterCondition[] rowFilter)
		{
			DataRowView result;

			result = FindEarliestRow(sourceView,
									 currentDate, currentHour,
									 "BEGIN_DATE", "BEGIN_HOUR",
									 "END_DATE", "END_HOUR",
									 notFilter, rowFilter);

			return result;
		}

		/// <summary>
		/// Finds the row that contains the current date/hour and with the earliest began date/hour.
		/// If the began date/hour are the same for mote than one row, then uses the row with the
		/// latest end date/hour, picking first record if more than one record has the latest end 
		/// date/hour.
		/// </summary>
		/// <param name="sourceView">The data view object in which to count rows</param>
		/// <param name="currentDate">The began date of the date and hour range to test rows against</param>
		/// <param name="currentHour">The began hour of the date and hour range to test rows against</param>
		/// <returns>The earliest row based on its begin date and hour</returns>
		public static DataRowView FindEarliestRow(DataView sourceView,
												  DateTime currentDate, int currentHour)
		{
			DataRowView result;

			result = FindEarliestRow(sourceView,
									 currentDate, currentHour,
									 "BEGIN_DATE", "BEGIN_HOUR",
									 "END_DATE", "END_HOUR",
									 false, null);

			return result;
		}


		/// <summary>
		/// Finds the row that contains the current date/hour and with the earliest began date/hour.
		/// If the began date/hour are the same for mote than one row, then uses the row with the
		/// latest end date/hour, picking first record if more than one record has the latest end 
		/// date/hour.
		/// </summary>
		/// <param name="sourceView">The data view object in which to count rows</param>
		/// <param name="rowFilter">The row filter to apply against each row.</param>
		/// <param name="notFilter">Whether to negate the value returned by the filter</param>
		/// <param name="beganDateColumnName">The began date field name of the rows</param>
		/// <param name="beganHourColumnName">The began hour field name of the rows</param>
		/// <param name="endedDateColumnName">The ended date field name of the rows</param>
		/// <param name="endedHourColumnName">The ended hour field name of the rows</param>
		/// <returns>The earliest row based on its begin date and hour</returns>
		public static DataRowView FindEarliestRow(DataView sourceView,
												  cFilterCondition[] rowFilter, bool notFilter,
												  string beganDateColumnName, string beganHourColumnName,
												  string endedDateColumnName, string endedHourColumnName)
		{
			DataRowView result = null;

			DateTime resultBeganDate = DateTime.MaxValue; int resultBeganHour = 23;
			DateTime resultEndedDate = DateTime.MinValue; int resultEndedHour = 0;

			if ((sourceView != null) && (sourceView.Table != null) && (sourceView.Count > 0))
			{
				bool match;

				foreach (DataRowView sourceRow in sourceView)
				{
					DateTime sourceBeganDate = GetColumnDate(sourceRow, beganDateColumnName, DateTypes.START);
					int sourceBeganHour = GetColumnHour(sourceRow, beganHourColumnName, DateTypes.START);
					DateTime sourceEndedDate = GetColumnDate(sourceRow, endedDateColumnName, DateTypes.END);
					int sourceEndedHour = GetColumnHour(sourceRow, endedHourColumnName, DateTypes.END);

					if ((result == null) ||
						((sourceBeganDate < resultBeganDate) ||
						 ((sourceBeganDate == resultBeganDate) && (sourceBeganHour < resultBeganHour)) ||
						 ((sourceBeganDate == resultBeganDate) && (sourceBeganHour == resultBeganHour) &&
						  ((sourceEndedDate > resultEndedDate) ||
						   ((sourceEndedDate == resultEndedDate) && (sourceEndedHour > resultEndedHour))))))
					{
						match = true;

						if (rowFilter != null)
						{
							foreach (cFilterCondition filterPair in rowFilter)
							{
								if (!RowMatches(sourceRow, filterPair))
									match = false;
							}
						}

						if ((rowFilter == null) || (match == !notFilter))
						{
							result = sourceRow;
							resultBeganDate = sourceBeganDate;
							resultBeganHour = sourceBeganHour;
							resultEndedDate = sourceEndedDate;
							resultEndedHour = sourceEndedHour;
						}
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Finds the row that contains the current date/hour and with the earliest began date/hour.
		/// If the began date/hour are the same for mote than one row, then uses the row with the
		/// latest end date/hour, picking first record if more than one record has the latest end 
		/// date/hour.
		/// </summary>
		/// <param name="sourceView">The data view object in which to count rows</param>
		/// <param name="rowFilter">The row filter to apply against each row.</param>
		/// <param name="notFilter">Whether to negate the value returned by the filter</param>
		/// <returns>The earliest row based on its begin date and hour</returns>
		public static DataRowView FindEarliestRow(DataView sourceView, cFilterCondition[] rowFilter, bool notFilter)
		{
			DataRowView result;

			result = FindEarliestRow(sourceView,
									 rowFilter, notFilter,
									 "BEGIN_DATE", "BEGIN_HOUR",
									 "END_DATE", "END_HOUR");

			return result;
		}

		/// <summary>
		/// Finds the row that contains the current date/hour and with the earliest began date/hour.
		/// If the began date/hour are the same for mote than one row, then uses the row with the
		/// latest end date/hour, picking first record if more than one record has the latest end 
		/// date/hour.
		/// </summary>
		/// <param name="sourceView">The data view object in which to count rows</param>
		/// <param name="rowFilter">The row filter to apply against each row.</param>
		/// <returns>The earliest row based on its begin date and hour</returns>
		public static DataRowView FindEarliestRow(DataView sourceView, cFilterCondition[] rowFilter)
		{
			DataRowView result;

			result = FindEarliestRow(sourceView,
									 rowFilter, false,
									 "BEGIN_DATE", "BEGIN_HOUR",
									 "END_DATE", "END_HOUR");

			return result;
		}

		/// <summary>
		/// Finds the row that contains the current date/hour and with the earliest began date/hour.
		/// If the began date/hour are the same for mote than one row, then uses the row with the
		/// latest end date/hour, picking first record if more than one record has the latest end 
		/// date/hour.
		/// </summary>
		/// <param name="sourceView">The data view object in which to count rows</param>
		/// <returns>The earliest row based on its begin date and hour</returns>
		public static DataRowView FindEarliestRow(DataView sourceView)
		{
			DataRowView result;

			result = FindEarliestRow(sourceView,
									 null, false,
									 "BEGIN_DATE", "BEGIN_HOUR",
									 "END_DATE", "END_HOUR");

			return result;
		}

		/// <summary>
		/// Finds the row using combined Datehour column (dateTime only, no hour columns)
		/// If the began date/hour are the same for more than one row, then uses the row with the
		/// latest end date/hour, picking first record if more than one record has the latest end 
		/// date/hour.
		/// </summary>
		/// <param name="sourceView">The data view object in which to count rows</param>
		/// <param name="rowFilter">The row filter to apply against each row.</param>
		/// <param name="notFilter">Whether to negate the value returned by the filter</param>
		/// <param name="beganDateColumnName">The began date field name of the rows</param>
		/// <param name="endedDateColumnName">The ended date field name of the rows</param>
		/// <returns>The earliest row based on its begin date and hour</returns>
		public static DataRowView FindEarliestRow(DataView sourceView,
												  cFilterCondition[] rowFilter, bool notFilter,
												  string beganDateColumnName,
												  string endedDateColumnName)
		{
			DataRowView result = null;

			DateTime resultBeganDate = DateTime.MaxValue;
			DateTime resultEndedDate = DateTime.MinValue;

			if ((sourceView != null) && (sourceView.Table != null) && (sourceView.Count > 0))
			{
				bool match;

				foreach (DataRowView sourceRow in sourceView)
				{
					DateTime sourceBeganDate = GetColumnDate(sourceRow, beganDateColumnName, DateTypes.START);
					DateTime sourceEndedDate = GetColumnDate(sourceRow, endedDateColumnName, DateTypes.END);

					if ((result == null) ||
						(sourceBeganDate <= resultBeganDate) ||
						(sourceEndedDate > resultEndedDate))
					{
						match = true;

						if (rowFilter != null)
						{
							foreach (cFilterCondition filterPair in rowFilter)
							{
								if (!RowMatches(sourceRow, filterPair))
									match = false;
							}
						}

						if ((rowFilter == null) || (match == !notFilter))
						{
							result = sourceRow;
							resultBeganDate = sourceBeganDate;
							resultEndedDate = sourceEndedDate;
						}
					}
				}
			}

			return result;
		}

        /// <summary>
        /// Finds the row using combined Datehour column (dateTime only, no hour columns)
        /// If the began date/hour are the same for more than one row, then uses the row with the
        /// latest end date/hour, picking first record if more than one record has the latest end 
        /// date/hour.
        /// </summary>
        /// <param name="sourceView">The data view object in which to count rows</param>
        /// <param name="rowFilterList">The or row filters to apply against each row.</param>
        /// <param name="notFilter">Whether to negate the value returned by the filter</param>
        /// <param name="beganDateColumnName">The began date field name of the rows</param>
        /// <param name="endedDateColumnName">The ended date field name of the rows</param>
        /// <returns>The earliest row based on its begin date and hour</returns>
        public static DataRowView FindEarliestRowByDate(DataView sourceView,
                                                        cFilterCondition[][] rowFilterList, 
                                                        bool notFilter,
                                                        string beganDateColumnName,
                                                        string endedDateColumnName)
        {
            DataRowView result = null;

            DateTime resultBeganDate = DateTime.MaxValue;
            DateTime resultEndedDate = DateTime.MinValue;

            if ((sourceView != null) && (sourceView.Table != null) && (sourceView.Count > 0))
            {
                foreach (DataRowView sourceRow in sourceView)
                {
                    DateTime sourceBeganDate = GetColumnDate(sourceRow, beganDateColumnName, DateTypes.START);
                    DateTime sourceEndedDate = GetColumnDate(sourceRow, endedDateColumnName, DateTypes.END);

                    if ((result == null) ||
                        (sourceBeganDate <= resultBeganDate) ||
                        (sourceEndedDate > resultEndedDate))
                    {
                        bool match = false;

                        foreach (cFilterCondition[] FilterPairs in rowFilterList)
                        {
                            bool FilterMatch = true;

                            foreach (cFilterCondition FilterPair in FilterPairs)
                            {
                                if (!RowMatches(sourceRow, FilterPair))
                                    FilterMatch = false;
                            }

                            if (FilterMatch) match = true;
                        }

                        if (match == !notFilter)
                        {
                            result = sourceRow;
                            resultBeganDate = sourceBeganDate;
                            resultEndedDate = sourceEndedDate;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Finds the row using combined Datehour column (dateTime only, no hour columns)
        /// If the began date/hour are the same for more than one row, then uses the row with the
        /// latest end date/hour, picking first record if more than one record has the latest end 
        /// date/hour.
        /// </summary>
        /// <param name="sourceView">The data view object in which to count rows</param>
        /// <param name="beganDateColumnName">The began date field name of the rows</param>
        /// <param name="endedDateColumnName">The ended date field name of the rows</param>
        /// <returns>The earliest row based on its begin date and hour</returns>
        public static DataRowView FindEarliestRow(DataView sourceView,
												  string beganDateColumnName,
												  string endedDateColumnName)
		{
			DataRowView result = null;

			DateTime resultBeganDate = DateTime.MaxValue;
			DateTime resultEndedDate = DateTime.MinValue;

			if ((sourceView != null) && (sourceView.Table != null) && (sourceView.Count > 0))
			{
				foreach (DataRowView sourceRow in sourceView)
				{
					DateTime sourceBeganDate = GetColumnDate(sourceRow, beganDateColumnName, DateTypes.START);
					DateTime sourceEndedDate = GetColumnDate(sourceRow, endedDateColumnName, DateTypes.END);

					if ((result == null) ||
						(sourceBeganDate <= resultBeganDate) ||
						(sourceEndedDate > resultEndedDate))
					{
						result = sourceRow;
						resultBeganDate = sourceBeganDate;
						resultEndedDate = sourceEndedDate;
					}
				}
			}

			return result;
		}

		#endregion

		#endregion

		#region Utility Methods

		private static DataTable CloneTable(DataTable ASourceTable)
		{
			DataTable FilterTable;

			try
			{
				DataColumn FilterColumn;

				FilterTable = new DataTable(ASourceTable.TableName);

				foreach (DataColumn SourceColumn in ASourceTable.Columns)
				{
					FilterColumn = new DataColumn();

					FilterColumn.ColumnName = SourceColumn.ColumnName;
					FilterColumn.DataType = SourceColumn.DataType;
					FilterColumn.MaxLength = SourceColumn.MaxLength;
					FilterColumn.Unique = SourceColumn.Unique;

					FilterTable.Columns.Add(FilterColumn);
				}
			}
			catch
			{
				FilterTable = null;
			}

			return FilterTable;
		}

		/// <summary>
		/// Attempts to return the date value from the data row view and column passed.
		/// Returns DBNULL if it cannot return the value.
		/// </summary>
		/// <param name="rowView">The row containing the value.</param>
		/// <param name="columnName">The column containing the value.</param>
		/// <param name="dateType">The Date Type of the column.</param>
		/// <returns></returns>
		private static DateTime GetColumnDate(DataRowView rowView, string columnName, DateTypes dateType)
		{
			DateTime result;

			DateTime defaultValue;
			{
				if (dateType == DateTypes.END)
					defaultValue = DateTime.MaxValue;
				else
					defaultValue = DateTime.MinValue;
			}

			if ((rowView == null) || string.IsNullOrEmpty(columnName))
			{
				result = defaultValue;
			}
			else if ((rowView.Row == null) ||
					 (rowView.Row.Table == null) ||
					 !rowView.Row.Table.Columns.Contains(columnName))
			{
				result = defaultValue;
			}
			else if (!DateTime.TryParse(rowView[columnName].AsString(), out result))
			{
				result = defaultValue;
			}

			return result;
		}

		/// <summary>
		/// Attempts to return the hour value from the data row view and column passed.
		/// Returns DBNULL if it cannot return the value.
		/// </summary>
		/// <param name="rowView">The row containing the value.</param>
		/// <param name="columnName">The column containing the value.</param>
		/// <param name="dateType">The Date Type of the column.</param>
		/// <returns></returns>
		private static int GetColumnHour(DataRowView rowView, string columnName, DateTypes dateType)
		{
			int result;

			int defaultValue;
			{
				if (dateType == DateTypes.END)
					defaultValue = int.MaxValue;
				else
					defaultValue = int.MinValue;
			}

			if ((rowView == null) || string.IsNullOrEmpty(columnName))
			{
				result = defaultValue;
			}
			else if ((rowView.Row == null) ||
					 (rowView.Row.Table == null) ||
					 !rowView.Row.Table.Columns.Contains(columnName))
			{
				result = defaultValue;
			}
			else if (!int.TryParse(rowView[columnName].AsString(), out result))
			{
				result = defaultValue;
			}

			return result;
		}

		#endregion

	}

}
