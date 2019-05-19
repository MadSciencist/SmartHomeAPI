//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Threading.Tasks;
//using SmartHome.Core.DataAccess;

//namespace SmartHome.DataAccess.Tests.Setup
//{
//    public class InMemoryTestDb
//    {
//        public static async Task Run(Func testFunc)
//        {
//            var options = new DbContextOptionsBuilder<AppDbContext>()
//                .UseInMemoryDatabase("IN_MEMORY_DATABASE")
//                .Options;

//            using (var context = new AppDbContext(options))
//            {
//                await context.Database.EnsureCreatedAsync();
//                //PrepareTestDatabase(context);
//                await testFunc(context);

//                CleanupTestDatabase(context);
//            }
//        }

//        public static void CleanupTestDatabase(AppDbContext context)
//        {
//            if (context.Database.IsInMemory())
//            {
//                context.Database.EnsureDeleted();
//            }
//        }
//    }
//}
