using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SAE2._01_Application_WPF.Classes
{
    public class DataAccess
    {
        private static NpgsqlConnection connection;

        private string login;
        private string password;
        private string role;

        public string Login
        {
            get
            {
                return login;
            }

            set
            {
                login = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public string Role
        {
            get
            {
                return this.role;
            }

            set
            {
                this.role = value;
            }
        }

        public DataAccess(string login, string password)
        {
            this.Login = login;
            this.Password = password;

            string ConnectionString = $"Host=127.0.0.1;Port=5432;Username={login};Password={password};Database=SAE201_BasiFit";
            connection = new NpgsqlConnection(ConnectionString);

            connection.Open();
            using (var cmd = new NpgsqlCommand("SELECT current_user", connection))
            {
                string user = cmd.ExecuteScalar().ToString();
                if (user.StartsWith("responsable"))
                    role = "responsable_club";
                else if (user.StartsWith("employe"))
                    role = "employe";
            }
            connection.Close();
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
