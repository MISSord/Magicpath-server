using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Reflection;

namespace Server
{
    public class SqlManager
    {
        enum AllColumnName
        {
            SelectSkin, SkinFirst, SkinTwo, SkinThird, SkinFour
        }
        public  Class1 ClassOne = new Class1();

        public SqlManager _instance;
        SqlConnection myConn = new SqlConnection("Server='DESKTOP-4KTJ6F7';Integrated security=SSPI;database='GameData'; User ID ='sa'; Password ='123456'");
        public SqlManager()//构造函数
        {
            _instance = this;
        }
        
        public void ConnectionSql()//连接数据库
        {
            ClassOne.DataFormInitialization();
            try
            {
                myConn.Open();
                Console.WriteLine("DataBase is Opened Successfully");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void ShowMessage()
        {
            try
            {
                String str = "select Cno as 课号, Cname as 课名, COUNT(Cno) as 选课人数 from StudentCourse where Cno = 0702 group by Cno,Cname; ";
                SqlCommand myCommand = new SqlCommand(str, myConn);
                SqlDataReader reader = myCommand.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine(reader["课号"].ToString() + reader["课名"].ToString() + reader["选课人数"].ToString());
                }
                reader.Close();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void ShowAllMessage(string FormName)
        {
            string str = null;
            //根据需求选择语句
            str = "Select * from " + FormName + ";";
            try
            {
                //String str = "select * from UserBaseData";
                SqlCommand myCommand = new SqlCommand(str, myConn);
                SqlDataReader reader = myCommand.ExecuteReader();

                //显示表中数据的列名
                if(FormName == "UserBaseData")
                {
                    Console.WriteLine("ID" +"  " +"PassWord" + "  " + "RegisteredTime" + "  " + "Name");
                }
                else if(FormName == "UserDataNumble")
                {
                    Console.WriteLine("ID" + "  " + "DiamondCount" + "  " + "BestScoreOne" + "  " + "BestScoreSecond" + "  " + "BestScoreThird");
                }
                else if(FormName == "UserDataSkin")
                {
                    Console.WriteLine("ID" + "  " + "SelectSkin" + "  " + "SkinFirst" + "  " + "SkinTwo" + "  " + "SkinThird" + "  " + "SkinFour");
                }
                //显示表中数据
                while (reader.Read())
                {
                    if(FormName == "UserBaseData")
                    {
                        Console.WriteLine(reader["ID"].ToString() + reader["PassWord"].ToString() + reader["RegisteredTime"].ToString() + reader["Name"].ToString());
                    }
                    else if (FormName == "UserDataNumble")
                    {
                        Console.WriteLine(reader["ID"].ToString() + reader["DiamondCount"].ToString() +"  " + reader["BestScoreOne"].ToString() + "  " + reader["BestScoreSecond"].ToString() + "  " + reader["BestScoreThird"].ToString());
                    }
                    else if(FormName == "UserDataSkin")
                    {
                        Console.WriteLine(reader["ID"].ToString() + reader["SelectSkin"].ToString() + "  " + reader["SkinFirst"].ToString() + "  " + reader["SkinTwo"].ToString() + "  " + reader["SkinThird"].ToString() + "  " + reader["SkinFour"].ToString());
                    }
                    
                }
                reader.Close();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void ShowSingleMessage(string SQL)
        {
            SqlCommand myCommand = new SqlCommand(SQL, myConn);
            SqlDataReader reader = myCommand.ExecuteReader();

            reader.Close();
        }

        public void UpdateMessage(string ColumnName, string a, string ID)
        {
            string FormName = ClassOne.FindForm(ColumnName);
            string str;
            str = "Update " + FormName + " SET " + ColumnName + " = '" + a + "' where ID = '" + ID.ToString() + "';";
            try
            {
                Console.WriteLine(str);
                SqlCommand myCommand = new SqlCommand(str, myConn);
                SqlDataReader reader = myCommand.ExecuteReader();
                reader.Close();
                ShowAllMessage(FormName);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public void InsertMessage(string ID, string PassWord, string Time, string Name)
        {
            string str = null;
            str = "Insert into UserBaseData Values (" + "'" + ID + "','" + PassWord + "','" + Time + "','" + Name + "');";
            try
            {
                SqlCommand myCommand = new SqlCommand(str, myConn);
                SqlDataReader reader = myCommand.ExecuteReader();
                reader.Close();
                ShowAllMessage("UserBaseData");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void InsertMessage(string ID, string DiamondCount, string BestScore, string BestScoreSecond, string BestScoreThird)
        {
            string str = null;
            str = "Insert into UserDataNumble Values (" + "'" + ID + "','" + DiamondCount + "' ,'" + BestScore + "','" + BestScoreSecond+ "','" + BestScoreThird + "');";
            try
            {
                SqlCommand myCommand = new SqlCommand(str, myConn);
                SqlDataReader reader = myCommand.ExecuteReader();
                reader.Close();
                ShowAllMessage("UserDataNumble");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void InsertMessage(string ID, string SelectSkin, string SkinFirst, string SkinTwo, string SkinThird, string SkinFour)
        {
            string str = null;
            str = "Insert into UserDataSkin Values (" + "'" + ID + "','" + SelectSkin + "','" + SkinFirst + "','" + SkinTwo + "','" + SkinThird + "','" + SkinFour + "');";
            try
            {
                SqlCommand myCommand = new SqlCommand(str, myConn);
                SqlDataReader reader = myCommand.ExecuteReader();
                reader.Close();
                ShowAllMessage("UserDataSkin");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public string SplitString(string Message)
        {
            string[] stringArrFirst = Message.Split("|");
            int LengthArr = stringArrFirst.Length - 1;
            int i = 0;
            if (stringArrFirst[LengthArr] == "1")
            {
                i = 1;
            }
            else if(stringArrFirst[LengthArr] == "2")
            {
                i = 2;
            }
            int s = 0;
            foreach (string Ina in stringArrFirst)
            {
                string[] stringArr = Ina.Split(",");
                if(Ina == "" || Ina == null)
                {
                    break;
                }
                s++;
                if (stringArr[0].Contains("114514"))
                {
                    return "114514";
                }
                Console.WriteLine("执行次数:"+ s);
                int a = stringArr.Length;
                Console.WriteLine(a.ToString());
                Console.WriteLine(Ina);
                string str = null;
                if(i == 1)
                {
                    str = InsterID(stringArr);
                    return str;
                }
                else if (i == 2)
                {
                    str = SelectID(stringArr);
                    return str;
                }
                switch (a)
                {
                    case 1: Console.WriteLine(stringArr[0]); break;

                    case 4: UpdateMessage(stringArr[0], stringArr[1], stringArr[3]); break;

                    case 8: InsertMessage(stringArr[1], stringArr[3], stringArr[5], stringArr[7]); break;

                    case 10: InsertMessage(stringArr[1], stringArr[3], stringArr[5], stringArr[7], stringArr[9]); break;

                    case 12: InsertMessage(stringArr[1], stringArr[3], stringArr[5], stringArr[7], stringArr[9], stringArr[11]); break;

                    default: Console.WriteLine("切分出错"); break;
                }
            }
            return "";
        }

        public string SelectID(string[] arr)
        {
            string ID = arr[1];
            string str = "Select * from UserBaseData where ID = " + ID;
            Console.WriteLine(str);
            SqlCommand myCommand = new SqlCommand(str, myConn);
            SqlDataReader reader = myCommand.ExecuteReader();
            while (reader.Read())
            {
                string strOne = reader["ID"].ToString();
                Console.WriteLine(strOne);
                if(strOne!= null || strOne != "")
                {
                    Console.WriteLine("重复ID");
                    string strTwo = reader["PassWord"].ToString();
                    int a = int.Parse(strTwo);
                    Console.WriteLine(strTwo);

                    int PassWord = int.Parse(arr[3]);
                    Console.WriteLine(arr[3]);

                    if (a == PassWord)
                    {
                        Console.WriteLine("密码正确");
                        reader.Close();
                        string One = SendData(strOne);
                        return One;
                        
                    }
                    else
                    {
                        Console.WriteLine("密码错误");
                    }
                    reader.Close();
                    return "密码错误";
                }
            }
            reader.Close();
            return "登录失败";
        }

        private string InsterID(string[] arr)
        {
            string ID = arr[1];
            string str = "Select * from UserBaseData where ID = " + ID;
            Console.WriteLine(str);
            SqlCommand myCommand = new SqlCommand(str, myConn);
            SqlDataReader reader = myCommand.ExecuteReader();
            while (reader.Read())
            {
                string strOne = reader["ID"].ToString();
                Console.WriteLine(strOne);
                if (strOne != null || strOne != "")
                {
                    Console.WriteLine("重复ID");
                    reader.Close();
                    return "注册失败，已存在账号";
                }
            }
            Console.WriteLine("执行插入ID");
            reader.Close();
            InsertMessage(arr[1], arr[3], arr[5], arr[7]);
            Console.WriteLine("插入ID成功");

            InsertMessage(arr[1], "10", "0", "0", "0");
            InsertMessage(arr[1], "0", "true", "false", "false", "false");
            return "注册成功";
        }

        private string SendData(string ID)
        {
            string FinalMessage = null;

            string FirstMessageFind = "Select * from UserDataNumble where ID = " + ID;
            string SecondMessageFind = "Select * from UserDataSkin where ID = " + ID;
            for(int i = 0; i < 2; i++)
            {
                Console.WriteLine("执行次数：" + i);
                try
                {
                    if(i == 0)
                    {
                        FirstMessageFind = "Select * from UserDataNumble where ID = " + ID;
                        SqlCommand myCommand = new SqlCommand(FirstMessageFind, myConn);
                        SqlDataReader reader = myCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            FinalMessage += reader["ID"].ToString() + "," + reader["DiamondCount"].ToString() + "," + reader["BestScoreOne"].ToString() + ","
                                + reader["BestScoreSecond"].ToString() + "," + reader["BestScoreSecond"].ToString() + "|";
                            break;
                        }
                        reader.Close();
                    }
                    else if(i == 1)
                    {
                        FirstMessageFind = "Select * from UserDataSkin where ID = " + ID;
                        SqlCommand myCommand = new SqlCommand(FirstMessageFind, myConn);
                        SqlDataReader reader = myCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            FinalMessage += reader["ID"].ToString() + "," + reader["SelectSkin"].ToString().ToString() + "," + reader["SkinFirst"].ToString().ToString() + ","
                                + reader["SkinTwo"].ToString().ToString() + "," + reader["SkinThird"].ToString().ToString() + ","+ reader["SkinFour"].ToString().ToString() + "|";
                            
                            break;
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine(FinalMessage);
            return FinalMessage;
        }
    }
}
