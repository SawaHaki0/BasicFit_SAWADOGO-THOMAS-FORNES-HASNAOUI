using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SAE2._01_Application_WPF.Classes
{
    public class DataAccess
    {

        private static readonly string connectionString;
        private static readonly string hakim_connectionString;
        private static readonly string soren_connectionString;
        private static readonly string vincent_connectionString;
        private static readonly string hasnaoui_connectionString;
        private static NpgsqlConnection connection;




        static DataAccess()
        {
            hakim_connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=Ncxkk3pxfd6@;Database=SAE201_BasiFit";
            soren_connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=;Database=SAE201";
            vincent_connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=;Database=SAE201";
            hasnaoui_connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=;Database=SAE201";
            connectionString = "Host=srv-peda-new;Port=5433;Username=hakima;Password=ZBJvmN;Database=SAE201_BasicFit_TD4;Options='-c search_path=basicfit_schema'";
            try
            {
                connection = new NpgsqlConnection(hakim_connectionString);
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb à la connexion  \n");
                throw;
            }
        }


        // pour récupérer la connexion (et l'ouvrir si nécessaire)
        public static NpgsqlConnection GetConnection()
        {

            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)

                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    LogError.Log(ex, "Pb à la connexion  \n");
                    throw;
                }

            return connection;
        }

        public static bool TestConnection()
        {
            try
            {
                NpgsqlConnection testConn = new NpgsqlConnection(connectionString);
                testConn.Open();
                testConn.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur détaillée", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        public static DataTable ExecuteSelect(NpgsqlCommand cmd)
        {
            DataTable dataTable = new DataTable();
            try
            {
                cmd.Connection = GetConnection();
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb de executeSelect \n" + cmd.CommandText);
                throw;
            }

            return dataTable;
        }

        //   pour requêtes INSERT et renvoie l'ID généré

        public static int ExecuteInsert(NpgsqlCommand cmd)
        {
            int nb = 0;
            try
            {
                cmd.Connection = GetConnection();
                nb = (int)cmd.ExecuteScalar();

            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb de executeInsert \n" + cmd.CommandText);
                throw;
            }
            return nb;

        }




        //  pour requêtes UPDATE, DELETE
        public static int ExecuteSet(NpgsqlCommand cmd)
        {
            int nb = 0;
            try
            {
                cmd.Connection = GetConnection();
                nb = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb de executeSet \n" + cmd.CommandText);
                throw;
            }
            return nb;

        }

        // pour requêtes avec une seule valeur retour  (ex : 1 colonne, ou COUNT, SUM) 
        public static string ExecuteSelectOneValue(NpgsqlCommand cmd)
        {
            object res = null;
            try
            {
                cmd.Connection = GetConnection();
                res = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb de ExecuteSelectOneValue \n" + cmd.CommandText);
                throw;
            }
            return res.ToString();

        }

        //  Fermer la connexion 
        public static void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
