using System;
using System.Text.RegularExpressions;

namespace ECMPS.Checks.TypeUtilities
{
    /// <summary>
    /// String validation functions
    /// </summary>
    public class cStringValidator
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public cStringValidator()
        {
        }

        /// <summary>
        /// Function to test for Positive Integers
        /// </summary>
        /// <param name="strNumber">String representation of a number to check</param>
        /// <returns>true if a positive integer, false otherwise</returns>
        public static bool IsNaturalNumber( String strNumber )
        {
            Regex objNotNaturalPattern = new Regex( "[^0-9]" );
            Regex objNaturalPattern = new Regex( "0*[1-9][0-9]*" );

            return !objNotNaturalPattern.IsMatch( strNumber ) && objNaturalPattern.IsMatch( strNumber );
        }

        /// <summary>
        /// Function to test for Positive Integers with zero inclusive
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns>true if whole integer, false otherwise</returns>
        public static bool IsWholeNumber( string strNumber )
        {
            Regex objNotWholePattern = new Regex( "[^0-9]" );

            return !objNotWholePattern.IsMatch( strNumber );
        }

        /// <summary>
        /// Function to Test for Integers both Positive and Negative
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns>true if integer, false otherwise</returns>
        public static bool IsInteger( string strNumber )
        {
            Regex objNotIntPattern = new Regex( "[^0-9-]" );
            Regex objIntPattern = new Regex( "^-[0-9]+$|^[0-9]+$" );

            return !objNotIntPattern.IsMatch( strNumber ) &&
                objIntPattern.IsMatch( strNumber );
        }

        /// <summary>
        /// Function to Test for Positive Number both Integer and Real 
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns>true if positive number, false otherwise</returns>
        public static bool IsPositiveNumber( string strNumber )
        {
            Regex objNotPositivePattern = new Regex( "[^0-9.]" );
            Regex objPositivePattern = new Regex( "^[.][0-9]+$|[0-9]*[.]*[0-9]+$" );
            Regex objTwoDotPattern = new Regex( "[0-9]*[.][0-9]*[.][0-9]*" );

            return !objNotPositivePattern.IsMatch( strNumber ) &&
                objPositivePattern.IsMatch( strNumber ) &&
                !objTwoDotPattern.IsMatch( strNumber );
        }

        /// <summary>
        /// Function to test whether the string is valid number or not
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns>true if valid number, false otherwise</returns>
        public static bool IsNumber( string strNumber )
        {
            Regex objNotNumberPattern = new Regex( "[^0-9.-]" );
            Regex objTwoDotPattern = new Regex( "[0-9]*[.][0-9]*[.][0-9]*" );
            Regex objTwoMinusPattern = new Regex( "[0-9]*[-][0-9]*[-][0-9]*" );
            String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex( "(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")" );

            return !objNotNumberPattern.IsMatch( strNumber ) &&
                !objTwoDotPattern.IsMatch( strNumber ) &&
                !objTwoMinusPattern.IsMatch( strNumber ) &&
                objNumberPattern.IsMatch( strNumber );
        }

        /// <summary>
        /// Function To test for Alphabets. 
        /// </summary>
        /// <param name="strToCheck"></param>
        /// <returns>true if Alphabets, false otherwise</returns>
        public static bool IsAlpha( string strToCheck )
        {
            Regex objAlphaPattern = new Regex( "[^a-zA-Z]" );

            return !objAlphaPattern.IsMatch( strToCheck );
        }

        /// <summary>
        /// Function to Check for AlphaNumeric.
        /// </summary>
        /// <param name="strToCheck"></param>
        /// <returns>true if AlphaNumeric, false otherwise</returns>
        public static bool IsAlphaNumeric( string strToCheck )
        {
            Regex objAlphaNumericPattern = new Regex( "[^a-zA-Z0-9]" );

            return !objAlphaNumericPattern.IsMatch( strToCheck );
        }
    }
}
