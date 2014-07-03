using System;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.SqlTypes;

namespace NHMigration.Model
{
    /// <summary>Represents information about a column.</summary>
    public class ColumnModel
    {
        private int? _length;
        private int? _precision;
        private int? _scale;
    

        public ColumnModel()
        {
            IsNullable = true;
            IsUnique = false;
        }

        /// <summary>
        /// Gets or sets the length of the datatype in the database.
        /// </summary>
        /// <value>The length of the datatype in the database.</value>
        public int Length
        {
            get { return _length ?? NHibernate.Mapping.Column.DefaultLength; }
            set { _length = value; }
        }

        /// <summary>
        /// Gets or sets the name of the column in the database.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets if the column can have null values in it.
        /// </summary>
        /// <value><see langword="true" /> if the column can have a null value in it.</value>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Gets or sets if the column contains unique values.
        /// </summary>
        /// <value><see langword="true" /> if the column contains unique values.</value>
        public bool IsUnique { get; set; }

        /// <summary>
        /// Gets or sets the sql data type name of the column.
        /// </summary>
        /// <value>
        /// The sql data type name of the column. 
        /// </value>
        /// <remarks>
        /// This is usually read from the <c>sql-type</c> attribute.
        /// </remarks>
        public string SqlType { get; set; }

        /// <summary>
        /// Gets or sets a check constraint on the column
        /// </summary>
        public string CheckConstraint { get; set; }

        public int Precision
        {
            get { return _precision ?? NHibernate.Mapping.Column.DefaultPrecision; }
            set { _precision = value; }
        }

        public int Scale
        {
            get { return _scale ?? NHibernate.Mapping.Column.DefaultScale; }
            set { _scale = value; }
        }

        /// <summary> 
        /// The underlying columns SqlType.
        /// </summary>
        /// <remarks>
        /// If null, it is because the sqltype code is unknown.
        /// 
        /// Use <see cref="GetSqlTypeCode(IMapping)"/> to retreive the sqltypecode used
        /// for the columns associated Value/Type.
        /// </remarks>
        public SqlType SqlTypeCode { get; set; }

        public string Comment { get; set; }

        public string DefaultValue { get; set; }

        public bool IsPrecisionDefined()
        {
            return _precision.HasValue || _scale.HasValue;
        }

        public bool IsLengthDefined()
        {
            return _length.HasValue;
        }
    }
}