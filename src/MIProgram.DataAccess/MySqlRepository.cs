using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MIProgram.Model;
using MySql.Data.MySqlClient;

namespace MIProgram.DataAccess
{
    public class MySqlRepository
    {
        private readonly string _strConnexion;

        public MySqlRepository(string strConnexion)
        {
            _strConnexion = strConnexion;
        }        

        public static bool TestDbAccess(string connectionString, ref string message)
        {
            MySqlConnection oConnection = null;
            var result = true;
            try
            {
                oConnection = new MySqlConnection(connectionString);
                oConnection.Open();
            }
            catch(Exception e)
            {
                result = false;
                message = e.Message;
            }
            finally
            {
                if (oConnection != null && oConnection.State != ConnectionState.Closed)
                {
                    oConnection.Close();
                }
            }

            return result;
        }

        public IList<MIDBRecord> GetAllRecords()
        {
            var results = new List<MIDBRecord>();

            string strRequete = "SELECT TOP 10 * FROM mi_reviews";
            strRequete += " WHERE pn_id=3910";
            MySqlConnection oConnection = null;
            try
            {
                oConnection = new MySqlConnection(_strConnexion);
                oConnection.Open();
               
                // Chargement de la liste des catégories dans oDataSet
                MySqlDataAdapter oSqlDataAdapter = new MySqlDataAdapter(strRequete, oConnection);

                DataSet oDataSet = new DataSet("mi_reviews");
                oSqlDataAdapter.Fill(oDataSet, "mi_reviews");
               
                // Affichage du contenu de oDataSet avant insertion de données

                foreach (DataRow line in oDataSet.Tables["mi_reviews"].Rows)
                {
                    results.Add(new MIDBRecord(line[0].ToString(), line[1].ToString(), line[2].ToString(), line[3].ToString(), line[4].ToString(), line[5].ToString(), line[6].ToString(), line[7].ToString(), line[8].ToString(), line[9].ToString(), line[10].ToString(), line[11].ToString(), line[12].ToString(), line[13].ToString()));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("L'erreur suivante a été rencontrée :" + e.Message);
            }
            finally
            {
                if (oConnection != null && oConnection.State != ConnectionState.Closed)
                {
                    oConnection.Close();
                }
            }
            return results;
        }
    }
}
