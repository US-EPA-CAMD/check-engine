using System;
using System.Collections;

namespace ECMPS.Checks.CheckEngine
{
    /// <summary>
    /// Summary description for HourRangeCollection.
    /// </summary>
    public class cHourRange
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public cHourRange()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ABeganDate"></param>
        /// <param name="ABeganHour"></param>
        /// <param name="AEndedDate"></param>
        /// <param name="AEndedHour"></param>
        public cHourRange( DateTime ABeganDate, int ABeganHour, DateTime AEndedDate, int AEndedHour )
        {
            BeganDate = ABeganDate;
            BeganHour = ABeganHour;
            EndedDate = AEndedDate;
            EndedHour = AEndedHour;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ABeganDate"></param>
        /// <param name="AEndedDate"></param>
        public cHourRange( DateTime ABeganDate, DateTime AEndedDate )
        {
            BeganDate = ABeganDate;
            BeganHour = 0;
            EndedDate = AEndedDate;
            EndedHour = 23;
        }

        #endregion


        #region Public Properties and Fields

        /// <summary>
        /// Began date
        /// </summary>
        public DateTime BeganDate;
        
        /// <summary>
        /// The Began Date and Hour
        /// </summary>
        public DateTime BeganDateHour { get { return BeganDate.Date.AddHours(BeganHour); } set { BeganDate = value.Date; BeganHour = value.Hour; } }

        /// <summary>
        /// Began hour
        /// </summary>
        public int BeganHour;

        /// <summary>
        /// ended date
        /// </summary>
        public DateTime EndedDate;

        /// <summary>
        /// The Ended Date and Hour
        /// </summary>
        public DateTime EndedDateHour { get { return EndedDate.Date.AddHours(EndedHour); } set { EndedDate = value.Date; EndedHour = value.Hour; } }

        /// <summary>
        /// ended hour
        /// </summary>
        public int EndedHour;

        #endregion

        /// <summary>
        /// DisplayRange
        /// </summary>
        /// <returns></returns>
        public string DisplayRange()
        {
            return BeganDate.ToShortDateString() + "-" + BeganHour.ToString() + " to " +
                   EndedDate.ToShortDateString() + "-" + EndedHour.ToString();
        }
    }

    /// <summary>
    /// Collection of HourRange objects
    /// </summary>
    public class cHourRangeCollection : System.Collections.CollectionBase
    {
        #region Constructor Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public cHourRangeCollection()
        {
        }


        #endregion


        #region Public Properties

        /// <summary>
        /// Item
        /// </summary>
        /// <param name="AIndex"></param>
        /// <returns></returns>
        public cHourRange Item( int AIndex )
        {
            if( ( 0 <= AIndex ) && ( AIndex < Count ) )
            {
                return (cHourRange)List[AIndex];
            }
            else return null;
        }


        #endregion


