using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Sample05
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");
            //test_insert();
            //test_mult_insert();
            //test_del();
            //test_mult_del();
            //test_update();
            //test_mult_update();
            //test_select_one();
            //test_select_list();
            //insertComment();
            test_select_content_with_comment();
            Console.ReadLine();
        }
        //插入一条数据
        public static void test_insert()
        {
            var content=new Content()
            {
                id = 1,
                title = "标题",
                content = "内容",
                status = 1,
                add_time = DateTime.Now,
                modify_time = DateTime.Now
            };
            using (var conn=new SqlConnection("server=.;uid=sa;pwd=123456;database=Sample05;"))
            {
                string sql_insert = @"Insert into [Content](id,title, [content], status, add_time, modify_time) 
                values (@id,@title,@content,@status,@add_time,@modify_time)";
                var result = conn.Execute(sql_insert, content);
                Console.WriteLine($"test_insert:插入了{result}条数据");
            }
         

        }
        //插入多条数据
        public static void test_mult_insert()
        {
            var da = new List<Content>();
            for (int i = 5001; i < 50000; i++)
            {
                da.Add(new Content()
                {
                    id = i,
                    title = "标题2",
                    content = "内容2",
                    status = 1,
                    add_time = DateTime.Now,
                    modify_time = DateTime.Now
                });
            }
            
            
            
            using (var conn = new SqlConnection("server=.;uid=sa;pwd=123456;database=Sample05;"))
            {
                string sql_insert = @"Insert into [Content](id,title, [content], status, add_time, modify_time) 
                values (@id,@title,@content,@status,@add_time,@modify_time)";
                var result = conn.Execute(sql_insert, da);
                Console.WriteLine($"test_insert:插入了{result}条数据");
            }
        }
        //删除数据
        public static void test_del()
        {
            var content=new Content()
            {
                id = 0
            };
            using (var conn = new SqlConnection("server=.;uid=sa;pwd=123456;database=Sample05;"))
            {
                string sql_insert = @"Delete from [Content] where (id=@id)";
                var result = conn.Execute(sql_insert, content);
                Console.WriteLine($"test_insert:删除了{result}条数据");
            }
        }
        //删除多条数据
        public static void test_mult_del()
        {
            var da=new List<Content>();
            da.Add(new Content()
            {
                id = 1,
            });
            da.Add(new Content()
            {
                id = 2
            });
            using (var conn = new SqlConnection("server=.;uid=sa;pwd=123456;database=Sample05;"))
            {
                string sql_insert = @"Delete from [Content] where (id=@id)";
                var result = conn.Execute(sql_insert, da);
                Console.WriteLine($"test_insert:删除了{result}条数据");
            }
        }
        //修改一条书
        public static void test_update()
        {
            var content = new Content()
            {
                id = 1,
                title = "修改的标题",
                content = "修改的内容",
                
            };
            using (var conn = new SqlConnection("server=.;uid=sa;pwd=123456;database=Sample05;"))
            {
                string sql_insert = @"update [content] set title = @title, [content] = @content, modify_time = GETDATE()
where (id=@id)";
                var result = conn.Execute(sql_insert, content);
                Console.WriteLine($"test_insert:修改了{result}条数据");
            }
        }
        //修改多条数据
        public static void test_mult_update()
        {
            var contents=new List<Content>()
            {
                new Content()
                {
                    id = 1,
                    title = "批量修改数据",
                    content = "批量修改内容"
                },
                new Content()
                {
                    id = 3,
                    title = "批量修改标题",
                    content = "批量修改内容"
                }
            };
            using (var conn = new SqlConnection("server=.;uid=sa;pwd=123456;database=Sample05;"))
            {
                string sql_insert = @"update [content] set title = @title, [content] = @content, modify_time = GETDATE()
where (id=@id)";
                var result = conn.Execute(sql_insert, contents);
                Console.WriteLine($"test_insert:修改了{result}条数据");
            }
        }

        //查询一条数据
        public static void test_select_one()
        {
            using (var conn = new SqlConnection("server=.;uid=sa;pwd=123456;database=Sample05;"))
            {
                string sql_insert = @"select * from [dbo].[content] where id=@id";
                var result = conn.QueryFirstOrDefault<Content>(sql_insert, new{id=1});
                Console.WriteLine($"test_insert:查询到的数据为:");
            }
        }
        //查询多条数据
        public static void test_select_list()
        {
            using (var conn = new SqlConnection("server=.;uid=sa;pwd=123456;database=Sample05;"))
            {
                string sql_insert = @"select * from [dbo].[content]";
                var result = conn.Query<Content>(sql_insert);
                Console.WriteLine($"test_select_one：查到的数据为：");
            }
        }
        //插入Comment表
        public static void insertComment()
        {
            var commnet=new List<Comment>()
            {
                new Comment()
                {
                    id = 1,
                    content_id = 5,
                    content="测试1",
                   
                },
                new Comment()
                {
                    id = 2,
                    content_id = 5,
                    content="测试2",

                },
            };
            using (var conn = new SqlConnection("server=.;uid=sa;pwd=123456;database=Sample05;"))
            {
                string sql_insert = @"Insert into [Comment](id,content_id, [content], add_time) 
                values (@id,@content_id,@content,@add_time)";
                var result = conn.Execute(sql_insert, commnet);
                Console.WriteLine($"insertComment:插入了{result}条数据");
            }
        }

        public static void test_select_content_with_comment()
        {
            using (var conn = new SqlConnection("server=.;uid=sa;pwd=123456;database=Sample05;"))
            {
                string sql_insert = @"select * from content where id=@id;select* from comment where content_id=@id;";
                using (var result=conn.QueryMultiple(sql_insert,new{id=5}))
                {
                    var content = result.ReadFirstOrDefault<ContentWithCommnet>();
                    content.comments = result.Read<Comment>();
                    Console.WriteLine($"test_select_content_witg_comment:内容5的评论数量{content.comments.Count()}");
                }
            }
        }
    }
}
