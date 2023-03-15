using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 异步服务器
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using MySql.Data.MySqlClient;

    public class MySqlHelper
    {
        string zen = "INSERT INTO `name`.`account` (`id`, `username`, `password`) VALUES ('10004', 'cnm', '1234');";

        string shan = "DELETE FROM `name`.`account` WHERE (`id` = '10002');";

        string gai = "UPDATE `name`.`account` SET `username` = '1111' WHERE (`id` = '10002');";

        string cha = "SELECT * FROM account where id = 10001";

        string chuanjianbiao = "CREATE TABLE name.acc (id INT UNSIGNED NOT NULL AUTO_INCREMENT,username VARCHAR(45) NOT NULL,password VARCHAR(45) NOT NULL,PRIMARY KEY(id))ENGINE = InnoDB DEFAULT CHARACTER SET = utf8; ";

        string shanchubiao = "DROP TABLE `name`.`acc`;";

        string chuangjiansql = "CREATE SCHEMA user111 DEFAULT CHARACTER SET utf8 COLLATE utf8_bin ;";

        string shanchusql = "DROP DATABASE user111;";

        static string sqlStr = "server = 127.0.0.1; port = 3306; user = root; password = root;database = name; charset=utf8";

        

        public  static string Name;
        public string tableName;
        /// <summary>
        /// 数据库工具类初始化
        /// </summary>
        /// <param name="name">数据库名</param>
        /// <param name="tableName">表名</param>
        public MySqlHelper(string name, string tableName)
        {
            Name = name;
            this.tableName = tableName;
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <returns></returns>
        public MySqlConnection Open()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(OpenSql());
                connection.Open();
                Console.WriteLine($"与数据库{Name}链接成功....");
                return  connection;
            }
            catch (Exception)
            {
                Console.WriteLine("连接失败...");
                throw;
            }
        }

        

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public MySqlDataReader Command(string sql,MySqlConnection conn)
        {
            if (string.IsNullOrEmpty(sql))
            {
                sql=sqlCheck("id","10001");
            }
            try
            {
                //向目标数据库输入sql语句
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                //执行sql语句
                return cmd.ExecuteReader();
            }
            catch (Exception)
            {
                Console.WriteLine(sql+"语法不正确");
                throw;
            }
        }


        /// <summary>
        /// 获取电脑上所有数据库信息
        /// </summary>
        public void GetDatabases(MySqlConnection conn)
        {
            DataTable dt = conn.GetSchema("Databases",null);
            foreach (DataRow dr in dt.Rows)
            {
                Console.WriteLine(
                    $"数据库名：{dr["DATABASE_NAME"]}，" +
                    $"字符集：{dr["DEFAULT_CHARACTER_SET_NAME"]}，" +
                    $"排序规则：{dr["DEFAULT_COLLATION_NAME"]}");
            }
        }

        /// <summary>
        /// 获取数据库name的所有表信息
        /// </summary>
        /// <param name="name">数据库名称</param>
        public void GetTables(MySqlConnection conn)
        {
            string[] restrictionValues = new string[4];
            restrictionValues[0] = null;        // Catalog
            restrictionValues[1] = Name;        // Owner
            restrictionValues[2] = null;        // Table
            restrictionValues[3] = null;        // Column
            DataTable dt = conn.GetSchema("Tables", restrictionValues);
            foreach (DataRow dr in dt.Rows)
            {
                Console.WriteLine(
                    $"表名：{dr["TABLE_NAME"]}，" +
                    $"创建时间：{dr["CREATE_TIME"]}，" +
                    $"排序规则：{dr["TABLE_COLLATION"]}，" +
                    $"备注：{dr["TABLE_COMMENT"]}");
            }
        }



        /// <summary>
        /// 获取数据库name表的所有字段名，及长度
        /// </summary>
        /// <param name="database">数据库</param>
        /// <param name="table">表或视图</param>
        public string[] GetColumns(MySqlConnection conn)
        {
            string[] restrictionValues = new string[4];
            restrictionValues[0] = null;        // Catalog
            restrictionValues[1] = Name;        // Owner
            restrictionValues[2] = tableName;   // Table
            restrictionValues[3] = null;        // Column
            DataTable dt = conn.GetSchema("Columns", restrictionValues);
            //存储表的字段
            string[] keyData = new string[dt.Rows.Count];
            for (int i = 0; i < keyData.Length; i++)
            {
                DataRow dr = dt.Rows[i];
                Console.WriteLine(
                    $"字段名：{dr["COLUMN_NAME"]}，" +
                    $"默认值：{dr["COLUMN_DEFAULT"]}，" +
                    $"可空：{dr["IS_NULLABLE"]}，" +
                    $"类型：{dr["DATA_TYPE"]}，" +
                    $"文本长度：{dr["CHARACTER_MAXIMUM_LENGTH"]}，" +
                    $"数字精度：{dr["NUMERIC_PRECISION"]}，" +
                    $"小数位数：{dr["NUMERIC_SCALE"]}，" +
                    $"时间精度：{dr["DATETIME_PRECISION"]}，" +
                    $"字符集：{dr["CHARACTER_SET_NAME"]}，" +
                    $"排序规则：{dr["COLLATION_NAME"]}，" +
                    $"字段类型：{dr["COLUMN_TYPE"]}，" +
                    $"键类型：{dr["COLUMN_KEY"]}，" +
                    $"扩展：{dr["EXTRA"]}，" +
                    $"备注：{dr["COLUMN_COMMENT"]}");
                keyData[i] = dr["COLUMN_NAME"].ToString();
            }
            
            return keyData;
        }
        private void GetValue(string sql, MySqlConnection connection)
        {
            
        }

        /// <summary>
        /// 返回打开sql语句
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">端口号</param>
        /// <param name="username">账号</param>
        /// <param name="parsword">密码</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string OpenSql(string ip = "127.0.0.1", string port = "3306", string username = "root", string parsword = "root", string charset = "utf8")
        {
            string sqlStr = $"server = {ip}; port = {port}; user = {username}; password = {parsword};database = {Name}; charset={charset}";
            return sqlStr;
        }

        /// <summary>
        /// 返回增加语句
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string sqlAdd(string[] values, MySqlConnection conn)
        {
            string[] Keys = GetColumns(conn);
            if (values.Length<Keys.Length)
            {
                Console.WriteLine("参数数量不正确");
                return "";
            }
            string[] str = new string[2];
            for (int i = 0; i < values.Length; i++)
            {
                str[0] += $"'{Keys[i]}'";
                str[1] += $"'{values[i]}'";
                if (i == Keys.Length - 1)
                {
                    continue;
                }
                str[0] += ",";
                str[1] += ",";
            }
            string add = $"INSERT INTO `{Name}`.`{tableName}` ({str[0]}) VALUES ({str[1]});";
            return add;
        }
        /// <summary>
        /// 返回删除语句
        /// </summary>
        /// <param name="primaryKeyId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string sqlDelete( string primaryKeyId, string value)
        {
            string shan = $"DELETE FROM `{Name}`.`{tableName}` WHERE (`{primaryKeyId}` = '{value}');";
            return shan;
        }
        /// <summary>
        /// 返回修改语句
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keyValue"></param>
        /// <param name="primaryKeyId"></param>
        /// <param name="primaryValue"></param>
        /// <returns></returns>
        public string sqlModify(string key, string keyValue, string primaryKeyId, string primaryValue)
        {
            string gai = $"UPDATE `{Name}`.`{tableName}` SET `{key}` = '{keyValue}' WHERE (`{primaryKeyId}` = '{primaryValue}');";
            return gai;
        }

        /// <summary>
        /// 返回查询语句
        /// </summary>
        /// <param name="primaryKeyId"></param>
        /// <param name="primaryValue"></param>
        /// <returns></returns>
        public string sqlCheck(string primaryKeyId, string primaryValue)
        {
            string cha = $"SELECT * FROM {tableName} where {primaryKeyId} = {primaryValue}";
            return cha;
        }
    }
}
