using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudRedactorPages
{
    public class Database
    {
        private static NpgsqlConnection Connection;
        public static void Connect(string host, string port, string user, string pass, string database)
        {
            string cs = string.Format("Server={0}; Port={1}; User Id={2}; Password={3}; Database={4}", host, port, user, pass, database);

            Connection = new NpgsqlConnection(cs);
            Connection.Open();
        }

        public static List<string> positions { get; set; } = new List<string>();
        public static ObservableCollection<ClSpeciality> specializations { get; set; } = new ObservableCollection<ClSpeciality>();
        public static ObservableCollection<ClGroup> groups { get; set; } = new ObservableCollection<ClGroup>();
        public static ObservableCollection<ClStudent> students { get; set; } = new ObservableCollection<ClStudent>();
        public static ObservableCollection<ClEmployee> employees { get; set; } = new ObservableCollection<ClEmployee>();
        public static NpgsqlCommand GetCommand(string sql)
        {
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = Connection;
            command.CommandText = sql;
            return command;
        }
    }
}
