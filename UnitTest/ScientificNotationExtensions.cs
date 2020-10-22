using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ECMPS.Definitions.Extensions;

namespace UnitTest.Miscellanea
{
  [TestClass]
  public class ScientificNotationExtensions
  {
    [TestMethod]
    public void DecimalToScientificNotation()
    {
      Assert.AreEqual("1.2E-1", 0.12345m.DecimaltoScientificNotation(2), "[0.12345]: with 2 SD");
      Assert.AreEqual("1.23E-1", 0.12345m.DecimaltoScientificNotation(3), "[0.12345]: with 3 SD");
      Assert.AreEqual("1.235E-1", 0.12345m.DecimaltoScientificNotation(4), "[0.12345]: with 4 SD");
      Assert.AreEqual("1.23E-2", 0.012345m.DecimaltoScientificNotation(), "[0.012345]: default SD");

      Assert.AreEqual("1.23E0", 1.23456m.DecimaltoScientificNotation(3), "[1.23456]: with 3 SD");
      Assert.AreEqual("1.23E1", 12.3456m.DecimaltoScientificNotation(3), "[12.3456]: with 3 SD");

      Assert.AreEqual("1.23E-1", cExtensions.DecimaltoScientificNotation(0.12345m, 3), "[0.12345]: direct with 3 SD");
      Assert.AreEqual("1.23E-2", cExtensions.DecimaltoScientificNotation(0.012345m), "[0.012345]: direct with default SD");
    }

    [TestMethod]
    public void ScientificNotationSignificantDigits()
    {
      Assert.AreEqual(3, "1.23E-4".ScientificNotationSignificantDigits(), "[1.23E-4]");
      Assert.AreEqual(3, "-1.23E4".ScientificNotationSignificantDigits(), "[-1.23E4]");
      Assert.AreEqual(3, "+1.23E4".ScientificNotationSignificantDigits(), "[-1.23E4]");
      Assert.AreEqual(3, "9.99E-4".ScientificNotationSignificantDigits(), "[9.99E-4]");
      Assert.AreEqual(3, "-9.99E-4".ScientificNotationSignificantDigits(), "[-9.99E-4]");
      Assert.AreEqual(3, "+9.99E-4".ScientificNotationSignificantDigits(), "[-9.99E-4]");
      Assert.AreEqual(3, " 1.23E-4".ScientificNotationSignificantDigits(), "[ 1.23E-4]");
      Assert.AreEqual(3, "1.23E-4 ".ScientificNotationSignificantDigits(), "[1.23E-4 ]");
      Assert.AreEqual(2, "1.2E-4".ScientificNotationSignificantDigits(), "[1.2E-4]");
      Assert.AreEqual(3, "1.23".ScientificNotationSignificantDigits(), "[1.23]");
      Assert.AreEqual(4, "10.00E-4".ScientificNotationSignificantDigits(), "[10.00E-4]");
      Assert.AreEqual(4, "-10.00E-4".ScientificNotationSignificantDigits(), "[-10.00E-4]");
      Assert.AreEqual(4, "+10.00E-4".ScientificNotationSignificantDigits(), "[-10.00E-4]");

      Assert.AreEqual(null, "1.23 E-4".ScientificNotationSignificantDigits(), "[1.23 E-4]");
      Assert.AreEqual(null, "1.23E -4".ScientificNotationSignificantDigits(), "[1.23E -4]");
      Assert.AreEqual(null, "1.23E-".ScientificNotationSignificantDigits(), "[1.23E-]");
      Assert.AreEqual(null, "1.23E".ScientificNotationSignificantDigits(), "[1.23E]");
      Assert.AreEqual(null, "E-4".ScientificNotationSignificantDigits(), "[E-4]");
      Assert.AreEqual(null, "E".ScientificNotationSignificantDigits(), "[E]");
      Assert.AreEqual(null, "1.23-4".ScientificNotationSignificantDigits(), "[1.23-4]");

      Assert.AreEqual(3, cExtensions.ScientificNotationSignificantDigits("1.23E-4"), "[1.23E-4]");
    }
    
