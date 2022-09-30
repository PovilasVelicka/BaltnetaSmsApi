using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaltnetaSmsApi.Entities
{
    internal class Sms
    {
        public string Login { get; private set; }
        public string Phone { get; private set; }
        public string Sender { get; private set; }
        public string Text { get; private set; }
        public int TimeStamp { get; private set; }

        public Sms (string login, string phone, string sender, string text)
        {
            Login = login;
            Phone = phone;
            Sender = sender;
            Text = text;
            TimeStamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public override string ToString ( )
        {
            return string.Format("{0}{1}{2}{3}{4}", Login, Phone, Sender, Text, TimeStamp);
        }
    }
}
