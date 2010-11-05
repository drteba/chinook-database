﻿/*******************************************************************************
 * Chinook Database
 * Description: Test fixture for Oracle version of Chinook database.
 * DB Server: SQL Server
 * License: http://www.codeplex.com/ChinookDatabase/license
 * 
 * IMPORTANT: In order to run these test fixtures, you will need to:
 *            1. Run the generated SQL script to create the database to be tested.
 *            2. Verify that app.config has the proper connection string (user/password).
 ********************************************************************************/
using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using NUnit.Framework;

namespace ChinookMetadata.Test.DatabaseTests
{
    /// <summary>
    /// Class fixture for Oracle version of Chinook database.
    /// </summary>
    [TestFixture]
    public class ChinookOracleFixture : DatabaseFixture
    {
        static OleDbConnection _connection;

        /// <summary>
        /// Retrieves the cached connection object.
        /// </summary>
        /// <returns>A connection object for this specific database.</returns>
        protected static OleDbConnection GetConnection()
        {
            // Creates an ADO.NET connection to the database, if not created yet.
            if (_connection == null)
            {
                var section = (ConnectionStringsSection) ConfigurationManager.GetSection("connectionStrings");
                foreach (ConnectionStringSettings entry in section.ConnectionStrings)
                {
                    if (entry.Name == "ChinookOracle")
                    {
                        _connection = new OleDbConnection(entry.ConnectionString);
                        break;
                    }
                }
            }

            // If we failed to create a connection, then throw an exception.
            if (_connection == null)
            {
                throw new ApplicationException("There is no connection string defined in app.config file.");
            }

            return _connection;
        }

        /// <summary>
        /// Method to execute a SQL query and return a dataset.
        /// </summary>
        /// <param name="query">Query string to be executed.</param>
        /// <returns>DataSet with the query results.</returns>
        protected override DataSet ExecuteQuery(string query)
        {
            var dataset = new DataSet();

            // Verify if number of entities match number of records.
            using (var adapter = new OleDbDataAdapter())
            {
                adapter.SelectCommand = new OleDbCommand(query, GetConnection());
                adapter.Fill(dataset);
            }

            return dataset;
        }
    }
}
