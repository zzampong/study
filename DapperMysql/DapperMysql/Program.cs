using DapperMysql.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperMysql
{
    class Program
    {
        static void Main(string[] args)
        {
            IGameInfoRepository grepo = new GameInfoRepository();

            var tables = grepo.GetAll();

            foreach (var i in tables)
            {
                Console.WriteLine($"uid:{i.uid}, coin:{i.coin}, cash:{i.cash}, nut:{i.nut}");
            }

            DrGameInfo data = new DrGameInfo();
            data.uid = 125;
            data.coin = 343;
            data.cash = 333;
            data.nut = 10;

            //DrGameInfo rdata = grepo.Add(data);

            //rdata.coin = 1999;
            //rdata.cash = 199;

            //grepo.Update(rdata);

            //DrGameInfo get = grepo.GetById(rdata.uid);

            grepo.Remove(125);

            Console.ReadLine();
        }
    }
}