    [TestMethod]
    public void ScientificNotationToDecimal()
    {
      Assert.AreEqual(0.000123m, "1.23E-4".ScientificNotationtoDecimal(), "[1.23E-4]");
      Assert.AreEqual(-12300m, "-1.23E4".ScientificNotationtoDecimal(), "[-1.23E4]");
      Assert.AreEqual(0.000999m, "9.99E-4".ScientificNotationtoDecimal(), "[9.99E-4]");
      Assert.AreEqual(-0.000999m, "-9.99E-4".ScientificNotationtoDecimal(), "[-9.99E-4]");
      Assert.AreEqual(0.000123m, " 1.23E-4".ScientificNotationtoDecimal(), "[ 1.23E-4]");
      Assert.AreEqual(0.000123m, "1.23E-4 ".ScientificNotationtoDecimal(), "[1.23E-4 ]");
      Assert.AreEqual(0.00012m, "1.2E-4".ScientificNotationtoDecimal(), "[1.2E-4]");
      Assert.AreEqual(1.23m, "1.23".ScientificNotationtoDecimal(), "[1.23]");
      Assert.AreEqual(0.001m, "10.00E-4".ScientificNotationtoDecimal(), "[10.00E-4]");
      Assert.AreEqual(-0.001m, "-10.00E-4".ScientificNotationtoDecimal(), "[-10.00E-4]");

      Assert.AreEqual(0m, "1.23 E-4".ScientificNotationtoDecimal(), "[1.23 E-4]");
      Assert.AreEqual(0m, "1.23E -4".ScientificNotationtoDecimal(), "[1.23E -4]");
      Assert.AreEqual(0m, "1.23E-".ScientificNotationtoDecimal(), "[1.23E-]");
      Assert.AreEqual(0m, "1.23E".ScientificNotationtoDecimal(), "[1.23E]");
      Assert.AreEqual(0m, "E-4".ScientificNotationtoDecimal(), "[E-4]");
      Assert.AreEqual(0m, "E".ScientificNotationtoDecimal(), "[E]");
      Assert.AreEqual(0m, "1.23-4".ScientificNotationtoDecimal(), "[1.23-4]");

      Assert.AreEqual(0.000123m, cExtensions.ScientificNotationtoDecimal("1.23E-4"), "[1.23E-4]");
    }

    [TestMethod]
    public void SignificantDigitNotationValid()
    {
      Assert.AreEqual(true, "1.2E-3".SignificantDigitNotationValid(2), "[1.2E-3] with 2 SD");
      Assert.AreEqual(true, "1.23E-4".SignificantDigitNotationValid(3), "[1.23E-4] with 3 SD");
      Assert.AreEqual(true, "1.234E-5".SignificantDigitNotationValid(4), "[1.234E-5] with 4 SD");

      Assert.AreEqual(true, "1.23E-4".SignificantDigitNotationValid(), "[1.23E-4] default SD");
      Assert.AreEqual(true, "-1.23E4".SignificantDigitNotationValid(), "[-1.23E4] default SD");
      Assert.AreEqual(true, "9.99E-4".SignificantDigitNotationValid(), "[9.99E-4] default SD");
      Assert.AreEqual(true, "-9.99E-4".SignificantDigitNotationValid(), "[-9.99E-4] default SD");

      Assert.AreEqual(false, " 1.23E-4".SignificantDigitNotationValid(), "[ 1.23E-4] default SD");
      Assert.AreEqual(false, "1.23E-4 ".SignificantDigitNotationValid(), "[1.23E-4 ] default SD");
      Assert.AreEqual(false, "1.23 E-4".SignificantDigitNotationValid(), "[1.23 E-4] default SD");
      Assert.AreEqual(false, "1.23E -4".SignificantDigitNotationValid(), "[1.23E -4] default SD");
      Assert.AreEqual(false, "1.2E-4".SignificantDigitNotationValid(), "[1.2E-4] default SD");
      Assert.AreEqual(false, "1.23E-".SignificantDigitNotationValid(), "[1.23E-] default SD");
      Assert.AreEqual(false, "1.23E".SignificantDigitNotationValid(), "[1.23E] default SD");
      Assert.AreEqual(false, "E-4".SignificantDigitNotationValid(), "[E-4] default SD");
      Assert.AreEqual(false, "E".SignificantDigitNotationValid(), "[E] default SD");
      Assert.AreEqual(false, "1.23-4".SignificantDigitNotationValid(), "[1.23-4] default SD");
      Assert.AreEqual(false, "1.23".SignificantDigitNotationValid(), "[1.23] default SD");
      Assert.AreEqual(false, "10.00E-4".SignificantDigitNotationValid(), "[10.00E-4] default SD");
      Assert.AreEqual(false, "-10.00E-4".SignificantDigitNotationValid(), "[-10.00E-4] default SD");

      Assert.AreEqual(true, cExtensions.SignificantDigitNotationValid("1.2E-3", 2), "[1.2E-3]: direct with 2 SD");
      Assert.AreEqual(true, cExtensions.SignificantDigitNotationValid("1.23E-4", 3), "[1.23E-4]: direct with 3 SD");
      Assert.AreEqual(true, cExtensions.SignificantDigitNotationValid("1.234E-5", 4), "[1.234E-5]: direct with 4 SD");

      Assert.AreEqual(true, cExtensions.SignificantDigitNotationValid("1.23E-4"), "[1.23E-4]: direct default SD");
      Assert.AreEqual(false, cExtensions.SignificantDigitNotationValid("1.23"), "[1.23]: direct default SD");
    }
  }
}
