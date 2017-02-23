using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Transactions;

namespace DapperDemo.Models
{
    public class TableViewModel
    {
        public TableViewModel()
        {
            SubTableViewModels = new List<SubTableViewModel>();
        }
        public int Id { get; set; }
        public string Note { get; set; }

        public List<SubTableViewModel> SubTableViewModels { get; set; }
    }

    public class SubTableViewModel
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public string Note { get; set; }
    }

    public interface ITableRepository
    {
        List<TableViewModel> GetAll();
        TableViewModel Add(TableViewModel model);
        TableViewModel GetById(int id);
        TableViewModel Update(TableViewModel model);
        void Remove(int id);
        int BulkInsertRecords(List<TableViewModel> records);
        List<TableViewModel> GetById(params int[] ids);
        List<dynamic> GetDynamicAll();
        List<TableViewModel> GetAllWithSp();
        TableViewModel GetByIdWithSp(int id);
        TableViewModel GetByIdWithSpWithDynamic(int id);

        TableViewModel GetMutiData(int id);
        void RemoveWith(int id);
        List<TableViewModel> SearchTablesByNote(string note);
        List<TableViewModel> GetAllWithPaging(int pageIndex, int pageSize);
        int GetTotalCount();
        string GetTablesNoteByIdWithOutput(int id);
    }

    public class TableRepository : ITableRepository
    {
        private IDbConnection db;

        public TableRepository()
        {
            db = new SqlConnection(ConfigurationManager.ConnectionStrings["DapperConnectStr"].ConnectionString);
        }

        public TableViewModel Add(TableViewModel model)
        {
            var sql = "INSERT INTO Tables(Note) VALUES(@Note); " +
                "SELECT CAST(SCOPE_IDENTITY() AS Int);";

            var id = db.Query<int>(sql, model).Single();
            model.Id = id;

            return model;
        }

        public List<TableViewModel> GetAll()
        {
            string str = "Select * From Tables";

            return db.Query<TableViewModel>(str).ToList();
        }

        public TableViewModel GetById(int id)
        {
            string sql = "SELECT * FROM Tables WHERE Id = @Id";

            return db.Query<TableViewModel>(sql, new { id }).SingleOrDefault();
        }

        public void Remove(int id)
        {
            string sql = "DELETE FROM Tables WHERE Id=@Id";
            db.Execute(sql, new { Id = id });
        }

        public TableViewModel Update(TableViewModel model)
        {
            var sql = "UPDATE Tables SET Note = @Note WHERE Id=@Id";
            db.Execute(sql, model);

            return model;
        }
        // 벌크인써트 (한번에 여러개 인써트)
        public int BulkInsertRecords(List<TableViewModel>records)
        {
            if (db.State != ConnectionState.Open)
            {
                db.Open();
            }

            var sql = "INSERT INTO Tables(Note) VALUES(@Note); " +
                "SELECT CAST(SCOPE_IDENTITY() AS Int);";

            return db.Execute(sql, records);
        }

        // 다중 레코드 검색
        public List<TableViewModel> GetById(params int[] ids)
        {
            string sql = "SELECT * FROM Tables WHERE Id In @Ids";

            return db.Query<TableViewModel>(sql, new { Ids=ids }).ToList();
        }

        // 다이나믹 출력
        public List<dynamic> GetDynamicAll()
        {
            string str = "Select * From Tables";

            return db.Query(str).ToList();
        }

        // 저장프로시저 사용
        public List<TableViewModel> GetAllWithSp()
        {
            string sql = "GetTables";

            return db.Query<TableViewModel>(sql, null, commandType: CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        ///  파라메터 사용 저장프로시저
        /// </summary>
        
        public TableViewModel GetByIdWithSp(int id)
        {
            string sql = "GetTableById";

            return db.Query<TableViewModel>(sql, new { Id = id }, commandType: CommandType.StoredProcedure).SingleOrDefault();
        }

        /// <summary>
        /// 다이나믹 파라미터 사용 저장프로시저 (참조에서 System.CSharp 추가)
        /// </summary>
        public TableViewModel GetByIdWithSpWithDynamic(int id)
        {
            string sql = "GetTableById";

            var parametes = new DynamicParameters();
            parametes.Add("@Id", value: id, dbType: DbType.Int32, direction: ParameterDirection.Input);

            return db.Query<TableViewModel>(sql, parametes, commandType: CommandType.StoredProcedure).SingleOrDefault();
            
            // parameters.Get<int>("@Id");
        }
        
        //// 다중 테이블에서 데이터 가져오기
        public TableViewModel GetMutiData(int id)
        {
            var sql = "Select * From Tables Where Id=@Id; " +
                "Select * From SubTables Where TableId=@Id;";

            using (var multiRecords = db.QueryMultiple(sql, new { Id = id }))
            {
                var table = multiRecords.Read<TableViewModel>().SingleOrDefault();
                var sub = multiRecords.Read<SubTableViewModel>().ToList();

                if (table != null && sub != null)
                {
                    table.SubTableViewModels.AddRange(sub);
                }
                return table;
            }
        }

        /// <summary>
        /// 다중삭제 트랜잭션  (참조에서 System.Transactions 추가)
        /// </summary>
        public void RemoveWith(int id)
        {
            using (var tran = new TransactionScope())
            {
                var sqlTables = "Delete Tables Where Id=@Id";
                db.Execute(sqlTables, new { Id = id });
                var sqlSublTables = "Delete SubTables Where TableId=@Id";
                db.Execute(sqlSublTables, new { Id = id });
                tran.Complete();
            };
            
        }

        /// <summary>
        /// 검색조건 처리시 Like 절 처리
        /// </summary>
        public List<TableViewModel> SearchTablesByNote(string note)
        {
            string sql = "Select * From Tables Where Note Like N'%' + @Note + '%' ";

            return db.Query<TableViewModel>(sql, new { Note = note }).ToList();
        }

        /// <summary>
        /// 페이징 처리
        /// </summary>
        public List<TableViewModel> GetAllWithPaging(int pageIndex, int pageSize=5)
        {
            string sql = "GetTablesWithPaging";

            var parametes = new DynamicParameters();
            parametes.Add("@PageIndex", value: pageIndex, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parametes.Add("@PageSize", value: pageSize, dbType: DbType.Int32, direction: ParameterDirection.Input);

            return db.Query<TableViewModel>(sql, parametes, commandType: CommandType.StoredProcedure).ToList();
        }

        // Tables 테이블 총레코드수 반환
        public int GetTotalCount()
        {
            string sql = "Select Count(*) From Tables";

            return db.Query<int>(sql).Single();
        }

        // OUTPUT 매개변수 저장프로시저
        public string GetTablesNoteByIdWithOutput(int id)
        {
            string sql = "GetTablesNoteByOutput";

            var parametes = new DynamicParameters();
            parametes.Add("@Id", value: id, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parametes.Add("@Note", value: "", dbType: DbType.String, direction: ParameterDirection.InputOutput);

            db.Execute(sql, parametes, commandType: CommandType.StoredProcedure);

            return parametes.Get<string>("@Note");
        }
    }
}