        #region Add Methods

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="AHourRange"></param>
        public void Add( cHourRange AHourRange )
        {
            List.Add( AHourRange );
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="ABeganDate"></param>
        /// <param name="ABeganHour"></param>
        /// <param name="AEndedDate"></param>
        /// <param name="AEndedHour"></param>
        public void Add( DateTime ABeganDate, int ABeganHour, DateTime AEndedDate, int AEndedHour )
        {
            List.Add( new cHourRange( ABeganDate, ABeganHour, AEndedDate, AEndedHour ) );
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="ABeganDate"></param>
        /// <param name="AEndedDate"></param>
        public void Add( DateTime ABeganDate, DateTime AEndedDate )
        {
            List.Add( new cHourRange( ABeganDate, AEndedDate ) );
        }


        #endregion


        #region Union Methods

        /// <summary>
        /// Union
        /// </summary>
        public void Union()
        {
            bool Merged;
            int Pos = Count;
            int Dex;

            while( Pos > 1 )
            {
                Merged = false;
                Pos--;
                Dex = Pos;

                while( !Merged && ( Dex > 0 ) )
                {
                    Dex--;

                    if( ( ( Item( Pos ).BeganDate < Item( Dex ).EndedDate ) ||
                         ( ( Item( Pos ).BeganDate == Item( Dex ).EndedDate ) &&
                          ( Item( Pos ).BeganHour <= Item( Dex ).EndedHour ) ) ) &&
                        ( ( Item( Pos ).EndedDate > Item( Dex ).BeganDate ) ||
                         ( ( Item( Pos ).EndedDate == Item( Dex ).BeganDate ) &&
                          ( Item( Pos ).EndedHour >= Item( Dex ).BeganHour ) ) ) )
                    {
                        if( ( Item( Pos ).BeganDate < Item( Dex ).BeganDate ) ||
                            ( ( Item( Pos ).BeganDate == Item( Dex ).BeganDate ) &&
                             ( Item( Pos ).BeganHour < Item( Dex ).BeganHour ) ) )
                        {
                            Item( Dex ).BeganDate = Item( Pos ).BeganDate;
                            Item( Dex ).BeganHour = Item( Pos ).BeganHour;
                        }

                        if( ( Item( Pos ).EndedDate > Item( Dex ).EndedDate ) ||
                            ( ( Item( Pos ).EndedDate == Item( Dex ).EndedDate ) &&
                             ( Item( Pos ).EndedHour > Item( Dex ).EndedHour ) ) )
                        {
                            Item( Dex ).EndedDate = Item( Pos ).EndedDate;
                            Item( Dex ).EndedHour = Item( Pos ).EndedHour;
                        }

                        RemoveAt( Pos );
                        Merged = true;
                    }
                    else if( ( ( Item( Pos ).BeganDate == Item( Dex ).EndedDate ) &&
                              ( Item( Pos ).BeganHour == ( Item( Dex ).EndedHour + 1 ) ) ) ||
                             ( ( Item( Dex ).EndedDate != DateTime.MaxValue ) &&
                              ( Item( Pos ).BeganDate == Item( Dex ).EndedDate.AddDays( 1 ) ) &&
                              ( ( Item( Pos ).BeganHour == 0 ) && ( Item( Dex ).EndedHour == 23 ) ) ) )
                    {
                        Item( Dex ).EndedDate = Item( Pos ).EndedDate;
                        Item( Dex ).EndedHour = Item( Pos ).EndedHour;
                        RemoveAt( Pos );
                        Merged = true;
                    }
                    else if( ( ( Item( Pos ).EndedDate == Item( Dex ).BeganDate ) &&
                              ( ( Item( Pos ).EndedHour + 1 ) == Item( Dex ).BeganHour ) ) ||
                            ( ( Item( Pos ).EndedDate != DateTime.MaxValue ) &&
                              ( Item( Pos ).EndedDate.AddDays( 1 ) == Item( Dex ).BeganDate ) &&
                              ( ( Item( Pos ).EndedHour == 23 ) && ( Item( Dex ).BeganHour == 0 ) ) ) )
                    {
                        Item( Dex ).BeganDate = Item( Pos ).BeganDate;
                        Item( Dex ).BeganHour = Item( Pos ).BeganHour;
                        RemoveAt( Pos );
                        Merged = true;
                    }
                }
            }
        }

        /// <summary>
        /// Union
        /// </summary>
        /// <param name="ABeganDate"></param>
        /// <param name="ABeganHour"></param>
        /// <param name="AEndedDate"></param>
        /// <param name="AEndedHour"></param>
        public void Union( DateTime ABeganDate, int ABeganHour, DateTime AEndedDate, int AEndedHour )
        {
            Add( ABeganDate, ABeganHour, AEndedDate, AEndedHour );
            Union();
        }

        /// <summary>
        /// Union
        /// </summary>
        /// <param name="ABeganDate"></param>
        /// <param name="AEndedDate"></param>
        public void Union( DateTime ABeganDate, DateTime AEndedDate )
        {
            Add( ABeganDate, AEndedDate );
            Union();
        }

        #endregion


        #region Other Methods

        /// <summary>
        /// Performs an insertion sort on the collection, comparing the begin hours and then the end hours if the begin hours are equal.
        /// </summary>
        public void Sort()
        {
            if (Count > 1)
            {
                int holePosition;
                cHourRange insertValue = (cHourRange)List[0];

                /* Start with the second element */
                for (int dex = 1; dex < Count; dex++)
                {
                    /* Select value to insert and current position */
                    holePosition = dex;
                    insertValue = (cHourRange)List[dex];

                    /* Starting with the hole position, and continually switch it with the value in the next lower position until the value is not higher  */
                    while   (
                                (holePosition > 0) && 
                                (
                                    (((cHourRange)List[holePosition - 1]).BeganDateHour > insertValue.BeganDateHour) ||
                                    (
                                        (((cHourRange)List[holePosition - 1]).BeganDateHour == insertValue.BeganDateHour) &&
                                        (((cHourRange)List[holePosition - 1]).EndedDateHour > insertValue.EndedDateHour)
                                    )
                                )
                            )
                    {
                        List[holePosition] = List[holePosition - 1];
                        holePosition -= 1;
                    }

                    /* Place the insert value in the position of the first value higher than the insert value */
                    List[holePosition] = insertValue;
                }
            }
        }

        #endregion

    }
}
