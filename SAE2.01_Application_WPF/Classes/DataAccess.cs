using Npgsql;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Utilities;
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
            soren_connectionString = "Host=srv-peda-new;Port=5433;Username=postgres;Password=rCupUg;Database=SAE201_thomasso;Options='-c search_path=thomasso'";
            vincent_connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=;Database=SAE201";
            hasnaoui_connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=;Database=SAE201";
        }

        public DataAccess(string login, string password)
        {
            this.Login = login;
            this.Password = password;

            string ConnectionStr = $"Host=srv-peda-new;Port=5433;Username=hakima;Password={password};Database=sae201_basicfit;Options='-c search_path=basicfit_schema'";

            try
            {
                connection = new NpgsqlConnection(soren_connectionString);
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT role FROM users WHERE username = @login", connection))
                {
                    cmd.Parameters.AddWithValue("@login", login);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            this.Role = reader["role"].ToString();
                        }
                    }
                }
            }
            catch (PostgresException ex) when (ex.SqlState == "28P01")
            {
                LogError.Log(ex, "Authentification échouée : Identifiants Postgres incorrects.\n");
                throw new Exception("Nom d'utilisateur ou mot de passe Postgres incorrect.");
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur lors de la connexion initiale.\n");
                throw;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public static NpgsqlConnection GetConnection()
        {
            if (connection == null)
            {
                throw new InvalidOperationException("La connexion n'a pas été initialisée. Veuillez d'abord instancier DataAccess.");
            }

            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
            {
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    LogError.Log(ex, "Pb à la connexion \n");
                    throw;
                }
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

        public static int ExecuteInsert(NpgsqlCommand cmd)
        {
            int nb = 0;
            try
            {
                cmd.Connection = GetConnection();
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    nb = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb de executeInsert \n" + cmd.CommandText);
                throw;
            }
            return nb;
        }

        public static int ExecuteUpdate(NpgsqlCommand cmd)
        {
            int nb = 0;
            try
            {
                cmd.Connection = GetConnection();
                nb = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb de executeUpdate \n" + cmd.CommandText);
                throw;
            }
            return nb;
        }

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
            return res?.ToString() ?? string.Empty;
        }

        public static void CloseConnection()
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }


    }
}
