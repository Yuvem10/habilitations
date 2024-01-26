using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Habilitations.bddmanager
{
    class BddManager
    {
        private static BddManager instance = null;
        private readonly MySqlConnection connection;

        private BddManager(string stringConnect)
        {
            connection = new MySqlConnection(stringConnect);
            connection.Open();
        }

        public static BddManager GetInstance(string stringConnect)
        {
            if (instance == null)
            {
                instance = new BddManager(stringConnect);
            }
            return instance;
        }

        public void ReqUpdate(string stringQuery, Dictionary<string, object> parameters = null)
        {
            MySqlCommand command = new MySqlCommand(stringQuery, connection);
            if (!(parameters is null))
            {
                foreach (KeyValuePair<string, object> element in parameters)
                {
                    command.Parameters.Add(new MySqlParameter(element.Key, element.Value));
                }
            }
            command.Prepare();
            command.ExecuteNonQuery();
        }
        public List<Object[]> ReqSelect(string stringQuery, Dictionary<string, object> parameters = null)
        {
            MySqlCommand command = new MySqlCommand(stringQuery, connection);
            if (!(parameters is null))
            {
                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    command.Parameters.Add(new MySqlParameter(parameter.Key, parameter.Value));
                }
            }
            command.Prepare();
            MySqlDataReader reader = command.ExecuteReader();
            int nbCols = reader.FieldCount;
            List<Object[]> records = new List<object[]>();
            while (reader.Read())
            {
                Object[] attributs = new Object[nbCols];
                reader.GetValues(attributs);
                records.Add(attributs);
            }
            reader.Close();
            return records;
        }

    }



}

