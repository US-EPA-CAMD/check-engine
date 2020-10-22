using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTest.UtilityClasses
{

  public class GeneralUtilites
  {

    #region Cross Reference Methods

    public static string MethodParametersForProgramParameter(string programParameterCd)
    {
      string result;

      switch (programParameterCd)
      {
        case "CO2": result = "CO2,CO2M"; break;
        case "H2O": result = "H2O"; break;
        case "HCL": result = "HCLRE,HCLRH,SO2RE,SO2RH"; break;
        case "HF": result = "HFRE,HFRH"; break;
        case "HG": result = "HGRE,HGRH"; break;
        case "HI": result = "HI,HIT"; break;
        case "NOX": result = "NOX,NOXM,NOXR"; break;
        case "NOXR": result = "NOXM,NOXR"; break;
        case "OP": result = "OP"; break;
        case "SO2": result = "SO2,SO2M"; break;
        default: result = null; break;
      }

      return result;
    }

    #endregion

  }

}
