using System;
using System.Collections;

namespace ECMPS.Checks.CheckEngine
{
    /// <summary>
    /// Summary description for LongCollection.
    /// </summary>
    public class cLongCollection : DictionaryBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public cLongCollection()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ADefaultValue"></param>
        public cLongCollection( long ADefaultValue )
        {
            DefaultValue = ADefaultValue;
        }

        /// <summary>
        /// Default value (long.MinValue by default)
        /// </summary>
        public long DefaultValue = long.MinValue;

        /// <summary>
        /// this
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns></returns>
        public long this[String AKey]
        {
            get
            {
                if( Contains( AKey ) )
                    return (long)Dictionary[AKey];
                else
                    return DefaultValue;
            }
            set { this.Dictionary[AKey] = value; }
        }

        /// <summary>
        /// Keys collection
        /// </summary>
        public ICollection Keys
        {
            get { return ( Dictionary.Keys ); }
        }

        /// <summary>
        /// Values collection
        /// </summary>
        public ICollection Values
        {
            get { return ( Dictionary.Values ); }
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add( String key, long value )
        {
            Dictionary.Add( key, value.ToString() );
        }

        /// <summary>
        /// Contains
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains( String key )
        {
            return ( Dictionary.Contains( key ) );
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="key"></param>
        public void Remove( String key )
        {
            Dictionary.Remove( key );
        }

        /// <summary>
        /// OnInsert
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected override void OnInsert( Object key, Object value )
        {
            if( key.GetType() != Type.GetType( "System.String" ) )
                throw new ArgumentException( "key must be of type String.", "key" );

            if( ( value.GetType() != Type.GetType( "System.Int64" ) ) &&
              ( value.GetType() != Type.GetType( "System.Int32" ) ) &&
              ( value.GetType() != Type.GetType( "System.Int16" ) ) )
                throw new ArgumentException( "value must be an integer.", "value" );
        }

        /// <summary>
        /// OnRemove
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected override void OnRemove( Object key, Object value )
        {
            if( key.GetType() != Type.GetType( "System.String" ) )
                throw new ArgumentException( "key must be of type String.", "key" );
        }

        /// <summary>
        /// OnSet
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected override void OnSet( Object key, Object oldValue, Object newValue )
        {
            if( key.GetType() != Type.GetType( "System.String" ) )
                throw new ArgumentException( "key must be of type String.", "key" );

            if( ( newValue.GetType() != Type.GetType( "System.Int64" ) ) &&
              ( newValue.GetType() != Type.GetType( "System.Int32" ) ) &&
              ( newValue.GetType() != Type.GetType( "System.Int16" ) ) )
                throw new ArgumentException( "value must be an integer.", "value" );
        }

        /// <summary>
        /// OnValidate
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected override void OnValidate( Object key, Object value )
        {
            if( key.GetType() != Type.GetType( "System.String" ) )
                throw new ArgumentException( "key must be of type String.", "key" );

            if( ( value.GetType() != Type.GetType( "System.Int64" ) ) &&
              ( value.GetType() != Type.GetType( "System.Int32" ) ) &&
              ( value.GetType() != Type.GetType( "System.Int16" ) ) )
                throw new ArgumentException( "value must be an integer.", "value" );
        }
    }

}
