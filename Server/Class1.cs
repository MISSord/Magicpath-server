using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class Class1
    {
        Class1 ClassOne;

        public Class1()
        {       
            ClassOne = this;
        }

        Dictionary<string, string> DataForm = new Dictionary<string, string>();

        public void DataFormInitialization()
        {
            DataForm.Add("PassWord", "UserBaseData");
            DataForm.Add("RegisteredTime", "UserBaseData");
            DataForm.Add("Name", "UserBaseData");
            DataForm.Add("ID", "UserBaseData");

            DataForm.Add("DiamondCount", "UserDataNumble");
            DataForm.Add("BestScoreOne", "UserDataNumble");
            DataForm.Add("BestScoreSecond", "UserDataNumble");
            DataForm.Add("BestScoreThird", "UserDataNumble");

            DataForm.Add("SelectSkin", "UserDataSkin");
            DataForm.Add("SkinFirst", "UserDataSkin");
            DataForm.Add("SkinTwo", "UserDataSkin");
            DataForm.Add("SkinThird", "UserDataSkin");
            DataForm.Add("SkinFour", "UserDataSkin");
        }

        public string FindForm(string ColumnName)
        {
            string FormName = null;
            if (DataForm.ContainsKey(ColumnName))
            {
                foreach(KeyValuePair<string,string> Find in DataForm)
                {
                    //Console.WriteLine(Find.Key, Find.Value);
                    if(Find.Key == ColumnName)
                    {
                        FormName = Find.Value;
                        break;
                    }
                }
            }
            return FormName;
        }
    }
}
