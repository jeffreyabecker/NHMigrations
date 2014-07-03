using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Cfg.ConfigurationSchema;
using NHibernate.Id;
using NHibernate.Mapping;
using NHibernate.Type;
using NHMigration.Operations;
using NHMigration.Versioning;
using Table = NHibernate.Mapping.Table;

namespace NHMigration
{
    public class MigrationGenerator
    {


        public IEnumerable<IOperation> GenerateOperations(IVersion prevVersion, IVersion currentVersion)
        {
            if (prevVersion == null)
            {
                return GetCreateOperations(currentVersion);

            }
            else
            {
                return GetAlterOperations(prevVersion, currentVersion);
            }
        }

        private IEnumerable<IOperation> GetAlterOperations(IVersion prevVersion, IVersion currentVersion)
        {
            var prev = new ConfigHelper(prevVersion.Configuration);
            var curr = new ConfigHelper(currentVersion.Configuration);


            var createdTables = GetCreatedTables(prev, curr);
            var alteredTables = GetAlteredTables(prev, curr).ToList();
            var droppedTables = GetDroppedTables(prev, curr).ToList();

            foreach (var o in GetCreateOperationsForTables(createdTables))
            {
                yield return o;
            }
            //TODO: POID Generators

            foreach (var t in alteredTables)
            {
                foreach (var fk in t.DroppedForeignKeys)
                {
                    yield return new DropForeignKeyOperation(fk);
                }
                foreach (var index in t.DroppedIndices)
                {
                    yield return new DropIndexOperation(index);
                }
            }
            foreach (var t in droppedTables)
            {
                foreach (var fk in t.ForeignKeyIterator)
                {
                    yield return new DropForeignKeyOperation(fk);
                }
                foreach (var index in t.IndexIterator)
                {
                    yield return new DropIndexOperation(index);
                }
            }
            foreach (var t in alteredTables)
            {

                foreach (var c in t.AddedColumns)
                {
                    yield return new AlterTableAddColumnOperation(t, c);
                }
                foreach (var c in t.AlteredColumns)
                {
                    yield return new AlterTableAlterColumnOperation(t, c);
                }
                foreach (Column c in t.DroppedColumns)
                {
                    yield return new AlterTableDropColumnOperation(c);
                }
            }
            foreach (var t in alteredTables)
            {
                foreach (var fk in t.AddedForeignKeys)
                {
                    yield return new AlterTableAddForeignKeyOperation(fk);
                }
                foreach (Index ix in t.AddedIndices)
                {
                    yield return new CreateIndexOperation(ix);
                }
            }

            foreach (var t in droppedTables)
            {
                
                foreach (var index in t.IndexIterator)
                {
                    yield return new DropIndexOperation(index);
                }
                yield return new DropTableOperation(t);
            }
        }

        private IEnumerable<Table> GetDroppedTables(ConfigHelper prev, ConfigHelper curr)
        {
            yield break;
        }

        private IEnumerable<AlteredTable> GetAlteredTables(ConfigHelper prev, ConfigHelper curr)
        {
            yield break;
            
        }

        public class AlteredTable
        {
            public IEnumerable<ForeignKey> DroppedForeignKeys { get; set; }

            public IEnumerable<Index> DroppedIndices { get; set; }

            public IEnumerable<Column> AddedColumns { get; set; }

            public IEnumerable<Column> AlteredColumns { get; set; }

            public IEnumerable<Column> DroppedColumns { get; set; }

            public IEnumerable<ForeignKey> AddedForeignKeys { get; set; }

            public IEnumerable<Index> AddedIndices { get; set; }
        }

        private IEnumerable<Table> GetCreatedTables(ConfigHelper prev, ConfigHelper curr)
        {
            yield break;
        }

        private IEnumerable<IOperation> GetCreateOperations(IVersion currentVersion)
        {
            
            var cfg = new ConfigHelper(currentVersion.Configuration);

            var tablesToExport = cfg.Tables.Where(t => t.IsPhysicalTable && t.SchemaActions.HasFlag(SchemaAction.Export));
            foreach (var operation in GetCreateOperationsForTables(tablesToExport)) yield return operation;

            //TODO: Figure out how to make sense out of POID generators without going into private reflection hell
            
            foreach (var auxDbObj in cfg.AuxiliaryDatabaseObjects)
            {
                yield return new CreateAuxiliaryDatabaseObjectOperation(auxDbObj);
            }

        }

        private static IEnumerable<IOperation> GetCreateOperationsForTables(IEnumerable<Table> tablesToExport)
        {
            foreach (var table in tablesToExport)
            {
                yield return new CreateTableOperation(table);
            }
            foreach (var table in tablesToExport)
            {
                foreach (var uk in table.UniqueKeyIterator)
                {
                    yield return new AlterTableAddUniqueConstriantOperation(uk);
                }
                foreach (var index in table.IndexIterator)
                {
                    yield return new CreateIndexOperation(index);
                }
                foreach (
                    var fk in
                        table.ForeignKeyIterator.Where(
                            f => f.HasPhysicalConstraint && f.ReferencedTable.SchemaActions.HasFlag(SchemaAction.Export)))
                {
                    yield return new AlterTableAddForeignKeyOperation(fk);
                }
            }
        }

        //private IEnumerable<IOperation> GetDropOperations(ConfigHelper cfg)
        //{
        //    foreach (var auxDbObj in cfg.AuxiliaryDatabaseObjects.Reverse())
        //    {
        //        yield return SqlOperation.GetDropSqlOperationForAuxObj(auxDbObj);
        //    }

        //    foreach (var table in cfg.Tables.Where(t => t.IsPhysicalTable && t.SchemaActions.HasFlag(SchemaAction.Drop)))
        //    {
        //        foreach (var fk in table.ForeignKeyIterator.Where(k => k.HasPhysicalConstraint && k.ReferencedTable.SchemaActions.HasFlag(SchemaAction.Drop)))
        //        {
        //            yield return new DropForeignKeyOperation(fk);
        //        }

        //    }
        //    foreach (var table in cfg.Tables)
        //    {
        //        //TODO: Consider if following the schema action guides is appropriate for the migration generator                
        //        if (table.IsPhysicalTable && table.SchemaActions.HasFlag(SchemaAction.Export))
        //        {
        //            yield return new DropTableOperation(table);
        //        }
        //    }


        //    cfg.ClassMappings.Where(p => !p.IsInherited)
        //        .Select(p => p.Identifier)
        //        .Cast<SimpleValue>();

        //}

    }


}
