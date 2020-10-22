using System;
using System.Collections.Generic;
using System.Text;

namespace ECMPS.Checks.Parameters
{
  /// <summary>
  /// The base for all Category objects
  /// </summary>
  public class cCheckCategory
  {

    #region Protected Constructors

    /// <summary>
    /// Instantiates a cCheckCategory object
    /// </summary>
    /// <param name="ACheckProcess">The process object associated with the check category object</param>
    /// <param name="ACategoryCd">The cateogry code associated with the check category object</param>
    protected cCheckCategory(cCheckProcess ACheckProcess, string ACategoryCd)
    {
      FCheckProcess = ACheckProcess;
      FCategoryCd = ACategoryCd;
      FParentCategory = null;
    }

    /// <summary>
    /// Instantiates a cCheckCategory object
    /// </summary>
    /// <param name="ACheckProcess">The process object associated with the check category object.</param>
    /// <param name="ACategoryCd">The cateogry code associated with the check category object.</param>
    /// <param name="AParentCategory">The parent category of this category.</param>
    protected cCheckCategory(cCheckProcess ACheckProcess, string ACategoryCd, cCheckCategory AParentCategory)
    {
      FCheckProcess = ACheckProcess;
      FCategoryCd = ACategoryCd;
      FParentCategory = AParentCategory;
    }


    /// <summary>
    /// Instantiates a cCheckCategory object primarily for unit testing purposes.
    /// </summary>
    protected cCheckCategory()
    {
    }

    #endregion


    #region Public Properties

    #region Property Fields

    /// <summary>
    /// The category code of the category object instance.
    /// </summary>
    protected string FCategoryCd;

    /// <summary>
    /// Check process in which the category is running.
    /// </summary>
    protected cCheckProcess FCheckProcess;

    /// <summary>
    /// The parent category of this category.
    /// </summary>
    protected cCheckCategory FParentCategory;

    /// <summary>
    /// Indicates which rule check is being processed by a check
    /// </summary>
    protected cParameterizedCheck FRunningRuleCheck = null;

    #endregion

    /// <summary>
    /// The category code of the category object instance.
    /// </summary>
    public string CategoryCd { get { return FCategoryCd; } }

    /// <summary>
    /// The check process object parent of the category object instance.
    /// </summary>
    public cCheckProcess CheckProcess { get { return FCheckProcess; } }

    /// <summary>
    /// The parent category of this category.
    /// </summary>
    public cCheckCategory ParentCategory
    {
      get { return FParentCategory; }
    }

    /// <summary>
    /// Indicates which rule check is being processed by a check
    /// </summary>
    public cParameterizedCheck RunningRuleCheck { get { return FRunningRuleCheck; } }

    #endregion

  }
}
