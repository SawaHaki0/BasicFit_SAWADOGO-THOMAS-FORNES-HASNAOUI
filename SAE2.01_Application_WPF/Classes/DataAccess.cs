using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        private static readonly string connectionString;
        private static readonly string hakim_connectionString;
        private static readonly string soren_connectionString;
        private static readonly string vincent_connectionString;
        private static readonly string hasnaoui_connectionString;



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

        public string Login
        {
            get
            {
                return this.login;
            }

            set
            {
                this.login = value;
            }
        }

        static DataAccess()
        {
            hakim_connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=Ncxkk3pxfd6@;Database=SAE201_BasiFit;Options='-c search_path=public'";
            soren_connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=;Database=SAE201";
            vincent_connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=;Database=SAE201";
            hasnaoui_connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=;Database=SAE201";
            connectionString = ConfigurationManager.ConnectionStrings["ConnexionDB"].ConnectionString;
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

        public DataAccess(string login, string password)
        {
            this.Login = login;
            this.Password = password;

            connection.Open();
            using (var cmd = new NpgsqlCommand(
                "SELECT password, role FROM users WHERE username = @login", connection))
            {
                cmd.Parameters.AddWithValue("@login", login);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string dbPassword = reader["password"].ToString();
                        string dbRole = reader["role"].ToString();


                        if (password == dbPassword)
                        {
                            this.role = dbRole;
                        }
                        else
                        {
                            throw new Exception("Invalid password.");
                        }
                    }
                    else
                    {
                        throw new Exception("User not found.");
                    }
                }
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
