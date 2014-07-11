﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Mapping;

namespace NHMigration.Builder
{


    public interface ICreateTableBuilder<TColumns>
    {

        ICreateTableBuilder<TColumns> PrimaryKey(Expression<Func<TColumns, object>> keyExpression, bool identity = false, string name = null,
            bool clustered = true);

        ICreateTableBuilder<TColumns> Index(Expression<Func<TColumns, object>> indexExpression, string name = null,
            bool unique = false, bool clustered = false);

        ICreateTableBuilder<TColumns> ForeignKey(string principalTable,
            Expression<Func<TColumns, object>> dependentKeyExpression, bool cascadeDelete = false, string name = null);

    }




    public interface IMigrationBuilder
    {
        ICreateTableBuilder<TColumns> CreateTable<TColumns>(string name, TColumns columns);

        void AddForeignKey(string dependentTable, string dependentColumn, string principalTable, string principalColumn = null, bool cascadeDelete = false, string name = null);
        void AddForeignKey(string dependentTable, string[] dependentColumns, string principalTable, string[] principalColumns = null, bool cascadeDelete = false, string name = null);
        void DropForeignKey(string dependentTable, string name);
        void DropForeignKey(string dependentTable, string dependentColumn, string principalTable);
        void DropForeignKey(string dependentTable, string dependentColumn, string principalTable, string principalColumn);
        void DropForeignKey(string dependentTable, string[] dependentColumns, string principalTable);
        void DropTable(string name);


        void AddColumn(string table, string name, Column column);
        void DropColumn(string table, string name);
        void AlterColumn(string table, string name, Column column);
        void AddPrimaryKey(string table, string column, string name = null, bool clustered = true);
        void AddPrimaryKey(string table, string[] columns, string name = null, bool clustered = true);
        void DropPrimaryKey(string table, string name);
        void DropPrimaryKey(string table);
        void CreateIndex(string table, string column, bool unique = false, string name = null, bool clustered = false);
        void CreateIndex(string table, string[] columns, bool unique = false, string name = null, bool clustered = false);
        void DropIndex(string table, string name);
        void DropIndex(string table, string[] columns);
        void Sql(string sql); 
    }

    
}