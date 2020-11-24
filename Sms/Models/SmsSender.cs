using Inetlab.SMPP;
using Inetlab.SMPP.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sms.Models
{
    public class SmsSender
    {
        [Obsolete]
        public async Task<string> Send(string from, string to, string message)
        {
            var conf = new Conf();
            conf.host = "0.0.0.0";//это хост смпп сервера
            conf.port = 0000; //это порт смпп сервера
            conf.systemId = "test"; //это логин или системный ид смпп позователя
            conf.password = "test";//это парол смпп позователя

            SmppClient client = new SmppClient();
            await client.Connect(conf.host, conf.port); 
            await client.Bind(conf.systemId, conf.password, ConnectionMode.Transceiver);

            var resp = await client.Submit(
               SMS.ForSubmit()
                   .From(from, AddressTON.Alphanumeric, AddressNPI.Unknown)
                   .To(to, AddressTON.International, AddressNPI.ISDN)
                   .Coding(DataCodings.Cyrllic)
                   .Text(message)
               );
            if (resp.All(x => x.Header.Status == CommandStatus.ESME_ROK))
            {
                return "Message has been sent.";
            }
            return resp.GetValue(0).ToString();
        }

        public class Conf
        {
            public string host { get; set; }
            public int port { get; set; }
            public string systemId { get; set; }
            public string password { get; set; }
        }
    }
}
