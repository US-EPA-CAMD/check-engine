using System;

using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.TypeUtilities
{
    /// <summary>
    /// List class for checks
    /// </summary>
    [Obsolete("Use string extension methods", true)]
    public static class cList
    {

        /// <summary>
        /// Add an item to the list
        /// </summary>
        /// <param name="AList">The list to add the item to (may be null)</param>
        /// <param name="AItem">The item to add to the list</param>
        /// <param name="ADelimeter">The delimeter for the list</param>
        /// <param name="AllowDuplicates">Are duplicates allowed in the list?</param>
        /// <returns>The new list with the item appened to the end</returns>
        [Obsolete( "use the string.ListAdd extension method", true )]
        public static string ListAdd( string AList, string AItem, string ADelimeter, bool AllowDuplicates )
        {
            return AList.ListAdd( AItem, ADelimeter, AllowDuplicates );
        }

        /// <summary>
        /// Add an item to the list, using default of comma as the delimeter - NO DUPLICATES ALLOWED
        /// </summary>
        /// <param name="AList">The list to add the item to (may be null)</param>
        /// <param name="AItem">The item to add to the list</param>
        /// <returns>The new list with the item appened to the end</returns>
        [Obsolete( "use the string.ListAdd extension method", true )]
        public static string ListAdd( string AList, string AItem )
        {
            return AList.ListAdd( AItem );
        }

        /// <summary>
        /// Add an item to the list, using default of comma as the delimeter
        /// </summary>
        /// <param name="AList">The list to add the item to (may be null)</param>
        /// <param name="AItem">The item to add to the list</param>
        /// <param name="AllowDuplicates">Are duplicates allowed in the list?</param>
        /// <returns>The new list with the item appened to the end</returns>
        [Obsolete( "use the string.ListAdd extension method", true )]
        public static string ListAdd( string AList, string AItem, bool AllowDuplicates )
        {
            return AList.ListAdd( AItem, AllowDuplicates );
        }

        /// <summary>
        /// Get the number of items in the list
        /// </summary>
        /// <param name="AList">The list in question</param>
        /// <param name="ADelimeter">The delimeter for the list</param>
        /// <returns>Number of items in the list</returns>
        [Obsolete("use the string.ListCount extension method", true)]
        public static int ListCount(string AList, string ADelimeter)
        {
            return AList.ListCount( ADelimeter );
        }

        /// <summary>
        /// Get the number of items in the list, using default of comma as the delimeter
        /// </summary>
        /// <param name="AList">The list in question</param>
        /// <returns>Number of items in the list</returns>
        [Obsolete("use the string.ListCount extension method", true)]
        public static int ListCount(string AList)
        {
            return AList.ListCount();
        }

        /// <summary>
        /// Get an item in the list by index
        /// </summary>
        /// <param name="AList">The list in question</param>
        /// <param name="AIndex">The index of the item to get</param>
        /// <param name="ADelimeter">The delimeter for the list</param>
        /// <returns>The item at the requested index</returns>
        [Obsolete("use the string.ListItem extension method",true)]
        public static string ListItem(string AList, int AIndex, string ADelimeter)
        {
            return AList.ListItem( AIndex, ADelimeter );
        }

        /// <summary>
        /// Get an item in the list by index, using default of comma as delimeter
        /// </summary>
        /// <param name="AList">The list in question</param>
        /// <param name="AIndex">The index of the item to get</param>
        /// <returns>The item at the requested index</returns>
        [Obsolete("use the string.ListItem extension method",true)]
        public static string ListItem(string AList, int AIndex)
        {
            return AList.ListItem( AIndex, "," );
        }

        /// <summary>
        /// Indicates whether the specified comma separated list contains the specified item.
        /// 
        /// The return value is always false if the list is null or a null string, 
        /// or if the item is null.  The return value will be true if the item is
        /// a null string and the list begins or ends with a comma, or two
        /// commas appear side by side.
        /// </summary>
        /// <param name="AList">The list in question</param>
        /// <param name="AItem">The item to search for</param>
        /// <returns>true if yes, false if no</returns>
        [Obsolete("use the string.InList extension method",true)]
        public static bool ListHas(string AList, string AItem)
        {
            return AItem.InList( AList, "," );
        }

        /// <summary>
        /// Format a list for display
        /// <example>fred,barney,wilma becomes fred, barney and wilma</example>
        /// </summary>
        /// <param name="AList">The list in question</param>
        /// <param name="OrText">Is the last item in the list 'and' or 'or'</param>
        /// <returns>The formated list for display</returns>
        [Obsolete("use the string.FormatList extension method",true)]
        public static string FormatList( string AList, bool OrText )
        {
            return AList.FormatList( OrText );
        }

        /// <summary>
        /// Format a list for display
        /// <example>fred,barney,wilma becomes fred, barney and wilma</example>
        /// </summary>
        /// <param name="AList">The list in question</param>
        /// <returns>The formated list for display</returns>
        [Obsolete("use the string.FormatList extension method",true)]
        public static string FormatList( string AList )
        {
            return AList.FormatList();
        }
    }
}
