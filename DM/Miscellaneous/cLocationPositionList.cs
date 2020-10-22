using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.DM.Definitions;
using ECMPS.DM.Utilities;

namespace ECMPS.DM.Miscellaneous
{
  /// <summary>
  /// Implemnts a Reduce By Spec, basically a list of locations.
  /// 
  /// Needed to get around not being able to create a int[,][]
  /// that implements the way I need it to.
  /// </summary>
  public class cLocationPositionList
  {

    #region Public Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="locationPositionList">List of reduce by locaiton positions</param>
    public cLocationPositionList(int[] locationPositionList)
    {
      LocationPositionList = locationPositionList;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public cLocationPositionList()
    {
      LocationPositionList = new int[0];
    }

    #endregion


    #region Public Properties

    /// <summary>
    /// The list of reduce by location positions.
    /// </summary>
    public int[] LocationPositionList { get; private set; }

    #endregion

  }
}
