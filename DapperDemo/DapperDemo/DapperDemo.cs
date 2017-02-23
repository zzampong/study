using DapperDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            ITableRepository repo = new TableRepository();

            //var tables = repo.GetAll();

            //var actual = tables.Count;
            ////var expect = 0;

            ////Assert.AreEqual(expect, actual);
            //TableViewModel model0 = new TableViewModel();
            //model0.Note = "추워요 호호.";
            //repo.Add(model0);

            //TableViewModel model = new TableViewModel();
            //model.Id = 3;
            //model.Note = "너무더워요..";

            //repo.Update(model);
            //TableViewModel rmodel = repo.GetById(3);

            //repo.Remove(3);

            //// 벌크인써트 테스트
            //var records = new List<TableViewModel> {
            //    new TableViewModel { Note="레코드1" },
            //    new TableViewModel { Note="레코드" }
            //};

            //int rcount = repo.BulkInsertRecords(records);

            //// 다중검색
            //var tables = repo.GetById(1, 2, 4, 5);

            //// 다이나믹 출력
            var tables = repo.GetDynamicAll();
            var firstNote = tables[0].Note;
            var firstNote1 = tables.First().Note;
            var lastNote = tables.Last().Note;

            //// 저장프로시져 호출
            //var rows = repo.GetAllWithSp();

            //foreach(var v in rows)
            //{
            //    Console.WriteLine($"Id:{v.Id}, Note:{v.Note}");
            //}

            //// 파라메터 사용 저장프로시져
            //var row1 = repo.GetByIdWithSp(4);

            //// 다이나믹 파라미터 사용 저장프로시져
            //var row2 = repo.GetByIdWithSpWithDynamic(5);

            //// 다중테이블 가져오기
            //var ts = repo.GetMutiData(5);
            //Console.WriteLine($"Id:{ts.Id}, Note:{ts.Note}");

            //foreach(var sub in ts.SubTableViewModels)
            //{
            //    Console.WriteLine($"subId:{sub.Id}, subNote:{sub.Note}, tableId:{sub.TableId}");
            //}

            //// 트랜잭션 다중삭제
            //repo.RemoveWith(5);

            // 검색조건 처리 Like 절 사용
            //var search = repo.SearchTablesByNote("구라");
            //foreach (var s in search)
            //{
            //    Console.WriteLine($"Id:{s.Id}, Note:{s.Note}");
            //}


            // 페이징처리
            var page = repo.GetAllWithPaging(1, 5);
            foreach(var p in page)
            {
                Console.WriteLine($"Id:{p.Id}, Note:{p.Note}");
            }

            // 총레코드수
            Console.WriteLine($"Total Count : { repo.GetTotalCount()}");

            // 저장프로시저 OUTPUT 매개변수
            var note1 = repo.GetTablesNoteByIdWithOutput(10);
            Console.WriteLine($"Note:{note1}");

            Console.ReadLine();

            
        }
    }
}
