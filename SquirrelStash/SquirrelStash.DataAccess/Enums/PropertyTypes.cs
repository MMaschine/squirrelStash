namespace SquirrelStash.DataAccess.Enums
{
    public enum PropertyTypes
    {
        /// <summary>
        /// Basic property with single string values
        /// </summary>
        Basic = 0,

        /// <summary>
        /// Value is string representation of number 
        /// </summary>
        Numeric,

        /// <summary>
        /// Value should belong to predefined list of values
        /// </summary>
        AllowedValues
    } 
}
