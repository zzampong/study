using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperMysql.Models
{
    public class DrGameInfo
    {
        public long uid { get; set; }
        public int coin { get; set; }
        public int cash { get; set; }
        public int nut { get; set; }
    }
    public interface IGameInfoRepository
    {
        List<DrGameInfo> GetAll();
        DrGameInfo GetById(long id);
        DrGameInfo Update(DrGameInfo model);
        DrGameInfo Add(DrGameInfo model);
        void Remove(long id);
    }

    public class GameInfoRepository : IGameInfoRepository
    {
        private IDbConnection db;

        public GameInfoRepository()
        {
            db = new MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings["DapperConnectStr"].ConnectionString);
        }

        public DrGameInfo Add(DrGameInfo model)
        {
            string sql = "INSERT INTO dr_gameinfo(uid, coin, cash, nut) VALUES(@uid, @coin, @cash, @nut); " + 
                "SELECT last_insert_id();";
            var id = Dapper.SqlMapper.Query<long>(db, sql, model).Single();
            //Dapper.SqlMapper.Execute(db, sql, model);

            return model;
        }

        public List<DrGameInfo> GetAll()
        {
            string sql = "SELECT * FROM dr_gameinfo";

            return Dapper.SqlMapper.Query<DrGameInfo>(db, sql).ToList();
        }

        public DrGameInfo GetById(long id)
        {
            string sql = "SELECT * FROM dr_gameinfo WHERE uid = @uid";

            return Dapper.SqlMapper.Query<DrGameInfo>(db, sql, new { uid = id }).SingleOrDefault();
        }

        public void Remove(long id)
        {
            string sql = "DELETE FROM dr_gameinfo WHERE uid=@Id";
            Dapper.SqlMapper.Execute(db, sql, new { Id = id });
        }

        public DrGameInfo Update(DrGameInfo model)
        {
            string sql = "UPDATE dr_gameinfo SET coin = @coin, cash=@cash, nut=@nut WHERE uid=@uid";
            Dapper.SqlMapper.Execute(db, sql, model);

            return model;
        }
    }
}
