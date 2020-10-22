using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.Parameters
{
  /// <summary>
  /// The root class for all parameters.  The class is abstract and cannot be instantiated.
  /// </summary>
  public abstract class cCheckParameter
  {

    #region Protected Constructors

    /// <summary>
    /// Instantiates a cCheckParameter class and sets the name and data type properties
    /// of the parameter
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="AParameterDataType">The data type of the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    protected cCheckParameter(int? AParameterKey, string AParameterName, eParameterDataType AParameterDataType, 
                              cCheckParameters ACheckParameters)
    {
      FCheckParameters = ACheckParameters;
      FName = AParameterName.Trim().ToUpper();
      FDataType = AParameterDataType;
    }

    #endregion


    #region Protected Fields

    /// <summary>
    /// The parent check parameters object for this parameter.
    /// </summary>
    protected cCheckParameters FCheckParameters;

    #endregion


    #region Public Get Properties

    #region Property Fields

    /// <summary>
    /// The property field for the data type of the parameter
    /// </summary>
    protected eParameterDataType FDataType = eParameterDataType.Object;

    /// <summary>
    /// The property field for the rule check id of the check that initialized the parameter value.
    /// </summary>
    protected cParameterizedCheck FInitParameterizedCheck = null;

    /// <summary>
    /// The property field that indicates whether the parameter is an array.
    /// </summary>
    protected bool FIsArray = false;

    /// <summary>
    /// Indicates whether the parameter has been set.
    /// </summary>
    public bool FIsSet = false;

    /// <summary>
    /// The property field for the parameter key.
    /// </summary>
    protected int FKey = int.MinValue;

    /// <summary>
    /// The property field that indicates the action that produced the last error.
    /// </summary>
    protected string FLastErrorAction = "";

    /// <summary>
    /// The property Field that holds the last error message.
    /// </summary>
    protected string FLastErrorMessage = "";

    /// <summary>
    /// The property field for the parameter name.
    /// </summary>
    protected string FName = "";

    /// <summary>
    /// The property field for the category owner of the parameter.
    /// </summary>
    protected cCheckCategory FOwner = null;

    #endregion


    /// <summary>
    /// The property for the data type of the parameter
    /// </summary>
    public eParameterDataType DataType { get { return FDataType; } }

    /// <summary>
    /// The property for the rule check id of the check that initialized the parameter value.
    /// </summary>
    public cParameterizedCheck InitParameterizedCheck { get { return FInitParameterizedCheck; } }

    /// <summary>
    /// The property that indicates whether the parameter is an array.
    /// </summary>
    public bool IsArray { get { return FIsArray; } }

    /// <summary>
    /// Indicates whether the parameter is null.
    /// </summary>
    public bool IsNull { get { return !IsSet || (LegacyValue == null); } }

    /// <summary>
    /// Indicates whether the parameter has been set.
    /// </summary>
    public bool IsSet { get { return FIsSet; } }

    /// <summary>
    /// The property field for the parameter key.
    /// </summary>
    public int Key { get { return FKey; } }

    /// <summary>
    /// The property that indicates the action that produced the last error.
    /// </summary>
    public string LastErrorAction { get { return FLastErrorAction; } }

    /// <summary>
    /// The property that holds the last error message.
    /// </summary>
    public string LastErrorMessage { get { return FLastErrorMessage; } }

    /// <summary>
    /// The property for the parameter name.
    /// </summary>
    public string Name { get { return FName; } }

    /// <summary>
    /// The property for the category owner of the parameter.
    /// </summary>
    public cCheckCategory Owner { get { return FOwner; } }

    #endregion


    #region Public Abstract Get Properties

    /// <summary>
    /// Returns the value as an object but with any nulls or dbnulls defaulted.
    /// </summary>
    public abstract object NullDefaultedObject { get; }

    /// <summary>
    /// Returns the value as an object.
    /// </summary>
    public abstract object LegacyValue { get; }

    #endregion


    #region Public Methods

    /// <summary>
    /// Check to see if the owner is an ancestor of the passed check category.
    /// </summary>
    /// <param name="ACategory">The potential descendant to check.</param>
    /// <returns>True if the owner is an ancestor of the check category.</returns>
    public bool OwnerIsOrAncestorOf(cCheckCategory ACategory)
    {
      bool Result;

      if (FOwner == null)
        Result = true;
      else if (ACategory == null)
        Result = false;
      else if (ACategory.CategoryCd == FOwner.CategoryCd)
        Result = true;
      else
      {
        Result = false;

        cCheckCategory Ancestor = ACategory.ParentCategory;

        while (!Result && (Ancestor != null))
        {
          if (Ancestor.CategoryCd == FOwner.CategoryCd)
            Result = true;
          else
            Ancestor = Ancestor.ParentCategory;
        }
      }

      return Result;
    }

    /// <summary>
    /// Clears the value, initializing rule check id and owner
    /// as well as the last error action and message, if they are set.
    /// </summary>
    public void Reset()
    {
      ResetValue();

      FOwner = null;
      FInitParameterizedCheck = null;
      FIsSet = false;

      FLastErrorAction = "";
      FLastErrorMessage = "";
    }

    /// <summary>
    /// Returns the current value of the parameter as a string.
    /// </summary>
    /// <returns>The current value of the parameter as a string.</returns>
    public string ValueAsString()
    {
      return cDBConvert.ToString(ValueAsObject());
    }

    #endregion


    #region Public Static Methods

    /// <summary>
    /// Appends the Check Parameter if it is not already in the list.
    /// </summary>
    /// <param name="ACheckParameter">The parameter to append</param>
    /// <param name="ACheckParameterList">The list into which to append</param>
    /// <param name="ACheckParameterCnt">The current count of parameters in the list</param>
    public static void FindOrAddCheckParameter(cCheckParameter ACheckParameter,
                                               ref cCheckParameter[] ACheckParameterList,
                                               ref int ACheckParameterCnt)
    {
      bool Found = false;

      foreach (cCheckParameter CheckParameter in ACheckParameterList)
      {
        if ((CheckParameter != null) && 
            (CheckParameter.Name.ToUpper().Trim() == ACheckParameter.Name.ToUpper().Trim()))
        {
          Found = true;
        }
      }

      if (!Found)
      {
        ACheckParameterList[ACheckParameterCnt] = ACheckParameter;
        ACheckParameterCnt += 1;
      }
    }

    /// <summary>
    /// Appends the check parameters in the source that do not exist in the target list
    /// into the target list.
    /// </summary>
    /// <param name="ASourceParameterList">The source list of parameters to append</param>
    /// <param name="ATargetParameterList">The target list into which to append parameters</param>
    /// <param name="ATargetParameterCnt">The current count of parameters in the target list</param>
    public static void MergeCheckParameterList(cCheckParameter[] ASourceParameterList,
                                               ref cCheckParameter[] ATargetParameterList,
                                               ref int ATargetParameterCnt)
    {
      foreach (cCheckParameter CheckParameter in ASourceParameterList)
      {
        FindOrAddCheckParameter(CheckParameter, ref ATargetParameterList, ref ATargetParameterCnt);
      }
    }

    /// <summary>
    /// Returns a list of check parameters with nulls removed and no empty cells.
    /// </summary>
    /// <param name="ASourceParameters"></param>
    /// <returns></returns>
    public static cCheckParameter[] NormalizeParameterList(cCheckParameter[] ASourceParameters)
    {
      if (ASourceParameters != null)
      {
        // Get the number of non null parameters in the source parameter list
        int ParameterCnt = 0;

        foreach (cCheckParameter CheckParameter in ASourceParameters)
        {
          if (CheckParameter != null) ParameterCnt += 1;
        }


        //Normalize list
        cCheckParameter[] Result;

        if (ParameterCnt > 0)
        {
          Result = new cCheckParameter[ParameterCnt];

          int ParameterDex = 0;

          foreach (cCheckParameter CheckParameter in ASourceParameters)
          {
            if (CheckParameter != null)
            {
              Result[ParameterDex] = CheckParameter;
              ParameterDex += 1;
            }
          }
        }
        else
          Result = new cCheckParameter[0];

        return Result;
      }
      else
        return new cCheckParameter[0];
    }

    #endregion


    #region Public Override Methods

    /// <summary>
    /// Displays information that identifies the parameter.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return string.Format("{0}: {2}{1}",
                           FName,
                           FDataType.ToString(),
                           (IsArray ? "Array of " : ""));
    }

    #endregion


    #region Public Abstract Methods

    /// <summary>
    /// Returns the 'Type' default value for null or dbnull of the parameter.
    /// </summary>
    /// <returns>The 'Type'</returns>
    public abstract object GetDefaultObjectForNull();

    /// <summary>
    /// Returns the current value of the parameter as an object.
    /// </summary>
    /// <returns>The current value of the parameter as an object.</returns>
    public abstract object ValueAsObject();

    #endregion


    #region Protected Methods

    /// <summary>
    /// Determines whether an update by te specified category can proceed.
    /// </summary>
    /// <param name="ACategory">The category to check.</param>
    /// <returns>True if the update can proceed.</returns>
    protected bool UpdateValueCheck(cCheckCategory ACategory)
    {
      bool Result;

      if (!FIsSet)
      {
        if (FOwner != null)
          System.Diagnostics.Debug.WriteLine(string.Format("Cannot update uninitialized parameter: Category - {0}, Parameter - {1}", FOwner.CategoryCd, this.Name));
        else
          System.Diagnostics.Debug.WriteLine(string.Format("Cannot update uninitialized parameter: For Process, Parameter - {0}", this.Name));

        Result = false;
      }
      else if (!OwnerIsOrAncestorOf(ACategory))
      {
        if (FOwner != null)
          System.Diagnostics.Debug.WriteLine(string.Format("Cannot update parameter initialized by another category: Category - {0}, Parameter - {1}", FOwner.CategoryCd, this.Name));
        else
          System.Diagnostics.Debug.WriteLine(string.Format("Cannot update parameter initialized by another category: For Process, Parameter - {0}", this.Name));

        Result = false;
      }
      else
      {
        Result = true;
      }

      return Result;
    }

    /// <summary>
    /// Inforces the rules for duplicate parameter setting.
    /// </summary>
    /// <param name="AOwner">The current category setting the parameter.</param>
    /// <returns>Returns true if the parameter can be set, false if it can not.</returns>
    protected bool SetValueCheckAndPrep(cCheckCategory AOwner)
    {
      bool Result;

      cParameterizedCheck OwnerParameterizedCheck = (AOwner != null) ? AOwner.RunningRuleCheck : null;

      if (!FIsSet || cParameterizedCheck.AreSameCheck(OwnerParameterizedCheck, FInitParameterizedCheck))
      {
        FInitParameterizedCheck = OwnerParameterizedCheck;
        FOwner = AOwner;
        FIsSet = true;

        Result = true;
      }
      else if (OwnerIsOrAncestorOf(AOwner))
      {
        Result = true;
      }
      else
      {
        FInitParameterizedCheck = OwnerParameterizedCheck;
        FOwner = AOwner;
        FIsSet = true;

        Result = true;
        
        /*
        if (AOwner != null)
          System.Diagnostics.Debug.WriteLine(string.Format("Duplicate Parameter Value Init: Category - {0}, Parameter - {1}", AOwner.CategoryCd, this.Name));
        else
          System.Diagnostics.Debug.WriteLine(string.Format("Duplicate Parameter Value Init: For Process, Parameter - {0}", this.Name));

        Result = false;
        */
      }

      return Result;
    }


    #region Commented Out Code from cCheckEngine.cCategory

    //// From Category with Accum and Array Attr
    //if (mCheckParameters.ContainsKey(AParameterName))
    //{
    //  ((cLegacyCheckParameter)CheckParameters[AParameterName]).ParameterValue = ParameterValue;
    //  mProcess.SetCheckParameter(AParameterName, ParameterValue, ParameterType, RunningRuleCheck, IsAccumulator, IsArray);
    //}
    //else if (!SetParentCheckParameter(AParameterName, ParameterValue, ParameterType, IsAccumulator, IsArray))
    //{
    //  if (mProcess.CheckParametersOld.ContainsKey(AParameterName))
    //  {
    //    if (mProcess.GetCheckParameter(AParameterName).SameUnderlyingInitCheck(RunningRuleCheck))
    //    {
    //      mCheckParameters.Add(AParameterName, new cLegacyCheckParameter(AParameterName, ParameterValue, ParameterType, IsAccumulator, IsArray));
    //      mProcess.SetCheckParameter(AParameterName, ParameterValue, ParameterType, RunningRuleCheck, IsAccumulator, IsArray);
    //    }
    //    else
    //      System.Diagnostics.Debug.WriteLine(string.Format("Duplicate Parameter Set: Category - {0}, Parameter - {1}", this.CategoryCd, ParameterName));
    //  }
    //  else
    //  {
    //    mCheckParameters.Add(AParameterName, new cLegacyCheckParameter(AParameterName, ParameterValue, ParameterType, IsAccumulator, IsArray));
    //    mProcess.SetCheckParameter(AParameterName, ParameterValue, ParameterType, RunningRuleCheck, IsAccumulator, IsArray);
    //  }
    //}


    //// From Category
    //if (mCheckParameters.ContainsKey(ParameterName))
    //{
    //  ((cLegacyCheckParameter)CheckParameters[ParameterName]).ParameterValue = ParameterValue;
    //  mProcess.SetCheckParameter(ParameterName, ParameterValue, ParameterType, RunningRuleCheck);
    //}
    //else if (!SetParentCheckParameter(ParameterName, ParameterValue, ParameterType))
    //{
    //  if (mProcess.CheckParametersOld.ContainsKey(ParameterName))
    //  {
    //    if (mProcess.GetCheckParameter(ParameterName).SameUnderlyingInitCheck(RunningRuleCheck))
    //    {
    //      mCheckParameters.Add(ParameterName, new cLegacyCheckParameter(ParameterName, ParameterValue, ParameterType));
    //      mProcess.SetCheckParameter(ParameterName, ParameterValue, ParameterType, RunningRuleCheck);
    //    }
    //    else
    //      System.Diagnostics.Debug.WriteLine(string.Format("Duplicate Parameter Set: Category - {0}, Parameter - {1}", this.CategoryCd, ParameterName));
    //  }
    //  else
    //  {
    //    mCheckParameters.Add(ParameterName, new cLegacyCheckParameter(ParameterName, ParameterValue, ParameterType));
    //    mProcess.SetCheckParameter(ParameterName, ParameterValue, ParameterType, RunningRuleCheck);
    //  }
    //}

    //public bool SetParentCheckParameter(string ParameterName, object ParameterValue, eParameterDataType ParameterType,
    //                                    bool IsAccumulator, bool IsArray)
    //{
    //  ParameterName = ParameterName.ToUpper().Trim();

    //  if ((ParameterType == eParameterDataType.DataView) || (ParameterType == eParameterDataType.DataRowView))
    //    return false;
    //  else if (FParentCategory == null)
    //    return false;
    //  else if (FParentCategory.CheckParameters.ContainsKey(ParameterName))
    //  {
    //    ((cLegacyCheckParameter)FParentCategory.CheckParameters[ParameterName]).ParameterValue = ParameterValue;
    //    FParentCategory.Process.SetCheckParameter(ParameterName, ParameterValue, ParameterType, IsAccumulator, IsArray);
    //    return true;
    //  }
    //  else
    //  {
    //    return FParentCategory.SetParentCheckParameter(ParameterName, ParameterValue, ParameterType);
    //  }
    //}

    //public bool SetParentCheckParameter(string ParameterName, object ParameterValue, eParameterDataType ParameterType)
    //{
    //  ParameterName = ParameterName.ToUpper().Trim();

    //  //nixed to allow resetting of DataRow parameters used in an updatable manner
    //  //if ((ParameterType == ParameterTypes.DATAVW) || (ParameterType == ParameterTypes.DATAROW)) 
    //  if (ParameterType == eParameterDataType.DataView)
    //    return false;
    //  else if (FParentCategory == null)
    //    return false;
    //  else if (FParentCategory.CheckParameters.ContainsKey(ParameterName))
    //  {
    //    ((cLegacyCheckParameter)FParentCategory.CheckParameters[ParameterName]).ParameterValue = ParameterValue;
    //    FParentCategory.Process.SetCheckParameter(ParameterName, ParameterValue, ParameterType);
    //    return true;
    //  }
    //  else
    //  {
    //    return FParentCategory.SetParentCheckParameter(ParameterName, ParameterValue, ParameterType);
    //  }
    //}

    //private void SetArrayParameterDo(string AParameterName, object AValue)
    //{
    //  AParameterName = AParameterName.ToUpper().Trim();

    //  if (mCheckParameters.ContainsKey(AParameterName))
    //  {
    //    cLegacyCheckParameter Parameter = (cLegacyCheckParameter)CheckParameters[AParameterName];
    //    ((cLegacyCheckParameter)CheckParameters[AParameterName]).ParameterValue = AValue;
    //    mProcess.SetCheckParameter(AParameterName, AValue, Parameter.ParameterType);
    //  }
    //  else if ((FParentCategory != null) && (FParentCategory.CheckParameters.ContainsKey(AParameterName)))
    //  {
    //    cLegacyCheckParameter Parameter = (cLegacyCheckParameter)FParentCategory.CheckParameters[AParameterName];
    //    ((cLegacyCheckParameter)FParentCategory.CheckParameters[AParameterName]).ParameterValue = AValue;
    //    FParentCategory.Process.SetCheckParameter(AParameterName, AValue, Parameter.ParameterType);
    //  }
    //  else if (mProcess.CheckParametersOld.ContainsKey(AParameterName))
    //  {
    //    cLegacyCheckParameter Parameter = (cLegacyCheckParameter)mProcess.CheckParametersOld[AParameterName];
    //    mProcess.SetCheckParameter(AParameterName, AValue, Parameter.ParameterType);
    //  }
    //  else
    //  {
    //    System.Diagnostics.Debug.WriteLine(string.Format("Aggregate Parameter does not exist: Category - {0}, Parameter - {1}", this.CategoryCd, AParameterName));
    //  }
    //}

    #endregion

    #endregion


    #region Protected Abstract Methods

    /// <summary>
    /// Resets the value of the parameter to its uninitialized value.
    /// </summary>
    protected abstract void ResetValue();

    #endregion

  }
}
