using BaltnetaSmsApi;

var service = new SmsService("", "");
 var RESP=  await service.SendSmsAsync("37069553298","tett","CRAMO");


Console.ReadLine( );