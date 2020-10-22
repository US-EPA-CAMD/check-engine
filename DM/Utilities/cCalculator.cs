using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECMPS.DM.Utilities
{
  /// <summary>
  /// Implements a reverse polish notation calculator
  /// </summary>
  public static class cCalculator
  {

    #region Public Enumerators

    /// <summary>
    /// Enumerations of the errors returned by cCalculator
    /// </summary>
    public enum eCalculatorError
    {

      /// <summary>
      /// Invalid index in formula
      /// </summary>
      Index,

      /// <summary>
      /// Invalid value in formula
      /// </summary>
      Value,

      /// <summary>
      /// Division by zero
      /// </summary>
      DivideByZero,

      /// <summary>
      /// Unhandled operator
      /// </summary>
      Operator,

      /// <summary>
      /// Incorrect number of operands for operator
      /// </summary>
      OperandCount,

      /// <summary>
      /// Multiple results from formula evaluation
      /// </summary>
      MultipleResults,

      /// <summary>
      /// Unexpected character found in formula
      /// </summary>
      UnexpectedCharacter,

      /// <summary>
      /// Unhandled exception
      /// </summary>
      Exception

    }

    #endregion


    #region Public Properties

    /// <summary>
    /// Error detail
    /// </summary>
    public static string ErrorDetail { get; private set; }

    /// <summary>
    /// Unexpected exception trapped by calculator
    /// </summary>
    public static Exception TrappedException { get; private set; }

    #endregion


    #region Public Methods: Evaluate

    /// <summary>
    /// Evaluates a reverse polish notation formula using a set of values where the formula
    /// indicates the position of the values in the value list using the [#] format where #
    /// is the position of the value in the value list.
    /// </summary>
    /// <param name="equation">Reverse polish notation equation.</param>
    /// <param name="values">List of values to use in the formula.</param>
    /// <param name="errorResult">Error enumeration value.</param>
    /// <returns>Return the result of the evaluation, or null if the evaluation failed.</returns>
    public static decimal? EvaluateReversePolish(string equation, decimal?[] values, out eCalculatorError? errorResult)
    {
      bool failed = false;

      TrappedException = null;
      errorResult = null;

      Stack<decimal> stack = new Stack<decimal>();

      int charPos = 0;

      while ((charPos < equation.Length) && !failed)
      {
        char character = equation[charPos];

        switch (character)
        {
          case '[':
            {
              int? index;

              if (GetIndex(equation, ref charPos, out index) 
                  && (index.Value < values.Length) 
                  && values[index.Value].HasValue)
              {
                stack.Push(values[index.Value].Value);
              }
              else
              {
                ErrorDetail = equation.Substring(charPos);
                errorResult = eCalculatorError.Index;
                failed = true;
              }
            }
            break;

          case '{':
            {
              decimal? value;

              if (GetValue(equation, ref charPos, out value))
              {
                stack.Push(value.Value);
              }
              else
              {
                ErrorDetail = equation.Substring(charPos);
                errorResult = eCalculatorError.Value;
                failed = true;
              }
            }
            break;
            /*
             */

          case '*':
          case '+':
          case '/':
          case '-':
            {
              if (stack.Count >= 2)
              {
                decimal operand2 = stack.Pop();
                decimal operand1 = stack.Pop();

                try
                {
                  switch (character)
                  {
                    case '+':
                      {
                        stack.Push(operand1 + operand2);
                        charPos += 1;
                      }
                      break;

                    case '*':
                      {
                        stack.Push(operand1 * operand2);
                        charPos += 1;
                      }
                      break;

                    case '-':
                      {
                        stack.Push(operand1 - operand2);
                        charPos += 1;
                      }
                      break;

                    case '/':
                      {
                        if (operand2 != 0)
                        {
                          stack.Push(operand1 / operand2);
                          charPos += 1;
                        }
                        else
                        {
                          errorResult = eCalculatorError.DivideByZero;
                          failed = true;
                        }
                      }
                      break;

                    default:
                      {
                        ErrorDetail = character.ToString();
                        errorResult = eCalculatorError.Operator;
                        failed = true;
                      }
                      break;
                  }
                }
                catch (Exception ex)
                {
                  TrappedException = ex;
                  errorResult = eCalculatorError.Exception;
                  failed = true;
                }
              }
              else
              {
                ErrorDetail = stack.Count.ToString();
                errorResult = eCalculatorError.OperandCount;
                failed = true;
              }
            }
            break;

          default:
            {
              ErrorDetail = character.ToString();
              errorResult = eCalculatorError.UnexpectedCharacter;
              failed = true;
            }
            break;
        }
      }

      decimal? result = null;

      if (!failed)
      {
        if (stack.Count == 1)
        {
          result = stack.Pop();
        }
        else
        {
          errorResult = eCalculatorError.MultipleResults;
          result = null;
        }
      }
      else
      {
        result = null;
      }

      return result;
    }

    #endregion


    #region Public Methods: Utilities

    /// <summary>
    /// Returns a list of indexes in the passed equation.
    /// </summary>
    /// <param name="equation">The equation to search.</param>
    /// <returns>The list of indexes</returns>
    public static bool[] GetIndexUse(string equation)
    {
      bool[] result;

      if (!string.IsNullOrEmpty(equation))
      {
        int pos = equation.IndexOf('[', 0);

        if (pos >= 0)
        {
          List<int> indexes = new List<int>();
          int maxIndex = -1;

          bool error = false;
          bool done = false;

          while ((pos >= 0) && !done)
          {
            int? index;

            if (GetIndex(equation, ref pos, out index))
            {
              if (!indexes.Contains(index.Value))
              {
                indexes.Add(index.Value);

                if (index.Value > maxIndex)
                  maxIndex = index.Value;
              }

              pos = equation.IndexOf('[', pos);
            }
            else
            {
              error = true;
              done = true;
            }
          }

          if (!error)
          {
            result = new bool[maxIndex + 1];

            foreach (int index in indexes)
              result[index] = true;
          }
          else
            result = new bool[0];
        }
        else
          result = new bool[0];
      }
      else
        result = new bool[0];

      return result;
    }

    /// <summary>
    /// Normalizes a standard equation
    /// </summary>
    /// <param name="inputEquation">The equation to normalize.</param>
    /// <param name="outputEquation">The normalized equation.</param>
    /// <returns>Returns true if the equation was normalized.</returns>
    public static bool NormalizeStandard(string inputEquation, out string outputEquation)
    {
      bool result = true;

      string hold = "";
      string safe = "";

      string equation = inputEquation.Trim();

      bool multOrDivFound = false;
      bool operandWasLast = false; // To prevent following an operand with another operand
      bool operatorWasLast = false; // To prevent following an operator with another operator

      int charPos = 0;

      while ((charPos < equation.Length) && result)
      {
        switch (equation[charPos])
        {
          case '+':
          case '-':
            {
              if (!operatorWasLast)
              {
                if (multOrDivFound)
                  safe += '(' + hold + ')' + equation.Substring(charPos, 1);
                else
                  safe += hold + equation.Substring(charPos, 1);

                hold = "";

                charPos += 1;

                multOrDivFound = false;
                operandWasLast = false;
                operatorWasLast = true;
              }
              else
              {
                safe = null;
                result = false;
              }
            }
            break;

          case '*':
          case '/':
            {
              if (!operatorWasLast)
              {
                hold += equation.Substring(charPos, 1);

                charPos += 1;

                multOrDivFound = true;
                operandWasLast = false;
                operatorWasLast = true;
              }
              else
              {
                safe = null;
                result = false;
              }
            }
            break;

          case ' ':
            {
              charPos += 1;
            }
            break;

          case '(':
            {
              string content;

              if (!operandWasLast && 
                  GetParentheticContent(equation, ref charPos, out content) &&
                  NormalizeStandard(content, out content))
              {
                hold += '(' + content + ')';

                operandWasLast = true;
                operatorWasLast = false;
              }
              else
              {
                safe = null;
                result = false;
              }
            }
            break;

          case '{':
            {
              decimal? value;

              if (!operandWasLast &&
                  GetValue(equation, ref charPos, out value))
              {
                hold += '{' + value.Value.ToString() + '}';

                operandWasLast = true;
                operatorWasLast = false;
              }
              else
              {
                safe = null;
                result = false;
              }
            }
            break;

          case '[':
            {
              int? index;

              if (!operandWasLast &&
                  GetIndex(equation, ref charPos, out index))
              {
                hold += '[' + index.Value.ToString() + ']';

                operandWasLast = true;
                operatorWasLast = false;
              }
              else
              {
                safe = null;
                result = false;
              }
            }
            break;

          default:
            {
              decimal? value;

              if (!operandWasLast &&
                  IsNumericChar(equation[charPos]) &&
                  GetNumber(equation, ref charPos, out value))
              {
                hold += '{' + value.Value.ToString() + '}';

                operandWasLast = true;
                operatorWasLast = false;
              }
              else
              {
                safe = null;
                result = false;
              }
            }
            break;
        }
      }

      // Handle last part of the equation
      if (result)
      {
        if (multOrDivFound)
          safe += '(' + hold + ')';
        else
          safe += hold;
      }

      outputEquation = safe;

      return result;
    }

    /// <summary>
    /// Converts standard notation to Reverse Polish notation.
    /// </summary>
    /// <param name="standardEquation">The equation to convert.</param>
    /// <param name="polishEquation">The converted equation.</param>
    /// <returns>Returns true if the conversion is successful.</returns>
    public static bool StandardToReversePolish(string standardEquation, out string polishEquation)
    {
      bool result;

      string equation;

      polishEquation = null;

      result = NormalizeStandard(standardEquation, out equation) &&
               StandardToReversePolishDo(equation, out polishEquation);

      return result;
    }

    /// <summary>
    /// Converts standard notation to Reverse Polish notation.
    /// </summary>
    /// <param name="standardEquation">The equation to convert.</param>
    /// <returns>The converted equation or null if the conversion failed.</returns>
    public static string StandardToReversePolish(string standardEquation)
    {
      string result;

      StandardToReversePolish(standardEquation, out result);

      return result;
    }


    #region Helper Methods

    /// <summary>
    /// Converts normalized standard notation to Reverse Polish notation.
    /// </summary>
    /// <param name="standardEquation">The equation to convert in normalized standard notation.</param>
    /// <param name="polishEquation">The converted location.</param>
    /// <returns>Returns true if the conversion is successful.</returns>
    private static bool StandardToReversePolishDo(string standardEquation, out string polishEquation)
    {
      bool result;

      result = true;
      polishEquation = "";

      char? currentOperator = null;

      bool operandWasLast = false; // To prevent following an operand with another operand
      bool operatorWasLast = false; // To prevent following an operator with another operator

      int charPos = 0;

      while ((charPos < standardEquation.Length) && result)
      {
        switch (standardEquation[charPos])
        {
          case '*':
          case '/':
          case '+':
          case '-':
            {
              if (!operatorWasLast)
              {
                currentOperator = standardEquation[charPos];

                charPos += 1;

                operandWasLast = false;
                operatorWasLast = true;
              }
              else
              {
                polishEquation = null;
                result = false;
              }
            }
            break;

          case ' ':
            {
              charPos += 1;
            }
            break;

          case '(':
            {
              string segment;

              if (!operandWasLast &&
                  GetParentheticContent(standardEquation, ref charPos, out segment) &&
                  StandardToReversePolishDo(segment, out segment))
              {
                polishEquation += segment;

                if (currentOperator.HasValue)
                {
                  polishEquation += currentOperator;
                  currentOperator = null;
                }

                operandWasLast = true;
                operatorWasLast = false;
              }
              else
              {
                polishEquation = null;
                result = false;
              }
            }
            break;

          case '{':
            {
              decimal? value;

              if (!operandWasLast &&
                  GetValue(standardEquation, ref charPos, out value))
              {
                polishEquation += '{' + value.Value.ToString() + '}';

                if (currentOperator.HasValue)
                {
                  polishEquation += currentOperator;
                  currentOperator = null;
                }

                operandWasLast = true;
                operatorWasLast = false;
              }
              else
              {
                polishEquation = null;
                result = false;
              }
            }
            break;

          case '[':
            {
              int? index;

              if (!operandWasLast &&
                  GetIndex(standardEquation, ref charPos, out index))
              {
                polishEquation += '[' + index.Value.ToString() + ']';

                if (currentOperator.HasValue)
                {
                  polishEquation += currentOperator;
                  currentOperator = null;
                }

                operandWasLast = true;
                operatorWasLast = false;
              }
              else
              {
                polishEquation = null;
                result = false;
              }
            }
            break;

          default:
            {
              decimal? value;

              if (!operandWasLast &&
                  IsNumericChar(standardEquation[charPos]) &&
                  GetNumber(standardEquation, ref charPos, out value))
              {
                polishEquation += '{' + value.Value.ToString() + '}';

                if (currentOperator.HasValue)
                {
                  polishEquation += currentOperator;
                  currentOperator = null;
                }

                operandWasLast = true;
                operatorWasLast = false;
              }
              else
              {
                polishEquation = null;
                result = false;
              }
            }
            break;
        }
      }

      // Handle last operator
      if (result && currentOperator.HasValue)
      {
        polishEquation += currentOperator;
      }

      return result;
    }

    #endregion

    #endregion


    #region Private Methods

    /// <summary>
    /// Returns the text bound by the begin and end characters at the character position.
    /// </summary>
    /// <param name="source">The text to inspect.</param>
    /// <param name="charPos">The character position of the beginning bound.</param>
    /// <param name="beginChar">The begin bounding character.</param>
    /// <param name="endChar">The end bounding chracter.</param>
    /// <param name="text">The bounded text.</param>
    /// <returns>Returns true if bound text is found.</returns>
    private static bool GetBoundText(string source, ref int charPos, char beginChar, char endChar, out string text)
    {
      bool result;

      int endPos;

      if (!string.IsNullOrEmpty(source)
          && (source[charPos] == beginChar)
          && (source.Length > (charPos + 2))
          && ((endPos = source.IndexOf(endChar, charPos)) > charPos))
      {
        text = source.Substring(charPos + 1, endPos - (charPos + 1));
        charPos = endPos + 1;
        result = true;
      }
      else
      {
        text = null;
        result = false;
      }

      return result;
    }

    /// <summary>
    /// Returns the index continaed in the index specifcation at the character position.
    /// </summary>
    /// <param name="formula">The formula containing the specification.</param>
    /// <param name="charPos">The position of the specification.</param>
    /// <param name="index">The index in the spec, otherwise null.</param>
    /// <returns>True if an index is returned.</returns>
    private static bool GetIndex(string formula, ref int charPos, out int? index)
    {
      string text;

      if (GetBoundText(formula, ref charPos, '[', ']', out text))
      {
        int temp;

        if (int.TryParse(text, out temp))
          index = temp;
        else
          index = null;
      }
      else
      {
        index = null;
      }

      return index.HasValue;
    }

    /// <summary>
    /// Returns the numeric value at the character position.
    /// </summary>
    /// <param name="formula">The formula containing the specification.</param>
    /// <param name="charPos">The position of the specification.</param>
    /// <param name="number">The valid number, otherwise null if invalid.</param>
    /// <returns>True if a value is returned.</returns>
    private static bool GetNumber(string formula, ref int charPos, out decimal? number)
    {
      string text = "";

      if (IsNumericChar(formula[charPos]))
      {
        int pos = charPos;

        while ((pos < formula.Length) && (IsNumericChar(formula[pos])))
        {
          text += formula[pos];
          pos += 1;
        }

        decimal temp;

        if (decimal.TryParse(text, out temp))
        {
          number = temp;
          charPos = pos;
        }
        else
          number = null;
      }
      else
        number = null;

      return number.HasValue;
    }

    /// <summary>
    /// Returns the content of an equation within parenthesis.
    /// </summary>
    /// <param name="equation">The equation.</param>
    /// <param name="startPos">The position of the opening parenthesis.</param>
    /// <param name="content">The content of the parenthesis.</param>
    /// <returns>Returns true if the content was returned.</returns>
    private static bool GetParentheticContent(string equation, ref int startPos, out string content)
    {
      bool result;

      content = null;
      result = false;

      if (equation[startPos] == '(')
      {
        string hold = "";
        int pos = startPos + 1;
        int openCount = 1;

        bool done = false;

        result = true;

        while (!done)
        {
          int openPos = equation.IndexOf('(', pos);
          int closePos = equation.IndexOf(')', pos);

          if (closePos < 0)
          {
            // More open than close parenthesis
            content = null;
            result = false;
            done = true;
          }
          else if ((0 <= openPos) && (openPos < closePos))
          {
            hold += equation.Substring(pos, openPos - pos + 1);
            pos += openPos - pos + 1;
            openCount += 1;
          }
          else
          {
            if (openCount > 1)
              hold += equation.Substring(pos, closePos - pos + 1);
            else
              hold += equation.Substring(pos, closePos - pos);

            pos += closePos - pos + 1;
            openCount -= 1;

            if (openCount == 0)
            {
              startPos = pos;
              content = hold.Trim();
              result = true;
              done = true;
            }
          }
        }
      }

      return result;
    }

    /// <summary>
    /// Returns the value continaed in the value specifcation at the character position.
    /// </summary>
    /// <param name="formula">The formula containing the specification.</param>
    /// <param name="charPos">The position of the specification.</param>
    /// <param name="value">The value in the spec, otherwise null.</param>
    /// <returns>True if a value is returned.</returns>
    private static bool GetValue(string formula, ref int charPos, out decimal? value)
    {
      string text;

      if (GetBoundText(formula, ref charPos, '{', '}', out text))
      {
        decimal temp;

        if (decimal.TryParse(text, out temp))
          value = temp;
        else
          value = null;
      }
      else
        value = null;

      return value.HasValue;
    }

    /// <summary>
    /// Determines whether the passed character is numeric including decimal point.
    /// </summary>
    /// <param name="character">The character to inspect.</param>
    /// <returns>True if the character is numeric.</returns>
    private static bool IsNumericChar(char character)
    {
      bool result;

      result = ((character == '0') ||
                (character == '1') ||
                (character == '2') ||
                (character == '3') ||
                (character == '4') ||
                (character == '5') ||
                (character == '6') ||
                (character == '7') ||
                (character == '8') ||
                (character == '9') ||
                (character == '.'));

      return result;
    }

    #endregion

  }
}
