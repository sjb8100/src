using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;

public class MailDefine
    {

    public class MailLocalData 
    {
        private uint mailItemId;
        public static MailLocalData Create(uint mailItemId)
        {
            MailLocalData data = new MailLocalData();
            data.UpdateData(mailItemId);
            return data;
         }

        /// <summary>
        /// 根据邮件ID更新邮件本地数据类
        /// </summary>
        /// <param name="mailItemId"></param>
        public void UpdateData(uint mailItemId) 
        {
            this.mailItemId = mailItemId;
        }


    }

    
    }

